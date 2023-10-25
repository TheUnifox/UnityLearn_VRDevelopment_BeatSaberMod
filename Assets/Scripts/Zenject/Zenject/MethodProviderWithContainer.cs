using System;
using System.Collections.Generic;
using ModestTree;
using static UnityEditor.Rendering.FilterWindow;

namespace Zenject
{
    // Token: 0x02000262 RID: 610
    [NoReflectionBaking]
    public class MethodProviderWithContainer<TValue> : IProvider
    {
        // Token: 0x06000DDE RID: 3550 RVA: 0x00025818 File Offset: 0x00023A18
        public MethodProviderWithContainer(Func<DiContainer, TValue> method)
        {
            this._method = method;
        }

        // Token: 0x17000129 RID: 297
        // (get) Token: 0x06000DDF RID: 3551 RVA: 0x00025828 File Offset: 0x00023A28
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700012A RID: 298
        // (get) Token: 0x06000DE0 RID: 3552 RVA: 0x0002582C File Offset: 0x00023A2C
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DE1 RID: 3553 RVA: 0x00025830 File Offset: 0x00023A30
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        // Token: 0x06000DE2 RID: 3554 RVA: 0x0002583C File Offset: 0x00023A3C
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEmpty<TypeValuePair>(args);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            injectAction = null;
            if (context.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TValue)));
                return;
            }
            buffer.Add(this._method(context.Container));
        }

        // Token: 0x04000413 RID: 1043
        private readonly Func<DiContainer, TValue> _method;
    }

    [NoReflectionBaking]
    public class MethodProviderWithContainer<TParam1, TValue> : IProvider
    {
        // Token: 0x06000DE3 RID: 3555 RVA: 0x000258B4 File Offset: 0x00023AB4
        public MethodProviderWithContainer(Func<DiContainer, TParam1, TValue> method)
        {
            this._method = method;
        }

        // Token: 0x1700012B RID: 299
        // (get) Token: 0x06000DE4 RID: 3556 RVA: 0x000258C4 File Offset: 0x00023AC4
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700012C RID: 300
        // (get) Token: 0x06000DE5 RID: 3557 RVA: 0x000258C8 File Offset: 0x00023AC8
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DE6 RID: 3558 RVA: 0x000258CC File Offset: 0x00023ACC
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        // Token: 0x06000DE7 RID: 3559 RVA: 0x000258D8 File Offset: 0x00023AD8
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 1);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual(typeof(TParam1)));
            injectAction = null;
            if (context.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TValue)));
                return;
            }
            buffer.Add(this._method(context.Container, (TParam1)((object)args[0].Value)));
        }

        // Token: 0x04000414 RID: 1044
        private readonly Func<DiContainer, TParam1, TValue> _method;
    }

    [NoReflectionBaking]
    public class MethodProviderWithContainer<TParam1, TParam2, TValue> : IProvider
    {
        // Token: 0x06000DE8 RID: 3560 RVA: 0x00025990 File Offset: 0x00023B90
        public MethodProviderWithContainer(Func<DiContainer, TParam1, TParam2, TValue> method)
        {
            this._method = method;
        }

        // Token: 0x1700012D RID: 301
        // (get) Token: 0x06000DE9 RID: 3561 RVA: 0x000259A0 File Offset: 0x00023BA0
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700012E RID: 302
        // (get) Token: 0x06000DEA RID: 3562 RVA: 0x000259A4 File Offset: 0x00023BA4
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DEB RID: 3563 RVA: 0x000259A8 File Offset: 0x00023BA8
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        // Token: 0x06000DEC RID: 3564 RVA: 0x000259B4 File Offset: 0x00023BB4
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 2);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual(typeof(TParam1)));
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual(typeof(TParam2)));
            injectAction = null;
            if (context.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TValue)));
                return;
            }
            buffer.Add(this._method(context.Container, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value)));
        }

        // Token: 0x04000415 RID: 1045
        private readonly Func<DiContainer, TParam1, TParam2, TValue> _method;
    }

    [NoReflectionBaking]
    public class MethodProviderWithContainer<TParam1, TParam2, TParam3, TValue> : IProvider
    {
        // Token: 0x06000DED RID: 3565 RVA: 0x00025AA0 File Offset: 0x00023CA0
        public MethodProviderWithContainer(Func<DiContainer, TParam1, TParam2, TParam3, TValue> method)
        {
            this._method = method;
        }

        // Token: 0x1700012F RID: 303
        // (get) Token: 0x06000DEE RID: 3566 RVA: 0x00025AB0 File Offset: 0x00023CB0
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000130 RID: 304
        // (get) Token: 0x06000DEF RID: 3567 RVA: 0x00025AB4 File Offset: 0x00023CB4
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DF0 RID: 3568 RVA: 0x00025AB8 File Offset: 0x00023CB8
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        // Token: 0x06000DF1 RID: 3569 RVA: 0x00025AC4 File Offset: 0x00023CC4
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 3);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual(typeof(TParam1)));
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual(typeof(TParam2)));
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual(typeof(TParam3)));
            injectAction = null;
            if (context.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TValue)));
                return;
            }
            buffer.Add(this._method(context.Container, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value)));
        }

        // Token: 0x04000416 RID: 1046
        private readonly Func<DiContainer, TParam1, TParam2, TParam3, TValue> _method;
    }

    [NoReflectionBaking]
    public class MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TValue> : IProvider
    {
        // Token: 0x06000DF2 RID: 3570 RVA: 0x00025BE0 File Offset: 0x00023DE0
        public MethodProviderWithContainer(Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TValue> method)
        {
            this._method = method;
        }

        // Token: 0x17000131 RID: 305
        // (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00025BF0 File Offset: 0x00023DF0
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000132 RID: 306
        // (get) Token: 0x06000DF4 RID: 3572 RVA: 0x00025BF4 File Offset: 0x00023DF4
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DF5 RID: 3573 RVA: 0x00025BF8 File Offset: 0x00023DF8
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        // Token: 0x06000DF6 RID: 3574 RVA: 0x00025C04 File Offset: 0x00023E04
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 4);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual(typeof(TParam1)));
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual(typeof(TParam2)));
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual(typeof(TParam3)));
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual(typeof(TParam4)));
            injectAction = null;
            if (context.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TValue)));
                return;
            }
            buffer.Add(this._method(context.Container, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value)));
        }

        // Token: 0x04000417 RID: 1047
        private readonly Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TValue> _method;
    }

    [NoReflectionBaking]
    public class MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : IProvider
    {
        // Token: 0x06000DF7 RID: 3575 RVA: 0x00025D50 File Offset: 0x00023F50
        public MethodProviderWithContainer(Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TValue> method)
        {
            this._method = method;
        }

        // Token: 0x17000133 RID: 307
        // (get) Token: 0x06000DF8 RID: 3576 RVA: 0x00025D60 File Offset: 0x00023F60
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000134 RID: 308
        // (get) Token: 0x06000DF9 RID: 3577 RVA: 0x00025D64 File Offset: 0x00023F64
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DFA RID: 3578 RVA: 0x00025D68 File Offset: 0x00023F68
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        // Token: 0x06000DFB RID: 3579 RVA: 0x00025D74 File Offset: 0x00023F74
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 5);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual(typeof(TParam1)));
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual(typeof(TParam2)));
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual(typeof(TParam3)));
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual(typeof(TParam4)));
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual(typeof(TParam5)));
            injectAction = null;
            if (context.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TValue)));
                return;
            }
            buffer.Add(this._method(context.Container, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value)));
        }

        // Token: 0x04000418 RID: 1048
        private readonly Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TValue> _method;
    }

    [NoReflectionBaking]
    public class MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : IProvider
    {
        // Token: 0x06000DFC RID: 3580 RVA: 0x00025EF0 File Offset: 0x000240F0
        public MethodProviderWithContainer(Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> method)
        {
            this._method = method;
        }

        // Token: 0x17000135 RID: 309
        // (get) Token: 0x06000DFD RID: 3581 RVA: 0x00025F00 File Offset: 0x00024100
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000136 RID: 310
        // (get) Token: 0x06000DFE RID: 3582 RVA: 0x00025F04 File Offset: 0x00024104
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000DFF RID: 3583 RVA: 0x00025F08 File Offset: 0x00024108
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        // Token: 0x06000E00 RID: 3584 RVA: 0x00025F14 File Offset: 0x00024114
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 5);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual(typeof(TParam1)));
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual(typeof(TParam2)));
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual(typeof(TParam3)));
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual(typeof(TParam4)));
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual(typeof(TParam5)));
            ModestTree.Assert.That(args[5].Type.DerivesFromOrEqual(typeof(TParam6)));
            injectAction = null;
            if (context.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TValue)));
                return;
            }
            buffer.Add(this._method(context.Container, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), (TParam6)((object)args[5].Value)));
        }

        // Token: 0x04000419 RID: 1049
        private readonly Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> _method;
    }

    [NoReflectionBaking]
    public class MethodProviderWithContainer<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue> : IProvider
    {
        // Token: 0x06000E01 RID: 3585 RVA: 0x000260C4 File Offset: 0x000242C4
        public MethodProviderWithContainer(Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue> method)
        {
            this._method = method;
        }

        // Token: 0x17000137 RID: 311
        // (get) Token: 0x06000E02 RID: 3586 RVA: 0x000260D4 File Offset: 0x000242D4
        public bool IsCached
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000138 RID: 312
        // (get) Token: 0x06000E03 RID: 3587 RVA: 0x000260D8 File Offset: 0x000242D8
        public bool TypeVariesBasedOnMemberType
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000E04 RID: 3588 RVA: 0x000260DC File Offset: 0x000242DC
        public Type GetInstanceType(InjectContext context)
        {
            return typeof(TValue);
        }

        // Token: 0x06000E05 RID: 3589 RVA: 0x000260E8 File Offset: 0x000242E8
        public void GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            ModestTree.Assert.IsEqual(args.Count, 10);
            ModestTree.Assert.IsNotNull(context);
            ModestTree.Assert.That(typeof(TValue).DerivesFromOrEqual(context.MemberType));
            ModestTree.Assert.That(args[0].Type.DerivesFromOrEqual(typeof(TParam1)));
            ModestTree.Assert.That(args[1].Type.DerivesFromOrEqual(typeof(TParam2)));
            ModestTree.Assert.That(args[2].Type.DerivesFromOrEqual(typeof(TParam3)));
            ModestTree.Assert.That(args[3].Type.DerivesFromOrEqual(typeof(TParam4)));
            ModestTree.Assert.That(args[4].Type.DerivesFromOrEqual(typeof(TParam5)));
            ModestTree.Assert.That(args[5].Type.DerivesFromOrEqual(typeof(TParam6)));
            ModestTree.Assert.That(args[6].Type.DerivesFromOrEqual(typeof(TParam7)));
            ModestTree.Assert.That(args[7].Type.DerivesFromOrEqual(typeof(TParam8)));
            ModestTree.Assert.That(args[8].Type.DerivesFromOrEqual(typeof(TParam9)));
            ModestTree.Assert.That(args[9].Type.DerivesFromOrEqual(typeof(TParam10)));
            injectAction = null;
            if (context.Container.IsValidating)
            {
                buffer.Add(new ValidationMarker(typeof(TValue)));
                return;
            }
            buffer.Add(this._method(context.Container, (TParam1)((object)args[0].Value), (TParam2)((object)args[1].Value), (TParam3)((object)args[2].Value), (TParam4)((object)args[3].Value), (TParam5)((object)args[4].Value), (TParam6)((object)args[5].Value), (TParam7)((object)args[6].Value), (TParam8)((object)args[7].Value), (TParam9)((object)args[8].Value), (TParam10)((object)args[9].Value)));
        }

        // Token: 0x0400041A RID: 1050
        private readonly Func<DiContainer, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TValue> _method;
    }


}