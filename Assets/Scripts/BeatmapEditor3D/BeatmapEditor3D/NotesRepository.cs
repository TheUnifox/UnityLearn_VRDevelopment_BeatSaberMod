// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.NotesRepository
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class NotesRepository
  {
    private readonly IntervalTree.IntervalTree<float, NoteEditorData> _tree = new IntervalTree.IntervalTree<float, NoteEditorData>();
    private readonly IDictionary<BeatmapEditorObjectId, NoteEditorData> _dictionary = (IDictionary<BeatmapEditorObjectId, NoteEditorData>) new Dictionary<BeatmapEditorObjectId, NoteEditorData>();

    public List<NoteEditorData> notes => this._tree.Values.ToList<NoteEditorData>();

    public void Add(NoteEditorData note)
    {
      this._dictionary.Add(note.id, note);
      this._tree.Add(note.beat, note.beat, note);
    }

    public void Add(IEnumerable<NoteEditorData> notes)
    {
      foreach (NoteEditorData note in notes)
        this.Add(note);
    }

    public void Remove(NoteEditorData note)
    {
      NoteEditorData noteEditorData;
      if (!this._dictionary.TryGetValue(note.id, out noteEditorData))
        return;
      this._dictionary.Remove(noteEditorData.id);
      this._tree.Remove(noteEditorData);
    }

    public void Remove(IEnumerable<NoteEditorData> notes)
    {
      foreach (NoteEditorData note in notes)
        this.Remove(note);
    }

    public NoteEditorData GetNoteByPosition(BeatmapObjectCellData cellData) => this._tree.Query(cellData.beat).FirstOrDefault<NoteEditorData>((Func<NoteEditorData, bool>) (note => note.column == cellData.cellPosition.x && note.row == cellData.cellPosition.y));

    public NoteEditorData GetNoteById(BeatmapEditorObjectId noteId)
    {
      NoteEditorData noteEditorData;
      return !this._dictionary.TryGetValue(noteId, out noteEditorData) ? (NoteEditorData) null : noteEditorData;
    }

    public IEnumerable<NoteEditorData> GetNotesInterval(float startBeat, float endBeat) => (IEnumerable<NoteEditorData>) this._tree.Query(startBeat, endBeat).OrderBy<NoteEditorData, float>((Func<NoteEditorData, float>) (n => n.beat));

    public bool AnyNoteExists(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow)
    {
      return this._tree.Query(startTime, endTime).Any<NoteEditorData>((Func<NoteEditorData, bool>) (n => NotesRepository.Overlap(n, startColumn, startRow, endColumn, endRow)));
    }

    public bool AnyNoteExistsWithoutIntersecting(
      float startTime,
      int startColumn,
      int startRow,
      float endTime,
      int endColumn,
      int endRow,
      BeatmapEditorObjectId avoidId)
    {
      return this._tree.Query(startTime, endTime).Where<NoteEditorData>((Func<NoteEditorData, bool>) (n => NotesRepository.Overlap(n, startColumn, startRow, endColumn, endRow))).Any<NoteEditorData>((Func<NoteEditorData, bool>) (n => n.id != avoidId));
    }

    public void Clear()
    {
      this._dictionary.Clear();
      this._tree.Clear();
    }

    private static bool Overlap(
      NoteEditorData note,
      int startColumn,
      int startRow,
      int endColumn,
      int endRow)
    {
      if (startColumn != endColumn || startRow != endRow)
        return new RectInt(startColumn, startRow, endColumn - startColumn + 1, endRow - startRow + 1).Contains(new Vector2Int(note.column, note.row));
      return note.column == startColumn && note.row == startRow;
    }
  }
}
