using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invert.Core.GraphDesigner.Unity.WindowsPlugin;
using Invert.IOC;
using UnityEditor;

namespace Invert.Core.GraphDesigner.Unity.WindowsSystem
{
    
    public class WindowSystemTestPlugin : DiagramPlugin, IContextMenuQuery
    {
        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
            container.RegisterDrawer<HelloWorldWindowViewModel,HelloWorldWindowDrawer>();
        }

        [MenuItem("uFrame Dev/Window System Tester/Open Normal Window In The Center")]
        public static void OpenNormalWindowInTheCenter()
        {
            InvertApplication.SignalEvent<IOpenWindow>(w => w.OpenWindow(new HelloWorldWindowViewModel()));
        }

        public void QueryContextMenu(ICommandUI ui, object obj)
        {
//            var diagramNode = obj as DiagramNodeViewModel;
//            if (diagramNode != null)
//            {
//                ui.AddCommand(new OpenWinCommand());
//            }
        }

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, object obj)
        {
          //  throw new NotImplementedException();
        }
    }
}
