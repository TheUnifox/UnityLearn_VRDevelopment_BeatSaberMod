// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EditBeatmapObjectsKeyboardBinder
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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class EditBeatmapObjectsKeyboardBinder
  {
    private readonly KeyboardBinder _keyboardBinder;
    private readonly SignalBus _signalBus;
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    private readonly IReadonlyBeatmapState _beatmapState;
    private readonly List<Tuple<KeyCode[], NoteCutDirection>> _directionKeyCombos = new List<Tuple<KeyCode[], NoteCutDirection>>();

    public EditBeatmapObjectsKeyboardBinder(
      KeyboardBinder keyboardBinder,
      SignalBus signalBus,
      IReadonlyBeatmapObjectsState readonlyBeatmapObjectsState,
      IReadonlyBeatmapState beatmapState)
    {
      this._keyboardBinder = keyboardBinder;
      this._signalBus = signalBus;
      this._readonlyBeatmapObjectsState = readonlyBeatmapObjectsState;
      this._beatmapState = beatmapState;
      this._directionKeyCombos.Add(new Tuple<KeyCode[], NoteCutDirection>(new KeyCode[2]
      {
        KeyCode.A,
        KeyCode.W
      }, NoteCutDirection.UpLeft));
      this._directionKeyCombos.Add(new Tuple<KeyCode[], NoteCutDirection>(new KeyCode[2]
      {
        KeyCode.W,
        KeyCode.D
      }, NoteCutDirection.UpRight));
      this._directionKeyCombos.Add(new Tuple<KeyCode[], NoteCutDirection>(new KeyCode[2]
      {
        KeyCode.D,
        KeyCode.S
      }, NoteCutDirection.DownRight));
      this._directionKeyCombos.Add(new Tuple<KeyCode[], NoteCutDirection>(new KeyCode[2]
      {
        KeyCode.S,
        KeyCode.A
      }, NoteCutDirection.DownLeft));
      this._directionKeyCombos.Add(new Tuple<KeyCode[], NoteCutDirection>(new KeyCode[1]
      {
        KeyCode.W
      }, NoteCutDirection.Up));
      this._directionKeyCombos.Add(new Tuple<KeyCode[], NoteCutDirection>(new KeyCode[1]
      {
        KeyCode.A
      }, NoteCutDirection.Left));
      this._directionKeyCombos.Add(new Tuple<KeyCode[], NoteCutDirection>(new KeyCode[1]
      {
        KeyCode.S
      }, NoteCutDirection.Down));
      this._directionKeyCombos.Add(new Tuple<KeyCode[], NoteCutDirection>(new KeyCode[1]
      {
        KeyCode.D
      }, NoteCutDirection.Right));
    }

    public void Enable()
    {
      this._keyboardBinder.AddBinding(KeyCode.X, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleCutKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.C, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleCopyKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.V, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandlePasteKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.Delete, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleDeleteKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.Backspace, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleDeleteKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleSelectAllKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleShiftKeyPressDown));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleShiftKeyPressUp));
      this._keyboardBinder.AddBinding(KeyCode.UpArrow, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleUpArrowKeyUp));
      this._keyboardBinder.AddBinding(KeyCode.RightArrow, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleRightArrowKeyUp));
      this._keyboardBinder.AddBinding(KeyCode.DownArrow, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleDownArrowKeyUp));
      this._keyboardBinder.AddBinding(KeyCode.LeftArrow, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleLeftArrowKeyUp));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleAltPressed));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleAltPressed));
      this._keyboardBinder.AddBinding(KeyCode.RightAlt, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleAltPressed));
      this._keyboardBinder.AddBinding(KeyCode.RightAlt, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleAltPressed));
      this.BindDirectionKey(KeyCode.W);
      this.BindDirectionKey(KeyCode.A);
      this.BindDirectionKey(KeyCode.S);
      this.BindDirectionKey(KeyCode.D);
      this._keyboardBinder.AddBinding(KeyCode.F, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleNoteCutDirectionAny));
      this.BindKey(KeyCode.Alpha1, (object) new ChangeBeatmapObjectTypeSignal(BeatmapObjectType.Note));
      this.BindKey(KeyCode.Alpha1, (object) new ChangeNoteColorTypeSignal(ColorType.ColorA));
      this.BindKey(KeyCode.Alpha2, (object) new ChangeBeatmapObjectTypeSignal(BeatmapObjectType.Note));
      this.BindKey(KeyCode.Alpha2, (object) new ChangeNoteColorTypeSignal(ColorType.ColorB));
      this.BindKey(KeyCode.Alpha3, (object) new ChangeBeatmapObjectTypeSignal(BeatmapObjectType.Bomb));
      this.BindKey(KeyCode.Alpha4, (object) new ChangeBeatmapObjectTypeSignal(BeatmapObjectType.Obstacle));
      this.BindKey(KeyCode.Alpha5, (object) new ChangeInteractionModeSignal(InteractionMode.Delete));
      this.BindKey(KeyCode.Alpha7, (object) new ChangeBeatmapObjectTypeSignal(BeatmapObjectType.Arc));
      this.BindKey(KeyCode.Q, (object) new ChangeArcMidAnchorModeSignal(SliderMidAnchorMode.Clockwise));
      this.BindKey(KeyCode.E, (object) new ChangeArcMidAnchorModeSignal(SliderMidAnchorMode.CounterClockwise));
      this.BindKey(KeyCode.R, (object) new ChangeArcMidAnchorModeSignal(SliderMidAnchorMode.Straight));
    }

    private void HandleCutKeyPress(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<CutBeatmapObjectsSignal>();
    }

    private void HandleCopyKeyPress(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<CopyBeatmapObjectsSignal>();
    }

    private void HandlePasteKeyPress(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<PasteBeatmapObjectsSignal>();
    }

    private void HandleDeleteKeyPress(bool pressed) => this._signalBus.Fire<DeleteBeatmapObjectsSignal>();

    private void HandleSelectAllKeyPress(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        this._signalBus.Fire<DeselectAllBeatmapObjectsSignal>();
      else
        this._signalBus.Fire<DeselectAllBeatmapObjectsSignal>();
    }

    private void HandleShiftKeyPressDown(bool pressed)
    {
      if (this._beatmapState.interactionMode == InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Select));
    }

    private void HandleShiftKeyPressUp(bool pressed)
    {
      if (this._beatmapState.interactionMode == InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Place));
    }

    private void HandleUpArrowKeyUp(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        return;
      if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        this._signalBus.Fire<MoveBeatmapObjectSelectionInTimeSignal>(new MoveBeatmapObjectSelectionInTimeSignal(MoveBeatmapObjectSelectionInTimeSignal.Direction.Forward));
      else
        this._signalBus.Fire<MoveBeatmapObjectSelectionOnGridSignal>(new MoveBeatmapObjectSelectionOnGridSignal(MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Up));
    }

    private void HandleRightArrowKeyUp(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        return;
      this._signalBus.Fire<MoveBeatmapObjectSelectionOnGridSignal>(new MoveBeatmapObjectSelectionOnGridSignal(MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Right));
    }

    private void HandleDownArrowKeyUp(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        return;
      if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        this._signalBus.Fire<MoveBeatmapObjectSelectionInTimeSignal>(new MoveBeatmapObjectSelectionInTimeSignal(MoveBeatmapObjectSelectionInTimeSignal.Direction.Backward));
      else
        this._signalBus.Fire<MoveBeatmapObjectSelectionOnGridSignal>(new MoveBeatmapObjectSelectionOnGridSignal(MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Down));
    }

    private void HandleLeftArrowKeyUp(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        return;
      this._signalBus.Fire<MoveBeatmapObjectSelectionOnGridSignal>(new MoveBeatmapObjectSelectionOnGridSignal(MoveBeatmapObjectSelectionOnGridSignal.MoveDirection.Left));
    }

    private void HandleAltPressed(bool pressed)
    {
      if (this._beatmapState.interactionMode == InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(pressed ? InteractionMode.Modify : InteractionMode.Place));
    }

    private void HandleNoteCutDirectionAny(bool _)
    {
      if (this._readonlyBeatmapObjectsState.noteCutDirection == NoteCutDirection.Any)
      {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
          this._signalBus.Fire<ChangeHoveredDotNoteAngleSignal>();
        this._signalBus.Fire<ChangeDotNoteAngleSignal>(new ChangeDotNoteAngleSignal());
      }
      else
        this._signalBus.Fire<ChangeNoteCutDirectionSignal>(new ChangeNoteCutDirectionSignal(NoteCutDirection.Any));
    }

    public void Disable() => this._keyboardBinder.ClearBindings();

    private void BindKey(KeyCode keyCode, object signal) => this._keyboardBinder.AddBinding(keyCode, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (keyPressed => this._signalBus.Fire(signal)));

    private void BindDirectionKey(KeyCode keyCode) => this._keyboardBinder.AddBinding(keyCode, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleNoteCutDirectionKeyPress));

    private void HandleNoteCutDirectionKeyPress(bool keyPressed)
    {
      if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        return;
      foreach (Tuple<KeyCode[], NoteCutDirection> directionKeyCombo in this._directionKeyCombos)
      {
        if (EditBeatmapObjectsKeyboardBinder.AllKeysPressed(directionKeyCombo.Item1))
        {
          this._signalBus.Fire<ChangeNoteCutDirectionSignal>(new ChangeNoteCutDirectionSignal(directionKeyCombo.Item2));
          break;
        }
      }
    }

    private static bool AllKeysPressed(KeyCode[] keys) => ((IEnumerable<KeyCode>) keys).All<KeyCode>(new Func<KeyCode, bool>(Input.GetKey));
  }
}
