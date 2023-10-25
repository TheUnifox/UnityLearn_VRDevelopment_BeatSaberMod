using System;
using UnityEngine;
using Zenject;

namespace Ice
{
    // Token: 0x02000007 RID: 7
    public class NoteCutFloorLightStreakTileEffectSpawner : MonoBehaviour
    {
        // Token: 0x17000002 RID: 2
        // (get) Token: 0x0600001B RID: 27 RVA: 0x00002588 File Offset: 0x00000788
        // (set) Token: 0x0600001A RID: 26 RVA: 0x0000257F File Offset: 0x0000077F
        public Color spawnColor
        {
            get
            {
                return this._spawnColor;
            }
            set
            {
                this._spawnColor = value;
            }
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00002590 File Offset: 0x00000790
        protected void Start()
        {
            this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000025A9 File Offset: 0x000007A9
        protected void OnDestroy()
        {
            if (this._beatmapObjectManager != null)
            {
                this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x000025CC File Offset: 0x000007CC
        private void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
        {
            NoteData noteData = noteController.noteData;
            int lineIndex = noteData.lineIndex;
            this._floorLightStreakTileEffect.SpawnEffect(lineIndex, this._colorManager.ColorForType(noteData.colorType));
        }

        // Token: 0x0400001A RID: 26
        [SerializeField]
        private FloorLightStreakTileEffect _floorLightStreakTileEffect;

        // Token: 0x0400001B RID: 27
        [Inject]
        private readonly BeatmapObjectManager _beatmapObjectManager;

        // Token: 0x0400001C RID: 28
        [Inject]
        private readonly ColorManager _colorManager;

        // Token: 0x0400001D RID: 29
        private Color _spawnColor = Color.clear;
    }
}
