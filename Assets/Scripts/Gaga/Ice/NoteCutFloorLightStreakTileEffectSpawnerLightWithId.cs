using System;
using UnityEngine;

namespace Ice
{
    // Token: 0x02000008 RID: 8
    public class NoteCutFloorLightStreakTileEffectSpawnerLightWithId : LightWithIdMonoBehaviour
    {
        // Token: 0x06000020 RID: 32 RVA: 0x00002617 File Offset: 0x00000817
        public override void ColorWasSet(Color color)
        {
            this._noteCutFloorLightStreakTileEffectSpawner.spawnColor = color;
        }

        // Token: 0x0400001E RID: 30
        [SerializeField]
        private NoteCutFloorLightStreakTileEffectSpawner _noteCutFloorLightStreakTileEffectSpawner;
    }
}
