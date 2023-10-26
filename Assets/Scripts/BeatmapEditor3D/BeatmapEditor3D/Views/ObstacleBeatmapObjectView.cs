// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.ObstacleBeatmapObjectView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class ObstacleBeatmapObjectView : AbstractBeatmapObjectView
  {
    [SerializeField]
    private ObstacleObjectsMouseInputSource _obstacleObjectsMouseInputSource;
    [Inject]
    private readonly ObstacleView.Pool _obstaclePool;
    private HashSet<ObstacleEditorData> _currentObstacles = new HashSet<ObstacleEditorData>();
    private readonly Dictionary<BeatmapEditorObjectId, ObstacleView> _obstacleObjects = new Dictionary<BeatmapEditorObjectId, ObstacleView>();

    public IReadOnlyDictionary<BeatmapEditorObjectId, ObstacleView> obstacleObjects => (IReadOnlyDictionary<BeatmapEditorObjectId, ObstacleView>) this._obstacleObjects;

    public override void RefreshView(float startTime, float endTime, bool clearView = false)
    {
      ObstacleEditorData[] array1 = this.beatmapLevelDataModel.GetObstaclesInterval(startTime, endTime).ToArray<ObstacleEditorData>();
      HashSet<ObstacleEditorData> newObstaclesHashSet = new HashSet<ObstacleEditorData>((IEnumerable<ObstacleEditorData>) array1);
      ObstacleEditorData[] array2 = ((IEnumerable<ObstacleEditorData>) array1).Where<ObstacleEditorData>((Func<ObstacleEditorData, bool>) (obstacle => !this._currentObstacles.Contains(obstacle))).ToArray<ObstacleEditorData>();
      this.DeleteObstacles((IReadOnlyCollection<ObstacleEditorData>) this._currentObstacles.Where<ObstacleEditorData>((Func<ObstacleEditorData, bool>) (prevObstacle => !newObstaclesHashSet.Contains(prevObstacle))).ToArray<ObstacleEditorData>());
      this.UpdateObstacles();
      this.InsertObstacles((IReadOnlyCollection<ObstacleEditorData>) array2);
      this._currentObstacles = newObstaclesHashSet;
    }

    public override void UpdateTimeScale()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, ObstacleView> obstacleObject in this._obstacleObjects)
      {
        float position1 = this.beatmapObjectPlacementHelper.BeatToPosition(obstacleObject.Value.obstacleData.beat);
        float position2 = this.beatmapObjectPlacementHelper.BeatToPosition(obstacleObject.Value.obstacleData.beat + obstacleObject.Value.obstacleData.duration);
        obstacleObject.Value.SetLength(position2 - position1);
      }
    }

    public override void ClearView()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, ObstacleView> obstacleObject in this._obstacleObjects)
      {
        if (!((UnityEngine.Object) obstacleObject.Value == (UnityEngine.Object) null))
        {
          this._obstacleObjectsMouseInputSource.UnsubscribeFromMouseEvents((Component) obstacleObject.Value);
          obstacleObject.Value.gameObject.SetActive(false);
          this._obstaclePool.Despawn(obstacleObject.Value);
        }
      }
      this._obstacleObjects.Clear();
      this._obstaclePool.Clear();
      this._currentObstacles = new HashSet<ObstacleEditorData>();
    }

    private void DeleteObstacles(
      IReadOnlyCollection<ObstacleEditorData> obstaclesToDelete)
    {
      if (obstaclesToDelete == null || obstaclesToDelete.Count == 0)
        return;
      foreach (ObstacleEditorData obstacleEditorData in (IEnumerable<ObstacleEditorData>) obstaclesToDelete)
      {
        ObstacleView obstacleObject = this._obstacleObjects[obstacleEditorData.id];
        this._obstaclePool.Despawn(obstacleObject);
        this._obstacleObjects.Remove(obstacleEditorData.id);
        this._obstacleObjectsMouseInputSource.UnsubscribeFromMouseEvents((Component) obstacleObject);
      }
    }

    private void UpdateObstacles()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, ObstacleView> obstacleObject in this._obstacleObjects)
      {
        float relativeNoteTime = obstacleObject.Value.obstacleData.beat - this.beatmapState.beat;
        Vector3 localPosition = obstacleObject.Value.transform.localPosition;
        localPosition.z = this.beatmapObjectPlacementHelper.BeatToPosition(obstacleObject.Value.obstacleData.beat);
        obstacleObject.Value.transform.localPosition = localPosition;
        AbstractBeatmapObjectView.BeatmapObjectEditState state = this.beatmapObjectsSelectionState.IsObstacleSelected(obstacleObject.Value.obstacleData.id) ? AbstractBeatmapObjectView.BeatmapObjectEditState.Selected : AbstractBeatmapObjectView.BeatmapObjectEditState.Default;
        obstacleObject.Value.SetState(BeatmapObjectViewColorHelper.GetBeatmapObjectColor(Color.cyan, state, relativeNoteTime));
      }
    }

    private void InsertObstacles(
      IReadOnlyCollection<ObstacleEditorData> obstaclesToInsert)
    {
      if (obstaclesToInsert == null || obstaclesToInsert.Count == 0)
        return;
      foreach (ObstacleEditorData obstacleData in (IEnumerable<ObstacleEditorData>) obstaclesToInsert)
      {
        float relativeNoteTime = obstacleData.beat - this.beatmapState.beat;
        float position1 = this.beatmapObjectPlacementHelper.BeatToPosition(obstacleData.beat);
        float position2 = this.beatmapObjectPlacementHelper.BeatToPosition(obstacleData.beat + obstacleData.duration);
        ObstacleView obstacleView = this._obstaclePool.Spawn();
        obstacleView.transform.SetParent(this.beatmapObjectsContainer.transform, true);
        this._obstacleObjects.Add(obstacleData.id, obstacleView);
        Color beatmapObjectColor = BeatmapObjectViewColorHelper.GetBeatmapObjectColor(Color.cyan, this.beatmapObjectsSelectionState.IsObstacleSelected(obstacleData.id) ? AbstractBeatmapObjectView.BeatmapObjectEditState.Selected : AbstractBeatmapObjectView.BeatmapObjectEditState.Default, relativeNoteTime);
        obstacleView.Init(obstacleData, (float) obstacleData.width * 0.8f, (float) obstacleData.height * 0.8f, position2 - position1, beatmapObjectColor);
        this._obstacleObjectsMouseInputSource.SubscribeToMouseEvents(obstacleData.id, new BeatmapObjectCellData(new Vector2Int(obstacleData.column, obstacleData.row), obstacleData.beat), (Component) obstacleView);
        obstacleView.transform.localPosition = new Vector3((float) (((double) obstacleData.column - 2.0 + (double) obstacleData.width / 2.0) * 0.800000011920929), (float) ((double) this._beatmapObjectYOffset + (double) obstacleData.row * 0.800000011920929 - 0.40000000596046448), position1);
        obstacleView.gameObject.SetActive(true);
      }
    }
  }
}
