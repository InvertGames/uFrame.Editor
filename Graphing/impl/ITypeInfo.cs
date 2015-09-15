using System.Collections.Generic;
using Invert.Core;
using Invert.Data;

public interface ITypeInfo : IItem, IValueItem
{
    bool IsArray { get; }
    bool IsList { get; }
    bool IsEnum { get; }
    ITypeInfo InnerType { get; }
    string TypeName { get; }
    string FullName { get; }
    string Namespace { get; }
    IEnumerable<IMemberInfo> GetMembers();
    bool IsAssignableTo(ITypeInfo info);
}