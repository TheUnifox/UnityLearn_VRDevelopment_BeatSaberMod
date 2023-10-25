using System;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002EA RID: 746
    public class DefaultGameObjectParentInstaller : Installer<string, DefaultGameObjectParentInstaller>
    {
        // Token: 0x06001010 RID: 4112 RVA: 0x0002D6BC File Offset: 0x0002B8BC
        public DefaultGameObjectParentInstaller(string name)
        {
            this._name = name;
        }

        // Token: 0x06001011 RID: 4113 RVA: 0x0002D6CC File Offset: 0x0002B8CC
        public override void InstallBindings()
        {
            GameObject gameObject = new GameObject(this._name);
            gameObject.transform.SetParent(base.Container.InheritedDefaultParent, false);
            base.Container.DefaultParent = gameObject.transform;
            base.Container.Bind<IDisposable>().To<DefaultGameObjectParentInstaller.DefaultParentObjectDestroyer>().AsCached().WithArguments<GameObject>(gameObject);
            base.Container.BindDisposableExecutionOrder<DefaultGameObjectParentInstaller.DefaultParentObjectDestroyer>(int.MinValue);
        }

        // Token: 0x06001012 RID: 4114 RVA: 0x0002D73C File Offset: 0x0002B93C
        private static object __zenCreate(object[] P_0)
        {
            return new DefaultGameObjectParentInstaller((string)P_0[0]);
        }

        // Token: 0x06001013 RID: 4115 RVA: 0x0002D760 File Offset: 0x0002B960
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(DefaultGameObjectParentInstaller), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(DefaultGameObjectParentInstaller.__zenCreate), new InjectableInfo[]
            {
                new InjectableInfo(false, null, "name", typeof(string), null, InjectSources.Any)
            }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x04000511 RID: 1297
        private readonly string _name;

        // Token: 0x020002EB RID: 747
        private class DefaultParentObjectDestroyer : IDisposable
        {
            // Token: 0x06001014 RID: 4116 RVA: 0x0002D7D4 File Offset: 0x0002B9D4
            public DefaultParentObjectDestroyer(GameObject gameObject)
            {
                this._gameObject = gameObject;
            }

            // Token: 0x06001015 RID: 4117 RVA: 0x0002D7E4 File Offset: 0x0002B9E4
            public void Dispose()
            {
                UnityEngine.Object.Destroy(this._gameObject);
            }

            // Token: 0x06001016 RID: 4118 RVA: 0x0002D7F4 File Offset: 0x0002B9F4
            private static object __zenCreate(object[] P_0)
            {
                return new DefaultGameObjectParentInstaller.DefaultParentObjectDestroyer((GameObject)P_0[0]);
            }

            // Token: 0x06001017 RID: 4119 RVA: 0x0002D818 File Offset: 0x0002BA18
            [Preserve]
            private static InjectTypeInfo __zenCreateInjectTypeInfo()
            {
                return new InjectTypeInfo(typeof(DefaultGameObjectParentInstaller.DefaultParentObjectDestroyer), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(DefaultGameObjectParentInstaller.DefaultParentObjectDestroyer.__zenCreate), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "gameObject", typeof(GameObject), null, InjectSources.Any)
                }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
            }

            // Token: 0x04000512 RID: 1298
            private readonly GameObject _gameObject;
        }
    }
}
