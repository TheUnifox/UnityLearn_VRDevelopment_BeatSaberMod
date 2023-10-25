// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.ToggleToolbar
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.LevelEditor;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class ToggleToolbar : AbstractBeatmapEditorToolbar
  {
    [Space]
    [SerializeField]
    private Button _onButton;
    [SerializeField]
    private Button _offButton;
    [Inject]
    private readonly SignalBus _signalBus;

    public override void SetValue(int value, float floatValue, object payload)
    {
      this._onButton.interactable = value != 1;
      this._offButton.interactable = value == 1;
    }

    public override void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(1)));
      keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(0)));
    }

    protected void OnEnable()
    {
      this._onButton.onClick.AddListener((UnityAction) (() => this.HandleChangeState(1)));
      this._offButton.onClick.AddListener((UnityAction) (() => this.HandleChangeState(0)));
    }

    protected void OnDisable()
    {
      this._onButton.onClick.RemoveAllListeners();
      this._offButton.onClick.RemoveAllListeners();
    }

    private void HandleChangeState(int value)
    {
      if (Input.GetMouseButton(1))
        return;
      this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, value));
    }
  }
}
