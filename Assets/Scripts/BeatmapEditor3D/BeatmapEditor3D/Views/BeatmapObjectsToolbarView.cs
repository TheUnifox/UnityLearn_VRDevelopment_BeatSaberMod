// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BeatmapObjectsToolbarView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class BeatmapObjectsToolbarView : MonoBehaviour
  {
    [SerializeField]
    private Toggle _noteAToggle;
    [SerializeField]
    private Toggle _noteBToggle;
    [SerializeField]
    private Toggle _bombNoteToggle;
    [SerializeField]
    private Toggle _obstacleToggle;
    [SerializeField]
    private Toggle _slidersToggle;
    [SerializeField]
    private Toggle _deleteModeToggle;
    [Space]
    [SerializeField]
    private GameObject _notesOrientationToolbar;
    [SerializeField]
    private GameObject _obstacleLengthToolbar;
    [SerializeField]
    private GameObject _sliderToolbar;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly ButtonBinder _buttonBinder = new ButtonBinder();
    private readonly ToggleBinder _toggleBinder = new ToggleBinder();
    private GameObject _prevExtraToolbar;

    protected void Start()
    {
      this._toggleBinder.AddBinding(this._noteAToggle, true, this.GetChangeNoteColorTypeSignal(ColorType.ColorA));
      this._toggleBinder.AddBinding(this._noteBToggle, true, this.GetChangeNoteColorTypeSignal(ColorType.ColorB));
      this._toggleBinder.AddBinding(this._bombNoteToggle, true, this.GetChangeBeatmapObjectTypeSignal(BeatmapObjectType.Bomb));
      this._toggleBinder.AddBinding(this._obstacleToggle, true, this.GetChangeBeatmapObjectTypeSignal(BeatmapObjectType.Obstacle));
      this._toggleBinder.AddBinding(this._slidersToggle, true, this.GetChangeBeatmapObjectTypeSignal(BeatmapObjectType.Arc));
      this._toggleBinder.AddBinding(this._deleteModeToggle, true, (Action) (() =>
      {
        if (this._beatmapState.cameraMoving)
          return;
        this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Delete));
      }));
    }

    protected void OnDestroy()
    {
      this._toggleBinder.ClearBindings();
      this._buttonBinder.ClearBindings();
    }

    protected void OnEnable()
    {
      this._signalBus.Subscribe<InteractionModeChangedSignal>(new Action(this.HandleBeatmapObjectModeChanged));
      this._signalBus.Subscribe<BeatmapObjectTypeChangedSignal>(new Action(this.HandleBeatmapObjectTypeChanged));
      this._signalBus.Subscribe<NoteColorTypeChangedSignal>(new Action(this.HandleNoteColorTypeChanged));
      this.UpdateSelection();
    }

    protected void OnDisable()
    {
      this._signalBus.TryUnsubscribe<InteractionModeChangedSignal>(new Action(this.HandleBeatmapObjectModeChanged));
      this._signalBus.TryUnsubscribe<BeatmapObjectTypeChangedSignal>(new Action(this.HandleBeatmapObjectTypeChanged));
      this._signalBus.TryUnsubscribe<NoteColorTypeChangedSignal>(new Action(this.HandleNoteColorTypeChanged));
    }

    private void HandleBeatmapObjectModeChanged() => this.UpdateSelection();

    private void HandleBeatmapObjectTypeChanged() => this.UpdateSelection();

    private void HandleNoteColorTypeChanged() => this.UpdateSelection();

    private void UpdateSelection()
    {
      BeatmapObjectType beatmapObjectType = this._beatmapObjectsState.beatmapObjectType;
      if (this._beatmapState.interactionMode == InteractionMode.Delete)
      {
        if ((UnityEngine.Object) this._prevExtraToolbar != (UnityEngine.Object) null)
          this._prevExtraToolbar.SetActive(false);
        this._deleteModeToggle.SetIsOnWithoutNotify(true);
      }
      else
      {
        this.GetToggleByBeatmapObjectType(beatmapObjectType).SetIsOnWithoutNotify(true);
        GameObject extraToolbar = this.GetExtraToolbar(beatmapObjectType);
        if ((UnityEngine.Object) this._prevExtraToolbar != (UnityEngine.Object) null)
          this._prevExtraToolbar.SetActive(false);
        if ((UnityEngine.Object) extraToolbar != (UnityEngine.Object) null)
          extraToolbar.SetActive(true);
        this._prevExtraToolbar = extraToolbar;
      }
    }

    private Toggle GetToggleByBeatmapObjectType(BeatmapObjectType editorBeatmapObjectType)
    {
      switch (editorBeatmapObjectType)
      {
        case BeatmapObjectType.Note:
          return this._beatmapObjectsState.noteColorType != ColorType.ColorA ? this._noteBToggle : this._noteAToggle;
        case BeatmapObjectType.Bomb:
          return this._bombNoteToggle;
        case BeatmapObjectType.Obstacle:
          return this._obstacleToggle;
        case BeatmapObjectType.Arc:
          return this._slidersToggle;
        default:
          return (Toggle) null;
      }
    }

    private GameObject GetExtraToolbar(BeatmapObjectType editorBeatmapObjectType)
    {
      switch (editorBeatmapObjectType)
      {
        case BeatmapObjectType.Note:
          return this._notesOrientationToolbar;
        case BeatmapObjectType.Obstacle:
          return this._obstacleLengthToolbar;
        case BeatmapObjectType.Arc:
          return this._sliderToolbar;
        default:
          return (GameObject) null;
      }
    }

    private Action GetChangeBeatmapObjectTypeSignal(BeatmapObjectType type) => (Action) (() =>
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._signalBus.Fire<ChangeBeatmapObjectTypeSignal>(new ChangeBeatmapObjectTypeSignal(type));
    });

    private Action GetChangeNoteColorTypeSignal(ColorType colorType) => (Action) (() =>
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._signalBus.Fire<ChangeBeatmapObjectTypeSignal>(new ChangeBeatmapObjectTypeSignal(BeatmapObjectType.Note));
      this._signalBus.Fire<ChangeNoteColorTypeSignal>(new ChangeNoteColorTypeSignal(colorType));
    });
  }
}
