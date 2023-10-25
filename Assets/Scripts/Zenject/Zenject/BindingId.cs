using System;
using System.Diagnostics;
using ModestTree;

namespace Zenject
{
    // Token: 0x0200022D RID: 557
    [DebuggerStepThrough]
    public struct BindingId : IEquatable<BindingId>
    {
        // Token: 0x06000BE7 RID: 3047 RVA: 0x0001FA74 File Offset: 0x0001DC74
        public BindingId(Type type, object identifier)
        {
            this._type = type;
            this._identifier = identifier;
        }

        // Token: 0x170000DA RID: 218
        // (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0001FA84 File Offset: 0x0001DC84
        // (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0001FA8C File Offset: 0x0001DC8C
        public Type Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        // Token: 0x170000DB RID: 219
        // (get) Token: 0x06000BEA RID: 3050 RVA: 0x0001FA98 File Offset: 0x0001DC98
        // (set) Token: 0x06000BEB RID: 3051 RVA: 0x0001FAA0 File Offset: 0x0001DCA0
        public object Identifier
        {
            get
            {
                return this._identifier;
            }
            set
            {
                this._identifier = value;
            }
        }

        // Token: 0x06000BEC RID: 3052 RVA: 0x0001FAAC File Offset: 0x0001DCAC
        public override string ToString()
        {
            if (this._identifier == null)
            {
                return this._type.PrettyName();
            }
            return "{0} (ID: {1})".Fmt(new object[]
            {
                this._type,
                this._identifier
            });
        }

        // Token: 0x06000BED RID: 3053 RVA: 0x0001FAE4 File Offset: 0x0001DCE4
        public override int GetHashCode()
        {
            return (17 * 29 + this._type.GetHashCode()) * 29 + ((this._identifier == null) ? 0 : this._identifier.GetHashCode());
        }

        // Token: 0x06000BEE RID: 3054 RVA: 0x0001FB14 File Offset: 0x0001DD14
        public override bool Equals(object other)
        {
            return other is BindingId && (BindingId)other == this;
        }

        // Token: 0x06000BEF RID: 3055 RVA: 0x0001FB34 File Offset: 0x0001DD34
        public bool Equals(BindingId that)
        {
            return this == that;
        }

        // Token: 0x06000BF0 RID: 3056 RVA: 0x0001FB44 File Offset: 0x0001DD44
        public static bool operator ==(BindingId left, BindingId right)
        {
            return left.Type == right.Type && object.Equals(left.Identifier, right.Identifier);
        }

        // Token: 0x06000BF1 RID: 3057 RVA: 0x0001FB70 File Offset: 0x0001DD70
        public static bool operator !=(BindingId left, BindingId right)
        {
            return !left.Equals(right);
        }

        // Token: 0x04000387 RID: 903
        private Type _type;

        // Token: 0x04000388 RID: 904
        private object _identifier;
    }
}
