using System;
using System.CodeDom;
using System.Reflection;

namespace Invert.Core.GraphDesigner
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class AsOverride : TemplateAttribute
    {
        public override void Modify(object templateInstance, MemberInfo info, TemplateContext ctx)
        {
            base.Modify(templateInstance, info, ctx);
            if (ctx.CurrentMember != null)
            {
                ctx.CurrentMember.Attributes |= MemberAttributes.Override;
            }
        }
    }
}