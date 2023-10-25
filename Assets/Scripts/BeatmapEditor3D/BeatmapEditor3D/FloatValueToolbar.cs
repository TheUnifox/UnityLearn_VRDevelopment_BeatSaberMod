// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.FloatValueToolbar
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Views;
using HMUI;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class FloatValueToolbar : AbstractBeatmapEditorToolbar
  {
    [SerializeField]
    private FloatInputFieldValidator _floatInputFieldValidator;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapState _beatmapState;
    private int _intValue;

    public override void SetValue(int value, float floatValue, object payload)
    {
      this._intValue = value;
      this._floatInputFieldValidator.value = floatValue;
    }

    public override void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(0.0f)));
      keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(0.5f)));
      keyboardBinder.AddBinding(KeyCode.D, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(1f)));
      keyboardBinder.AddBinding(KeyCode.F, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(1.5f)));
      keyboardBinder.AddBinding(KeyCode.G, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(2f)));
      keyboardBinder.AddBinding(KeyCode.Q, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(3f)));
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(4f)));
      keyboardBinder.AddBinding(KeyCode.E, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(5f)));
      keyboardBinder.AddBinding(KeyCode.R, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(6f)));
      keyboardBinder.AddBinding(KeyCode.T, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(7f)));
    }

    protected void OnEnable() => this._floatInputFieldValidator.onInputValidated += new Action<float>(this.HandleFloatOnValidated);

    protected void OnDisable() => this._floatInputFieldValidator.onInputValidated -= new Action<float>(this.HandleFloatOnValidated);

    private void HandleChangeState(float floatValue)
    {
      if (Input.GetMouseButton(1))
        return;
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
        this._signalBus.Fire<ModifyHoveredFloatEventValueSignal>(new ModifyHoveredFloatEventValueSignal(floatValue));
      else
        this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, this._intValue, floatValue));
    }

    private void HandleFloatOnValidated(float floatValue) => this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, this._intValue, floatValue));
  }
}
