using Invert.Core.GraphDesigner;

public class EnumChildItem : GenericNodeChildItem, IMemberInfo
{
    public string MemberName { get { return this.Name; }}
    public ITypeInfo MemberType { get { return new SystemTypeInfo(typeof(int)); } }
}