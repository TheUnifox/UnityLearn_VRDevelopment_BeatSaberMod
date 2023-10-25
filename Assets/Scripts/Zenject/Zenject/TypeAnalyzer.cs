using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModestTree;
using UnityEditor.Rendering.LookDev;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002F2 RID: 754
    public static class TypeAnalyzer
    {
        // Token: 0x1700015E RID: 350
        // (get) Token: 0x0600103E RID: 4158 RVA: 0x0002DD3C File Offset: 0x0002BF3C
        // (set) Token: 0x0600103F RID: 4159 RVA: 0x0002DD44 File Offset: 0x0002BF44
        public static ReflectionBakingCoverageModes ReflectionBakingCoverageMode { get; set; }

        // Token: 0x06001040 RID: 4160 RVA: 0x0002DD4C File Offset: 0x0002BF4C
        public static bool ShouldAllowDuringValidation<T>()
        {
            return TypeAnalyzer.ShouldAllowDuringValidation(typeof(T));
        }

        // Token: 0x06001041 RID: 4161 RVA: 0x0002DD60 File Offset: 0x0002BF60
        public static bool ShouldAllowDuringValidation(Type type)
        {
            bool flag;
            if (!TypeAnalyzer._allowDuringValidation.TryGetValue(type, out flag))
            {
                flag = TypeAnalyzer.ShouldAllowDuringValidationInternal(type);
                TypeAnalyzer._allowDuringValidation.Add(type, flag);
            }
            return flag;
        }

        // Token: 0x06001042 RID: 4162 RVA: 0x0002DD90 File Offset: 0x0002BF90
        private static bool ShouldAllowDuringValidationInternal(Type type)
        {
            return type.DerivesFrom<IInstaller>() || type.DerivesFrom<IValidatable>() || type.DerivesFrom<Context>() || type.HasAttribute<ZenjectAllowDuringValidationAttribute>();
        }

        // Token: 0x06001043 RID: 4163 RVA: 0x0002DDB4 File Offset: 0x0002BFB4
        public static bool HasInfo<T>()
        {
            return TypeAnalyzer.HasInfo(typeof(T));
        }

        // Token: 0x06001044 RID: 4164 RVA: 0x0002DDC8 File Offset: 0x0002BFC8
        public static bool HasInfo(Type type)
        {
            return TypeAnalyzer.TryGetInfo(type) != null;
        }

        // Token: 0x06001045 RID: 4165 RVA: 0x0002DDD4 File Offset: 0x0002BFD4
        public static InjectTypeInfo GetInfo<T>()
        {
            return TypeAnalyzer.GetInfo(typeof(T));
        }

        // Token: 0x06001046 RID: 4166 RVA: 0x0002DDE8 File Offset: 0x0002BFE8
        public static InjectTypeInfo GetInfo(Type type)
        {
            InjectTypeInfo injectTypeInfo = TypeAnalyzer.TryGetInfo(type);
            ModestTree.Assert.IsNotNull(injectTypeInfo, "Unable to get type info for type '{0}'", type);
            return injectTypeInfo;
        }

        // Token: 0x06001047 RID: 4167 RVA: 0x0002DDFC File Offset: 0x0002BFFC
        public static InjectTypeInfo TryGetInfo<T>()
        {
            return TypeAnalyzer.TryGetInfo(typeof(T));
        }

        // Token: 0x06001048 RID: 4168 RVA: 0x0002DE10 File Offset: 0x0002C010
        public static InjectTypeInfo TryGetInfo(Type type)
        {
            InjectTypeInfo infoInternal;
            if (TypeAnalyzer._typeInfo.TryGetValue(type, out infoInternal))
            {
                return infoInternal;
            }
            infoInternal = TypeAnalyzer.GetInfoInternal(type);
            if (infoInternal != null)
            {
                ModestTree.Assert.IsEqual(infoInternal.Type, type);
                ModestTree.Assert.IsNull(infoInternal.BaseTypeInfo);
                Type type2 = type.BaseType();
                if (type2 != null && !TypeAnalyzer.ShouldSkipTypeAnalysis(type2))
                {
                    infoInternal.BaseTypeInfo = TypeAnalyzer.TryGetInfo(type2);
                }
            }
            TypeAnalyzer._typeInfo.Add(type, infoInternal);
            return infoInternal;
        }

        // Token: 0x06001049 RID: 4169 RVA: 0x0002DE80 File Offset: 0x0002C080
        private static InjectTypeInfo GetInfoInternal(Type type)
        {
            if (TypeAnalyzer.ShouldSkipTypeAnalysis(type))
            {
                return null;
            }
            MethodInfo method = type.GetMethod("__zenCreateInjectTypeInfo", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (method != null)
            {
                return ((ZenTypeInfoGetter)Delegate.CreateDelegate(typeof(ZenTypeInfoGetter), method))();
            }
            if (TypeAnalyzer.ReflectionBakingCoverageMode == ReflectionBakingCoverageModes.NoCheckAssumeFullCoverage)
            {
                return null;
            }
            if (TypeAnalyzer.ReflectionBakingCoverageMode == ReflectionBakingCoverageModes.FallbackToDirectReflectionWithWarning)
            {
                ModestTree.Log.Warn("No reflection baking information found for type '{0}' - using more costly direct reflection instead", new object[]
                {
                    type
                });
            }
            return TypeAnalyzer.CreateTypeInfoFromReflection(type);
        }

        // Token: 0x0600104A RID: 4170 RVA: 0x0002DEF8 File Offset: 0x0002C0F8
        public static bool ShouldSkipTypeAnalysis(Type type)
        {
            return type == null || type.IsEnum() || type.IsArray || type.IsInterface() || type.ContainsGenericParameters() || TypeAnalyzer.IsStaticType(type) || type == typeof(object);
        }

        // Token: 0x0600104B RID: 4171 RVA: 0x0002DF48 File Offset: 0x0002C148
        private static bool IsStaticType(Type type)
        {
            return type.IsAbstract() && type.IsSealed();
        }

        // Token: 0x0600104C RID: 4172 RVA: 0x0002DF5C File Offset: 0x0002C15C
        private static InjectTypeInfo CreateTypeInfoFromReflection(Type type)
        {
            Zenject.Internal.ReflectionTypeInfo reflectionInfo = Zenject.Internal.ReflectionTypeAnalyzer.GetReflectionInfo(type);
            InjectTypeInfo.InjectConstructorInfo injectConstructor = Zenject.Internal.ReflectionInfoTypeInfoConverter.ConvertConstructor(reflectionInfo.InjectConstructor, type);
            InjectTypeInfo.InjectMethodInfo[] injectMethods = reflectionInfo.InjectMethods.Select(new Func<Zenject.Internal.ReflectionTypeInfo.InjectMethodInfo, InjectTypeInfo.InjectMethodInfo>(Zenject.Internal.ReflectionInfoTypeInfoConverter.ConvertMethod)).ToArray<InjectTypeInfo.InjectMethodInfo>();
            InjectTypeInfo.InjectMemberInfo[] injectMembers = (from x in reflectionInfo.InjectFields
                                                               select Zenject.Internal.ReflectionInfoTypeInfoConverter.ConvertField(type, x)).Concat(from x in reflectionInfo.InjectProperties
                                                                                                                                                     select Zenject.Internal.ReflectionInfoTypeInfoConverter.ConvertProperty(type, x)).ToArray<InjectTypeInfo.InjectMemberInfo>();
            return new InjectTypeInfo(type, injectConstructor, injectMethods, injectMembers);
        }

        // Token: 0x0400051E RID: 1310
        private static Dictionary<Type, InjectTypeInfo> _typeInfo = new Dictionary<Type, InjectTypeInfo>();

        // Token: 0x0400051F RID: 1311
        private static Dictionary<Type, bool> _allowDuringValidation = new Dictionary<Type, bool>();

        // Token: 0x04000520 RID: 1312
        public const string ReflectionBakingGetInjectInfoMethodName = "__zenCreateInjectTypeInfo";

        // Token: 0x04000521 RID: 1313
        public const string ReflectionBakingFactoryMethodName = "__zenCreate";

        // Token: 0x04000522 RID: 1314
        public const string ReflectionBakingInjectMethodPrefix = "__zenInjectMethod";

        // Token: 0x04000523 RID: 1315
        public const string ReflectionBakingFieldSetterPrefix = "__zenFieldSetter";

        // Token: 0x04000524 RID: 1316
        public const string ReflectionBakingPropertySetterPrefix = "__zenPropertySetter";
    }
}
