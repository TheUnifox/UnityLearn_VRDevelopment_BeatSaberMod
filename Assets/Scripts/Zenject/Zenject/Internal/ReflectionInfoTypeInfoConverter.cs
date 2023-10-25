using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ModestTree;
using UnityEngine;

namespace Zenject.Internal
{
    // Token: 0x02000302 RID: 770
    public static class ReflectionInfoTypeInfoConverter
    {
        // Token: 0x06001095 RID: 4245 RVA: 0x0002E9B8 File Offset: 0x0002CBB8
        public static InjectTypeInfo.InjectMethodInfo ConvertMethod(ReflectionTypeInfo.InjectMethodInfo injectMethod)
        {
            MethodInfo methodInfo = injectMethod.MethodInfo;
            ZenInjectMethod action = ReflectionInfoTypeInfoConverter.TryCreateActionForMethod(methodInfo);
            action = delegate (object obj, object[] args)
            {
                methodInfo.Invoke(obj, args);
            };
            return new InjectTypeInfo.InjectMethodInfo(action, (from x in injectMethod.Parameters
                                                                select x.InjectableInfo).ToArray<InjectableInfo>(), methodInfo.Name);
        }

        // Token: 0x06001096 RID: 4246 RVA: 0x0002EA34 File Offset: 0x0002CC34
        public static InjectTypeInfo.InjectConstructorInfo ConvertConstructor(ReflectionTypeInfo.InjectConstructorInfo injectConstructor, Type type)
        {
            return new InjectTypeInfo.InjectConstructorInfo(ReflectionInfoTypeInfoConverter.TryCreateFactoryMethod(type, injectConstructor), (from x in injectConstructor.Parameters
                                                                                                                                              select x.InjectableInfo).ToArray<InjectableInfo>());
        }

        // Token: 0x06001097 RID: 4247 RVA: 0x0002EA74 File Offset: 0x0002CC74
        public static InjectTypeInfo.InjectMemberInfo ConvertField(Type parentType, ReflectionTypeInfo.InjectFieldInfo injectField)
        {
            return new InjectTypeInfo.InjectMemberInfo(ReflectionInfoTypeInfoConverter.GetSetter(parentType, injectField.FieldInfo), injectField.InjectableInfo);
        }

        // Token: 0x06001098 RID: 4248 RVA: 0x0002EA90 File Offset: 0x0002CC90
        public static InjectTypeInfo.InjectMemberInfo ConvertProperty(Type parentType, ReflectionTypeInfo.InjectPropertyInfo injectProperty)
        {
            return new InjectTypeInfo.InjectMemberInfo(ReflectionInfoTypeInfoConverter.GetSetter(parentType, injectProperty.PropertyInfo), injectProperty.InjectableInfo);
        }

        // Token: 0x06001099 RID: 4249 RVA: 0x0002EAAC File Offset: 0x0002CCAC
        private static ZenFactoryMethod TryCreateFactoryMethod(Type type, ReflectionTypeInfo.InjectConstructorInfo reflectionInfo)
        {
            if (type.DerivesFromOrEqual<Component>())
            {
                return null;
            }
            if (type.IsAbstract())
            {
                ModestTree.Assert.That(reflectionInfo.Parameters.IsEmpty<ReflectionTypeInfo.InjectParameterInfo>());
                return null;
            }
            ConstructorInfo constructorInfo = reflectionInfo.ConstructorInfo;
            ZenFactoryMethod zenFactoryMethod = ReflectionInfoTypeInfoConverter.TryCreateFactoryMethodCompiledLambdaExpression(type, constructorInfo);
            if (zenFactoryMethod == null)
            {
                if (constructorInfo == null)
                {
                    zenFactoryMethod = delegate (object[] args)
                    {
                        ModestTree.Assert.That(args.Length == 0);
                        return Activator.CreateInstance(type, new object[0]);
                    };
                }
                else
                {
                    zenFactoryMethod = new ZenFactoryMethod(constructorInfo.Invoke);
                }
            }
            return zenFactoryMethod;
        }

        // Token: 0x0600109A RID: 4250 RVA: 0x0002EB34 File Offset: 0x0002CD34
        private static ZenFactoryMethod TryCreateFactoryMethodCompiledLambdaExpression(Type type, ConstructorInfo constructor)
        {
            if (type.ContainsGenericParameters)
            {
                return null;
            }
            ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]));
            if (constructor == null)
            {
                return Expression.Lambda<ZenFactoryMethod>(Expression.Convert(Expression.New(type), typeof(object)), new ParameterExpression[]
                {
                    parameterExpression
                }).Compile();
            }
            ParameterInfo[] parameters = constructor.GetParameters();
            Expression[] array = new Expression[parameters.Length];
            for (int num = 0; num != parameters.Length; num++)
            {
                array[num] = Expression.Convert(Expression.ArrayIndex(parameterExpression, Expression.Constant(num)), parameters[num].ParameterType);
            }
            return Expression.Lambda<ZenFactoryMethod>(Expression.Convert(Expression.New(constructor, array), typeof(object)), new ParameterExpression[]
            {
                parameterExpression
            }).Compile();
        }

        // Token: 0x0600109B RID: 4251 RVA: 0x0002EBF8 File Offset: 0x0002CDF8
        private static ZenInjectMethod TryCreateActionForMethod(MethodInfo methodInfo)
        {
            if (methodInfo.DeclaringType.ContainsGenericParameters)
            {
                return null;
            }
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters.Any((ParameterInfo x) => x.ParameterType.ContainsGenericParameters))
            {
                return null;
            }
            Expression[] array = new Expression[parameters.Length];
            ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]));
            ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
            for (int num = 0; num != parameters.Length; num++)
            {
                array[num] = Expression.Convert(Expression.ArrayIndex(parameterExpression, Expression.Constant(num)), parameters[num].ParameterType);
            }
            return Expression.Lambda<ZenInjectMethod>(Expression.Call(Expression.Convert(parameterExpression2, methodInfo.DeclaringType), methodInfo, array), new ParameterExpression[]
            {
                parameterExpression2,
                parameterExpression
            }).Compile();
        }

        // Token: 0x0600109C RID: 4252 RVA: 0x0002ECD0 File Offset: 0x0002CED0
        private static IEnumerable<FieldInfo> GetAllFields(Type t, BindingFlags flags)
        {
            if (t == null)
            {
                return Enumerable.Empty<FieldInfo>();
            }
            return t.GetFields(flags).Concat(ReflectionInfoTypeInfoConverter.GetAllFields(t.BaseType, flags)).Distinct<FieldInfo>();
        }

        // Token: 0x0600109D RID: 4253 RVA: 0x0002ED00 File Offset: 0x0002CF00
        private static ZenMemberSetterMethod GetOnlyPropertySetter(Type parentType, string propertyName)
        {
            ModestTree.Assert.That(parentType != null);
            ModestTree.Assert.That(!string.IsNullOrEmpty(propertyName));
            List<FieldInfo> source = ReflectionInfoTypeInfoConverter.GetAllFields(parentType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).ToList<FieldInfo>();
            List<FieldInfo> writeableFields = (from f in source
                                               where f.Name == string.Format("<" + propertyName + ">k__BackingField", propertyName)
                                               select f).ToList<FieldInfo>();
            if (!writeableFields.Any<FieldInfo>())
            {
                throw new ZenjectException(string.Format("Can't find backing field for get only property {0} on {1}.\r\n{2}", propertyName, parentType.FullName, string.Join(";", (from f in source
                                                                                                                                                                                   select f.Name).ToArray<string>())));
            }
            return delegate (object injectable, object value)
            {
                writeableFields.ForEach(delegate (FieldInfo f)
                {
                    f.SetValue(injectable, value);
                });
            };
        }

        // Token: 0x0600109E RID: 4254 RVA: 0x0002EDD0 File Offset: 0x0002CFD0
        private static ZenMemberSetterMethod GetSetter(Type parentType, MemberInfo memInfo)
        {
            ZenMemberSetterMethod zenMemberSetterMethod = ReflectionInfoTypeInfoConverter.TryGetSetterAsCompiledExpression(parentType, memInfo);
            if (zenMemberSetterMethod != null)
            {
                return zenMemberSetterMethod;
            }
            FieldInfo fieldInfo = memInfo as FieldInfo;
            PropertyInfo propInfo = memInfo as PropertyInfo;
            if (fieldInfo != null)
            {
                return delegate (object injectable, object value)
                {
                    fieldInfo.SetValue(injectable, value);
                };
            }
            ModestTree.Assert.IsNotNull(propInfo);
            if (propInfo.CanWrite)
            {
                return delegate (object injectable, object value)
                {
                    propInfo.SetValue(injectable, value, null);
                };
            }
            return ReflectionInfoTypeInfoConverter.GetOnlyPropertySetter(parentType, propInfo.Name);
        }

        // Token: 0x0600109F RID: 4255 RVA: 0x0002EE5C File Offset: 0x0002D05C
        private static ZenMemberSetterMethod TryGetSetterAsCompiledExpression(Type parentType, MemberInfo memInfo)
        {
            if (parentType.ContainsGenericParameters)
            {
                return null;
            }
            FieldInfo fieldInfo = memInfo as FieldInfo;
            PropertyInfo propertyInfo = memInfo as PropertyInfo;
            if (!parentType.IsValueType() && (fieldInfo == null || !fieldInfo.IsInitOnly) && (propertyInfo == null || propertyInfo.CanWrite))
            {
                Type type = (fieldInfo != null) ? fieldInfo.FieldType : propertyInfo.PropertyType;
                ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
                ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
                return Expression.Lambda<ZenMemberSetterMethod>(Expression.Assign(Expression.MakeMemberAccess(Expression.Convert(parameterExpression, parentType), memInfo), Expression.Convert(parameterExpression2, type)), new ParameterExpression[]
                {
                    parameterExpression,
                    parameterExpression2
                }).Compile();
            }
            return null;
        }
    }
}
