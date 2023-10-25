// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.EditEventBoxesKeyboardBinder
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

namespace BeatmapEditor3D.InputSignals
{
  public class EditEventBoxesKeyboardBinder
  {
    private readonly KeyboardBinder _keyboardBinder;
    private readonly SignalBus _signalBus;
    private readonly IReadonlyBeatmapState _beatmapState;

    public EditEventBoxesKeyboardBinder(
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
      this._keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleClearSelection));
      this._keyboardBinder.AddBinding(KeyCode.X, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleCutSelection));
      this._keyboardBinder.AddBinding(KeyCode.C, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleCopySelection));
      this._keyboardBinder.AddBinding(KeyCode.V, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandlePasteClipboard));
      this._keyboardBinder.AddBinding(KeyCode.Backspace, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleDeleteSelection));
      this._keyboardBinder.AddBinding(KeyCode.Delete, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleDeleteSelection));
      this._keyboardBinder.AddBinding(KeyCode.Escape, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleExitEventBoxesEditing));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleShiftKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleShiftKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleAltKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleAltKeyPress));
      this.BindKey(KeyCode.Alpha5, (object) new ChangeInteractionModeSignal(InteractionMode.Delete));
    }

    private void HandleClearSelection(bool _)
    {
      if (!EditEventBoxesKeyboardBinder.IsControlPressed())
        return;
      this._signalBus.Fire<ClearEventBoxesSelectionSignal>();
    }

    private void HandleCutSelection(bool _)
    {
      if (!EditEventBoxesKeyboardBinder.IsControlPressed())
        return;
      this._signalBus.Fire<CutEventBoxEventsSignal>();
    }

    private void HandleCopySelection(bool _)
    {
      if (!EditEventBoxesKeyboardBinder.IsControlPressed())
        return;
      this._signalBus.Fire<CopyEventBoxEventsSignal>();
    }

    private void HandlePasteClipboard(bool _)
    {
      if (!EditEventBoxesKeyboardBinder.IsControlPressed())
        return;
      this._signalBus.Fire<PasteEventBoxEventsSignal>();
    }

    private void HandleDeleteSelection(bool _) => this._signalBus.Fire<DeleteSelectedEventBoxEventsSignal>();

    private void HandleExitEventBoxesEditing(bool _) => this._signalBus.Fire<ExitEditEventBoxGroupSignal>();

    private void HandleAltKeyPress(bool pressed)
    {
      if (this._beatmapState.interactionMode == InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(pressed ? InteractionMode.Modify : InteractionMode.Place));
    }

    private void HandleShiftKeyPress(bool pressed) => this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(pressed ? InteractionMode.Select : InteractionMode.Place));

    private void BindKey(KeyCode keyCode, object signal) => this._keyboardBinder.AddBinding(keyCode, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (keyPressed => this._signalBus.Fire(signal)));

    private static bool IsControlPressed() => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
  }
}
