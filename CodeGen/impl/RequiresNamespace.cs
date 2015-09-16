using System;
using System.CodeDom;
using System.Reflection;
using Invert.IOC;
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
}