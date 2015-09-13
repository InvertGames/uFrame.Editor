using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.Data;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public class CodeFileSyncSystem : DiagramPlugin
        //, 
        //IDataRecordRemoved,
        , ICommandExecuted,
        ICommandExecuting,
        IExecuteCommand<LambdaFileSyncCommand>

        //IDataRecordPropertyBeforeChange
    {
        public List<IDataRecord> ChangedRecords { get; set; }

        public void RecordRemoved(IDataRecord record)
        {

            var items = record.GetCodeGeneratorsForNode(Container.Resolve<DatabaseService>().CurrentConfiguration).ToArray();
            foreach (var item in items)
            {
                var fullpath = Path.Combine(Application.dataPath, item.RelativeFullPathName);
                if (File.Exists(fullpath))
                {
                    File.Delete(fullpath);
                }
            }
        }

        public struct GenFileInfo
        {
            public GenFileInfo(string fullPath, OutputGenerator generator)
            {

                FullPath = fullPath;
                Generator = generator;
            }


            public string FullPath;
            public OutputGenerator Generator;
        }
        public void CommandExecuted(ICommand command)
        {
            if (command is ApplyRenameCommand)
            {
                RenameApplied(command as ApplyRenameCommand);
            }
            if (!(command is IFileSyncCommand)) return;
            var items = Container.Resolve<IRepository>().AllOf<IDataRecord>().ToArray();

            var gensNow = InvertGraphEditor.GetAllCodeGenerators(
                Container.Resolve<DatabaseService>().CurrentConfiguration, items)
                .ToDictionary(p => Path.GetFileName(p.Filename), x => new GenFileInfo(Path.Combine(Application.dataPath, x.RelativeFullPathName), x));

            foreach (var item in Gens)
            {
                if (!gensNow.ContainsKey(item.Key))
                {
                    // Its been removed or renamed
                    if (File.Exists(item.Value.FullPath))
                        File.Delete(item.Value.FullPath);
                }
                else
                {
                    var nowItem = gensNow[item.Key];
                    if (nowItem.Generator.ObjectData == item.Value.Generator.ObjectData)
                    {
                        // It has been moved
                        if (File.Exists(item.Value.FullPath))
                            File.Move(item.Value.FullPath, nowItem.FullPath);
                    }
                }
            }

            Gens.Clear();
            GC.Collect();
            IsRename = false;

        }
        private void RenameApplying(ApplyRenameCommand applyRenameCommand)
        {
            RenameGens = InvertGraphEditor.GetAllCodeGenerators(
                Container.Resolve<DatabaseService>().CurrentConfiguration, new IDataRecord[] { applyRenameCommand.Item })
                .Select(p => Path.Combine(Application.dataPath, p.RelativeFullPathName)).ToArray();
        }

        public string[] RenameGens { get; set; }

        private void RenameApplied(ApplyRenameCommand applyRenameCommand)
        {
            var gensNow = InvertGraphEditor.GetAllCodeGenerators(
                Container.Resolve<DatabaseService>().CurrentConfiguration, new IDataRecord[] { applyRenameCommand.Item })
                .Select(p => Path.Combine(Application.dataPath, p.RelativeFullPathName)).ToArray();

            if (gensNow.Length == RenameGens.Length)
            {
                for (var i = 0; i < gensNow.Length; i++)
                {
                    if (File.Exists(RenameGens[i]))
                        File.Move(RenameGens[i], gensNow[i]);
                }
            }

        }

        public void CommandExecuting(ICommand command)
        {
            if (command is ApplyRenameCommand)
            {
                RenameApplying(command as ApplyRenameCommand);
            }
            if (!(command is IFileSyncCommand)) return;
            IsRename = command is ApplyRenameCommand;
            var items = Container.Resolve<IRepository>().AllOf<IDataRecord>().ToArray();
            Gens = InvertGraphEditor.GetAllCodeGenerators(
                Container.Resolve<DatabaseService>().CurrentConfiguration, items)
                .ToDictionary(p => Path.GetFileName(p.Filename), x => new GenFileInfo(Path.Combine(Application.dataPath, x.RelativeFullPathName), x));



        }


        public bool IsRename { get; set; }

        public Dictionary<string, GenFileInfo> Gens { get; set; }

        public void BeforePropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {

        }

        public void Execute(LambdaFileSyncCommand command)
        {
            Execute(new LambdaCommand(command.Title,command.Action));
        }
    }
}