// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.NoteBeatmapObjectPreview
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Visuals;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class NoteBeatmapObjectPreview : AbstractBeatmapObjectPreview
  {
    [SerializeField]
    private NormalNoteView _notePrefab;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;
    [Inject]
    private readonly SignalBus _signalBus;
    private RectInt _objectRect;

    public override void Show()
    {
      base.Show();
      this._signalBus.Subscribe<NoteCutDirectionChangedSignal>(new Action(this.HandleNoteCutDirectionChanged));
      this._signalBus.Subscribe<DotNoteAngleChangedSignal>(new Action(this.HandleDotNoteAngleChanged));
      this._signalBus.Subscribe<NoteColorTypeChangedSignal>(new Action(this.HandleNoteColorTypeChangedSignal));
    }

    public override void Hide()
    {
      base.Hide();
      this._signalBus.TryUnsubscribe<NoteCutDirectionChangedSignal>(new Action(this.HandleNoteCutDirectionChanged));
      this._signalBus.TryUnsubscribe<DotNoteAngleChangedSignal>(new Action(this.HandleDotNoteAngleChanged));
      this._signalBus.TryUnsubscribe<NoteColorTypeChangedSignal>(new Action(this.HandleNoteColorTypeChangedSignal));
    }

    public override void Preview(RectInt objectRect)
    {
      if (this._beatmapObjectsState.beatmapObjectType == BeatmapObjectType.Obstacle || this._beatmapObjectsState.beatmapObjectType == BeatmapObjectType.Arc)
        return;
      Vector3 vector3 = new Vector3((float) (((double) objectRect.x - 1.5) * 0.800000011920929), (float) objectRect.y * 0.8f, 0.0f);
      this._notePrefab.Init((NoteEditorData) null, BeatmapObjectViewColorHelper.GetBeatmapObjectColor(this._beatmapObjectsState.noteColorType.ToColor(), AbstractBeatmapObjectView.BeatmapObjectEditState.Preview, 0.0f), this._beatmapObjectsState.noteCutDirection, this._beatmapObjectsState.noteAngle);
      this._notePrefab.SetState(false, false, false);
      this._objectTransform.transform.localPosition = vector3;
      this._objectRect = objectRect;
    }

    private void HandleNoteCutDirectionChanged() => this.Preview(this._objectRect);

    private void HandleDotNoteAngleChanged() => this.Preview(this._objectRect);

    private void HandleNoteColorTypeChangedSignal() => this.Preview(this._objectRect);
  }
}
