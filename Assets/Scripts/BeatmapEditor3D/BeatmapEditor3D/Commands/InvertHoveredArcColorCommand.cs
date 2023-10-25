// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.InvertHoveredArcColorCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class InvertHoveredArcColorCommand : InvertHoveredBeatmapObjectColorTypeCommand
  {
    [Inject]
    private readonly InvertHoveredArcColorSignal _signal;
    private ArcEditorData _arcData;

    protected override bool IsValid()
    {
      this._arcData = this._beatmapLevelDataModel.GetArcById(this._signal.beatmapObjectId);
      return this._arcData != (ArcEditorData) null;
    }

    protected override BeatmapObjectCellData GetCellData() => new BeatmapObjectCellData(new Vector2Int(this._arcData.column, this._arcData.row), this._arcData.beat);
  }
}
