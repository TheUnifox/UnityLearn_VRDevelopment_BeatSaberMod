// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ObstacleBeatmapObjectPreview
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Visuals;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class ObstacleBeatmapObjectPreview : AbstractBeatmapObjectPreview
  {
    [SerializeField]
    private ObstacleView _obstaclePrefab;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly SignalBus _signalBus;
    private RectInt _objectRect;
    private float _obstacleStartBeat;
    private float _obstacleDuration;

    public void SetObstacleData(float startBeat, float duration)
    {
      this._obstacleStartBeat = startBeat;
      this._obstacleDuration = duration;
    }

    public override void Show()
    {
      base.Show();
      this._signalBus.Subscribe<ObstacleDurationChangedSignal>(new Action(this.HandleObstacleDurationChanged));
    }

    public override void Hide()
    {
      base.Hide();
      this._signalBus.TryUnsubscribe<ObstacleDurationChangedSignal>(new Action(this.HandleObstacleDurationChanged));
    }

    public override void Preview(RectInt objectRect)
    {
      Vector3 vector3 = new Vector3((float) (((double) objectRect.x - 1.5) * 0.800000011920929), (float) objectRect.y * 0.8f, 0.0f);
      Color beatmapObjectColor = BeatmapObjectViewColorHelper.GetBeatmapObjectColor(Color.cyan, AbstractBeatmapObjectView.BeatmapObjectEditState.Preview, 0.0f);
      ObstacleEditorData newWithId = ObstacleEditorData.CreateNewWithId(new BeatmapEditorObjectId(), 0.0f, objectRect.x, objectRect.y, this._obstacleDuration, objectRect.width, objectRect.height);
      float position = this._beatmapObjectPlacementHelper.BeatToPosition(this._obstacleStartBeat);
      float length = this._beatmapObjectPlacementHelper.BeatToPosition(this._obstacleStartBeat + this._obstacleDuration) - position;
      this._obstaclePrefab.Init(newWithId, (float) objectRect.width * 0.8f, (float) objectRect.height * 0.8f, length, beatmapObjectColor);
      vector3.x += (float) ((double) (objectRect.width - 1) / 2.0 * 0.800000011920929);
      vector3.y -= 0.4f;
      vector3.z = this._beatmapObjectPlacementHelper.BeatToPosition(this._obstacleStartBeat);
      this._objectRect = objectRect;
      this.objectTransform.localPosition = vector3;
    }

    private void HandleObstacleDurationChanged() => this.Preview(this._objectRect);
  }
}
