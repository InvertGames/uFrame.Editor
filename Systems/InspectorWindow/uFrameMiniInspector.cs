using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Animations;
using UnityEngine;

namespace Invert.Core.GraphDesigner.Unity.InspectorWindow
{
    public class uFrameMiniInspector
    {
        private List<PropertyFieldViewModel> _properties;
        private UnityDrawer _drawer;

        public uFrameMiniInspector(object target)
        {
            DiagramViewModel = InvertApplication.Container.Resolve<DesignerWindow>().DiagramViewModel;
            
            foreach (var item in target.GetPropertiesWithAttribute<InspectorProperty>())
            {
                var property = item.Key;
                var attribute = item.Value;
                var fieldViewModel = new PropertyFieldViewModel()
                {
                    Name = property.Name,
                };
                fieldViewModel.Getter = () => property.GetValue(target, null);
                fieldViewModel.Setter = _ => property.SetValue(target, _, null);
                fieldViewModel.InspectorType = attribute.InspectorType;
                fieldViewModel.Type = property.PropertyType;
                fieldViewModel.DiagramViewModel = DiagramViewModel;
                fieldViewModel.CustomDrawerType = attribute.CustomDrawerType;
                fieldViewModel.CachedValue = fieldViewModel.Getter();
                if (!string.IsNullOrEmpty(attribute.InspectorTip)) fieldViewModel.InspectorTip = attribute.InspectorTip;
                Properties.Add(fieldViewModel);
            }
        }

        public UnityDrawer Drawer
        {
            get { return _drawer ?? (InvertApplication.Container.Resolve<IPlatformDrawer>() as UnityDrawer); }
            set { _drawer = value; }
        }

        public DiagramViewModel DiagramViewModel { get; set; }

        public List<PropertyFieldViewModel> Properties
        {
            get { return _properties ?? (_properties = new List<PropertyFieldViewModel>()); }
            set { _properties = value; }
        }

        public void Draw()
        {
            foreach (var prop in Properties)
            {

                Drawer.DrawInspector(prop,CachedStyles.DefaultLabel as GUIStyle);
            }            
        }

    }
}
