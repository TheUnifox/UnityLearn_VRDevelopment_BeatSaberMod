using System;
using UnityEngine;
using Zenject;

namespace Ice
{
    // Token: 0x02000003 RID: 3
    public class FloorLightStreakTileEffect : MonoBehaviour
    {
        // Token: 0x06000003 RID: 3 RVA: 0x000020DA File Offset: 0x000002DA
        protected void Start()
        {
            this._elementsPool = new SimpleMemoryPool<FloorLightStreakTileEffect.Element>(50, new Func<FloorLightStreakTileEffect.Element>(this.CreateNewElement));
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000020F8 File Offset: 0x000002F8
        protected void Update()
        {
            float lastFrameDeltaSongTime = this._audioTimeSource.lastFrameDeltaSongTime;
            foreach (FloorLightStreakTileEffect.Element element in this._elementsPool.items)
            {
                element.ManualUpdate(lastFrameDeltaSongTime);
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x0000215C File Offset: 0x0000035C
        public void SpawnEffect(int x, Color color)
        {
            this._elementsPool.Spawn().Setup(color, x, this._stayOnTileDuration);
            this._floorLightTilesGrid.HighlightTile(x, 0, 0.1f, 0.4f, color);
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002190 File Offset: 0x00000390
        public void DespawnAllEffects()
        {
            for (int i = this._elementsPool.items.Count - 1; i >= 0; i--)
            {
                this._elementsPool.Despawn(this._elementsPool.items[i]);
            }
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000021D6 File Offset: 0x000003D6
        private FloorLightStreakTileEffect.Element CreateNewElement()
        {
            FloorLightStreakTileEffect.Element element = new FloorLightStreakTileEffect.Element();
            element.didMoveToNextTile = (Action<FloorLightStreakTileEffect.Element>)Delegate.Combine(element.didMoveToNextTile, new Action<FloorLightStreakTileEffect.Element>(this.HandleElementDidMoveToNextTile));
            return element;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002200 File Offset: 0x00000400
        private void HandleElementDidMoveToNextTile(FloorLightStreakTileEffect.Element element)
        {
            if (element.currentTileY >= this._floorLightTilesGrid.ySize)
            {
                this._elementsPool.Despawn(element);
                return;
            }
            this._floorLightTilesGrid.HighlightTile(element.lineIndex, element.currentTileY, 0.1f, 0.4f, element.color);
        }

        // Token: 0x04000006 RID: 6
        [SerializeField]
        private float _stayOnTileDuration = 0.1f;

        // Token: 0x04000007 RID: 7
        [SerializeField]
        private FloorLightTilesGrid _floorLightTilesGrid;

        // Token: 0x04000008 RID: 8
        [Inject]
        private readonly IAudioTimeSource _audioTimeSource;

        // Token: 0x04000009 RID: 9
        private const float kFadeInDuration = 0.1f;

        // Token: 0x0400000A RID: 10
        private const float kFadeOutDuration = 0.4f;

        // Token: 0x0400000B RID: 11
        private SimpleMemoryPool<FloorLightStreakTileEffect.Element> _elementsPool;

        // Token: 0x02000009 RID: 9
        private class Element
        {
            // Token: 0x17000003 RID: 3
            // (get) Token: 0x06000022 RID: 34 RVA: 0x0000262D File Offset: 0x0000082D
            public int lineIndex
            {
                get
                {
                    return this._lineIndex;
                }
            }

            // Token: 0x17000004 RID: 4
            // (get) Token: 0x06000023 RID: 35 RVA: 0x00002635 File Offset: 0x00000835
            public int currentTileY
            {
                get
                {
                    return this._currentTileY;
                }
            }

            // Token: 0x17000005 RID: 5
            // (get) Token: 0x06000024 RID: 36 RVA: 0x0000263D File Offset: 0x0000083D
            public Color color
            {
                get
                {
                    return this._color;
                }
            }

            // Token: 0x06000025 RID: 37 RVA: 0x00002645 File Offset: 0x00000845
            public void Setup(Color color, int lineIndex, float stayOnTileDuration)
            {
                this._color = color;
                this._lineIndex = lineIndex;
                this._stayOnTileDuration = stayOnTileDuration;
                this._currentTileY = 0;
                this._nextTileRemainingTime = stayOnTileDuration;
            }

            // Token: 0x06000026 RID: 38 RVA: 0x0000266C File Offset: 0x0000086C
            public void ManualUpdate(float deltaTime)
            {
                this._nextTileRemainingTime -= deltaTime;
                while (this._nextTileRemainingTime < 0f)
                {
                    this._nextTileRemainingTime += this._stayOnTileDuration;
                    this._currentTileY++;
                    this.didMoveToNextTile(this);
                }
            }

            // Token: 0x0400001F RID: 31
            public Action<FloorLightStreakTileEffect.Element> didMoveToNextTile;

            // Token: 0x04000020 RID: 32
            private int _currentTileY;

            // Token: 0x04000021 RID: 33
            private int _lineIndex;

            // Token: 0x04000022 RID: 34
            private float _nextTileRemainingTime;

            // Token: 0x04000023 RID: 35
            private float _stayOnTileDuration;

            // Token: 0x04000024 RID: 36
            private Color _color;
        }
    }
}
