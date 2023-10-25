using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020001FD RID: 509
    [NoReflectionBaking]
    public class InjectContext : IDisposable
    {
        // Token: 0x06000A88 RID: 2696 RVA: 0x0001BCA0 File Offset: 0x00019EA0
        public InjectContext()
        {
            this._bindingId = default(BindingId);
            this.Reset();
        }

        // Token: 0x06000A89 RID: 2697 RVA: 0x0001BCBC File Offset: 0x00019EBC
        public InjectContext(DiContainer container, Type memberType) : this()
        {
            this.Container = container;
            this.MemberType = memberType;
        }

        // Token: 0x06000A8A RID: 2698 RVA: 0x0001BCD4 File Offset: 0x00019ED4
        public InjectContext(DiContainer container, Type memberType, object identifier) : this(container, memberType)
        {
            this.Identifier = identifier;
        }

        // Token: 0x06000A8B RID: 2699 RVA: 0x0001BCE8 File Offset: 0x00019EE8
        public InjectContext(DiContainer container, Type memberType, object identifier, bool optional) : this(container, memberType, identifier)
        {
            this.Optional = optional;
        }

        // Token: 0x06000A8C RID: 2700 RVA: 0x0001BCFC File Offset: 0x00019EFC
        public void Dispose()
        {
            Zenject.Internal.ZenPools.DespawnInjectContext(this);
        }

        // Token: 0x06000A8D RID: 2701 RVA: 0x0001BD04 File Offset: 0x00019F04
        public void Reset()
        {
            this._objectType = null;
            this._parentContext = null;
            this._objectInstance = null;
            this._memberName = "";
            this._optional = false;
            this._sourceType = InjectSources.Any;
            this._fallBackValue = null;
            this._container = null;
            this._bindingId.Type = null;
            this._bindingId.Identifier = null;
        }

        // Token: 0x1700009C RID: 156
        // (get) Token: 0x06000A8E RID: 2702 RVA: 0x0001BD68 File Offset: 0x00019F68
        public BindingId BindingId
        {
            get
            {
                return this._bindingId;
            }
        }

        // Token: 0x1700009D RID: 157
        // (get) Token: 0x06000A8F RID: 2703 RVA: 0x0001BD70 File Offset: 0x00019F70
        // (set) Token: 0x06000A90 RID: 2704 RVA: 0x0001BD78 File Offset: 0x00019F78
        public Type ObjectType
        {
            get
            {
                return this._objectType;
            }
            set
            {
                this._objectType = value;
            }
        }

        // Token: 0x1700009E RID: 158
        // (get) Token: 0x06000A91 RID: 2705 RVA: 0x0001BD84 File Offset: 0x00019F84
        // (set) Token: 0x06000A92 RID: 2706 RVA: 0x0001BD8C File Offset: 0x00019F8C
        public InjectContext ParentContext
        {
            get
            {
                return this._parentContext;
            }
            set
            {
                this._parentContext = value;
            }
        }

        // Token: 0x1700009F RID: 159
        // (get) Token: 0x06000A93 RID: 2707 RVA: 0x0001BD98 File Offset: 0x00019F98
        // (set) Token: 0x06000A94 RID: 2708 RVA: 0x0001BDA0 File Offset: 0x00019FA0
        public object ObjectInstance
        {
            get
            {
                return this._objectInstance;
            }
            set
            {
                this._objectInstance = value;
            }
        }

        // Token: 0x170000A0 RID: 160
        // (get) Token: 0x06000A95 RID: 2709 RVA: 0x0001BDAC File Offset: 0x00019FAC
        // (set) Token: 0x06000A96 RID: 2710 RVA: 0x0001BDBC File Offset: 0x00019FBC
        public object Identifier
        {
            get
            {
                return this._bindingId.Identifier;
            }
            set
            {
                this._bindingId.Identifier = value;
            }
        }

        // Token: 0x170000A1 RID: 161
        // (get) Token: 0x06000A97 RID: 2711 RVA: 0x0001BDCC File Offset: 0x00019FCC
        // (set) Token: 0x06000A98 RID: 2712 RVA: 0x0001BDD4 File Offset: 0x00019FD4
        public string MemberName
        {
            get
            {
                return this._memberName;
            }
            set
            {
                this._memberName = value;
            }
        }

        // Token: 0x170000A2 RID: 162
        // (get) Token: 0x06000A99 RID: 2713 RVA: 0x0001BDE0 File Offset: 0x00019FE0
        // (set) Token: 0x06000A9A RID: 2714 RVA: 0x0001BDF0 File Offset: 0x00019FF0
        public Type MemberType
        {
            get
            {
                return this._bindingId.Type;
            }
            set
            {
                this._bindingId.Type = value;
            }
        }

        // Token: 0x170000A3 RID: 163
        // (get) Token: 0x06000A9B RID: 2715 RVA: 0x0001BE00 File Offset: 0x0001A000
        // (set) Token: 0x06000A9C RID: 2716 RVA: 0x0001BE08 File Offset: 0x0001A008
        public bool Optional
        {
            get
            {
                return this._optional;
            }
            set
            {
                this._optional = value;
            }
        }

        // Token: 0x170000A4 RID: 164
        // (get) Token: 0x06000A9D RID: 2717 RVA: 0x0001BE14 File Offset: 0x0001A014
        // (set) Token: 0x06000A9E RID: 2718 RVA: 0x0001BE1C File Offset: 0x0001A01C
        public InjectSources SourceType
        {
            get
            {
                return this._sourceType;
            }
            set
            {
                this._sourceType = value;
            }
        }

        // Token: 0x170000A5 RID: 165
        // (get) Token: 0x06000A9F RID: 2719 RVA: 0x0001BE28 File Offset: 0x0001A028
        // (set) Token: 0x06000AA0 RID: 2720 RVA: 0x0001BE30 File Offset: 0x0001A030
        public object ConcreteIdentifier
        {
            get
            {
                return this._concreteIdentifier;
            }
            set
            {
                this._concreteIdentifier = value;
            }
        }

        // Token: 0x170000A6 RID: 166
        // (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0001BE3C File Offset: 0x0001A03C
        // (set) Token: 0x06000AA2 RID: 2722 RVA: 0x0001BE44 File Offset: 0x0001A044
        public object FallBackValue
        {
            get
            {
                return this._fallBackValue;
            }
            set
            {
                this._fallBackValue = value;
            }
        }

        // Token: 0x170000A7 RID: 167
        // (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0001BE50 File Offset: 0x0001A050
        // (set) Token: 0x06000AA4 RID: 2724 RVA: 0x0001BE58 File Offset: 0x0001A058
        public DiContainer Container
        {
            get
            {
                return this._container;
            }
            set
            {
                this._container = value;
            }
        }

        // Token: 0x170000A8 RID: 168
        // (get) Token: 0x06000AA5 RID: 2725 RVA: 0x0001BE64 File Offset: 0x0001A064
        public IEnumerable<InjectContext> ParentContexts
        {
            get
            {
                if (this.ParentContext == null)
                {
                    yield break;
                }
                yield return this.ParentContext;
                foreach (InjectContext injectContext in this.ParentContext.ParentContexts)
                {
                    yield return injectContext;
                }
                IEnumerator<InjectContext> enumerator = null;
                yield break;
                yield break;
            }
        }

        // Token: 0x170000A9 RID: 169
        // (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0001BE74 File Offset: 0x0001A074
        public IEnumerable<InjectContext> ParentContextsAndSelf
        {
            get
            {
                yield return this;
                foreach (InjectContext injectContext in this.ParentContexts)
                {
                    yield return injectContext;
                }
                IEnumerator<InjectContext> enumerator = null;
                yield break;
                yield break;
            }
        }

        // Token: 0x170000AA RID: 170
        // (get) Token: 0x06000AA7 RID: 2727 RVA: 0x0001BE84 File Offset: 0x0001A084
        public IEnumerable<Type> AllObjectTypes
        {
            get
            {
                foreach (InjectContext injectContext in this.ParentContextsAndSelf)
                {
                    if (injectContext.ObjectType != null)
                    {
                        yield return injectContext.ObjectType;
                    }
                }
                IEnumerator<InjectContext> enumerator = null;
                yield break;
                yield break;
            }
        }

        // Token: 0x06000AA8 RID: 2728 RVA: 0x0001BE94 File Offset: 0x0001A094
        public InjectContext CreateSubContext(Type memberType)
        {
            return this.CreateSubContext(memberType, null);
        }

        // Token: 0x06000AA9 RID: 2729 RVA: 0x0001BEA0 File Offset: 0x0001A0A0
        public InjectContext CreateSubContext(Type memberType, object identifier)
        {
            return new InjectContext
            {
                ParentContext = this,
                Identifier = identifier,
                MemberType = memberType,
                ConcreteIdentifier = null,
                MemberName = "",
                FallBackValue = null,
                ObjectType = this.ObjectType,
                ObjectInstance = this.ObjectInstance,
                Optional = this.Optional,
                SourceType = this.SourceType,
                Container = this.Container
            };
        }

        // Token: 0x06000AAA RID: 2730 RVA: 0x0001BF1C File Offset: 0x0001A11C
        public InjectContext Clone()
        {
            return new InjectContext
            {
                ObjectType = this.ObjectType,
                ParentContext = this.ParentContext,
                ConcreteIdentifier = this.ConcreteIdentifier,
                ObjectInstance = this.ObjectInstance,
                Identifier = this.Identifier,
                MemberType = this.MemberType,
                MemberName = this.MemberName,
                Optional = this.Optional,
                SourceType = this.SourceType,
                FallBackValue = this.FallBackValue,
                Container = this.Container
            };
        }

        // Token: 0x06000AAB RID: 2731 RVA: 0x0001BFB4 File Offset: 0x0001A1B4
        public string GetObjectGraphString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (InjectContext injectContext in this.ParentContextsAndSelf.Reverse<InjectContext>())
            {
                if (!(injectContext.ObjectType == null))
                {
                    stringBuilder.AppendLine(injectContext.ObjectType.PrettyName());
                }
            }
            return stringBuilder.ToString();
        }

        // Token: 0x04000317 RID: 791
        private BindingId _bindingId;

        // Token: 0x04000318 RID: 792
        private Type _objectType;

        // Token: 0x04000319 RID: 793
        private InjectContext _parentContext;

        // Token: 0x0400031A RID: 794
        private object _objectInstance;

        // Token: 0x0400031B RID: 795
        private string _memberName;

        // Token: 0x0400031C RID: 796
        private bool _optional;

        // Token: 0x0400031D RID: 797
        private InjectSources _sourceType;

        // Token: 0x0400031E RID: 798
        private object _fallBackValue;

        // Token: 0x0400031F RID: 799
        private object _concreteIdentifier;

        // Token: 0x04000320 RID: 800
        private DiContainer _container;
    }
}
