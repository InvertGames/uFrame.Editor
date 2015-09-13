using System;
using System.Collections.Generic;
using Invert.Data;
using Invert.Json;

namespace Invert.Core.GraphDesigner
{
  
    public class GraphFilter : IGraphFilter, IJsonObject
    {
        private Type[] _allowedTypes;

        private FilterLocations _locations = new FilterLocations();
        private FilterCollapsedDictionary _collapsedValues = new FilterCollapsedDictionary();

        private string _identifier;

        public IRepository Repository { get; set; }

        public string Identifier
        {
            get
            {
                if (string.IsNullOrEmpty(_identifier))
                {
                    _identifier = Guid.NewGuid().ToString();
                }
                return _identifier;
            }
            set { _identifier = value; }
        }

        public bool Changed { get; set; }

        public virtual bool ImportedOnly
        {
            get { return false; }
        }

        public bool IsExplorerCollapsed { get; set; }

        public FilterLocations Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        public virtual string Name
        {
            get { return "All"; }
            set { }
        }

        public virtual bool UseStraightLines
        {
            get { return false; }
        }

        public IEnumerable<IDiagramNode> FilterNodes
        {
            get { yield break; }
        }

        public IEnumerable<IFilterItem> FilterItems { get; private set; }

        public bool AllowExternalNodes
        {
            get { return true; }
        }

        public string InfoLabel { get; private set; }

        public FilterCollapsedDictionary CollapsedValues
        {
            get { return _collapsedValues; }
            set { _collapsedValues = value; }
        }

        public virtual bool IsItemAllowed(object item, Type t)
        {
            return IsAllowed(item, t);
        }

        public virtual bool IsAllowed(object item, Type t)
        {
            return true;
        }

        public void Serialize(JSONClass cls)
        {
            cls.Add("Identifier", Identifier);
            cls.Add("Locations", _locations.Serialize());
            cls.Add("CollapsedValues", _collapsedValues.Serialize());
        }

        public void Deserialize(JSONClass cls)
        {
            if (cls["Identifier"] != null)
            {
                Identifier = cls["Identifier"].Value;
            }
            Locations.Deserialize(cls["Locations"].AsObject);
            CollapsedValues.Deserialize(cls["CollapsedValues"].AsObject);

        }
    }
}