// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.CarSelectionToolbar
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.LevelEditor;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class CarSelectionToolbar : AbstractBeatmapEditorToolbar
  {
    [Space]
    [SerializeField]
    private TMP_InputField _inputField;
    [Inject]
    private readonly SignalBus _signalBus;

    public override void SetValue(int value, float floatValue, object payload) => this._inputField.text = string.Format("{0}", (object) value);

    public override void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
    }

    protected void OnEnable() => this._inputField.onEndEdit.AddListener(new UnityAction<string>(this.HandleInputFieldEndEdit));

    protected void OnDisable() => this._inputField.onEndEdit.RemoveListener(new UnityAction<string>(this.HandleInputFieldEndEdit));

    private void HandleInputFieldEndEdit(string str)
    {
      int result;
      if (!int.TryParse(str, out result))
        return;
      this._signalBus.Fire<ChangeEventSignal>(new ChangeEventSignal(this.toolbarType, result));
    }
  }
}
