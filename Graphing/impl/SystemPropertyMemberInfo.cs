using System.Reflection;

public class SystemPropertyMemberInfo : IMemberInfo
{
    private PropertyInfo PropertyInfo;

    public SystemPropertyMemberInfo(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
    }

    public string MemberName { get { return PropertyInfo.Name; } }

    public ITypeInfo MemberType
    {
        get
        {
            return new SystemTypeInfo(PropertyInfo.PropertyType);
        }
    }
}