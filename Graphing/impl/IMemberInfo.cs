using System;
using System.Collections.Generic;
using System.Linq;

public interface IMemberInfo
{
    string MemberName { get;  }
    ITypeInfo MemberType { get; }
    IEnumerable<Attribute> GetAttributes();
}

public static class MemberInfoExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this IMemberInfo memberInfo)
    {
        return memberInfo.GetAttributes().OfType<TAttribute>().FirstOrDefault();
    }
    public static bool HasAttribute<TAttribute>(this IMemberInfo memberInfo)
    {
        return memberInfo.GetAttributes().OfType<TAttribute>().Any();
    }
}