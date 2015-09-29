using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Invert.Core;

public class SystemTypeInfo : ITypeInfo
{
    private Type _systemType;

    public Type SystemType
    {
        get { return _systemType ?? typeof(void); }
        set { _systemType = value; }
    }

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

    public bool IsArray
    {
        get
        {
            if (Other != null)
            {
                return Other.IsArray;
            }
            return SystemType.IsArray;
        }
    }

    public bool IsList
    {
        get { return typeof (IList).IsAssignableFrom(SystemType); }
    }

    public bool IsEnum
    {
        get
        {
            if (Other != null)
            {
                return Other.IsEnum;
            }
            return SystemType.IsEnum;
        }
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
        get
        {
            if (Other != null)
            {
                return Other.TypeName;
            }
            return SystemType.Name;
        }
    }

    public virtual string FullName
    {
        get
        {
            if (Other != null)
            {
                return Other.FullName;
            }
            return SystemType.FullName;
        }
    }

    public string Namespace
    {
        get
        {
            if (Other != null)
            {
                return Other.Namespace;
            }
            return SystemType.Namespace;
        }
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
            return systemInfo.SystemType.IsAssignableFrom(SystemType) || systemInfo.SystemType.IsCastableTo(SystemType);
        }
        return info.FullName == FullName;
    }

    public virtual string Title { get { return TypeName; } }
    public string Group { get { return Namespace; } }
    public string SearchTag { get { return FullName; } }
    public string Description { get; set; }
    public string Identifier { get {return FullName;} set {}}
}