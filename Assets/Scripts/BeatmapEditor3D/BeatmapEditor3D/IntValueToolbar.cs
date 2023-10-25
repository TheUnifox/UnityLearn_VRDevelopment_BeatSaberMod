// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.IntValueToolbar
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Views;
using HMUI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class IntValueToolbar : AbstractBeatmapEditorToolbar
  {
    [Space]
    [SerializeField]
    private Button _offButton;
    [SerializeField]
    private Button _button1;
    [SerializeField]
    private Button _button2;
    [SerializeField]
    private Button _button3;
    [SerializeField]
    private Button _button4;
    [SerializeField]
    private Button _button5;
    [SerializeField]
    private Button _button6;
    [SerializeField]
    private Button _button7;
    [SerializeField]
    private Button _button8;
    [SerializeField]
    private Button _button9;
    [SerializeField]
    private TMP_InputField _customInputField;
    [Inject]
    private readonly SignalBus _signalBus;

    public override void SetValue(int value, float floatValue, object payload)
    {
      this._offButton.interactable = value != 0;
      this._button1.interactable = value != 1;
      this._button2.interactable = value != 2;
      this._button3.interactable = value != 3;
      this._button4.interactable = value != 4;
      this._button5.interactable = value != 5;
      this._button6.interactable = value != 6;
      this._button7.interactable = value != 7;
      this._button8.interactable = value != 8;
      this._button9.interactable = value != 9;
      this._customInputField.text = string.Format("{0}", (object) value);
    }

    public override void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(0)));
      keyboardBinder.AddBinding(KeyCode.S, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(1)));
      keyboardBinder.AddBinding(KeyCode.D, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(2)));
      keyboardBinder.AddBinding(KeyCode.F, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(3)));
      keyboardBinder.AddBinding(KeyCode.G, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(4)));
      keyboardBinder.AddBinding(KeyCode.Q, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(5)));
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(6)));
      keyboardBinder.AddBinding(KeyCode.E, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(7)));
      keyboardBinder.AddBinding(KeyCode.R, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(8)));
      keyboardBinder.AddBinding(KeyCode.T, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (_ => this.HandleChangeState(9)));
    }

    protected void OnEnable()
    {
      this._offButton.onClick.AddListener((UnityAction) (() => this.HandleChangeState(0)));
      this._button1.onClick.AddListener((UnityAction) (() => this.HandleChangeState(1)));
      this._button2.onClick.AddListener((UnityAction) (() => this.HandleChangeState(2)));
      this._button3.onClick.AddListener((UnityAction) (() => this.HandleChangeState(3)));
      this._button4.onClick.AddListener((UnityAction) (() => this.HandleChangeState(4)));
      this._button5.onClick.AddListener((UnityAction) (() => this.HandleChangeState(5)));
      this._button6.onClick.AddListener((UnityAction) (() => this.HandleChangeState(6)));
      this._button7.onClick.AddListener((UnityAction) (() => this.HandleChangeState(7)));
      this._button8.onClick.AddListener((UnityAction) (() => this.HandleChangeState(8)));
      this._button9.onClick.AddListener((UnityAction) (() => this.HandleChangeState(9)));
      this._customInputField.onEndEdit.AddListener(new UnityAction<string>(this.HandleInputFieldEndEdit));
    }

    protected void OnDisable()
    {
      this._offButton.onClick.RemoveAllListeners();
      this._button1.onClick.RemoveAllListeners();
      this._button2.onClick.RemoveAllListeners();
      this._button3.onClick.RemoveAllListeners();
      this._button4.onClick.RemoveAllListeners();
      this._button5.onClick.RemoveAllListeners();
      this._button6.onClick.RemoveAllListeners();
      this._button7.onClick.RemoveAllListeners();
      this._button8.onClick.RemoveAllListeners();
      this._button9.onClick.RemoveAllListeners();
    }

    private void HandleChangeState(int value)
    {
      if (Input.GetMouseButton(1))
        return;
      this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, value));
    }

    private void HandleInputFieldEndEdit(string str)
    {
      int result;
      if (!int.TryParse(str, out result))
        return;
      this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, result));
    }
  }
}
