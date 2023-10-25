// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EditBpmGridNavigationViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor;
using BeatmapEditor3D.BpmEditor.Commands;
using BeatmapEditor3D.BpmEditor.Commands.Tools;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class EditBpmGridNavigationViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private Button _selectButton;
    [SerializeField]
    private Button _splitButton;
    [SerializeField]
    private Button _mergeButton;
    [Space]
    [SerializeField]
    private Button _noSnapButton;
    [SerializeField]
    private Button _roundSnapButton;
    [SerializeField]
    private Button _floorSnapButton;
    [SerializeField]
    private Button _ceilSnapButton;
    [Space]
    [SerializeField]
    private Toggle _oneBeatToggle;
    [SerializeField]
    private Toggle _stretchToggle;
    [Space]
    [SerializeField]
    private Button _importMidiButton;
    [Inject]
    private readonly BpmEditorState _bpmEditorState;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();
    private readonly ToggleBinder _toggleBinder = new ToggleBinder();

    protected void Update() => this._keyboardBinder.ManualUpdate();

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      this.SetButtonsState();
      this.SetSnapButtonsState();
      this.SetOneBeatToggleState();
      this.SetStretchToggleState();
      if (!addedToHierarchy)
        return;
      this.buttonBinder.AddBinding(this._selectButton, this.GetSwitchToolButtonHandler(BpmEditorToolType.Select));
      this.buttonBinder.AddBinding(this._splitButton, this.GetSwitchToolButtonHandler(BpmEditorToolType.Split));
      this.buttonBinder.AddBinding(this._mergeButton, this.GetSwitchToolButtonHandler(BpmEditorToolType.Merge));
      this.buttonBinder.AddBinding(this._noSnapButton, this.GetSwitchSnapButtonHandler(BpmEditorToolSnapType.No));
      this.buttonBinder.AddBinding(this._roundSnapButton, this.GetSwitchSnapButtonHandler(BpmEditorToolSnapType.Round));
      this.buttonBinder.AddBinding(this._floorSnapButton, this.GetSwitchSnapButtonHandler(BpmEditorToolSnapType.Floor));
      this.buttonBinder.AddBinding(this._ceilSnapButton, this.GetSwitchSnapButtonHandler(BpmEditorToolSnapType.Ceil));
      this.buttonBinder.AddBinding(this._importMidiButton, new Action(this.HandleImportMidiPressed));
      this._toggleBinder.AddBinding(this._oneBeatToggle, new Action<bool>(this.HandleOneBeatToggleChanged));
      this._toggleBinder.AddBinding(this._stretchToggle, new Action<bool>(this.HandleStretchToggleChanged));
      this._keyboardBinder.AddBinding(KeyCode.Alpha1, KeyboardBinder.KeyBindingType.KeyDown, this.GetSwitchToolKeyboardHandler(BpmEditorToolType.Select));
      this._keyboardBinder.AddBinding(KeyCode.Alpha2, KeyboardBinder.KeyBindingType.KeyDown, this.GetSwitchToolKeyboardHandler(BpmEditorToolType.Split));
      this._keyboardBinder.AddBinding(KeyCode.Alpha3, KeyboardBinder.KeyBindingType.KeyDown, this.GetSwitchToolKeyboardHandler(BpmEditorToolType.Merge));
      this._keyboardBinder.AddBinding(KeyCode.Q, KeyboardBinder.KeyBindingType.KeyDown, this.GetSwitchSnapKeyboardHandler(BpmEditorToolSnapType.No));
      this._keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, this.GetSwitchSnapKeyboardHandler(BpmEditorToolSnapType.Round));
      this._keyboardBinder.AddBinding(KeyCode.E, KeyboardBinder.KeyBindingType.KeyDown, this.GetSwitchSnapKeyboardHandler(BpmEditorToolSnapType.Floor));
      this._keyboardBinder.AddBinding(KeyCode.R, KeyboardBinder.KeyBindingType.KeyDown, this.GetSwitchSnapKeyboardHandler(BpmEditorToolSnapType.Ceil));
      this._keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleOneBeatKeyPressed));
      this._keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleStretchKeyPressed));
      this._signalBus.Subscribe<BpmToolSwitchedSignal>(new Action(this.HandleBpmToolSwitched));
      this._signalBus.Subscribe<BpmToolSnapTypeSwitchedSignal>(new Action(this.HandleBpmToolSnapSwitched));
      this._signalBus.Subscribe<BpmToolOneBeatToggleChanged>(new Action(this.HandleBpmToolOneBeatToggleChanged));
      this._signalBus.Subscribe<BpmToolStretchChanged>(new Action(this.HandleBpmToolStretchChanged));
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      if (!removedFromHierarchy)
        return;
      this.buttonBinder.ClearBindings();
      this._keyboardBinder.ClearBindings();
      this._toggleBinder.ClearBindings();
      this._signalBus.TryUnsubscribe<BpmToolSwitchedSignal>(new Action(this.HandleBpmToolSwitched));
      this._signalBus.TryUnsubscribe<BpmToolSnapTypeSwitchedSignal>(new Action(this.HandleBpmToolSnapSwitched));
      this._signalBus.TryUnsubscribe<BpmToolOneBeatToggleChanged>(new Action(this.HandleBpmToolOneBeatToggleChanged));
      this._signalBus.TryUnsubscribe<BpmToolStretchChanged>(new Action(this.HandleBpmToolStretchChanged));
    }

    private void HandleBpmToolSwitched() => this.SetButtonsState();

    private void HandleBpmToolSnapSwitched() => this.SetSnapButtonsState();

    private void HandleBpmToolOneBeatToggleChanged() => this.SetOneBeatToggleState();

    private void HandleBpmToolStretchChanged() => this.SetStretchToggleState();

    private void HandleOneBeatToggleChanged(bool isOn) => this._signalBus.Fire<BpmToolOneBeatToggleSignal>(new BpmToolOneBeatToggleSignal(isOn));

    private void HandleOneBeatKeyPressed(bool _) => this._signalBus.Fire<BpmToolOneBeatToggleSignal>(new BpmToolOneBeatToggleSignal(!this._bpmEditorState.splitRegionSingleBeat));

    private void HandleStretchToggleChanged(bool isOn) => this._signalBus.Fire<BpmToolStretchToggleSignal>(new BpmToolStretchToggleSignal(isOn));

    private void HandleStretchKeyPressed(bool _) => this._signalBus.Fire<BpmToolStretchToggleSignal>(new BpmToolStretchToggleSignal(!this._bpmEditorState.stretchRegion));

    private void HandleImportMidiPressed() => this._signalBus.Fire<ImportMidiTempoMapSignal>(new ImportMidiTempoMapSignal(NativeFileDialogs.OpenFileDialog("Open MIDI Tempo Map", "mid", "")));

    private Action GetSwitchToolButtonHandler(BpmEditorToolType toolType) => (Action) (() => this._signalBus.Fire<SwitchBpmToolSignal>(new SwitchBpmToolSignal(toolType)));

    private Action<bool> GetSwitchToolKeyboardHandler(BpmEditorToolType toolType) => (Action<bool>) (_ => this._signalBus.Fire<SwitchBpmToolSignal>(new SwitchBpmToolSignal(toolType)));

    private Action GetSwitchSnapButtonHandler(BpmEditorToolSnapType snapType) => (Action) (() => this._signalBus.Fire<SwitchBpmToolSnapTypeSignal>(new SwitchBpmToolSnapTypeSignal(snapType)));

    private Action<bool> GetSwitchSnapKeyboardHandler(BpmEditorToolSnapType snapType) => (Action<bool>) (_ => this._signalBus.Fire<SwitchBpmToolSnapTypeSignal>(new SwitchBpmToolSnapTypeSignal(snapType)));

    private void SetButtonsState()
    {
      this._selectButton.interactable = this._bpmEditorState.bpmToolType != 0;
      this._splitButton.interactable = this._bpmEditorState.bpmToolType != BpmEditorToolType.Split;
      this._mergeButton.interactable = this._bpmEditorState.bpmToolType != BpmEditorToolType.Merge;
    }

    private void SetSnapButtonsState()
    {
      this._noSnapButton.interactable = this._bpmEditorState.bpmToolSnapType != 0;
      this._roundSnapButton.interactable = this._bpmEditorState.bpmToolSnapType != BpmEditorToolSnapType.Round;
      this._floorSnapButton.interactable = this._bpmEditorState.bpmToolSnapType != BpmEditorToolSnapType.Floor;
      this._ceilSnapButton.interactable = this._bpmEditorState.bpmToolSnapType != BpmEditorToolSnapType.Ceil;
    }

    private void SetOneBeatToggleState() => this._oneBeatToggle.SetIsOnWithoutNotify(this._bpmEditorState.splitRegionSingleBeat);

    private void SetStretchToggleState() => this._stretchToggle.SetIsOnWithoutNotify(this._bpmEditorState.stretchRegion);
  }
}
