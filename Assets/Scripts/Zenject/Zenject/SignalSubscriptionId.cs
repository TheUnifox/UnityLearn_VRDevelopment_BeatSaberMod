using System;
using System.Diagnostics;

namespace Zenject
{
    // Token: 0x0200003D RID: 61
    [DebuggerStepThrough]
    public struct SignalSubscriptionId : IEquatable<SignalSubscriptionId>
    {
        // Token: 0x0600018E RID: 398 RVA: 0x00005D04 File Offset: 0x00003F04
        public SignalSubscriptionId(BindingId signalId, object callback)
        {
            this._signalId = signalId;
            this._callback = callback;
        }

        // Token: 0x17000027 RID: 39
        // (get) Token: 0x0600018F RID: 399 RVA: 0x00005D14 File Offset: 0x00003F14
        public BindingId SignalId
        {
            get
            {
                return this._signalId;
            }
        }

        // Token: 0x17000028 RID: 40
        // (get) Token: 0x06000190 RID: 400 RVA: 0x00005D1C File Offset: 0x00003F1C
        public object Callback
        {
            get
            {
                return this._callback;
            }
        }

        // Token: 0x06000191 RID: 401 RVA: 0x00005D24 File Offset: 0x00003F24
        public override int GetHashCode()
        {
            return (17 * 29 + this._signalId.GetHashCode()) * 29 + this._callback.GetHashCode();
        }

        // Token: 0x06000192 RID: 402 RVA: 0x00005D4C File Offset: 0x00003F4C
        public override bool Equals(object that)
        {
            return that is SignalSubscriptionId && this.Equals((SignalSubscriptionId)that);
        }

        // Token: 0x06000193 RID: 403 RVA: 0x00005D64 File Offset: 0x00003F64
        public bool Equals(SignalSubscriptionId that)
        {
            return object.Equals(this._signalId, that._signalId) && object.Equals(this.Callback, that.Callback);
        }

        // Token: 0x06000194 RID: 404 RVA: 0x00005D98 File Offset: 0x00003F98
        public static bool operator ==(SignalSubscriptionId left, SignalSubscriptionId right)
        {
            return left.Equals(right);
        }

        // Token: 0x06000195 RID: 405 RVA: 0x00005DA4 File Offset: 0x00003FA4
        public static bool operator !=(SignalSubscriptionId left, SignalSubscriptionId right)
        {
            return !left.Equals(right);
        }

        // Token: 0x04000085 RID: 133
        private BindingId _signalId;

        // Token: 0x04000086 RID: 134
        private object _callback;
    }
}
