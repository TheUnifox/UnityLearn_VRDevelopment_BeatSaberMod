using System;
using UnityEngine;
using Zenject;

namespace Ice
{
    // Token: 0x02000002 RID: 2
    public class BeatmapFloorLightTilesGrid : MonoBehaviour
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        protected void Start()
        {
            int noteLinesCount = this._beatmapObjectSpawnController.noteLinesCount;
            Vector3 position = base.transform.position;
            position.x -= this._tileWidth * ((float)noteLinesCount * 0.5f - 0.5f);
            this._floorLightTilesGrid.Init(position, noteLinesCount, this._numberOfRows, this._tileWidth, this._tileHeight);
        }

        // Token: 0x04000001 RID: 1
        [SerializeField]
        private int _numberOfRows = 10;

        // Token: 0x04000002 RID: 2
        [SerializeField]
        private float _tileWidth = 1f;

        // Token: 0x04000003 RID: 3
        [SerializeField]
        private float _tileHeight = 1f;

        // Token: 0x04000004 RID: 4
        [SerializeField]
        private FloorLightTilesGrid _floorLightTilesGrid;

        // Token: 0x04000005 RID: 5
        [Inject]
        private readonly IBeatmapObjectSpawnController _beatmapObjectSpawnController;
    }
}
