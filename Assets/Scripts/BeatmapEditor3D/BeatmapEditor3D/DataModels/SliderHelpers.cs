// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.SliderHelpers
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public static class SliderHelpers
  {
    public static (SliderEditorData, NoteEditorData, NoteEditorData) FlipSlider(
      SliderEditorData s,
      NoteEditorData headNote,
      NoteEditorData tailNote,
      bool flip)
    {
      return !flip ? (SliderEditorData.CreateNewWithId(s.id, s.type, s.colorType, s.beat, s.column, s.row, s.cutDirection, s.controlPointLengthMultiplier, s.tailBeat, s.tailColumn, s.tailRow, s.tailCutDirection, s.tailControlPointLengthMultiplier, s.midAnchorMode, s.sliceCount, s.squishAmount), headNote, tailNote) : (SliderEditorData.CreateNewWithId(s.id, s.type, s.colorType, s.tailBeat, s.tailColumn, s.tailRow, s.tailCutDirection, s.tailControlPointLengthMultiplier, s.beat, s.column, s.row, s.cutDirection, s.controlPointLengthMultiplier, s.midAnchorMode, s.sliceCount, s.squishAmount), tailNote, headNote);
    }

    public static Dictionary<BeatmapEditorObjectId, (BeatmapEditorObjectId headNoteId, BeatmapEditorObjectId tailNoteId)> GetSlidersToNotesMap(
      BeatmapLevelDataModel beatmapLevelDataModel,
      IEnumerable<NoteEditorData> notes)
    {
      return new Dictionary<BeatmapEditorObjectId, (BeatmapEditorObjectId, BeatmapEditorObjectId)>();
    }

    public static IEnumerable<(ArcEditorData, NoteEditorData, NoteEditorData)> GetSlidersFromDictionary(
      BeatmapLevelDataModel beatmapLevelDataModel,
      Dictionary<BeatmapEditorObjectId, (BeatmapEditorObjectId headNoteId, BeatmapEditorObjectId tailNoteId)> slidersToNotesMap)
    {
      return (IEnumerable<(ArcEditorData, NoteEditorData, NoteEditorData)>) slidersToNotesMap.Select<KeyValuePair<BeatmapEditorObjectId, (BeatmapEditorObjectId, BeatmapEditorObjectId)>, (ArcEditorData, NoteEditorData, NoteEditorData)>((Func<KeyValuePair<BeatmapEditorObjectId, (BeatmapEditorObjectId, BeatmapEditorObjectId)>, (ArcEditorData, NoteEditorData, NoteEditorData)>) (item => (beatmapLevelDataModel.GetArcById(item.Key), beatmapLevelDataModel.GetNoteById(item.Value.Item1), beatmapLevelDataModel.GetNoteById(item.Value.Item2)))).ToList<(ArcEditorData, NoteEditorData, NoteEditorData)>();
    }

    public static void WalkArcsAndChains(
      BeatmapLevelDataModel model,
      BeatmapObjectCellData origin,
      out List<NoteEditorData> notes,
      out List<ArcEditorData> arcs,
      out List<ChainEditorData> chains)
    {
      Queue<BeatmapObjectCellData> queue = new Queue<BeatmapObjectCellData>();
      queue.Enqueue(origin);
      HashSet<BeatmapEditorObjectId> processedSet = new HashSet<BeatmapEditorObjectId>();
      notes = new List<NoteEditorData>();
      arcs = new List<ArcEditorData>();
      chains = new List<ChainEditorData>();
      while (queue.Count > 0)
      {
        BeatmapObjectCellData cellData = queue.Dequeue();
        NoteEditorData note = model.GetNote(cellData);
        if (note != (NoteEditorData) null)
          InsertBeatmapObjectToList<NoteEditorData>((ICollection<NoteEditorData>) notes, note);
        ArcEditorData arcByHead = model.GetArcByHead(cellData);
        if (arcByHead != (ArcEditorData) null)
        {
          InsertBeatmapObjectCellToQueue(arcByHead.id, new BeatmapObjectCellData(new Vector2Int(arcByHead.tailColumn, arcByHead.tailRow), arcByHead.tailBeat));
          InsertBeatmapObjectToList<ArcEditorData>((ICollection<ArcEditorData>) arcs, arcByHead);
        }
        ArcEditorData arcByTail = model.GetArcByTail(cellData);
        if (arcByTail != (ArcEditorData) null)
        {
          InsertBeatmapObjectCellToQueue(arcByTail.id, new BeatmapObjectCellData(new Vector2Int(arcByTail.column, arcByTail.row), arcByTail.beat));
          InsertBeatmapObjectToList<ArcEditorData>((ICollection<ArcEditorData>) arcs, arcByTail);
        }
        ChainEditorData chainByHead = model.GetChainByHead(cellData);
        if (chainByHead != (ChainEditorData) null)
        {
          InsertBeatmapObjectCellToQueue(chainByHead.id, new BeatmapObjectCellData(new Vector2Int(chainByHead.tailColumn, chainByHead.tailRow), chainByHead.tailBeat));
          InsertBeatmapObjectToList<ChainEditorData>((ICollection<ChainEditorData>) chains, chainByHead);
        }
        ChainEditorData chainByTail = model.GetChainByTail(cellData);
        if (chainByTail != (ChainEditorData) null)
        {
          InsertBeatmapObjectCellToQueue(chainByTail.id, new BeatmapObjectCellData(new Vector2Int(chainByTail.column, chainByTail.row), chainByTail.beat));
          InsertBeatmapObjectToList<ChainEditorData>((ICollection<ChainEditorData>) chains, chainByTail);
        }
      }

      void InsertBeatmapObjectToList<T>(ICollection<T> list, T item) where T : BaseEditorData
      {
        if (!processedSet.Contains(item.id))
          list.Add(item);
        processedSet.Add(item.id);
      }

      void InsertBeatmapObjectCellToQueue(BeatmapEditorObjectId id, BeatmapObjectCellData cellData)
      {
        if (processedSet.Contains(id))
          return;
        queue.Enqueue(cellData);
      }
    }

    public static void InvertColorTypeForBeatmapObjects(
      List<NoteEditorData> originalNotes,
      List<ArcEditorData> originalArcs,
      List<ChainEditorData> originalChains,
      out List<NoteEditorData> invertedNotes,
      out List<ArcEditorData> invertedArcs,
      out List<ChainEditorData> invertedChains)
    {
      invertedNotes = originalNotes.Select<NoteEditorData, NoteEditorData>((Func<NoteEditorData, NoteEditorData>) (n => NoteEditorData.CopyWithModifications(n, type: new ColorType?(n.type.Opposite())))).ToList<NoteEditorData>();
      invertedArcs = originalArcs.Select<ArcEditorData, ArcEditorData>((Func<ArcEditorData, ArcEditorData>) (a => ArcEditorData.CopyWithModifications(a, colorType: new ColorType?(a.colorType.Opposite())))).ToList<ArcEditorData>();
      invertedChains = originalChains.Select<ChainEditorData, ChainEditorData>((Func<ChainEditorData, ChainEditorData>) (c => ChainEditorData.CopyWithModifications(c, colorType: new ColorType?(c.colorType.Opposite())))).ToList<ChainEditorData>();
    }
  }
}
