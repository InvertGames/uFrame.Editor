using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class SystemTypeInfo : ITypeInfo
{

    public Type SystemType { get; set; }
    public ITypeInfo Other { get; set; }
    public SystemTypeInfo(Type systemType)
    {
        SystemType = systemType;
    }

    public SystemTypeInfo(Type systemType, ITypeInfo other)
    {
        SystemType = systemType;
        Other = other;
    }

    public bool IsArray { get { return SystemType.IsArray; } }

    public bool IsList
    {
        get { return typeof (IList).IsAssignableFrom(SystemType); }
    }

    public bool IsEnum
    {
        get { return SystemType.IsEnum; }
    }

    public ITypeInfo InnerType
    {
        get
        {
            var genericType = SystemType.GetGenericArguments().FirstOrDefault();
            if (genericType != null)
            {
                return new SystemTypeInfo(genericType);
            }
            return null;
        }
    }

    public string TypeName
    {
        get { return SystemType.Name; }
    }

    public string FullName
    {
        get { return SystemType.FullName; }
    }

    public virtual IEnumerable<IMemberInfo> GetMembers()
    {
        if (Other != null)
        {
            foreach (var item in Other.GetMembers())
            {
                yield return item;
            }
        }
        if (SystemType != null)
        {
            if (IsEnum)
            {
                foreach (var item in SystemType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    if (item == null) continue;
                    yield return new SystemFieldMemberInfo(item);
                }
            }
            else
            {

                foreach (var item in SystemType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (item == null) continue;
                    yield return new SystemFieldMemberInfo(item);
                }
                foreach (var item in SystemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    yield return new SystemPropertyMemberInfo(item);
                }
            }
         
        }
        
    }

    public bool IsAssignableTo(ITypeInfo info)
    {
        var systemInfo = info as SystemTypeInfo;
        if (systemInfo != null)
        {
            return systemInfo.SystemType.IsAssignableFrom(SystemType);
        }
        return info.FullName == FullName;
    }
}