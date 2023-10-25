using System;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002FB RID: 763
    public class ZenjectStateMachineBehaviourAutoInjecter : MonoBehaviour
    {
        // Token: 0x0600107C RID: 4220 RVA: 0x0002E5E4 File Offset: 0x0002C7E4
        [Inject]
        public void Construct(DiContainer container)
        {
            this._container = container;
            this._animator = base.GetComponent<Animator>();
            ModestTree.Assert.IsNotNull(this._animator);
        }

        // Token: 0x0600107D RID: 4221 RVA: 0x0002E604 File Offset: 0x0002C804
        public void Start()
        {
            if (this._animator != null)
            {
                StateMachineBehaviour[] behaviours = this._animator.GetBehaviours<StateMachineBehaviour>();
                if (behaviours != null)
                {
                    foreach (StateMachineBehaviour injectable in behaviours)
                    {
                        this._container.Inject(injectable);
                    }
                }
            }
        }

        // Token: 0x0600107F RID: 4223 RVA: 0x0002E658 File Offset: 0x0002C858
        private static void __zenInjectMethod0(object P_0, object[] P_1)
        {
            ((ZenjectStateMachineBehaviourAutoInjecter)P_0).Construct((DiContainer)P_1[0]);
        }

        // Token: 0x06001080 RID: 4224 RVA: 0x0002E674 File Offset: 0x0002C874
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(ZenjectStateMachineBehaviourAutoInjecter), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[]
            {
                new InjectTypeInfo.InjectMethodInfo(new ZenInjectMethod(ZenjectStateMachineBehaviourAutoInjecter.__zenInjectMethod0), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "container", typeof(DiContainer), null, InjectSources.Any)
                }, "Construct")
            }, new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000535 RID: 1333
        private DiContainer _container;

        // Token: 0x04000536 RID: 1334
        private Animator _animator;
    }
}
