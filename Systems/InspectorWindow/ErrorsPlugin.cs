using System.Collections.Generic;
using Invert.Common;
using Invert.Common.UI;
using Invert.Core.GraphDesigner;
using Invert.Data;
using UnityEditor;
using UnityEngine;

public class ErrorsPlugin : DiagramPlugin
    , IDrawErrorsList
    , INodeValidated
    , IDataRecordRemoved
{
    private List<ErrorInfo> _errorInfo = new List<ErrorInfo>();
    private static GUIStyle _eventButtonStyleSmall;

    public List<ErrorInfo> ErrorInfo
    {
        get { return _errorInfo; }
        set { _errorInfo = value; }
    }
    public static GUIStyle EventButtonStyleSmall
    {
        get
        {
            var textColor = Color.white;
            if (_eventButtonStyleSmall == null)
                _eventButtonStyleSmall = new GUIStyle
                {
                    normal = { background = ElementDesignerStyles.GetSkinTexture("EventButton"), textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black },
                    active = { background = ElementDesignerStyles.CommandBarClosedStyle.normal.background },
                    stretchHeight = true,

                    fixedHeight = 25,
                    border = new RectOffset(3, 3, 3, 3),

                    padding = new RectOffset(25, 0, 5, 5)
                };

            return _eventButtonStyleSmall;
        }
    }
    public void DrawErrors(Rect rect)
    {
        GUIHelpers.IsInsepctor = false;
        if (InvertGraphEditor.PlatformDrawer == null) return;
        // var itemRect = new Rect(0f, 0f, rect.width, 25);
        if (GUIHelpers.DoToolbarEx("Issues"))
        {

            foreach (var item in ErrorInfo)
            {
                var item1 = item;
                var name = string.Empty;
                var node = item.Record as GraphNode;
                if (node != null)
                {
                    var filter = node.Filter;
                    if (filter != null)
                        name = filter.Name + ": ";
                }
                if (GUILayout.Button(name + item1.Message,EventButtonStyleSmall))
                {
                   
                    if (node != null)
                        Execute(new NavigateToNodeCommand()
                        {
                            Node = node
                        });
                }

                //InvertGraphEditor.PlatformDrawer.DoButton(itemRect.Pad(25f,0f,0f,0f),item.Message,CachedStyles.DefaultLabel, () =>
                //{

                //});
                //itemRect.y += 26;
                //var lineRect = itemRect;
                //lineRect.height -= 24;
                //InvertGraphEditor.PlatformDrawer.DrawRect(lineRect,new Color(0f,0f,0f,0.3f));
            }
        }
        
    
    }

    public void NodeValidated(IDiagramNode node)
    {
        ErrorInfo.Clear();
        Signal<IQueryErrors>(_=>_.QueryErrors(ErrorInfo));
    }

    public void RecordRemoved(IDataRecord record)
    {
        ErrorInfo.RemoveAll(p => p.Record.Identifier == record.Identifier);
    }
}