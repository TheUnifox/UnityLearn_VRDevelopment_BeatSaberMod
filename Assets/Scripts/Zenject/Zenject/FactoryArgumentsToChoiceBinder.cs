using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenject
{
    // Token: 0x02000070 RID: 112
    [NoReflectionBaking]
    public class FactoryArgumentsToChoiceBinder<TContract> : FactoryToChoiceBinder<TContract>
    {
        // Token: 0x060002CB RID: 715 RVA: 0x000088C0 File Offset: 0x00006AC0
        public FactoryArgumentsToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060002CC RID: 716 RVA: 0x000088CC File Offset: 0x00006ACC
        public FactoryToChoiceBinder<TContract> WithFactoryArguments<T>(T param)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<T>(param);
            return this;
        }

        // Token: 0x060002CD RID: 717 RVA: 0x000088E0 File Offset: 0x00006AE0
        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2>(TParam1 param1, TParam2 param2)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TParam1, TParam2>(param1, param2);
            return this;
        }

        // Token: 0x060002CE RID: 718 RVA: 0x000088F8 File Offset: 0x00006AF8
        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3>(param1, param2, param3);
            return this;
        }

        // Token: 0x060002CF RID: 719 RVA: 0x00008910 File Offset: 0x00006B10
        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2, TParam3, TParam4>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4>(param1, param2, param3, param4);
            return this;
        }

        // Token: 0x060002D0 RID: 720 RVA: 0x00008928 File Offset: 0x00006B28
        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2, TParam3, TParam4, TParam5>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4, TParam5>(param1, param2, param3, param4, param5);
            return this;
        }

        // Token: 0x060002D1 RID: 721 RVA: 0x00008944 File Offset: 0x00006B44
        public FactoryToChoiceBinder<TContract> WithFactoryArguments<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(param1, param2, param3, param4, param5, param6);
            return this;
        }

        // Token: 0x060002D2 RID: 722 RVA: 0x00008960 File Offset: 0x00006B60
        public FactoryToChoiceBinder<TContract> WithFactoryArguments(object[] args)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        // Token: 0x060002D3 RID: 723 RVA: 0x00008974 File Offset: 0x00006B74
        public FactoryToChoiceBinder<TContract> WithFactoryArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            base.FactoryBindInfo.Arguments = extraArgs.ToList<TypeValuePair>();
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryArgumentsToChoiceBinder<TParam1, TContract> : FactoryToChoiceBinder<TParam1, TContract>
    {
        // Token: 0x060002D4 RID: 724 RVA: 0x00008988 File Offset: 0x00006B88
        public FactoryArgumentsToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060002D5 RID: 725 RVA: 0x00008994 File Offset: 0x00006B94
        public FactoryToChoiceBinder<TParam1, TContract> WithFactoryArguments<T>(T param)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<T>(param);
            return this;
        }

        // Token: 0x060002D6 RID: 726 RVA: 0x000089A8 File Offset: 0x00006BA8
        public FactoryToChoiceBinder<TParam1, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2>(TFactoryParam1 param1, TFactoryParam2 param2)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2>(param1, param2);
            return this;
        }

        // Token: 0x060002D7 RID: 727 RVA: 0x000089C0 File Offset: 0x00006BC0
        public FactoryToChoiceBinder<TParam1, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3>(param1, param2, param3);
            return this;
        }

        // Token: 0x060002D8 RID: 728 RVA: 0x000089D8 File Offset: 0x00006BD8
        public FactoryToChoiceBinder<TParam1, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(param1, param2, param3, param4);
            return this;
        }

        // Token: 0x060002D9 RID: 729 RVA: 0x000089F0 File Offset: 0x00006BF0
        public FactoryToChoiceBinder<TParam1, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(param1, param2, param3, param4, param5);
            return this;
        }

        // Token: 0x060002DA RID: 730 RVA: 0x00008A0C File Offset: 0x00006C0C
        public FactoryToChoiceBinder<TParam1, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5, TFactoryParam6 param6)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(param1, param2, param3, param4, param5, param6);
            return this;
        }

        // Token: 0x060002DB RID: 731 RVA: 0x00008A28 File Offset: 0x00006C28
        public FactoryToChoiceBinder<TParam1, TContract> WithFactoryArguments(object[] args)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        // Token: 0x060002DC RID: 732 RVA: 0x00008A3C File Offset: 0x00006C3C
        public FactoryToChoiceBinder<TParam1, TContract> WithFactoryArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            base.FactoryBindInfo.Arguments = extraArgs.ToList<TypeValuePair>();
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryArgumentsToChoiceBinder<TParam1, TParam2, TContract> : FactoryToChoiceBinder<TParam1, TParam2, TContract>
    {
        // Token: 0x060002E6 RID: 742 RVA: 0x00008B18 File Offset: 0x00006D18
        public FactoryArgumentsToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060002E7 RID: 743 RVA: 0x00008B24 File Offset: 0x00006D24
        public FactoryToChoiceBinder<TParam1, TParam2, TContract> WithFactoryArguments<T>(T param)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<T>(param);
            return this;
        }

        // Token: 0x060002E8 RID: 744 RVA: 0x00008B38 File Offset: 0x00006D38
        public FactoryToChoiceBinder<TParam1, TParam2, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2>(TFactoryParam1 param1, TFactoryParam2 param2)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2>(param1, param2);
            return this;
        }

        // Token: 0x060002E9 RID: 745 RVA: 0x00008B50 File Offset: 0x00006D50
        public FactoryToChoiceBinder<TParam1, TParam2, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3>(param1, param2, param3);
            return this;
        }

        // Token: 0x060002EA RID: 746 RVA: 0x00008B68 File Offset: 0x00006D68
        public FactoryToChoiceBinder<TParam1, TParam2, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(param1, param2, param3, param4);
            return this;
        }

        // Token: 0x060002EB RID: 747 RVA: 0x00008B80 File Offset: 0x00006D80
        public FactoryToChoiceBinder<TParam1, TParam2, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(param1, param2, param3, param4, param5);
            return this;
        }

        // Token: 0x060002EC RID: 748 RVA: 0x00008B9C File Offset: 0x00006D9C
        public FactoryToChoiceBinder<TParam1, TParam2, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5, TFactoryParam6 param6)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(param1, param2, param3, param4, param5, param6);
            return this;
        }

        // Token: 0x060002ED RID: 749 RVA: 0x00008BB8 File Offset: 0x00006DB8
        public FactoryToChoiceBinder<TParam1, TParam2, TContract> WithFactoryArguments(object[] args)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        // Token: 0x060002EE RID: 750 RVA: 0x00008BCC File Offset: 0x00006DCC
        public FactoryToChoiceBinder<TParam1, TParam2, TContract> WithFactoryArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            base.FactoryBindInfo.Arguments = extraArgs.ToList<TypeValuePair>();
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TContract> : FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract>
    {
        // Token: 0x060002EF RID: 751 RVA: 0x00008BE0 File Offset: 0x00006DE0
        public FactoryArgumentsToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060002F0 RID: 752 RVA: 0x00008BEC File Offset: 0x00006DEC
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> WithFactoryArguments<T>(T param)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<T>(param);
            return this;
        }

        // Token: 0x060002F1 RID: 753 RVA: 0x00008C00 File Offset: 0x00006E00
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2>(TFactoryParam1 param1, TFactoryParam2 param2)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2>(param1, param2);
            return this;
        }

        // Token: 0x060002F2 RID: 754 RVA: 0x00008C18 File Offset: 0x00006E18
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3>(param1, param2, param3);
            return this;
        }

        // Token: 0x060002F3 RID: 755 RVA: 0x00008C30 File Offset: 0x00006E30
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(param1, param2, param3, param4);
            return this;
        }

        // Token: 0x060002F4 RID: 756 RVA: 0x00008C48 File Offset: 0x00006E48
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(param1, param2, param3, param4, param5);
            return this;
        }

        // Token: 0x060002F5 RID: 757 RVA: 0x00008C64 File Offset: 0x00006E64
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5, TFactoryParam6 param6)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(param1, param2, param3, param4, param5, param6);
            return this;
        }

        // Token: 0x060002F6 RID: 758 RVA: 0x00008C80 File Offset: 0x00006E80
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> WithFactoryArguments(object[] args)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        // Token: 0x060002F7 RID: 759 RVA: 0x00008C94 File Offset: 0x00006E94
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TContract> WithFactoryArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            base.FactoryBindInfo.Arguments = extraArgs.ToList<TypeValuePair>();
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> : FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract>
    {
        // Token: 0x060002F8 RID: 760 RVA: 0x00008CA8 File Offset: 0x00006EA8
        public FactoryArgumentsToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060002F9 RID: 761 RVA: 0x00008CB4 File Offset: 0x00006EB4
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithFactoryArguments<T>(T param)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<T>(param);
            return this;
        }

        // Token: 0x060002FA RID: 762 RVA: 0x00008CC8 File Offset: 0x00006EC8
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2>(TFactoryParam1 param1, TFactoryParam2 param2)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2>(param1, param2);
            return this;
        }

        // Token: 0x060002FB RID: 763 RVA: 0x00008CE0 File Offset: 0x00006EE0
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3>(param1, param2, param3);
            return this;
        }

        // Token: 0x060002FC RID: 764 RVA: 0x00008CF8 File Offset: 0x00006EF8
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(param1, param2, param3, param4);
            return this;
        }

        // Token: 0x060002FD RID: 765 RVA: 0x00008D10 File Offset: 0x00006F10
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(param1, param2, param3, param4, param5);
            return this;
        }

        // Token: 0x060002FE RID: 766 RVA: 0x00008D2C File Offset: 0x00006F2C
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5, TFactoryParam6 param6)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(param1, param2, param3, param4, param5, param6);
            return this;
        }

        // Token: 0x060002FF RID: 767 RVA: 0x00008D48 File Offset: 0x00006F48
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithFactoryArguments(object[] args)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        // Token: 0x06000300 RID: 768 RVA: 0x00008D5C File Offset: 0x00006F5C
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TContract> WithFactoryArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            base.FactoryBindInfo.Arguments = extraArgs.ToList<TypeValuePair>();
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> : FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract>
    {
        // Token: 0x06000301 RID: 769 RVA: 0x00008D70 File Offset: 0x00006F70
        public FactoryArgumentsToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x06000302 RID: 770 RVA: 0x00008D7C File Offset: 0x00006F7C
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> WithFactoryArguments<T>(T param)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<T>(param);
            return this;
        }

        // Token: 0x06000303 RID: 771 RVA: 0x00008D90 File Offset: 0x00006F90
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2>(TFactoryParam1 param1, TFactoryParam2 param2)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2>(param1, param2);
            return this;
        }

        // Token: 0x06000304 RID: 772 RVA: 0x00008DA8 File Offset: 0x00006FA8
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3>(param1, param2, param3);
            return this;
        }

        // Token: 0x06000305 RID: 773 RVA: 0x00008DC0 File Offset: 0x00006FC0
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(param1, param2, param3, param4);
            return this;
        }

        // Token: 0x06000306 RID: 774 RVA: 0x00008DD8 File Offset: 0x00006FD8
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(param1, param2, param3, param4, param5);
            return this;
        }

        // Token: 0x06000307 RID: 775 RVA: 0x00008DF4 File Offset: 0x00006FF4
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5, TFactoryParam6 param6)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(param1, param2, param3, param4, param5, param6);
            return this;
        }

        // Token: 0x06000308 RID: 776 RVA: 0x00008E10 File Offset: 0x00007010
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> WithFactoryArguments(object[] args)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        // Token: 0x06000309 RID: 777 RVA: 0x00008E24 File Offset: 0x00007024
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TContract> WithFactoryArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            base.FactoryBindInfo.Arguments = extraArgs.ToList<TypeValuePair>();
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> : FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract>
    {
        // Token: 0x0600030A RID: 778 RVA: 0x00008E38 File Offset: 0x00007038
        public FactoryArgumentsToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x0600030B RID: 779 RVA: 0x00008E44 File Offset: 0x00007044
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> WithFactoryArguments<T>(T param)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<T>(param);
            return this;
        }

        // Token: 0x0600030C RID: 780 RVA: 0x00008E58 File Offset: 0x00007058
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2>(TFactoryParam1 param1, TFactoryParam2 param2)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2>(param1, param2);
            return this;
        }

        // Token: 0x0600030D RID: 781 RVA: 0x00008E70 File Offset: 0x00007070
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3>(param1, param2, param3);
            return this;
        }

        // Token: 0x0600030E RID: 782 RVA: 0x00008E88 File Offset: 0x00007088
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(param1, param2, param3, param4);
            return this;
        }

        // Token: 0x0600030F RID: 783 RVA: 0x00008EA0 File Offset: 0x000070A0
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(param1, param2, param3, param4, param5);
            return this;
        }

        // Token: 0x06000310 RID: 784 RVA: 0x00008EBC File Offset: 0x000070BC
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5, TFactoryParam6 param6)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(param1, param2, param3, param4, param5, param6);
            return this;
        }

        // Token: 0x06000311 RID: 785 RVA: 0x00008ED8 File Offset: 0x000070D8
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> WithFactoryArguments(object[] args)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        // Token: 0x06000312 RID: 786 RVA: 0x00008EEC File Offset: 0x000070EC
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TContract> WithFactoryArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            base.FactoryBindInfo.Arguments = extraArgs.ToList<TypeValuePair>();
            return this;
        }
    }

    [NoReflectionBaking]
    public class FactoryArgumentsToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> : FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract>
    {
        // Token: 0x060002DD RID: 733 RVA: 0x00008A50 File Offset: 0x00006C50
        public FactoryArgumentsToChoiceBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo) : base(bindContainer, bindInfo, factoryBindInfo)
        {
        }

        // Token: 0x060002DE RID: 734 RVA: 0x00008A5C File Offset: 0x00006C5C
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> WithFactoryArguments<T>(T param)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<T>(param);
            return this;
        }

        // Token: 0x060002DF RID: 735 RVA: 0x00008A70 File Offset: 0x00006C70
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2>(TFactoryParam1 param1, TFactoryParam2 param2)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2>(param1, param2);
            return this;
        }

        // Token: 0x060002E0 RID: 736 RVA: 0x00008A88 File Offset: 0x00006C88
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3>(param1, param2, param3);
            return this;
        }

        // Token: 0x060002E1 RID: 737 RVA: 0x00008AA0 File Offset: 0x00006CA0
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4>(param1, param2, param3, param4);
            return this;
        }

        // Token: 0x060002E2 RID: 738 RVA: 0x00008AB8 File Offset: 0x00006CB8
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5>(param1, param2, param3, param4, param5);
            return this;
        }

        // Token: 0x060002E3 RID: 739 RVA: 0x00008AD4 File Offset: 0x00006CD4
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> WithFactoryArguments<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(TFactoryParam1 param1, TFactoryParam2 param2, TFactoryParam3 param3, TFactoryParam4 param4, TFactoryParam5 param5, TFactoryParam6 param6)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgListExplicit<TFactoryParam1, TFactoryParam2, TFactoryParam3, TFactoryParam4, TFactoryParam5, TFactoryParam6>(param1, param2, param3, param4, param5, param6);
            return this;
        }

        // Token: 0x060002E4 RID: 740 RVA: 0x00008AF0 File Offset: 0x00006CF0
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> WithFactoryArguments(object[] args)
        {
            base.FactoryBindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        // Token: 0x060002E5 RID: 741 RVA: 0x00008B04 File Offset: 0x00006D04
        public FactoryToChoiceBinder<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TContract> WithFactoryArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            base.FactoryBindInfo.Arguments = extraArgs.ToList<TypeValuePair>();
            return this;
        }
    }
}
