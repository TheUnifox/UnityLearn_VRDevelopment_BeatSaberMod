// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ChainBeatmapObjectPreview
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Visuals;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class ChainBeatmapObjectPreview : AbstractBeatmapObjectPreview
  {
    [SerializeField]
    private ChainNoteView _chainNoteView;
    [Inject]
    private readonly BeatmapObjectPlacementHelper _beatmapObjectPlacementHelper;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    private ColorType _colorType;
    private float _startBeat;
    private Vector2Int _startCoords;
    private NoteCutDirection _noteCutDirection;
    private int _sliceCount;
    private float _endBeat;
    private Vector2Int _endCoords;

    public void SetChainData(
      ColorType colorType,
      float startBeat,
      Vector2Int startCoords,
      NoteCutDirection noteCutDirection,
      int sliceCount,
      float endBeat,
      Vector2Int endCoords)
    {
      this._colorType = colorType;
      this._startBeat = startBeat;
      this._startCoords = startCoords;
      this._noteCutDirection = noteCutDirection;
      this._sliceCount = sliceCount;
      this._endBeat = endBeat;
      this._endCoords = endCoords;
    }

    public override void Preview(RectInt objectRect)
    {
      float beat1 = this._startBeat;
      float beat2 = this._endBeat;
      Vector2Int vector2Int1 = this._startCoords;
      Vector2Int vector2Int2 = this._endCoords;
      NoteCutDirection noteCutDirection = this._noteCutDirection;
      if ((double) beat1 > (double) beat2)
      {
        double num1 = (double) beat2;
        float num2 = beat1;
        beat1 = (float) num1;
        beat2 = num2;
        Vector2Int vector2Int3 = vector2Int2;
        Vector2Int vector2Int4 = vector2Int1;
        vector2Int1 = vector2Int3;
        vector2Int2 = vector2Int4;
        noteCutDirection = ChainUtils.GetNewNoteCutDirection(vector2Int1.x, vector2Int1.y, vector2Int2.x, vector2Int2.y, noteCutDirection, this._readonlyBeatmapObjectsState.sliderSquishAmount);
      }
      float position = this._beatmapObjectPlacementHelper.BeatToPosition(beat1);
      Vector3 localStartPosition = this.GetLocalStartPosition(vector2Int1.x, vector2Int1.y);
      Vector3 localEndPosition = this.GetLocalEndPosition(vector2Int2.x, vector2Int2.y, beat2, position);
      this._chainNoteView.Init((ChainEditorData) null, ColorTypeHelper.GetColorByColorType(this._colorType), noteCutDirection, this._sliceCount, this._readonlyBeatmapObjectsState.sliderSquishAmount, localStartPosition, localEndPosition, false);
      this._chainNoteView.transform.localPosition = new Vector3(localStartPosition.x, localStartPosition.y, this._beatmapObjectPlacementHelper.BeatToPosition(beat1));
    }

    private Vector3 GetLocalStartPosition(int column, int row) => new Vector3((float) (((double) column - 1.5) * 0.800000011920929), (float) row * 0.8f, 0.0f);

    private Vector3 GetLocalEndPosition(int column, int row, float beat, float sliderZPos) => new Vector3((float) (((double) column - 1.5) * 0.800000011920929), (float) row * 0.8f, this._beatmapObjectPlacementHelper.BeatToPosition(beat) - sliderZPos);
  }
}
