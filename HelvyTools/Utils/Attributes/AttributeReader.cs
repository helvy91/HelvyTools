using System.Reflection;
using HelvyTools.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace HelvyTools.Utils.Attributes
{
    public static class AttributeReader
    {
        public static Dictionary<string, PropertyInfo> ReadPropertyNames<TAttribute, TType>()
            where TAttribute : NameAttribute
        {
            var dict = new Dictionary<string, PropertyInfo>();

            var properties = typeof(TType).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<TAttribute>();
                if (attr != null)
                {
                    dict.Add(attr.Name, prop);
                }
                else
                {
                    dict.Add(prop.Name, prop);
                }
            }

            return dict;
        }

        public static string ReadEnumName<TAttribute, TEnum>(TEnum value)
            where TAttribute : NameAttribute
            where TEnum : System.Enum
        {
            var type = typeof(TEnum);   
            var enumValue = type.GetMember(value.ToString());
            var attr = enumValue.Single().GetCustomAttribute<TAttribute>();
            if (attr != null)
            {
                return attr.Name;
            }

            return string.Empty;
        }

        public static string ReadClassName<TAttribute, TType>()
            where TAttribute : NameAttribute
        {
            var type = typeof(TType);
            var attr = type.GetCustomAttribute<TAttribute>();
            if (attr != null)
            {
                return attr.Name;
            }

            return string.Empty;
        }
    }
}
