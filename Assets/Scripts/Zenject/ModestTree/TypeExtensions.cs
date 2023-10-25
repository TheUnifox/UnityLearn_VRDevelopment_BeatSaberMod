using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ModestTree
{
    // Token: 0x0200000F RID: 15
    public static class TypeExtensions
    {
        // Token: 0x06000064 RID: 100 RVA: 0x00002C14 File Offset: 0x00000E14
        public static bool DerivesFrom<T>(this Type a)
        {
            return a.DerivesFrom(typeof(T));
        }

        // Token: 0x06000065 RID: 101 RVA: 0x00002C28 File Offset: 0x00000E28
        public static bool DerivesFrom(this Type a, Type b)
        {
            return b != a && a.DerivesFromOrEqual(b);
        }

        // Token: 0x06000066 RID: 102 RVA: 0x00002C3C File Offset: 0x00000E3C
        public static bool DerivesFromOrEqual<T>(this Type a)
        {
            return a.DerivesFromOrEqual(typeof(T));
        }

        // Token: 0x06000067 RID: 103 RVA: 0x00002C50 File Offset: 0x00000E50
        public static bool DerivesFromOrEqual(this Type a, Type b)
        {
            return b == a || b.IsAssignableFrom(a);
        }

        // Token: 0x06000068 RID: 104 RVA: 0x00002C64 File Offset: 0x00000E64
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            foreach (Type type in givenType.Interfaces())
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }
            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
            Type baseType = givenType.BaseType;
            return !(baseType == null) && TypeExtensions.IsAssignableToGenericType(baseType, genericType);
        }

        // Token: 0x06000069 RID: 105 RVA: 0x00002CD4 File Offset: 0x00000ED4
        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        // Token: 0x0600006A RID: 106 RVA: 0x00002CDC File Offset: 0x00000EDC
        public static bool IsValueType(this Type type)
        {
            bool isValueType;
            if (!TypeExtensions._isValueType.TryGetValue(type, out isValueType))
            {
                isValueType = type.IsValueType;
                TypeExtensions._isValueType[type] = isValueType;
            }
            return isValueType;
        }

        // Token: 0x0600006B RID: 107 RVA: 0x00002D0C File Offset: 0x00000F0C
        public static MethodInfo[] DeclaredInstanceMethods(this Type type)
        {
            return type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        // Token: 0x0600006C RID: 108 RVA: 0x00002D18 File Offset: 0x00000F18
        public static PropertyInfo[] DeclaredInstanceProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        // Token: 0x0600006D RID: 109 RVA: 0x00002D24 File Offset: 0x00000F24
        public static FieldInfo[] DeclaredInstanceFields(this Type type)
        {
            return type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        // Token: 0x0600006E RID: 110 RVA: 0x00002D30 File Offset: 0x00000F30
        public static Type BaseType(this Type type)
        {
            return type.BaseType;
        }

        // Token: 0x0600006F RID: 111 RVA: 0x00002D38 File Offset: 0x00000F38
        public static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }

        // Token: 0x06000070 RID: 112 RVA: 0x00002D40 File Offset: 0x00000F40
        public static bool IsGenericTypeDefinition(this Type type)
        {
            return type.IsGenericTypeDefinition;
        }

        // Token: 0x06000071 RID: 113 RVA: 0x00002D48 File Offset: 0x00000F48
        public static bool IsPrimitive(this Type type)
        {
            return type.IsPrimitive;
        }

        // Token: 0x06000072 RID: 114 RVA: 0x00002D50 File Offset: 0x00000F50
        public static bool IsInterface(this Type type)
        {
            return type.IsInterface;
        }

        // Token: 0x06000073 RID: 115 RVA: 0x00002D58 File Offset: 0x00000F58
        public static bool ContainsGenericParameters(this Type type)
        {
            return type.ContainsGenericParameters;
        }

        // Token: 0x06000074 RID: 116 RVA: 0x00002D60 File Offset: 0x00000F60
        public static bool IsAbstract(this Type type)
        {
            return type.IsAbstract;
        }

        // Token: 0x06000075 RID: 117 RVA: 0x00002D68 File Offset: 0x00000F68
        public static bool IsSealed(this Type type)
        {
            return type.IsSealed;
        }

        // Token: 0x06000076 RID: 118 RVA: 0x00002D70 File Offset: 0x00000F70
        public static MethodInfo Method(this Delegate del)
        {
            return del.Method;
        }

        // Token: 0x06000077 RID: 119 RVA: 0x00002D78 File Offset: 0x00000F78
        public static Type[] GenericArguments(this Type type)
        {
            return type.GetGenericArguments();
        }

        // Token: 0x06000078 RID: 120 RVA: 0x00002D80 File Offset: 0x00000F80
        public static Type[] Interfaces(this Type type)
        {
            Type[] interfaces;
            if (!TypeExtensions._interfaces.TryGetValue(type, out interfaces))
            {
                interfaces = type.GetInterfaces();
                TypeExtensions._interfaces.Add(type, interfaces);
            }
            return interfaces;
        }

        // Token: 0x06000079 RID: 121 RVA: 0x00002DB0 File Offset: 0x00000FB0
        public static ConstructorInfo[] Constructors(this Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        // Token: 0x0600007A RID: 122 RVA: 0x00002DBC File Offset: 0x00000FBC
        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType())
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        // Token: 0x0600007B RID: 123 RVA: 0x00002DD0 File Offset: 0x00000FD0
        public static bool IsClosedGenericType(this Type type)
        {
            bool flag;
            if (!TypeExtensions._isClosedGenericType.TryGetValue(type, out flag))
            {
                flag = (type.IsGenericType() && type != type.GetGenericTypeDefinition());
                TypeExtensions._isClosedGenericType[type] = flag;
            }
            return flag;
        }

        // Token: 0x0600007C RID: 124 RVA: 0x00002E14 File Offset: 0x00001014
        public static IEnumerable<Type> GetParentTypes(this Type type)
        {
            if (type == null || type.BaseType() == null || type == typeof(object) || type.BaseType() == typeof(object))
            {
                yield break;
            }
            yield return type.BaseType();
            foreach (Type type2 in type.BaseType().GetParentTypes())
            {
                yield return type2;
            }
            IEnumerator<Type> enumerator = null;
            yield break;
            yield break;
        }

        // Token: 0x0600007D RID: 125 RVA: 0x00002E24 File Offset: 0x00001024
        public static bool IsOpenGenericType(this Type type)
        {
            bool flag;
            if (!TypeExtensions._isOpenGenericType.TryGetValue(type, out flag))
            {
                flag = (type.IsGenericType() && type == type.GetGenericTypeDefinition());
                TypeExtensions._isOpenGenericType[type] = flag;
            }
            return flag;
        }

        // Token: 0x0600007E RID: 126 RVA: 0x00002E68 File Offset: 0x00001068
        public static T GetAttribute<T>(this MemberInfo provider) where T : Attribute
        {
            return provider.AllAttributes<T>().Single<T>();
        }

        // Token: 0x0600007F RID: 127 RVA: 0x00002E78 File Offset: 0x00001078
        public static T TryGetAttribute<T>(this MemberInfo provider) where T : Attribute
        {
            return provider.AllAttributes<T>().OnlyOrDefault<T>();
        }

        // Token: 0x06000080 RID: 128 RVA: 0x00002E88 File Offset: 0x00001088
        public static bool HasAttribute(this MemberInfo provider, params Type[] attributeTypes)
        {
            return provider.AllAttributes(attributeTypes).Any<Attribute>();
        }

        // Token: 0x06000081 RID: 129 RVA: 0x00002E98 File Offset: 0x00001098
        public static bool HasAttribute<T>(this MemberInfo provider) where T : Attribute
        {
            return provider.AllAttributes(new Type[]
            {
                typeof(T)
            }).Any<Attribute>();
        }

        // Token: 0x06000082 RID: 130 RVA: 0x00002EB8 File Offset: 0x000010B8
        public static IEnumerable<T> AllAttributes<T>(this MemberInfo provider) where T : Attribute
        {
            return provider.AllAttributes(new Type[]
            {
                typeof(T)
            }).Cast<T>();
        }

        // Token: 0x06000083 RID: 131 RVA: 0x00002ED8 File Offset: 0x000010D8
        public static IEnumerable<Attribute> AllAttributes(this MemberInfo provider, params Type[] attributeTypes)
        {
            Attribute[] customAttributes = Attribute.GetCustomAttributes(provider, typeof(Attribute), true);
            if (attributeTypes.Length == 0)
            {
                return customAttributes;
            }
            return from a in customAttributes
                   where attributeTypes.Any((Type x) => a.GetType().DerivesFromOrEqual(x))
                   select a;
        }

        // Token: 0x06000084 RID: 132 RVA: 0x00002F24 File Offset: 0x00001124
        public static bool HasAttribute(this ParameterInfo provider, params Type[] attributeTypes)
        {
            return provider.AllAttributes(attributeTypes).Any<Attribute>();
        }

        // Token: 0x06000085 RID: 133 RVA: 0x00002F34 File Offset: 0x00001134
        public static bool HasAttribute<T>(this ParameterInfo provider) where T : Attribute
        {
            return provider.AllAttributes(new Type[]
            {
                typeof(T)
            }).Any<Attribute>();
        }

        // Token: 0x06000086 RID: 134 RVA: 0x00002F54 File Offset: 0x00001154
        public static IEnumerable<T> AllAttributes<T>(this ParameterInfo provider) where T : Attribute
        {
            return provider.AllAttributes(new Type[]
            {
                typeof(T)
            }).Cast<T>();
        }

        // Token: 0x06000087 RID: 135 RVA: 0x00002F74 File Offset: 0x00001174
        public static IEnumerable<Attribute> AllAttributes(this ParameterInfo provider, params Type[] attributeTypes)
        {
            Attribute[] customAttributes = Attribute.GetCustomAttributes(provider, typeof(Attribute), true);
            if (attributeTypes.Length == 0)
            {
                return customAttributes;
            }
            return from a in customAttributes
                   where attributeTypes.Any((Type x) => a.GetType().DerivesFromOrEqual(x))
                   select a;
        }

        // Token: 0x04000014 RID: 20
        private static readonly Dictionary<Type, bool> _isClosedGenericType = new Dictionary<Type, bool>();

        // Token: 0x04000015 RID: 21
        private static readonly Dictionary<Type, bool> _isOpenGenericType = new Dictionary<Type, bool>();

        // Token: 0x04000016 RID: 22
        private static readonly Dictionary<Type, bool> _isValueType = new Dictionary<Type, bool>();

        // Token: 0x04000017 RID: 23
        private static readonly Dictionary<Type, Type[]> _interfaces = new Dictionary<Type, Type[]>();
    }
}
