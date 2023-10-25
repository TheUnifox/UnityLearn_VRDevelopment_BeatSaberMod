// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EditBasicEventsKeyboardBinder
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using HMUI;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class EditBasicEventsKeyboardBinder
  {
    private readonly KeyboardBinder _keyboardBinder;
    private readonly SignalBus _signalBus;
    private readonly IReadonlyBeatmapState _beatmapState;

    public EditBasicEventsKeyboardBinder(
      KeyboardBinder keyboardBinder,
      SignalBus signalBus,
      IReadonlyBeatmapState beatmapState)
    {
      this._keyboardBinder = keyboardBinder;
      this._signalBus = signalBus;
      this._beatmapState = beatmapState;
    }

    public void Enable()
    {
      this.BindKey(KeyCode.Alpha5, (object) new ChangeInteractionModeSignal(InteractionMode.Delete));
      this.BindKey(KeyCode.Tab, (object) new ChangeEventsPageSignal());
      this._keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleClearEventsSelection));
      this._keyboardBinder.AddBinding(KeyCode.X, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleCutKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.C, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleCopyKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.V, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandlePasteKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.Backspace, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleDeleteKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.Delete, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleDeleteKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleShiftKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleShiftKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleAltKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleAltKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.UpArrow, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleUpArrowKeyUp));
      this._keyboardBinder.AddBinding(KeyCode.DownArrow, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleDownArrowKeyUp));
      this._keyboardBinder.AddBinding(KeyCode.LeftArrow, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleLeftArrowKeyUp));
      this._keyboardBinder.AddBinding(KeyCode.RightArrow, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleRightArrowKeyUp));
    }

    private void HandleCutKeyPress(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<CutEventsSignal>();
    }

    private void HandleDeleteKeyPress(bool pressed) => this._signalBus.Fire<DeleteSelectedEventsSignal>();

    private void HandlePasteKeyPress(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<PasteEventsSignal>();
    }

    private void HandleCopyKeyPress(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<CopyEventsSignal>();
    }

    private void HandleClearEventsSelection(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        return;
      this._signalBus.Fire<ClearEventsSelectionSignal>();
    }

    private void HandleShiftKeyPress(bool pressed) => this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(pressed ? InteractionMode.Select : InteractionMode.Place));

    private void HandleAltKeyPress(bool pressed)
    {
      if (this._beatmapState.interactionMode == InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(pressed ? InteractionMode.Modify : InteractionMode.Place));
    }

    private void HandleUpArrowKeyUp(bool pressed)
    {
      if ((!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt) || !Input.GetKey(KeyCode.LeftShift)) && !Input.GetKey(KeyCode.RightShift))
        return;
      this._signalBus.Fire<MoveEventsSelectionInTimeSignal>(new MoveEventsSelectionInTimeSignal(MoveEventsSelectionInTimeSignal.Direction.Forward));
    }

    private void HandleDownArrowKeyUp(bool pressed)
    {
      if ((!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt) || !Input.GetKey(KeyCode.LeftShift)) && !Input.GetKey(KeyCode.RightShift))
        return;
      this._signalBus.Fire<MoveEventsSelectionInTimeSignal>(new MoveEventsSelectionInTimeSignal(MoveEventsSelectionInTimeSignal.Direction.Backward));
    }

    private void HandleLeftArrowKeyUp(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        return;
      this._signalBus.Fire<MoveEventsSelectionSignal>(new MoveEventsSelectionSignal(MoveEventsSelectionSignal.MoveDirection.Left));
    }

    private void HandleRightArrowKeyUp(bool pressed)
    {
      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        return;
      this._signalBus.Fire<MoveEventsSelectionSignal>(new MoveEventsSelectionSignal(MoveEventsSelectionSignal.MoveDirection.Right));
    }

    private void BindKey(KeyCode keyCode, object signal) => this._keyboardBinder.AddBinding(keyCode, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this._signalBus.Fire(signal)));
  }
}
