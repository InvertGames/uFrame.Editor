using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using Invert.IOC;
using Invert.uFrame.ECS;
using uFrame.Attributes;

namespace Invert.Core.GraphDesigner
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class RequiresNamespace : TemplateAttribute
    {
        public override int Priority
        {
            get { return -3; }
        }

        public RequiresNamespace(string ns)
        {
            Namespace = ns;
        }

        public string Namespace { get; set; }
        public override void Modify(object templateInstance, MemberInfo info, TemplateContext ctx)
        {
            base.Modify(templateInstance, info, ctx);
            ctx.TryAddNamespace(Namespace);
        }
    }  
    
    [AttributeUsage(AttributeTargets.Class)]
    public class WithMetaInfo : TemplateAttribute
    {
        public override int Priority
        {
            get { return -3; }
        }

        public override void Modify(object templateInstance, MemberInfo info, TemplateContext ctx)
        {
            base.Modify(templateInstance, info, ctx);
            ctx.CurrentDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(uFrameIdentifier)),
                new CodeAttributeArgument(new CodePrimitiveExpression(ctx.DataObject.Identifier))
                ));
        }
    }    
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoNamespaces : TemplateAttribute
    {
        public override int Priority
        {
            get { return -3; }
        }

        public override void Modify(object templateInstance, MemberInfo info, TemplateContext ctx)
        {
            var obj = ctx.DataObject as IDiagramNodeItem ;
            if (obj != null)
            foreach (var item in obj.Graph.NodeItems.OfType<ISystemGroupProvider>().SelectMany(p => p.GetSystemGroups()))
            {
                ctx.TryAddNamespace(item.Namespace);
                foreach (var child in item.PersistedItems.OfType<IMemberInfo>())
                {
                    ctx.TryAddNamespace(child.MemberType.Namespace);
                }
            }
        }
    }
}