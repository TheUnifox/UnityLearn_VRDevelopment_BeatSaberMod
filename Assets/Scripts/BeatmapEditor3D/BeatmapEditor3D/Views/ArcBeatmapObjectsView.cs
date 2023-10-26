// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.ArcBeatmapObjectsView
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
  public class ArcBeatmapObjectsView : AbstractBeatmapObjectView
  {
    [SerializeField]
    private ArcObjectsMouseInputSource _arcObjectsMouseInputSource;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly ArcView.Pool _arcPool;
    private HashSet<ArcEditorData> _currentArcs = new HashSet<ArcEditorData>();
    private readonly Dictionary<BeatmapEditorObjectId, ArcView> _arcObjects = new Dictionary<BeatmapEditorObjectId, ArcView>();

    public IReadOnlyDictionary<BeatmapEditorObjectId, ArcView> arcObjects => (IReadOnlyDictionary<BeatmapEditorObjectId, ArcView>) this._arcObjects;

    public override void RefreshView(float startTime, float endTime, bool clearView = false)
    {
      if (clearView)
        this.ClearAllArcs();
      ArcEditorData[] array = this.beatmapLevelDataModel.GetArcsInterval(startTime, endTime).ToArray<ArcEditorData>();
      HashSet<ArcEditorData> newArcHashSet = new HashSet<ArcEditorData>((IEnumerable<ArcEditorData>) array);
      List<ArcEditorData> list1 = ((IEnumerable<ArcEditorData>) array).Where<ArcEditorData>((Func<ArcEditorData, bool>) (a => !this._currentArcs.Contains(a))).ToList<ArcEditorData>();
      List<ArcEditorData> list2 = this._currentArcs.Where<ArcEditorData>((Func<ArcEditorData, bool>) (a => !newArcHashSet.Contains(a))).ToList<ArcEditorData>();
      if (!clearView)
        this.DeleteArcs((IReadOnlyCollection<ArcEditorData>) list2);
      if (list1.Count != 0)
        this.InsertArcs((IReadOnlyCollection<ArcEditorData>) list1);
      if (list1.Count != 0 || list2.Count != 0)
        this._currentArcs = newArcHashSet;
      this.UpdateArcs();
    }

    public override void UpdateTimeScale()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, ArcView> arcObject in this._arcObjects)
      {
        float position = this.beatmapObjectPlacementHelper.BeatToPosition(arcObject.Value.arcData.beat);
        Vector3 localStartPosition = this.GetLocalStartPosition(arcObject.Value.arcData);
        Vector3 localEndPosition = this.GetLocalEndPosition(arcObject.Value.arcData, position);
        arcObject.Value.SetLength(localStartPosition, localEndPosition);
      }
    }

    public override void ClearView()
    {
      this.ClearAllArcs();
      this._arcPool.Clear();
    }

    public void SetControlPointsDataOverride(
      BeatmapEditorObjectId id,
      BeatmapObjectCellData cellData,
      float? overrideControlPointLength,
      NoteCutDirection? overrideCutDirection)
    {
      ArcView arcView;
      if (!this.arcObjects.TryGetValue(id, out arcView))
        return;
      arcView.SetControlPointsDataOverride(cellData, overrideControlPointLength, overrideCutDirection);
    }

    public void SetMidAnchorModeDataOverride(
      BeatmapEditorObjectId id,
      SliderMidAnchorMode? overrideMidAnchorMode)
    {
      ArcView arcView;
      if (!this.arcObjects.TryGetValue(id, out arcView))
        return;
      arcView.SetMidAnchorModeDataOverride(overrideMidAnchorMode);
    }

    public void ResetDataOverride(BeatmapEditorObjectId id, BeatmapObjectCellData cellData)
    {
      ArcView arcView;
      if (!this.arcObjects.TryGetValue(id, out arcView))
        return;
      arcView.SetControlPointsDataOverride(cellData, new float?(), new NoteCutDirection?());
      arcView.SetMidAnchorModeDataOverride(new SliderMidAnchorMode?());
    }

    private void DeleteArcs(IReadOnlyCollection<ArcEditorData> arcsToDelete)
    {
      if (arcsToDelete == null || arcsToDelete.Count <= 0)
        return;
      foreach (ArcEditorData arcEditorData in (IEnumerable<ArcEditorData>) arcsToDelete)
      {
        ArcView arcObject = this._arcObjects[arcEditorData.id];
        this._arcObjects.Remove(arcEditorData.id);
        this._arcPool.Despawn(arcObject);
        this._arcObjectsMouseInputSource.UnsubscribeFromMouseEvents((Component) arcObject);
      }
    }

        private void UpdateArcs()
        {
            foreach (ArcEditorData arcEditorData in this._currentArcs)
            {
                ArcView arcView = this._arcObjects[arcEditorData.id];
                Vector3 localPosition = arcView.transform.localPosition;
                localPosition.z = this.beatmapObjectPlacementHelper.BeatToPosition(arcView.arcData.beat);
                arcView.transform.localPosition = localPosition;
                float a = arcView.arcData.beat - this.beatmapState.beat;
                float a2 = arcView.arcData.tailBeat - this.beatmapState.beat;
                float a3 = arcView.arcData.beat + (arcView.arcData.tailBeat - arcView.arcData.beat) * 0.5f - this.beatmapState.beat;
                arcView.SetState(AudioTimeHelper.IsBeatSame(a, 0f), AudioTimeHelper.IsBeatSame(a2, 0f), AudioTimeHelper.IsBeatSame(a3, 0f));
            }
        }

        private void InsertArcs(IReadOnlyCollection<ArcEditorData> arcsToInsert)
    {
      if (arcsToInsert == null || arcsToInsert.Count <= 0)
        return;
      foreach (ArcEditorData arcData in (IEnumerable<ArcEditorData>) arcsToInsert)
      {
        ArcView arcView = this._arcPool.Spawn();
        arcView.transform.SetParent(this.beatmapObjectsContainer.transform, true);
        float position = this.beatmapObjectPlacementHelper.BeatToPosition(arcData.beat);
        Vector3 localStartPosition = this.GetLocalStartPosition(arcData);
        Vector3 localEndPosition = this.GetLocalEndPosition(arcData, position);
        BeatmapObjectCellData cellData = new BeatmapObjectCellData(new Vector2Int(arcData.column, arcData.row), arcData.beat);
        BeatmapObjectCellData beatmapObjectCellData = new BeatmapObjectCellData(new Vector2Int(arcData.tailColumn, arcData.tailRow), arcData.tailBeat);
        bool showHandles = this._beatmapState.interactionMode == InteractionMode.Modify;
        Color colorByColorType = ColorTypeHelper.GetColorByColorType(arcData.colorType);
        arcView.Init(arcData, colorByColorType, localStartPosition, localEndPosition, showHandles, !this.beatmapLevelDataModel.HasNoteOrChain(cellData), !this.beatmapLevelDataModel.HasNoteOrChain(beatmapObjectCellData));
        this._arcObjectsMouseInputSource.SubscribeToMouseEvents(arcData.id, cellData, beatmapObjectCellData, (Component) arcView);
        arcView.transform.localPosition = new Vector3(0.0f, 0.0f, position);
        arcView.gameObject.SetActive(true);
        this._arcObjects.Add(arcData.id, arcView);
      }
    }

    private void ClearAllArcs()
    {
      foreach (BaseEditorData currentArc in this._currentArcs)
      {
        ArcView arcObject = this._arcObjects[currentArc.id];
        if (!((UnityEngine.Object) arcObject == (UnityEngine.Object) null))
        {
          this._arcPool.Despawn(arcObject);
          this._arcObjectsMouseInputSource.UnsubscribeFromMouseEvents((Component) arcObject);
        }
      }
      this._arcObjects.Clear();
      this._currentArcs = new HashSet<ArcEditorData>();
    }

    private Vector3 GetLocalStartPosition(ArcEditorData arcData) => new Vector3((float) (((double) arcData.column - 1.5) * 0.800000011920929), this._beatmapObjectYOffset + (float) arcData.row * 0.8f, 0.0f);

    private Vector3 GetLocalEndPosition(ArcEditorData arcData, float arcZPos) => new Vector3((float) (((double) arcData.tailColumn - 1.5) * 0.800000011920929), this._beatmapObjectYOffset + (float) arcData.tailRow * 0.8f, this.beatmapObjectPlacementHelper.BeatToPosition(arcData.tailBeat) - arcZPos);
  }
}
