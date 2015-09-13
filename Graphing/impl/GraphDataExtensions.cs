using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.Data;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public static class GraphDataExtensions
    {
        public static FilterItem ShowInFilter(this IGraphFilter filter, IDiagramNode node, Vector2 position, bool collapsed = false)
        {
            var filterItem = new FilterItem()
            {
                FilterId = filter.Identifier,
                NodeId = node.Identifier,
                Position = position,
                Collapsed = collapsed
            };
            filter.Repository.Add(filterItem);
            var filterNode = filter as IDiagramNode;
            if (filterNode != null)
            {
                filterNode.NodeAddedInFilter(node);
            }
            return filterItem;
        }
        public static void HideInFilter(this IGraphFilter filter, IDiagramNode node)
        {
            filter.Repository.RemoveAll<FilterItem>(p => p.FilterId == filter.Identifier && p.NodeId == node.Identifier);
        }
        public static IEnumerable<IDiagramNode> GetImportableItems(this IGraphFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            var items = filter.FilterNodes.Select(p=>p.Identifier).ToArray();

            return
                filter.GetAllowedDiagramItems()
                    .Where(p => !items.Contains(p.Identifier))
                    .ToArray();
        }

        public static IEnumerable<IDiagramNode> GetAllowedDiagramItems(this IGraphFilter filter)
        {

            return filter.Repository.AllOf<IDiagramNode>().Where(p => filter.IsAllowed(p, p.GetType()));
        }

        public static IGraphFilter Container(this IDiagramNode node)
        {
            foreach (var item in node.Repository.All<FilterItem>())
            {
                if (item.NodeId == node.Identifier)
                {
                    return item.Filter;
                }
            }
            return null;
        }



        public static IEnumerable<IGraphFilter> FilterPath(this IDiagramNode node)
        {
            return FilterPathInternal(node).Reverse();
        }

        private static IEnumerable<IGraphFilter> FilterPathInternal(IDiagramNode node)
        {
            var container = node.Container();
            while (container != null)
            {
                yield return container;
                var filterNode = container as IDiagramNode;
                if (filterNode != null)
                {
                    container = filterNode.Container();
                    if (container == filterNode)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

            }
        }
        public static IEnumerable<IGraphFilter> GetFilterPath(this IGraphData t)
        {
            return t.FilterStack.Reverse();
        }

        public static IEnumerable<IGraphFilter> GetFilters(this INodeRepository t)
        {
            return t.NodeItems.OfType<IGraphFilter>();
        }



        //public static IEnumerable<EnumData> GetEnums(this INodeRepository t)
        //{
        //    return t.NodeItems.OfType<EnumData>();
        //}


        public static void Prepare(this IGraphData designerData)
        {
            designerData.RefactorCount = 0;
            designerData.Initialize();
        }

        //public static IEnumerable<IDiagramNode> FilterItems(this IGraphData designerData, INodeRepository repository)
        //{
        //    return designerData.CurrentFilter.FilterItems(repository);
        //}

        public static void FilterLeave(this INodeRepository data)
        {
        }

        public static void ApplyFilter(this INodeRepository designerData)
        {

            designerData.UpdateLinks();
            //foreach (var item in designerData.AllDiagramItems)
            //{


            //}
            //UpdateLinks();
        }


        public static string GetUniqueName(this INodeRepository designerData, string name)
        {
            var tempName = name;
            var index = 1;

            while (designerData.NodeItems.Any(p => p != null && p.Name != null && p.Name.ToUpper() == tempName.ToUpper()))
            {
                tempName = name + index;
                index++;
            }
            return tempName;
        }


        public static void UpdateLinks(this INodeRepository designerData)
        {
            //designerData.CleanUpFilters();
            //designerData.Links.Clear();

            //var items = designerData.GetDiagramItems().SelectMany(p => p.Items).Where(p => designerData.CurrentFilter.IsItemAllowed(p, p.GetType())).ToArray();
            //var diagramItems = designerData.GetDiagramItems().ToArray();
            //foreach (var item in items)
            //{
            //    designerData.Links.AddRange(item.GetLinks(diagramItems));
            //}

            //var diagramFilter = designerData.CurrentFilter as IDiagramNode;
            //if (diagramFilter != null)
            //{
            //    var diagramFilterItems = diagramFilter.Items.OfType<IDiagramNode>().ToArray();
            //    foreach (var diagramItem in diagramItems)
            //    {
            //        designerData.Links.AddRange(diagramItem.GetLinks(diagramFilterItems));
            //    }
            //}

            //var models = designerData.GetDiagramItems().ToArray();

            //foreach (var viewModelData in models)
            //{
            //    //viewModelData.Filter = CurrentFilter;
            //    designerData.Links.AddRange(viewModelData.GetLinks(diagramItems));
            //}
        }

        public static IDiagramNode RelatedNode(this ITypedItem item)
        {
            var gt = item as GenericTypedChildItem;
            if (gt != null)
            {
                return gt.RelatedTypeNode as IDiagramNode;
            }
            
            return item.Repository.GetById<IDiagramNode>(item.RelatedType);
        }

       

    }
}