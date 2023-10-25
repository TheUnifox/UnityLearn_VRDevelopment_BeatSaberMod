using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenject
{
    // Token: 0x0200000E RID: 14
    [NoReflectionBaking]
    public class InjectTypeInfo
    {
        // Token: 0x0600001B RID: 27 RVA: 0x000020EE File Offset: 0x000002EE
        public InjectTypeInfo(Type type, InjectTypeInfo.InjectConstructorInfo injectConstructor, InjectTypeInfo.InjectMethodInfo[] injectMethods, InjectTypeInfo.InjectMemberInfo[] injectMembers)
        {
            this.Type = type;
            this.InjectMethods = injectMethods;
            this.InjectMembers = injectMembers;
            this.InjectConstructor = injectConstructor;
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600001C RID: 28 RVA: 0x00002113 File Offset: 0x00000313
        // (set) Token: 0x0600001D RID: 29 RVA: 0x0000211B File Offset: 0x0000031B
        public InjectTypeInfo BaseTypeInfo { get; set; }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600001E RID: 30 RVA: 0x00002134 File Offset: 0x00000334
        public IEnumerable<InjectableInfo> AllInjectables
        {
            get
            {
                return this.InjectConstructor.Parameters.Concat(from x in this.InjectMembers
                                                                select x.Info).Concat(this.InjectMethods.SelectMany((InjectTypeInfo.InjectMethodInfo x) => x.Parameters));
            }
        }

        // Token: 0x0400000F RID: 15
        public readonly Type Type;

        // Token: 0x04000010 RID: 16
        public readonly InjectTypeInfo.InjectMethodInfo[] InjectMethods;

        // Token: 0x04000011 RID: 17
        public readonly InjectTypeInfo.InjectMemberInfo[] InjectMembers;

        // Token: 0x04000012 RID: 18
        public readonly InjectTypeInfo.InjectConstructorInfo InjectConstructor;

        // Token: 0x0200000F RID: 15
        [NoReflectionBaking]
        public class InjectMemberInfo
        {
            // Token: 0x06000021 RID: 33 RVA: 0x000021A6 File Offset: 0x000003A6
            public InjectMemberInfo(ZenMemberSetterMethod setter, InjectableInfo info)
            {
                this.Setter = setter;
                this.Info = info;
            }

            // Token: 0x04000016 RID: 22
            public readonly ZenMemberSetterMethod Setter;

            // Token: 0x04000017 RID: 23
            public readonly InjectableInfo Info;
        }

        // Token: 0x02000010 RID: 16
        [NoReflectionBaking]
        public class InjectConstructorInfo
        {
            // Token: 0x06000022 RID: 34 RVA: 0x000021BC File Offset: 0x000003BC
            public InjectConstructorInfo(ZenFactoryMethod factory, InjectableInfo[] parameters)
            {
                this.Parameters = parameters;
                this.Factory = factory;
            }

            // Token: 0x04000018 RID: 24
            public readonly ZenFactoryMethod Factory;

            // Token: 0x04000019 RID: 25
            public readonly InjectableInfo[] Parameters;
        }

        // Token: 0x02000011 RID: 17
        [NoReflectionBaking]
        public class InjectMethodInfo
        {
            // Token: 0x06000023 RID: 35 RVA: 0x000021D2 File Offset: 0x000003D2
            public InjectMethodInfo(ZenInjectMethod action, InjectableInfo[] parameters, string name)
            {
                this.Parameters = parameters;
                this.Action = action;
                this.Name = name;
            }

            // Token: 0x0400001A RID: 26
            public readonly string Name;

            // Token: 0x0400001B RID: 27
            public readonly ZenInjectMethod Action;

            // Token: 0x0400001C RID: 28
            public readonly InjectableInfo[] Parameters;
        }
    }
}
