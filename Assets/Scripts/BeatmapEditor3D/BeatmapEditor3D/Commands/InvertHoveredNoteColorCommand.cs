// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.InvertHoveredNoteColorCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class InvertHoveredNoteColorCommand : InvertHoveredBeatmapObjectColorTypeCommand
  {
    [Inject]
    private readonly InvertHoveredNoteColorSignal _signal;
    private NoteEditorData _noteData;

    protected override bool IsValid()
    {
      this._noteData = this._beatmapLevelDataModel.GetNoteById(this._signal.beatmapObjectId);
      return !(this._noteData == (NoteEditorData) null) && this._noteData.noteType != NoteType.Bomb;
    }

    protected override BeatmapObjectCellData GetCellData() => new BeatmapObjectCellData(new Vector2Int(this._noteData.column, this._noteData.row), this._noteData.beat);
  }
}
