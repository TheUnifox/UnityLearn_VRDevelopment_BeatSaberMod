// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.ChainBeatmapObjectsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class ChainBeatmapObjectsView : AbstractBeatmapObjectView
  {
    [SerializeField]
    private ChainObjectsMouseInputSource _chainObjectsMouseInputSource;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly ChainNoteView.Pool _chainPool;
    private HashSet<ChainEditorData> _currentChains = new HashSet<ChainEditorData>();
    private readonly Dictionary<BeatmapEditorObjectId, ChainNoteView> _chainObjects = new Dictionary<BeatmapEditorObjectId, ChainNoteView>();

    public IReadOnlyDictionary<BeatmapEditorObjectId, ChainNoteView> chainObjects => (IReadOnlyDictionary<BeatmapEditorObjectId, ChainNoteView>) this._chainObjects;

    public override void RefreshView(float startTime, float endTime, bool clearView = false)
    {
      if (clearView)
        this.ClearAllChains();
      ChainEditorData[] array1 = this.beatmapLevelDataModel.GetChainsInterval(startTime, endTime).ToArray<ChainEditorData>();
      HashSet<ChainEditorData> newChainsHashSet = new HashSet<ChainEditorData>((IEnumerable<ChainEditorData>) array1);
      ChainEditorData[] array2 = ((IEnumerable<ChainEditorData>) array1).Where<ChainEditorData>((Func<ChainEditorData, bool>) (chain => !this._currentChains.Contains(chain))).ToArray<ChainEditorData>();
      ChainEditorData[] array3 = this._currentChains.Where<ChainEditorData>((Func<ChainEditorData, bool>) (prevChain => !newChainsHashSet.Contains(prevChain))).ToArray<ChainEditorData>();
      if (!clearView)
        this.DeleteChains((IReadOnlyCollection<ChainEditorData>) array3);
      this.UpdateChains();
      if (array2.Length != 0)
        this.InsertChains((IReadOnlyCollection<ChainEditorData>) array2);
      if (array2.Length == 0 && array3.Length == 0)
        return;
      this._currentChains = newChainsHashSet;
    }

    public override void UpdateTimeScale()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, ChainNoteView> chainObject in this._chainObjects)
      {
        float position = this.beatmapObjectPlacementHelper.BeatToPosition(chainObject.Value.chainData.beat);
        Vector3 localStartPosition = this.GetLocalStartPosition(chainObject.Value.chainData);
        Vector3 localEndPosition = this.GetLocalEndPosition(chainObject.Value.chainData, position);
        chainObject.Value.SetLength(localStartPosition, localEndPosition);
      }
    }

    public override void ClearView()
    {
      this.ClearAllChains();
      this._chainPool.Clear();
    }

    private void DeleteChains(
      IReadOnlyCollection<ChainEditorData> chainsToDelete)
    {
      if (chainsToDelete == null || chainsToDelete.Count == 0)
        return;
      foreach (ChainEditorData chainEditorData in (IEnumerable<ChainEditorData>) chainsToDelete)
      {
        ChainNoteView chainObject = this._chainObjects[chainEditorData.id];
        this._chainObjects.Remove(chainEditorData.id);
        this._chainPool.Despawn(chainObject);
        this._chainObjectsMouseInputSource.UnsubscribeFromMouseEvents(chainObject.headMouseInputProvider);
        this._chainObjectsMouseInputSource.UnsubscribeFromMouseEvents(chainObject.tailMouseInputProvider);
      }
    }

        private void UpdateChains()
        {
            foreach (KeyValuePair<BeatmapEditorObjectId, ChainNoteView> keyValuePair in this._chainObjects)
            {
                float num = keyValuePair.Value.chainData.beat - this.beatmapState.beat;
                float num2 = keyValuePair.Value.chainData.tailBeat - this.beatmapState.beat;
                Vector3 localPosition = keyValuePair.Value.transform.localPosition;
                localPosition.z = this.beatmapObjectPlacementHelper.BeatToPosition(keyValuePair.Value.chainData.beat);
                keyValuePair.Value.transform.localPosition = localPosition;
                keyValuePair.Value.SetState(Mathf.Approximately(num, 0f), num < 0f, Mathf.Approximately(num2, 0f), num2 < 0f, false);
            }
        }

        private void InsertChains(
      IReadOnlyCollection<ChainEditorData> chainsToInsert)
    {
      if (chainsToInsert == null || chainsToInsert.Count == 0)
        return;
      foreach (ChainEditorData chainData in (IEnumerable<ChainEditorData>) chainsToInsert)
      {
        ChainNoteView chainNoteView = this._chainPool.Spawn();
        chainNoteView.transform.SetParent(this.beatmapObjectsContainer.transform, true);
        this._chainObjects.Add(chainData.id, chainNoteView);
        float position = this.beatmapObjectPlacementHelper.BeatToPosition(chainData.beat);
        Vector3 localStartPosition = this.GetLocalStartPosition(chainData);
        Vector3 localEndPosition = this.GetLocalEndPosition(chainData, position);
        float a1 = chainData.beat - this.beatmapState.beat;
        float a2 = chainData.tailBeat - this.beatmapState.beat;
        Color colorByColorType = ColorTypeHelper.GetColorByColorType(chainData.colorType);
        chainNoteView.Init(chainData, colorByColorType, chainData.cutDirection, chainData.sliceCount, chainData.squishAmount, localStartPosition, localEndPosition, this._beatmapState.interactionMode == InteractionMode.Modify);
        chainNoteView.SetState(Mathf.Approximately(a1, 0.0f), (double) a1 < 0.0, Mathf.Approximately(a2, 0.0f), (double) a2 < 0.0, false);
        this._chainObjectsMouseInputSource.SubscribeToMouseEvents(chainData.id, new BeatmapObjectCellData(new Vector2Int(chainData.column, chainData.row), chainData.beat), chainNoteView.headMouseInputProvider);
        this._chainObjectsMouseInputSource.SubscribeToMouseEvents(chainData.id, new BeatmapObjectCellData(new Vector2Int(chainData.tailColumn, chainData.tailRow), chainData.tailBeat), chainNoteView.tailMouseInputProvider);
        chainNoteView.transform.localPosition = new Vector3((float) (((double) chainData.column - 1.5) * 0.800000011920929), this._beatmapObjectYOffset + (float) chainData.row * 0.8f, this.beatmapObjectPlacementHelper.BeatToPosition(chainData.beat));
        chainNoteView.gameObject.SetActive(true);
      }
    }

    private void ClearAllChains()
    {
      foreach (BaseEditorData currentChain in this._currentChains)
      {
        ChainNoteView chainObject = this._chainObjects[currentChain.id];
        if (!((UnityEngine.Object) chainObject == (UnityEngine.Object) null))
        {
          this._chainPool.Despawn(chainObject);
          this._chainObjectsMouseInputSource.UnsubscribeFromMouseEvents(chainObject.headMouseInputProvider);
          this._chainObjectsMouseInputSource.UnsubscribeFromMouseEvents(chainObject.tailMouseInputProvider);
        }
      }
      this._chainObjects.Clear();
      this._currentChains.Clear();
    }

    private Vector3 GetLocalStartPosition(ChainEditorData chainData) => new Vector3((float) (((double) chainData.column - 1.5) * 0.800000011920929), this._beatmapObjectYOffset + (float) chainData.row * 0.8f, 0.0f);

    private Vector3 GetLocalEndPosition(ChainEditorData chainData, float sliderZPos) => new Vector3((float) (((double) chainData.tailColumn - 1.5) * 0.800000011920929), this._beatmapObjectYOffset + (float) chainData.tailRow * 0.8f, this.beatmapObjectPlacementHelper.BeatToPosition(chainData.tailBeat) - sliderZPos);
  }
}
