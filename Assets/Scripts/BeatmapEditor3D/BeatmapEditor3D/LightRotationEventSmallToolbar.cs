// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LightRotationEventSmallToolbar
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class LightRotationEventSmallToolbar : MonoBehaviour
  {
    [SerializeField]
    private Button _0RotButton;
    [SerializeField]
    private Button _45RotButton;
    [SerializeField]
    private Button _90RotButton;
    [SerializeField]
    private Button _135RotButton;
    [SerializeField]
    private Button _180RotButton;
    [SerializeField]
    private Button _225RotButton;
    [SerializeField]
    private Button _270RotButton;
    [SerializeField]
    private Button _315RotButton;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;
    private float _rotation;

    public void SetValue(float rotation)
    {
      this._0RotButton.interactable = !Mathf.Approximately(rotation, 0.0f);
      this._45RotButton.interactable = !Mathf.Approximately(rotation, 45f);
      this._90RotButton.interactable = !Mathf.Approximately(rotation, 90f);
      this._135RotButton.interactable = !Mathf.Approximately(rotation, 135f);
      this._180RotButton.interactable = !Mathf.Approximately(rotation, 180f);
      this._225RotButton.interactable = !Mathf.Approximately(rotation, 225f);
      this._270RotButton.interactable = !Mathf.Approximately(rotation, 270f);
      this._315RotButton.interactable = !Mathf.Approximately(rotation, 315f);
    }

    public void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(0.0f)));
      keyboardBinder.AddBinding(KeyCode.E, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(45f)));
      keyboardBinder.AddBinding(KeyCode.D, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(90f)));
      keyboardBinder.AddBinding(KeyCode.C, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(135f)));
      keyboardBinder.AddBinding(KeyCode.X, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(180f)));
      keyboardBinder.AddBinding(KeyCode.Z, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(225f)));
      keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(270f)));
      keyboardBinder.AddBinding(KeyCode.Q, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(315f)));
    }

    protected void OnEnable()
    {
      this._0RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(0.0f)));
      this._45RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(45f)));
      this._90RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(90f)));
      this._135RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(135f)));
      this._180RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(180f)));
      this._225RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(225f)));
      this._270RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(270f)));
      this._315RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(315f)));
    }

    protected void OnDisable()
    {
      this._0RotButton.onClick.RemoveAllListeners();
      this._45RotButton.onClick.RemoveAllListeners();
      this._90RotButton.onClick.RemoveAllListeners();
      this._135RotButton.onClick.RemoveAllListeners();
      this._180RotButton.onClick.RemoveAllListeners();
      this._225RotButton.onClick.RemoveAllListeners();
      this._270RotButton.onClick.RemoveAllListeners();
      this._315RotButton.onClick.RemoveAllListeners();
    }

    private void HandleChangeRotation(float rotation)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._rotation = rotation;
      this._signalBus.Fire<ChangeLightRotationEventSignal>(new ChangeLightRotationEventSignal(EaseType.InOutQuad, 0, this._rotation, LightRotationDirection.Automatic, false));
    }
  }
}
