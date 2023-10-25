// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ArcBeatmapObjectPreview
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Visuals;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class ArcBeatmapObjectPreview : AbstractBeatmapObjectPreview
  {
    [SerializeField]
    private ArcView _arcPrefab;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;
    [Inject]
    private readonly SignalBus _signalBus;
    private ColorType _colorType;
    private float _startBeat;
    private Vector2Int _startCoords;
    private NoteCutDirection _startCutDirection;
    private float _endBeat;
    private Vector2Int _endCoords;
    private NoteCutDirection _endCutDirection;

    public void SetSliderData(
      ColorType colorType,
      float startBeat,
      Vector2Int startCoords,
      NoteCutDirection startCutDirection,
      float endBeat,
      Vector2Int endCoords,
      NoteCutDirection endCutDirection)
    {
      this._colorType = colorType;
      this._startBeat = startBeat;
      this._startCoords = startCoords;
      this._startCutDirection = startCutDirection;
      this._endBeat = endBeat;
      this._endCoords = endCoords;
      this._endCutDirection = endCutDirection;
    }

    public override void Show()
    {
      base.Show();
      this._signalBus.Subscribe<ArcMidAnchorModeChangedSignal>(new Action(this.HandleSliderMidAnchorModeChanged));
      this._signalBus.Subscribe<NoteCutDirectionChangedSignal>(new Action(this.HandleNoteCutDirectionChanged));
    }

    public override void Hide()
    {
      base.Hide();
      this._signalBus.TryUnsubscribe<ArcMidAnchorModeChangedSignal>(new Action(this.HandleSliderMidAnchorModeChanged));
      this._signalBus.TryUnsubscribe<NoteCutDirectionChangedSignal>(new Action(this.HandleNoteCutDirectionChanged));
    }

    public override void Preview(RectInt objectRect)
    {
      float beat = this._startBeat;
      float num1 = this._endBeat;
      Vector2Int vector2Int1 = this._startCoords;
      Vector2Int vector2Int2 = this._endCoords;
      NoteCutDirection cutDirection = this._startCutDirection;
      NoteCutDirection tailCutDirection = this._endCutDirection;
      if ((double) beat > (double) num1)
      {
        double num2 = (double) num1;
        float num3 = beat;
        beat = (float) num2;
        num1 = num3;
        Vector2Int vector2Int3 = vector2Int2;
        Vector2Int vector2Int4 = vector2Int1;
        vector2Int1 = vector2Int3;
        vector2Int2 = vector2Int4;
        int num4 = (int) tailCutDirection;
        NoteCutDirection noteCutDirection = cutDirection;
        cutDirection = (NoteCutDirection) num4;
        tailCutDirection = noteCutDirection;
      }
      float position = this._beatmapObjectPlacementHelper.BeatToPosition(beat);
      float x1 = (float) (((double) vector2Int1.x - 1.5) * 0.800000011920929);
      float y1 = (float) vector2Int1.y * 0.8f;
      float z1 = 0.0f;
      float x2 = (float) (((double) vector2Int2.x - 1.5) * 0.800000011920929);
      float y2 = (float) vector2Int2.y * 0.8f;
      float z2 = this._beatmapObjectPlacementHelper.BeatToPosition(num1) - position;
      Vector3 localStartPosition = new Vector3(x1, y1, z1);
      Vector3 localEndPosition = new Vector3(x2, y2, z2);
      Color colorByColorType = ColorTypeHelper.GetColorByColorType(this._colorType);
      this._arcPrefab.Init(ArcEditorData.CreateNewWithId(new BeatmapEditorObjectId(), this._colorType, beat, vector2Int1.x, vector2Int1.y, cutDirection, this._beatmapObjectsState.arcControlPointLengthMultiplier, num1, vector2Int2.x, vector2Int2.y, tailCutDirection, this._beatmapObjectsState.arcTailControlPointLengthMultiplier, this._beatmapObjectsState.arcMidAnchorMode), colorByColorType, localStartPosition, localEndPosition, false, false, false);
      this.objectTransform.transform.localPosition = new Vector3(0.0f, 0.0f, position);
    }

    private void HandleSliderMidAnchorModeChanged() => this.Preview(new RectInt());

    private void HandleNoteCutDirectionChanged()
    {
      this._endCutDirection = this._beatmapObjectsState.noteCutDirection;
      this.Preview(new RectInt());
    }
  }
}
