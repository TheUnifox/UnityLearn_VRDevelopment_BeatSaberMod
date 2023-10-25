// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.InvertHoveredChainColorCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class InvertHoveredChainColorCommand : InvertHoveredBeatmapObjectColorTypeCommand
  {
    [Inject]
    private readonly InvertHoveredChainColorSignal _signal;
    private ChainEditorData _chainData;

    protected override bool IsValid()
    {
      this._chainData = this._beatmapLevelDataModel.GetChainById(this._signal.beatmapObjectId);
      return this._chainData != (ChainEditorData) null;
    }

    protected override BeatmapObjectCellData GetCellData() => new BeatmapObjectCellData(new Vector2Int(this._chainData.column, this._chainData.row), this._chainData.beat);
  }
}
