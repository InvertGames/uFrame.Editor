using System.Collections.Generic;

public interface ITypeInfo
{
    bool IsArray { get; }
    bool IsList { get; }
    bool IsEnum { get; }
    ITypeInfo InnerType { get; }
    string TypeName { get; }
    string FullName { get; }
    IEnumerable<IMemberInfo> GetMembers();
    bool IsAssignableTo(ITypeInfo info);
}