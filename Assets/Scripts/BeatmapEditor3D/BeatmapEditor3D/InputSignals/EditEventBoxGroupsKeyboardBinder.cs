// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.EditEventBoxGroupsKeyboardBinder
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
  public class EditEventBoxGroupsKeyboardBinder
  {
    private readonly KeyboardBinder _keyboardBinder;
    private readonly SignalBus _signalBus;
    private readonly IReadonlyBeatmapState _beatmapState;

    public EditEventBoxGroupsKeyboardBinder(
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
      this._keyboardBinder.AddBinding(KeyCode.D, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleDuplicateSelection));
      this._keyboardBinder.AddBinding(KeyCode.Backspace, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleDeleteSelection));
      this._keyboardBinder.AddBinding(KeyCode.Delete, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleDeleteSelection));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleShiftKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleShiftKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleAltKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleAltKeyPress));
      this._keyboardBinder.AddBinding(KeyCode.Tab, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleChangeEventBoxGroupPage));
      this._keyboardBinder.AddBinding(KeyCode.Alpha5, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Delete))));
    }

    private void HandleClearSelection(bool _)
    {
      if (!EditEventBoxGroupsKeyboardBinder.IsControlPressed())
        return;
      this._signalBus.Fire<ClearEventBoxGroupsSelectionSignal>();
    }

    private void HandleCutSelection(bool _)
    {
      if (!EditEventBoxGroupsKeyboardBinder.IsControlPressed())
        return;
      this._signalBus.Fire<CutEventBoxGroupsSignal>();
    }

    private void HandleCopySelection(bool _)
    {
      if (!EditEventBoxGroupsKeyboardBinder.IsControlPressed())
        return;
      this._signalBus.Fire<CopyEventBoxGroupsSignal>();
    }

    private void HandlePasteClipboard(bool _)
    {
      if (!EditEventBoxGroupsKeyboardBinder.IsControlPressed())
        return;
      this._signalBus.Fire<PasteEventBoxGroupsSignal>();
    }

    private void HandleDuplicateSelection(bool _)
    {
      if (!EditEventBoxGroupsKeyboardBinder.IsControlPressed())
        return;
      this._signalBus.Fire<DuplicateSelectedEventBoxGroupsSignal>();
    }

    private void HandleAltKeyPress(bool pressed)
    {
      if (this._beatmapState.interactionMode == InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(pressed ? InteractionMode.Modify : InteractionMode.Place));
    }

    private void HandleShiftKeyPress(bool pressed) => this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(pressed ? InteractionMode.Select : InteractionMode.Place));

    private void HandleDeleteSelection(bool _) => this._signalBus.Fire<DeleteSelectedEventBoxGroupsSignal>();

    private void HandleChangeEventBoxGroupPage(bool _) => this._signalBus.Fire<ChangeEventBoxGroupsPageSignal>();

    private static bool IsControlPressed() => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
  }
}
