using System;

namespace Invert.Core.GraphDesigner
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InspectorProperty : Attribute
    {
        public InspectorType InspectorType { get; set; }

        public Type CustomDrawerType { get; set; }

        public string InspectorTip { get; set; }

        public InspectorProperty(Type customDrawerType)
        {
            CustomDrawerType = customDrawerType;
        }

        public InspectorProperty()
        {
            InspectorType = InspectorType.Auto;
        }     
        
        public InspectorProperty(string tip)
        {
            InspectorTip = tip;
        }

        public InspectorProperty(string tip, InspectorType inspectorType)
        {
            InspectorType = inspectorType;
            InspectorTip = tip;
        }        
        
        public InspectorProperty(InspectorType inspectorType)
        {
            InspectorType = inspectorType;
        }
    }
}