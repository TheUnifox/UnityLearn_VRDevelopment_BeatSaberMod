using System;
using UnityEngine;
using Zenject;

namespace Ice
{
    // Token: 0x02000005 RID: 5
    public class FloorLightTileInstaller : ScriptableObjectInstaller
    {
        // Token: 0x06000011 RID: 17 RVA: 0x00002410 File Offset: 0x00000610
        public override void InstallBindings()
        {
            base.Container.BindMemoryPool<FloorLightTile, FloorLightTile.Pool>().WithInitialSize(30).FromComponentInNewPrefab(this._floorLightTilePrefab);
        }

        // Token: 0x04000012 RID: 18
        [SerializeField]
        private FloorLightTile _floorLightTilePrefab;
    }
}
