using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.Data;

namespace Invert.Core.GraphDesigner
{
    public class CompilingSystem : DiagramPlugin
        , IToolbarQuery
        , IContextMenuQuery
        , IExecuteCommand<SaveAndCompileCommand>
        , IDataRecordInserted
        , IDataRecordRemoved
        , IDataRecordPropertyChanged
       
    {
        private List<IDataRecord> _changedRecrods;

        public ValidationSystem ValidationSystem
        {
            get { return Container.Resolve<ValidationSystem>(); }
        }
        public void QueryToolbarCommands(ToolbarUI ui)
        {
            ui.AddCommand(new ToolbarItem()
            {
                Command = new SaveAndCompileCommand(),
                Title = "Save & Compile",
                Position = ToolbarPosition.Right,
                Description = "Start code generation process. This generates C# code based on the nodes and items in the diagram."
            });     
          
        }

        public void Execute(SaveAndCompileCommand command)
        {
            InvertApplication.SignalEvent<ITaskHandler>(_ => { _.BeginTask(Generate()); });
        }

        public IEnumerable<IDataRecord> GetItems(IRepository repository)
        {
            return repository.AllOf<IDataRecord>();
            //yield return repository.AllOf<uFrameDatabaseConfig>().FirstOrDefault();
            //var workspaceSvc = InvertApplication.Container.Resolve<WorkspaceService>();
            //foreach (var workspace in workspaceSvc.Workspaces)
            //{
            //    if (workspace.CompilationMode == CompilationMode.OnlyWhenActive &&
            //        workspace != workspaceSvc.CurrentWorkspace) continue;
            //    yield return workspace;
            //    foreach (var item in workspace.Graphs)
            //    {
            //        yield return item;
            //        foreach (var node in item.NodeItems)
            //        {
            //            yield return node;
            //            foreach (var child in node.PersistedItems)
            //                yield return child;
            //        }
            //    }
            //}
        }

        public IEnumerator Generate()
        {
            var a = ValidationSystem.ValidateDatabase();
            while (a.MoveNext())
            {
                yield return a.Current;
            }
            if (ValidationSystem.ErrorNodes.Count > 0)
            {
                Signal<INotify>(_=>_.Notify("Please fix all issues before compiling.",NotificationIcon.Error));
                yield break;
            }

            var repository = InvertGraphEditor.Container.Resolve<IRepository>();
            repository.Commit();
            var config = InvertGraphEditor.Container.Resolve<IGraphConfiguration>();
            var items = GetItems(repository).Distinct().ToArray();
            yield return 
                new TaskProgress(0f, "Refactoring");

            // Grab all the file generators
            var fileGenerators = InvertGraphEditor.GetAllFileGenerators(config, items).ToArray();
            var length = 100f / (fileGenerators.Length + 1);
            var index = 0;

            foreach (var codeFileGenerator in fileGenerators)
            {
                index++;
                yield return new TaskProgress(length * index, "Generating " + System.IO.Path.GetFileName(codeFileGenerator.AssetPath));
                // Grab the information for the file
                var fileInfo = new FileInfo(codeFileGenerator.SystemPath);
                // Make sure we are allowed to generate the file
                if (!codeFileGenerator.CanGenerate(fileInfo))
                {
                    var fileGenerator = codeFileGenerator;
                    InvertApplication.SignalEvent<ICompileEvents>(_ => _.FileSkipped(fileGenerator));
                    continue;
                }

                // Get the path to the directory
                var directory = System.IO.Path.GetDirectoryName(fileInfo.FullName);
                // Create it if it doesn't exist
                if (directory != null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                try
                {
                    // Write the file
                    File.WriteAllText(fileInfo.FullName, codeFileGenerator.ToString());
                }
                catch (Exception ex)
                {
                    InvertApplication.LogException(ex);
                    InvertApplication.Log("Coudln't create file " + fileInfo.FullName);
                }
                CodeFileGenerator generator = codeFileGenerator;
                InvertApplication.SignalEvent<ICompileEvents>(_ => _.FileGenerated(generator));
            }
            ChangedRecrods.Clear();
            InvertApplication.SignalEvent<ICompileEvents>(_ => _.PostCompile(config, items));

            yield return
                new TaskProgress(100f, "Complete");

#if UNITY_EDITOR
            repository.Commit();
            if (InvertGraphEditor.Platform != null) // Testability
                InvertGraphEditor.Platform.RefreshAssets();
#endif
        }


        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, object obj)
        {
            var node = obj as DiagramNodeViewModel;
            if (node != null)
            {
                var config = InvertGraphEditor.Container.Resolve<IGraphConfiguration>();
                var fileGenerators = InvertGraphEditor.GetAllFileGenerators(config, new [] {node.DataObject as IDataRecord}).ToArray();
                foreach (var file in fileGenerators)
                {
                    var file1 = file;
                    ui.AddCommand(new ContextMenuItem()
                    {
                        Title = "Open " + Path.GetFileName(file.AssetPath),
                        Command = new LambdaCommand("Open File", () =>
                        {
                            InvertApplication.Log(file1.AssetPath);
                            InvertGraphEditor.Platform.OpenScriptFile(file1.AssetPath);
                        })
                    });
                }
            }
        }

        public List<IDataRecord> ChangedRecrods
        {
            get { return _changedRecrods ?? (_changedRecrods = new List<IDataRecord>()); }
            set { _changedRecrods = value; }
        }

        public void RecordInserted(IDataRecord record)
        {
            if (ChangedRecrods.Contains(record)) return;
            ChangedRecrods.Add(record);
        }

        public void RecordRemoved(IDataRecord record)
        {
            ChangedRecrods.Remove(record);
        }

        public void PropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
            if (ChangedRecrods.Contains(record)) return;
            ChangedRecrods.Add(record);
        }
    }
}