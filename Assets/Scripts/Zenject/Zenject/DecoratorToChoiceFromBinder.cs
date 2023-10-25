using System;

namespace Zenject
{
    // Token: 0x02000100 RID: 256
    [NoReflectionBaking]
    public class DecoratorToChoiceFromBinder<TContract>
    {
        // Token: 0x060005B2 RID: 1458 RVA: 0x0000F55C File Offset: 0x0000D75C
        public DecoratorToChoiceFromBinder(DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo)
        {
            this._bindContainer = bindContainer;
            this._bindInfo = bindInfo;
            this._factoryBindInfo = factoryBindInfo;
        }

        // Token: 0x060005B3 RID: 1459 RVA: 0x0000F57C File Offset: 0x0000D77C
        public FactoryFromBinder<TContract, TConcrete> With<TConcrete>() where TConcrete : TContract
        {
            this._bindInfo.ToChoice = ToChoices.Concrete;
            this._bindInfo.ToTypes.Clear();
            this._bindInfo.ToTypes.Add(typeof(TConcrete));
            return new FactoryFromBinder<TContract, TConcrete>(this._bindContainer, this._bindInfo, this._factoryBindInfo);
        }

        // Token: 0x040001FE RID: 510
        private DiContainer _bindContainer;

        // Token: 0x040001FF RID: 511
        private BindInfo _bindInfo;

        // Token: 0x04000200 RID: 512
        private FactoryBindInfo _factoryBindInfo;
    }
}
