using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModestTree;
using UnityEngine;

namespace Zenject.Internal
{
    // Token: 0x02000310 RID: 784
    public static class ReflectionTypeAnalyzer
    {
        // Token: 0x060010D6 RID: 4310 RVA: 0x0002F528 File Offset: 0x0002D728
        static ReflectionTypeAnalyzer()
        {
            ReflectionTypeAnalyzer._injectAttributeTypes.Add(typeof(InjectAttributeBase));
        }

        // Token: 0x060010D7 RID: 4311 RVA: 0x0002F54C File Offset: 0x0002D74C
        public static void AddCustomInjectAttribute<T>() where T : Attribute
        {
            ReflectionTypeAnalyzer.AddCustomInjectAttribute(typeof(T));
        }

        // Token: 0x060010D8 RID: 4312 RVA: 0x0002F560 File Offset: 0x0002D760
        public static void AddCustomInjectAttribute(Type type)
        {
            ModestTree.Assert.That(type.DerivesFrom<Attribute>());
            ReflectionTypeAnalyzer._injectAttributeTypes.Add(type);
        }

        // Token: 0x060010D9 RID: 4313 RVA: 0x0002F57C File Offset: 0x0002D77C
        public static ReflectionTypeInfo GetReflectionInfo(Type type)
        {
            ModestTree.Assert.That(!type.IsEnum(), "Tried to analyze enum type '{0}'.  This is not supported", type);
            ModestTree.Assert.That(!type.IsArray, "Tried to analyze array type '{0}'.  This is not supported", type);
            Type type2 = type.BaseType();
            if (type2 == typeof(object))
            {
                type2 = null;
            }
            return new ReflectionTypeInfo(type, type2, ReflectionTypeAnalyzer.GetConstructorInfo(type), ReflectionTypeAnalyzer.GetMethodInfos(type), ReflectionTypeAnalyzer.GetFieldInfos(type), ReflectionTypeAnalyzer.GetPropertyInfos(type));
        }

        // Token: 0x060010DA RID: 4314 RVA: 0x0002F5EC File Offset: 0x0002D7EC
        private static List<ReflectionTypeInfo.InjectPropertyInfo> GetPropertyInfos(Type type)
        {
            return (from x in type.DeclaredInstanceProperties()
                    where ReflectionTypeAnalyzer._injectAttributeTypes.Any((Type a) => x.HasAttribute(new Type[]
                    {
                a
                    }))
                    select new ReflectionTypeInfo.InjectPropertyInfo(x, ReflectionTypeAnalyzer.GetInjectableInfoForMember(type, x))).ToList<ReflectionTypeInfo.InjectPropertyInfo>();
        }

        // Token: 0x060010DB RID: 4315 RVA: 0x0002F64C File Offset: 0x0002D84C
        private static List<ReflectionTypeInfo.InjectFieldInfo> GetFieldInfos(Type type)
        {
            return (from x in type.DeclaredInstanceFields()
                    where ReflectionTypeAnalyzer._injectAttributeTypes.Any((Type a) => x.HasAttribute(new Type[]
                    {
                a
                    }))
                    select new ReflectionTypeInfo.InjectFieldInfo(x, ReflectionTypeAnalyzer.GetInjectableInfoForMember(type, x))).ToList<ReflectionTypeInfo.InjectFieldInfo>();
        }

        // Token: 0x060010DC RID: 4316 RVA: 0x0002F6AC File Offset: 0x0002D8AC
        static List<ReflectionTypeInfo.InjectMethodInfo> GetMethodInfos(Type type)
        {
            var injectMethodInfos = new List<ReflectionTypeInfo.InjectMethodInfo>();

            // Note that unlike with fields and properties we use GetCustomAttributes
            // This is so that we can ignore inherited attributes, which is necessary
            // otherwise a base class method marked with [Inject] would cause all overridden
            // derived methods to be added as well
            var methodInfos = type.DeclaredInstanceMethods()
                .Where(x => _injectAttributeTypes.Any(a => x.GetCustomAttributes(a, false).Any())).ToList();

            for (int i = 0; i < methodInfos.Count; i++)
            {
                var methodInfo = methodInfos[i];
                var injectAttr = methodInfo.AllAttributes<InjectAttributeBase>().SingleOrDefault();

                if (injectAttr != null)
                {
                    Assert.That(!injectAttr.Optional && injectAttr.Id == null && injectAttr.Source == InjectSources.Any,
                        "Parameters of InjectAttribute do not apply to constructors and methodInfos");
                }

                var injectParamInfos = methodInfo.GetParameters()
                    .Select(x => CreateInjectableInfoForParam(type, x)).ToList();

                injectMethodInfos.Add(
                    new ReflectionTypeInfo.InjectMethodInfo(methodInfo, injectParamInfos));
            }

            return injectMethodInfos;
        }

        // Token: 0x060010DD RID: 4317 RVA: 0x0002F7A0 File Offset: 0x0002D9A0
        private static ReflectionTypeInfo.InjectConstructorInfo GetConstructorInfo(Type type)
        {
            List<ReflectionTypeInfo.InjectParameterInfo> list = new List<ReflectionTypeInfo.InjectParameterInfo>();
            ConstructorInfo constructorInfo = ReflectionTypeAnalyzer.TryGetInjectConstructor(type);
            if (constructorInfo != null)
            {
                list.AddRange(from x in constructorInfo.GetParameters()
                              select ReflectionTypeAnalyzer.CreateInjectableInfoForParam(type, x));
            }
            return new ReflectionTypeInfo.InjectConstructorInfo(constructorInfo, list);
        }

        // Token: 0x060010DE RID: 4318 RVA: 0x0002F7FC File Offset: 0x0002D9FC
        private static ReflectionTypeInfo.InjectParameterInfo CreateInjectableInfoForParam(Type parentType, ParameterInfo paramInfo)
        {
            List<InjectAttributeBase> list = paramInfo.AllAttributes<InjectAttributeBase>().ToList<InjectAttributeBase>();
            ModestTree.Assert.That(list.Count <= 1, "Found multiple 'Inject' attributes on type parameter '{0}' of type '{1}'.  Parameter should only have one", paramInfo.Name, parentType);
            InjectAttributeBase injectAttributeBase = list.SingleOrDefault<InjectAttributeBase>();
            object identifier = null;
            bool flag = false;
            InjectSources sourceType = InjectSources.Any;
            if (injectAttributeBase != null)
            {
                identifier = injectAttributeBase.Id;
                flag = injectAttributeBase.Optional;
                sourceType = injectAttributeBase.Source;
            }
            bool flag2 = (paramInfo.Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault;
            return new ReflectionTypeInfo.InjectParameterInfo(paramInfo, new InjectableInfo(flag2 || flag, identifier, paramInfo.Name, paramInfo.ParameterType, flag2 ? paramInfo.DefaultValue : null, sourceType));
        }

        // Token: 0x060010DF RID: 4319 RVA: 0x0002F894 File Offset: 0x0002DA94
        private static InjectableInfo GetInjectableInfoForMember(Type parentType, MemberInfo memInfo)
        {
            List<InjectAttributeBase> list = memInfo.AllAttributes<InjectAttributeBase>().ToList<InjectAttributeBase>();
            ModestTree.Assert.That(list.Count <= 1, "Found multiple 'Inject' attributes on type field '{0}' of type '{1}'.  Field should only container one Inject attribute", memInfo.Name, parentType);
            InjectAttributeBase injectAttributeBase = list.SingleOrDefault<InjectAttributeBase>();
            object identifier = null;
            bool optional = false;
            InjectSources sourceType = InjectSources.Any;
            if (injectAttributeBase != null)
            {
                identifier = injectAttributeBase.Id;
                optional = injectAttributeBase.Optional;
                sourceType = injectAttributeBase.Source;
            }
            Type memberType = (memInfo is FieldInfo) ? ((FieldInfo)memInfo).FieldType : ((PropertyInfo)memInfo).PropertyType;
            return new InjectableInfo(optional, identifier, memInfo.Name, memberType, null, sourceType);
        }

        // Token: 0x060010E0 RID: 4320 RVA: 0x0002F920 File Offset: 0x0002DB20
        private static ConstructorInfo TryGetInjectConstructor(Type type)
        {
            if (type.DerivesFromOrEqual<Component>())
            {
                return null;
            }
            if (type.IsAbstract())
            {
                return null;
            }
            ConstructorInfo[] array = type.Constructors();
            if (array.IsEmpty<ConstructorInfo>())
            {
                return null;
            }
            if (!array.HasMoreThan(1))
            {
                return array[0];
            }
            ConstructorInfo constructorInfo = (from c in array
                                               where ReflectionTypeAnalyzer._injectAttributeTypes.Any((Type a) => c.HasAttribute(new Type[]
                                               {
                a
                                               }))
                                               select c).SingleOrDefault<ConstructorInfo>();
            if (constructorInfo != null)
            {
                return constructorInfo;
            }
            ConstructorInfo constructorInfo2 = (from x in array
                                                where x.IsPublic
                                                select x).OnlyOrDefault<ConstructorInfo>();
            if (constructorInfo2 != null)
            {
                return constructorInfo2;
            }
            return (from x in array
                    orderby x.GetParameters().Count<ParameterInfo>()
                    select x).First<ConstructorInfo>();
        }

        // Token: 0x04000562 RID: 1378
        private static readonly HashSet<Type> _injectAttributeTypes = new HashSet<Type>();
    }
}
