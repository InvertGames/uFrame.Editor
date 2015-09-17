using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invert.Data;
using Invert.IOC;

namespace Invert.Core.GraphDesigner
{
    public class TypesSystem : DiagramPlugin
        , IContextMenuQuery
        , IExecuteCommand<SelectTypeCommand>
        , IQueryTypes
    {
        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
            TypesInfo = InvertGraphEditor.TypesContainer.ResolveAll<GraphTypeInfo>().ToArray();
            Repository = container.Resolve<IRepository>();
        }

        public IRepository Repository { get; set; }

        public GraphTypeInfo[] TypesInfo { get; set; }

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, object obj)
        {
            var typedItem = obj as TypedItemViewModel;
            
            if (typedItem != null)
            {
                foreach (var item in TypesInfo)
                {
                    var item1 = item;
                    ui.AddCommand(new ContextMenuItem()
                    {
                        Title = item1.Name,
                        Group = item.Group,
                        Command = new LambdaCommand("Change Type", () =>
                        {
                            typedItem.RelatedType = item1.Name;
                        })
                    });
                }
            }
            var nodeItem = obj as ItemViewModel;
            if (nodeItem != null)
            {
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Delete",
                    Command = new DeleteCommand()
                    {
                        Title = "Delete Item",
                        Item = nodeItem.DataObject as IDataRecord
                    }
                });
            }
        }

        public List<SelectionMenuItem> CachedItems = null;
        public void Execute(SelectTypeCommand command)
        {
            
            var menu = new SelectionMenu();
            if (command.AllowNone)
            {
                menu.AddItem(new SelectionMenuItem("", "None", () =>
                {
                    command.ItemViewModel.RelatedType = null;
                }));
            }

           // var types = GetRelatedTypes(command).ToArray();
            foreach (var item in GetRelatedTypes(command))
            {
                var type1 = item;
                if (command.Filter == null || command.Filter(item))
                {
                    menu.AddItem(new SelectionMenuItem(item, () =>
                    {
                        var record = type1 as IDataRecord;
                        if (record != null)
                        {
                            command.ItemViewModel.RelatedType = record.Identifier;
                        }
                        else
                        {
                            command.ItemViewModel.RelatedType = type1.FullName;
                        }
                    }));
                }
                
            }
            //if (command.AllowNone)
            //{
            //    menu.AddItem(new SelectionMenuItem("", "None", () =>
            //    {
            //        command.ItemViewModel.RelatedType = null;
            //    }));
            //}

            //var categories = types.Where(_=>!string.IsNullOrEmpty(_.Group)).Select(_ => _.Group).Distinct().Select(_ => new SelectionMenuCategory()
            //{
            //    Title = _
            //});

            //foreach (var category in categories)
            //{
            //    menu.AddItem(category);
            //    var category1 = category;
            //    foreach (var type in types.Where(_=>_.Group == category1.Title))
            //    {
            //        var type1 = type;
                    
            //        menu.AddItem(new SelectionMenuItem(type, () =>
            //        {
            //            var record = type1 as IDataRecord;
            //            if (record != null)
            //            {
            //                command.ItemViewModel.RelatedType = record.Identifier;
            //            }
            //            else
            //            {
            //                command.ItemViewModel.RelatedType = type.TypeName;
            //            }
                       
            //        }),category);
            //    }
            //}

            //foreach (var source in types.Where(_=>string.IsNullOrEmpty(_.Group)))
            //{
            //    var type1 = source;
            //    menu.AddItem(new SelectionMenuItem(type1, () =>
            //    {
            //        var record = type1 as IDataRecord;
            //        if (record != null)
            //        {
            //            command.ItemViewModel.RelatedType = record.Identifier;
            //        }
            //        else
            //        {
            //            command.ItemViewModel.RelatedType = type1.TypeName;
            //        }
            //    }));
            //}

            Signal<IShowSelectionMenu>(_=>_.ShowSelectionMenu(menu));
//
//
//            InvertGraphEditor.WindowManager.InitItemWindow(types.ToArray(),,command.AllowNone);
        }
        public virtual IEnumerable<ITypeInfo> GetRelatedTypes(SelectTypeCommand command)
        {
            if (command.AllowNone)
            {
                yield return new SystemTypeInfo(typeof(void));
            }

            var queriedTypes = new List<ITypeInfo>();
            Signal<IQueryTypes>(_=>_.QueryTypes(queriedTypes));

            foreach (var item in queriedTypes)
                yield return item;
        }

        public void QueryTypes(List<ITypeInfo> typeInfo)
        {
            foreach (var item in Repository.AllOf<IClassTypeNode>().OfType<ITypeInfo>())
            {
                typeInfo.Add(item);
            }
        }
    }

    public interface IQueryTypes
    {
        void QueryTypes(List<ITypeInfo> typeInfo);
    }
}
