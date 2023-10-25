using System;
using UnityEngine;
using Zenject;

namespace Ice
{
    // Token: 0x02000006 RID: 6
    public class FloorLightTilesGrid : MonoBehaviour
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000013 RID: 19 RVA: 0x00002438 File Offset: 0x00000638
        public int ySize
        {
            get
            {
                return this._ySize;
            }
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002440 File Offset: 0x00000640
        public void Init(Vector3 anchorPoint, int xSize, int ySize, float tileWidth, float tileHeight)
        {
            this._anchorPoint = anchorPoint;
            this._tileWidth = tileWidth;
            this._tileHeight = tileHeight;
            this._ySize = ySize;
            this._grid = new FloorLightTile[xSize][];
            for (int i = 0; i < xSize; i++)
            {
                this._grid[i] = new FloorLightTile[ySize];
            }
            this._floorLightTileMemoryPoolContainer = new MemoryPoolContainer<FloorLightTile>(this._floorLightTileMemoryPool);
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000024A4 File Offset: 0x000006A4
        public void HighlightTile(int x, int y, float fadeInDuration, float fadeOutDuration, Color color)
        {
            FloorLightTile floorLightTile = this._grid[x][y];
            if (floorLightTile == null)
            {
                floorLightTile = this._floorLightTileMemoryPoolContainer.Spawn();
                floorLightTile.transform.localPosition = new Vector3(this._tileWidth * (float)x, 0f, this._tileHeight * (float)y) + this._anchorPoint;
            }
            floorLightTile.didFinish = new Action<FloorLightTile>(this.HandleFloorLightTileDidFinish);
            floorLightTile.HighlightWithColor(color, fadeInDuration, fadeOutDuration);
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002520 File Offset: 0x00000720
        public void DespawnAllTiles()
        {
            for (int i = this._floorLightTileMemoryPoolContainer.activeItems.Count - 1; i >= 0; i--)
            {
                this.DespawnTile(this._floorLightTileMemoryPoolContainer.activeItems[i]);
            }
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002561 File Offset: 0x00000761
        private void HandleFloorLightTileDidFinish(FloorLightTile floorLightTile)
        {
            this.DespawnTile(floorLightTile);
        }

        // Token: 0x06000018 RID: 24 RVA: 0x0000256A File Offset: 0x0000076A
        private void DespawnTile(FloorLightTile floorLightTile)
        {
            floorLightTile.didFinish = null;
            this._floorLightTileMemoryPoolContainer.Despawn(floorLightTile);
        }

        // Token: 0x04000013 RID: 19
        [Inject]
        private readonly FloorLightTile.Pool _floorLightTileMemoryPool;

        // Token: 0x04000014 RID: 20
        private MemoryPoolContainer<FloorLightTile> _floorLightTileMemoryPoolContainer;

        // Token: 0x04000015 RID: 21
        private FloorLightTile[][] _grid;

        // Token: 0x04000016 RID: 22
        private float _tileWidth;

        // Token: 0x04000017 RID: 23
        private float _tileHeight;

        // Token: 0x04000018 RID: 24
        private Vector3 _anchorPoint;

        // Token: 0x04000019 RID: 25
        private int _ySize;
    }
}
