using System;
using System.Collections.Generic;
using System.Diagnostics;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x02000048 RID: 72
    [NoReflectionBaking]
    public class BindInfo : IDisposable
    {
        // Token: 0x060001DF RID: 479 RVA: 0x000069A0 File Offset: 0x00004BA0
        public BindInfo()
        {
            this.ContractTypes = new List<Type>();
            this.ToTypes = new List<Type>();
            this.Arguments = new List<TypeValuePair>();
            this.Reset();
        }

        // Token: 0x060001E0 RID: 480 RVA: 0x000069D0 File Offset: 0x00004BD0
        public void Dispose()
        {
            Zenject.Internal.ZenPools.DespawnBindInfo(this);
        }

        // Token: 0x060001E1 RID: 481 RVA: 0x000069D8 File Offset: 0x00004BD8
        [Conditional("UNITY_EDITOR")]
        public void SetContextInfo(string contextInfo)
        {
            this.ContextInfo = contextInfo;
        }

        // Token: 0x060001E2 RID: 482 RVA: 0x000069E4 File Offset: 0x00004BE4
        public void Reset()
        {
            this.MarkAsCreationBinding = true;
            this.MarkAsUniqueSingleton = false;
            this.ConcreteIdentifier = null;
            this.SaveProvider = false;
            this.OnlyBindIfNotBound = false;
            this.RequireExplicitScope = false;
            this.Identifier = null;
            this.ContractTypes.Clear();
            this.BindingInheritanceMethod = BindingInheritanceMethods.None;
            this.InvalidBindResponse = InvalidBindResponses.Assert;
            this.NonLazy = false;
            this.Condition = null;
            this.ToChoice = ToChoices.Self;
            this.ContextInfo = null;
            this.ToTypes.Clear();
            this.Scope = ScopeTypes.Unset;
            this.Arguments.Clear();
            this.InstantiatedCallback = null;
        }

        // Token: 0x040000A6 RID: 166
        public bool MarkAsCreationBinding;

        // Token: 0x040000A7 RID: 167
        public bool MarkAsUniqueSingleton;

        // Token: 0x040000A8 RID: 168
        public object ConcreteIdentifier;

        // Token: 0x040000A9 RID: 169
        public bool SaveProvider;

        // Token: 0x040000AA RID: 170
        public bool OnlyBindIfNotBound;

        // Token: 0x040000AB RID: 171
        public bool RequireExplicitScope;

        // Token: 0x040000AC RID: 172
        public object Identifier;

        // Token: 0x040000AD RID: 173
        public readonly List<Type> ContractTypes;

        // Token: 0x040000AE RID: 174
        public BindingInheritanceMethods BindingInheritanceMethod;

        // Token: 0x040000AF RID: 175
        public InvalidBindResponses InvalidBindResponse;

        // Token: 0x040000B0 RID: 176
        public bool NonLazy;

        // Token: 0x040000B1 RID: 177
        public BindingCondition Condition;

        // Token: 0x040000B2 RID: 178
        public ToChoices ToChoice;

        // Token: 0x040000B3 RID: 179
        public string ContextInfo;

        // Token: 0x040000B4 RID: 180
        public readonly List<Type> ToTypes;

        // Token: 0x040000B5 RID: 181
        public ScopeTypes Scope;

        // Token: 0x040000B6 RID: 182
        public readonly List<TypeValuePair> Arguments;

        // Token: 0x040000B7 RID: 183
        public Action<InjectContext, object> InstantiatedCallback;
    }
}
