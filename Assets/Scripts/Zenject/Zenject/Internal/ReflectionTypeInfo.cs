using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zenject.Internal
{
    // Token: 0x02000309 RID: 777
    [NoReflectionBaking]
    public class ReflectionTypeInfo
    {
        // Token: 0x060010BE RID: 4286 RVA: 0x0002F2BC File Offset: 0x0002D4BC
        public ReflectionTypeInfo(Type type, Type baseType, ReflectionTypeInfo.InjectConstructorInfo injectConstructor, List<ReflectionTypeInfo.InjectMethodInfo> injectMethods, List<ReflectionTypeInfo.InjectFieldInfo> injectFields, List<ReflectionTypeInfo.InjectPropertyInfo> injectProperties)
        {
            this.Type = type;
            this.BaseType = baseType;
            this.InjectFields = injectFields;
            this.InjectConstructor = injectConstructor;
            this.InjectMethods = injectMethods;
            this.InjectProperties = injectProperties;
        }

        // Token: 0x0400054E RID: 1358
        public readonly Type Type;

        // Token: 0x0400054F RID: 1359
        public readonly Type BaseType;

        // Token: 0x04000550 RID: 1360
        public readonly List<ReflectionTypeInfo.InjectPropertyInfo> InjectProperties;

        // Token: 0x04000551 RID: 1361
        public readonly List<ReflectionTypeInfo.InjectFieldInfo> InjectFields;

        // Token: 0x04000552 RID: 1362
        public readonly ReflectionTypeInfo.InjectConstructorInfo InjectConstructor;

        // Token: 0x04000553 RID: 1363
        public readonly List<ReflectionTypeInfo.InjectMethodInfo> InjectMethods;

        // Token: 0x0200030A RID: 778
        [NoReflectionBaking]
        public class InjectFieldInfo
        {
            // Token: 0x060010BF RID: 4287 RVA: 0x0002F2F4 File Offset: 0x0002D4F4
            public InjectFieldInfo(FieldInfo fieldInfo, InjectableInfo injectableInfo)
            {
                this.InjectableInfo = injectableInfo;
                this.FieldInfo = fieldInfo;
            }

            // Token: 0x04000554 RID: 1364
            public readonly FieldInfo FieldInfo;

            // Token: 0x04000555 RID: 1365
            public readonly InjectableInfo InjectableInfo;
        }

        // Token: 0x0200030B RID: 779
        [NoReflectionBaking]
        public class InjectParameterInfo
        {
            // Token: 0x060010C0 RID: 4288 RVA: 0x0002F30C File Offset: 0x0002D50C
            public InjectParameterInfo(ParameterInfo parameterInfo, InjectableInfo injectableInfo)
            {
                this.InjectableInfo = injectableInfo;
                this.ParameterInfo = parameterInfo;
            }

            // Token: 0x04000556 RID: 1366
            public readonly ParameterInfo ParameterInfo;

            // Token: 0x04000557 RID: 1367
            public readonly InjectableInfo InjectableInfo;
        }

        // Token: 0x0200030C RID: 780
        [NoReflectionBaking]
        public class InjectPropertyInfo
        {
            // Token: 0x060010C1 RID: 4289 RVA: 0x0002F324 File Offset: 0x0002D524
            public InjectPropertyInfo(PropertyInfo propertyInfo, InjectableInfo injectableInfo)
            {
                this.InjectableInfo = injectableInfo;
                this.PropertyInfo = propertyInfo;
            }

            // Token: 0x04000558 RID: 1368
            public readonly PropertyInfo PropertyInfo;

            // Token: 0x04000559 RID: 1369
            public readonly InjectableInfo InjectableInfo;
        }

        // Token: 0x0200030D RID: 781
        [NoReflectionBaking]
        public class InjectMethodInfo
        {
            // Token: 0x060010C2 RID: 4290 RVA: 0x0002F33C File Offset: 0x0002D53C
            public InjectMethodInfo(MethodInfo methodInfo, List<ReflectionTypeInfo.InjectParameterInfo> parameters)
            {
                this.MethodInfo = methodInfo;
                this.Parameters = parameters;
            }

            // Token: 0x0400055A RID: 1370
            public readonly MethodInfo MethodInfo;

            // Token: 0x0400055B RID: 1371
            public readonly List<ReflectionTypeInfo.InjectParameterInfo> Parameters;
        }

        // Token: 0x0200030E RID: 782
        [NoReflectionBaking]
        public class InjectConstructorInfo
        {
            // Token: 0x060010C3 RID: 4291 RVA: 0x0002F354 File Offset: 0x0002D554
            public InjectConstructorInfo(ConstructorInfo constructorInfo, List<ReflectionTypeInfo.InjectParameterInfo> parameters)
            {
                this.ConstructorInfo = constructorInfo;
                this.Parameters = parameters;
            }

            // Token: 0x0400055C RID: 1372
            public readonly ConstructorInfo ConstructorInfo;

            // Token: 0x0400055D RID: 1373
            public readonly List<ReflectionTypeInfo.InjectParameterInfo> Parameters;
        }
    }
}
