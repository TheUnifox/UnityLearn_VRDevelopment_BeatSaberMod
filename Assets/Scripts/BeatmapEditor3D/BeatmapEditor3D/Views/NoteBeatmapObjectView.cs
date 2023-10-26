// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.NoteBeatmapObjectView
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
  public class NoteBeatmapObjectView : AbstractBeatmapObjectView
  {
    [SerializeField]
    private NoteObjectsMouseInputSource _noteObjectsMouseInputSource;
    [Inject]
    private readonly NormalNoteView.Pool _notePool;
    [Inject]
    private readonly BombNoteView.Pool _bombPool;
    private HashSet<NoteEditorData> _currentNotes = new HashSet<NoteEditorData>();
    private readonly Dictionary<BeatmapEditorObjectId, NormalNoteView> _noteObjects = new Dictionary<BeatmapEditorObjectId, NormalNoteView>();
    private readonly Dictionary<BeatmapEditorObjectId, BombNoteView> _bombObjects = new Dictionary<BeatmapEditorObjectId, BombNoteView>();

    public IReadOnlyDictionary<BeatmapEditorObjectId, NormalNoteView> noteObjects => (IReadOnlyDictionary<BeatmapEditorObjectId, NormalNoteView>) this._noteObjects;

    public IReadOnlyDictionary<BeatmapEditorObjectId, BombNoteView> bombObjects => (IReadOnlyDictionary<BeatmapEditorObjectId, BombNoteView>) this._bombObjects;

    public override void RefreshView(float startTime, float endTime, bool clearView = false)
    {
      NoteEditorData[] array1 = this.beatmapLevelDataModel.GetNotesInterval(startTime, endTime).ToArray<NoteEditorData>();
      HashSet<NoteEditorData> newNotesHashSet = new HashSet<NoteEditorData>((IEnumerable<NoteEditorData>) array1);
      NoteEditorData[] array2 = ((IEnumerable<NoteEditorData>) array1).Where<NoteEditorData>((Func<NoteEditorData, bool>) (note => !this._currentNotes.Contains(note))).ToArray<NoteEditorData>();
      this.DeleteNotes((IReadOnlyCollection<NoteEditorData>) this._currentNotes.Where<NoteEditorData>((Func<NoteEditorData, bool>) (prevNote => !newNotesHashSet.Contains(prevNote))).ToArray<NoteEditorData>());
      this.UpdateNotes();
      this.InsertNotes((IReadOnlyCollection<NoteEditorData>) array2);
      this._currentNotes = newNotesHashSet;
    }

    public override void UpdateTimeScale()
    {
    }

    public override void ClearView()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, NormalNoteView> noteObject in this._noteObjects)
      {
        if (!((UnityEngine.Object) noteObject.Value == (UnityEngine.Object) null))
        {
          this._noteObjectsMouseInputSource.UnsubscribeFromMouseEvents((Component) noteObject.Value);
          this._notePool.Despawn(noteObject.Value);
        }
      }
      foreach (KeyValuePair<BeatmapEditorObjectId, BombNoteView> bombObject in this._bombObjects)
      {
        if (!((UnityEngine.Object) bombObject.Value == (UnityEngine.Object) null))
        {
          this._noteObjectsMouseInputSource.UnsubscribeFromMouseEvents((Component) bombObject.Value);
          this._bombPool.Despawn(bombObject.Value);
        }
      }
      this._noteObjects.Clear();
      this._bombObjects.Clear();
      this._notePool.Clear();
      this._bombPool.Clear();
      this._currentNotes = new HashSet<NoteEditorData>();
    }

    private void DeleteNotes(IReadOnlyCollection<NoteEditorData> notesToDelete)
    {
      if (notesToDelete == null || notesToDelete.Count == 0)
        return;
      foreach (NoteEditorData noteEditorData in (IEnumerable<NoteEditorData>) notesToDelete)
      {
        NormalNoteView normalNoteView;
        if (this._noteObjects.TryGetValue(noteEditorData.id, out normalNoteView))
        {
          this._notePool.Despawn(normalNoteView);
          this._noteObjects.Remove(noteEditorData.id);
          this._noteObjectsMouseInputSource.UnsubscribeFromMouseEvents((Component) normalNoteView);
        }
        BombNoteView bombNoteView;
        if (this._bombObjects.TryGetValue(noteEditorData.id, out bombNoteView))
        {
          this._bombPool.Despawn(bombNoteView);
          this._bombObjects.Remove(noteEditorData.id);
          this._noteObjectsMouseInputSource.UnsubscribeFromMouseEvents((Component) bombNoteView);
        }
      }
    }

        private void UpdateNotes()
        {
            foreach (KeyValuePair<BeatmapEditorObjectId, NormalNoteView> keyValuePair in this._noteObjects)
            {
                float num = keyValuePair.Value.noteData.beat - this.beatmapState.beat;
                Vector3 localPosition = keyValuePair.Value.transform.localPosition;
                localPosition.z = this.beatmapObjectPlacementHelper.BeatToPosition(keyValuePair.Value.noteData.beat);
                keyValuePair.Value.transform.localPosition = localPosition;
                keyValuePair.Value.SetState(Mathf.Approximately(num, 0f), num < 0f, this.beatmapObjectsSelectionState.IsNoteSelected(keyValuePair.Value.noteData.id));
            }
            foreach (KeyValuePair<BeatmapEditorObjectId, BombNoteView> keyValuePair2 in this._bombObjects)
            {
                float num2 = keyValuePair2.Value.noteData.beat - this.beatmapState.beat;
                Vector3 localPosition2 = keyValuePair2.Value.transform.localPosition;
                localPosition2.z = this.beatmapObjectPlacementHelper.BeatToPosition(keyValuePair2.Value.noteData.beat);
                keyValuePair2.Value.transform.localPosition = localPosition2;
                keyValuePair2.Value.SetState(Mathf.Approximately(num2, 0f), num2 < 0f, this.beatmapObjectsSelectionState.IsNoteSelected(keyValuePair2.Value.noteData.id));
            }
        }

        private void InsertNotes(IReadOnlyCollection<NoteEditorData> notesToInsert)
    {
      if (notesToInsert == null || notesToInsert.Count == 0)
        return;
      foreach (NoteEditorData noteData in (IEnumerable<NoteEditorData>) notesToInsert)
      {
        float a = noteData.beat - this.beatmapState.beat;
        Color color = noteData.type.ToColor();
        bool selected = this.beatmapObjectsSelectionState.IsNoteSelected(noteData.id);
        Component component;
        GameObject gameObject;
        if (noteData.noteType == NoteType.Note)
        {
          NormalNoteView normalNoteView = this._notePool.Spawn();
          this._noteObjects.Add(noteData.id, normalNoteView);
          normalNoteView.Init(noteData, color, noteData.cutDirection, noteData.angle);
          normalNoteView.SetState(Mathf.Approximately(a, 0.0f), (double) a < 0.0, selected);
          component = (Component) normalNoteView;
          gameObject = normalNoteView.gameObject;
        }
        else
        {
          BombNoteView bombNoteView = this._bombPool.Spawn();
          this._bombObjects.Add(noteData.id, bombNoteView);
          bombNoteView.Init(noteData, color);
          bombNoteView.SetState(Mathf.Approximately(a, 0.0f), (double) a < 0.0, selected);
          component = (Component) bombNoteView;
          gameObject = bombNoteView.gameObject;
        }
        this._noteObjectsMouseInputSource.SubscribeToMouseEvents(noteData.id, new BeatmapObjectCellData(new Vector2Int(noteData.column, noteData.row), noteData.beat), component);
        gameObject.transform.SetParent(this.beatmapObjectsContainer.transform, true);
        float position = this.beatmapObjectPlacementHelper.BeatToPosition(noteData.beat);
        gameObject.transform.localPosition = new Vector3((float) (((double) noteData.column - 1.5) * 0.800000011920929), this._beatmapObjectYOffset + (float) noteData.row * 0.8f, position);
        gameObject.SetActive(true);
      }
    }
  }
}
