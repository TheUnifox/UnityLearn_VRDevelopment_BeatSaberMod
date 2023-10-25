using System;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000204 RID: 516
    [ZenjectAllowDuringValidation]
    [NoReflectionBaking]
    public class LazyInject<T> : IValidatable
    {
        // Token: 0x06000ADC RID: 2780 RVA: 0x0001C97C File Offset: 0x0001AB7C
        public LazyInject(DiContainer container, InjectContext context)
        {
            ModestTree.Assert.DerivesFromOrEqual<T>(context.MemberType);
            this._container = container;
            this._context = context;
        }

        // Token: 0x06000ADD RID: 2781 RVA: 0x0001C9A0 File Offset: 0x0001ABA0
        void IValidatable.Validate()
        {
            this._container.Resolve(this._context);
        }

        // Token: 0x170000B1 RID: 177
        // (get) Token: 0x06000ADE RID: 2782 RVA: 0x0001C9B4 File Offset: 0x0001ABB4
        public T Value
        {
            get
            {
                if (!this._hasValue)
                {
                    this._value = (T)((object)this._container.Resolve(this._context));
                    this._hasValue = true;
                }
                return this._value;
            }
        }

        // Token: 0x04000334 RID: 820
        private readonly DiContainer _container;

        // Token: 0x04000335 RID: 821
        private readonly InjectContext _context;

        // Token: 0x04000336 RID: 822
        private bool _hasValue;

        // Token: 0x04000337 RID: 823
        private T _value;
    }
}
