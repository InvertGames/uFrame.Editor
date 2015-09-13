using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.Json;

namespace Invert.Data
{
    public class JsonFileRecordManager : IDataRecordManager
    {
        public IRepository Repository { get; set; }
        private Dictionary<string, IDataRecord> _cached;
        private DirectoryInfo _directoryInfo;
        private HashSet<string> _removed;

        public string RootPath { get; set; }

        public void Initialize(IRepository repository)
        {
            Repository = repository;
          
        }

        public Type For { get; set; }

        public string RecordsPath
        {
            get { return Path.Combine(RootPath, For.FullName); }
        }

        public JsonFileRecordManager(IRepository repository, string rootPath, Type @for)
        {
            RootPath = rootPath;
            For = @for;
            Repository = repository;
            Initialize(repository);
        }

        public DirectoryInfo DirectoryInfo
        {
            get { return _directoryInfo ?? (_directoryInfo = new DirectoryInfo(RecordsPath)); }
            set { _directoryInfo = value; }
        }

        private bool _loadedCached;
        private void LoadRecordsIntoCache()
        {
            if (_loadedCached) return;

            if (!DirectoryInfo.Exists)
            {
                DirectoryInfo.Create();
            }
            foreach (var file in DirectoryInfo.GetFiles())
            {
                LoadRecord(file);
            }
            _loadedCached = true;
        }

        private void LoadRecord(FileInfo file)
        {
            if (Cached.ContainsKey(Path.GetFileNameWithoutExtension(file.Name))) return;
            var record = InvertJsonExtensions.DeserializeObject(For, JSON.Parse(File.ReadAllText(file.FullName))) as IDataRecord;
            if (record != null)
            {
                record.Repository = this.Repository;
                
                Cached.Add(record.Identifier, record);
                record.Changed = false;
            }
        }

        public IDataRecord GetSingle(string identifier)
        {
 
            LoadRecordsIntoCache();
    
            if (!Cached.ContainsKey(identifier))
            {
   
                return null;
            }
            return Cached[identifier];
        }

        public IEnumerable<IDataRecord> GetAll()
        {
    
            LoadRecordsIntoCache();
        
            return Cached.Values.Where(p=>!Removed.Contains(p.Identifier));
        }

        public void Add(IDataRecord o)
        {
            if (Removed.Contains(o.Identifier))
                Removed.Remove(o.Identifier);

            o.Changed = true;
            if (string.IsNullOrEmpty(o.Identifier))
            {
                o.Identifier = Guid.NewGuid().ToString();
            }
            o.Repository = this.Repository;
            if (!Cached.ContainsKey(o.Identifier))
            {
                Cached.Add(o.Identifier, o);
                Repository.Signal<IDataRecordInserted>(_=>_.RecordInserted(o));
            }
        }

        public void Commit()
        {
            if (!DirectoryInfo.Exists)
            {
                DirectoryInfo.Create();
            }
            foreach (var item in Cached)
            {
                var filename = Path.Combine(RecordsPath, item.Key + ".json");
                if (Removed.Contains(item.Key))
                {
                    File.Delete(filename);
                }
                else
                {
                    if (item.Value.Changed)
                    {
                        var json = InvertJsonExtensions.SerializeObject(item.Value);
                        File.WriteAllText(filename, json.ToString(true));
                    }
                    item.Value.Changed = false;
                }
            }
        }

        public void Remove(IDataRecord item)
        {
            Removed.Add(item.Identifier);
            Repository.Signal<IDataRecordRemoved>(_ => _.RecordRemoved(item));
        }

        public HashSet<string> Removed
        {
            get { return _removed ?? (_removed = new HashSet<string>()); }
            set { _removed = value; }
        }

        public Dictionary<string, IDataRecord> Cached
        {
            get { return _cached ?? (_cached = new Dictionary<string, IDataRecord>(StringComparer.OrdinalIgnoreCase)); }
            set { _cached = value; }
        }
    }
}