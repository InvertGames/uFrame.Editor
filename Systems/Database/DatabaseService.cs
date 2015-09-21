using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Invert.Data;
using Invert.IOC;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public class DatabaseService : DiagramPlugin,  
        IDataRecordInserted, 
        IDataRecordRemoved, 
        IDataRecordPropertyBeforeChange,
        IDataRecordPropertyChanged,
        IDataRecordRemoving,
        IDataRecordManagerRefresh,
        IChangeDatabase,
        IToolbarQuery,
        IContextMenuQuery,
        IExecuteCommand<ChangeDatabaseCommand>,
        IExecuteCommand<SaveCommand>,
        IExecuteCommand<CreateDatabaseCommand>,
        IExecuteCommand<EditDatabaseCommand>
    {
        private Dictionary<string, uFrameDatabaseConfig> _configurations;

        public uFrameDatabaseConfig CurrentConfiguration { get; set; }

        // Important make sure we intialize very late so that other plugins can register graph configurations
        public override decimal LoadPriority
        {
            get { return 50000; }
        }

        public override bool Required
        {
            get { return true; }
        }

        public string CurrentDatabaseIdentifier
        {
            get { return InvertGraphEditor.Prefs.GetString("CurrentDatabaseIdentifier", string.Empty); }
            set {InvertGraphEditor.Prefs.SetString("CurrentDatabaseIdentifier",value); }
        }

        

        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
            var path = DbRootPath;
            var dbDirectories = Directory.GetDirectories(path,"*.db",SearchOption.AllDirectories);
            foreach (var item in dbDirectories)
            {
                var db = new TypeDatabase(new JsonRepositoryFactory(item));
                var config = GetConfig(db, Path.GetFileNameWithoutExtension(item));
                config.FullPath = item;
                container.RegisterInstance<IGraphConfiguration>(config, config.Identifier);
            }
           
            CurrentConfiguration = Configurations.ContainsKey(CurrentDatabaseIdentifier)
                ? Configurations[CurrentDatabaseIdentifier]
                : Configurations.Values.FirstOrDefault();


            //if (CurrentConfiguration == null)
            //{
            //    InvertApplication.Log("Database not found, creating one now.");
            //    var createDatabase = InvertApplication.Container.Resolve<ICreateDatabase>();
            //    if (createDatabase != null)
            //    {
            //        CurrentConfiguration = createDatabase.CreateDatabase("uFrameDB", "Code");
            //    }
            //    else
            //    {
            //        InvertApplication.Log("A plugin the creates a default database is not available.  Please implement ICreateDatabase to provide this feature.");
            //    }
            //}
            if (CurrentConfiguration != null)
            {
                container.RegisterInstance<IGraphConfiguration>(CurrentConfiguration);
           
                container.RegisterInstance<IRepository>(CurrentConfiguration.Database);
                
                //var typeDatabase = container.Resolve<IRepository>();
                CurrentConfiguration.Database.AddListener<IDataRecordInserted>(this);
                CurrentConfiguration.Database.AddListener<IDataRecordRemoved>(this);
                CurrentConfiguration.Database.AddListener<IDataRecordPropertyChanged>(this);
                CurrentConfiguration.Database.AddListener<IDataRecordPropertyBeforeChange>(this);
            }
            else
            {
                InvertApplication.Log("A uFrameDatabase doesn't exist.");
            }
           
        }

        private static string DbRootPath
        {
            get
            {
                var path = Application.dataPath;
                if (path.EndsWith("Assets"))
                {
                    path = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
                }
                return path;
            }
        }

        private uFrameDatabaseConfig GetConfig(TypeDatabase db, string title)
        {
            var config = db.GetSingle<uFrameDatabaseConfig>();

            if (config == null)
            {
                config = new uFrameDatabaseConfig
                {
                    CodeOutputPath = "Code",
                    Namespace = title
                };

                db.Add(config);
                db.Commit();
            }
            config.Database = db;
            config.Title = title;
            if (!Configurations.ContainsKey(config.Identifier))
            Configurations.Add(config.Identifier, config);
            return config;
        }

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
           
        }

        public Dictionary<string, uFrameDatabaseConfig> Configurations
        {
            get { return _configurations ?? (_configurations = new Dictionary<string, uFrameDatabaseConfig>()); }
            set { _configurations = value; }
        }

        public void RecordInserted(IDataRecord record)
        {
            InvertApplication.SignalEvent<IDataRecordInserted>(_ =>
            {
                if (_ != this) _.RecordInserted(record);
            });
        }

        public void RecordRemoved(IDataRecord record)
        {
            //TODO Check already invoked in JsonFileRecordManager and FastJsonFileRecordManager

            InvertApplication.SignalEvent<IDataRecordRemoved>(_ =>
            {
                if (_ != this) _.RecordRemoved(record);
            }); 
        }

        public void PropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
            InvertApplication.SignalEvent<IDataRecordPropertyChanged>(_ =>
            {
                if (_ != this) _.PropertyChanged(record, name, previousValue, nextValue);
            });
        }
        public void BeforePropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
            InvertApplication.SignalEvent<IDataRecordPropertyBeforeChange>(_ =>
            {
                if (_ != this) _.BeforePropertyChanged(record, name, previousValue, nextValue);
            });
        }
        public void ChangeDatabase(IGraphConfiguration configuration)
        {
            var configRecord = configuration as IDataRecord;
            if (configRecord != null) CurrentDatabaseIdentifier = configRecord.Identifier;
            InvertApplication.Container = null;
        }

        public void QueryToolbarCommands(ToolbarUI ui)
        {
            ui.AddCommand(new ToolbarItem()
            {
                Title = CurrentConfiguration == null ? "Change Database" : "Database: " + CurrentConfiguration.Title,
                Command = new ChangeDatabaseCommand(),
                Position = ToolbarPosition.BottomLeft,
                Order = -1
            });
            ui.AddCommand(new ToolbarItem()
            {
                Title = "Save",
                Command = new SaveCommand(),
                Position = ToolbarPosition.Right
            });
        }

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, object obj)
        {
          
        }

        public void Execute(ChangeDatabaseCommand command)
        {
            Signal<IShowContextMenu>(_=>_.Show(null,command));

            
        }

        public void Execute(SaveCommand command)
        {
            Container.Resolve<IRepository>().Commit();
        }

        public void Execute(CreateDatabaseCommand command)
        {
            if (Directory.Exists(command.Name))
            {
                throw new Exception(string.Format("Database {0} already exists.", command.Name));
            }
            var dbDir = Directory.CreateDirectory(Path.Combine(DbRootPath, command.Name + ".db"));
            var db = new TypeDatabase(new JsonRepositoryFactory(dbDir.FullName));
            var config = GetConfig(db, command.Name);
            config.Namespace = command.Namespace;
            config.Title = command.Name;
            config.CodeOutputPath = command.CodePath;
            config.Namespace = command.Namespace ?? config.Namespace;
            config.FullPath = dbDir.FullName;
            config.Database = db;
            db.Commit();
            CurrentDatabaseIdentifier = config.Identifier;
            InvertApplication.Container = null;
            if(InvertApplication.Container!=null)
                InvertApplication.SignalEvent<INotify>(_ => _.Notify(command.Name + " Database " + " has been created!", NotificationIcon.Info));

        }

        public void Execute(EditDatabaseCommand command)
        {
            command.Configuration.Namespace = command.Namespace;
            command.Configuration.CodeOutputPath = command.CodePath;
            command.Configuration.Database.Commit();
        }

        public void RecordRemoving(IDataRecord record)
        {
            InvertApplication.SignalEvent<IDataRecordRemoving>(_ =>
            {
                if (_ != this) _.RecordRemoving(record);
            }); 
        }

        public void ManagerRefreshed(IDataRecordManager manager)
        {
            InvertApplication.SignalEvent<IDataRecordManagerRefresh>(_ =>
            {
                if (_ != this) _.ManagerRefreshed(manager);
            }); 
        }
    }
}