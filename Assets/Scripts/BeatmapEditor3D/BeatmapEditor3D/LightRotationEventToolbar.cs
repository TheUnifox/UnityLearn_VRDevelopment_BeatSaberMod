// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LightRotationEventToolbar
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class LightRotationEventToolbar : MonoBehaviour
  {
    [SerializeField]
    private Button _noneEasingButton;
    [SerializeField]
    private Button _linearEasingButton;
    [SerializeField]
    private Button _inQuadEasingButton;
    [SerializeField]
    private Button _outQuadEasingButton;
    [SerializeField]
    private Button _inOutQuadEasingButton;
    [Space]
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
    [Space]
    [SerializeField]
    private Button _autoRotationDirectionButton;
    [SerializeField]
    private Button _cwRotationDirectionButton;
    [SerializeField]
    private Button _ccwRotationDirectionButton;
    [Space]
    [SerializeField]
    private Toggle _extensionToggle;
    [Space]
    [SerializeField]
    private TMP_InputField _loopsInputField;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;
    private EaseType _easeType;
    private int _loops;
    private float _rotation;
    private LightRotationDirection _rotationDirection;
    private bool _extension;

    public void SetValue(
      EaseType easeType,
      int loops,
      float rotation,
      LightRotationDirection rotationDirection,
      bool extension)
    {
      this._easeType = easeType;
      this._loops = loops;
      this._rotation = rotation;
      this._rotationDirection = rotationDirection;
      this._extension = extension;
      this._noneEasingButton.interactable = easeType != 0;
      this._linearEasingButton.interactable = easeType != EaseType.Linear;
      this._inQuadEasingButton.interactable = easeType != EaseType.InQuad;
      this._outQuadEasingButton.interactable = easeType != EaseType.OutQuad;
      this._inOutQuadEasingButton.interactable = easeType != EaseType.InOutQuad;
      this._loopsInputField.text = string.Format("{0}", (object) loops);
      this._0RotButton.interactable = !Mathf.Approximately(rotation, 0.0f);
      this._45RotButton.interactable = !Mathf.Approximately(rotation, 45f);
      this._90RotButton.interactable = !Mathf.Approximately(rotation, 90f);
      this._135RotButton.interactable = !Mathf.Approximately(rotation, 135f);
      this._180RotButton.interactable = !Mathf.Approximately(rotation, 180f);
      this._225RotButton.interactable = !Mathf.Approximately(rotation, 225f);
      this._270RotButton.interactable = !Mathf.Approximately(rotation, 270f);
      this._315RotButton.interactable = !Mathf.Approximately(rotation, 315f);
      this._extensionToggle.SetIsOnWithoutNotify(this._extension);
      this._autoRotationDirectionButton.interactable = rotationDirection != 0;
      this._cwRotationDirectionButton.interactable = rotationDirection != LightRotationDirection.Clockwise;
      this._ccwRotationDirectionButton.interactable = rotationDirection != LightRotationDirection.Counterclockwise;
    }

    public void SetKeyBindings(KeyboardBinder keyboardBinder)
    {
      keyboardBinder.AddBinding(KeyCode.G, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeEasing(EaseType.None)));
      keyboardBinder.AddBinding(KeyCode.B, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeEasing(EaseType.Linear)));
      keyboardBinder.AddBinding(KeyCode.V, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleToggleQuadEasing()));
      keyboardBinder.AddBinding(KeyCode.F, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeEasing(EaseType.InOutQuad)));
      keyboardBinder.AddBinding(KeyCode.W, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(0.0f)));
      keyboardBinder.AddBinding(KeyCode.E, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(45f)));
      keyboardBinder.AddBinding(KeyCode.D, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(90f)));
      keyboardBinder.AddBinding(KeyCode.C, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(135f)));
      keyboardBinder.AddBinding(KeyCode.X, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(180f)));
      keyboardBinder.AddBinding(KeyCode.Z, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(225f)));
      keyboardBinder.AddBinding(KeyCode.A, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(270f)));
      keyboardBinder.AddBinding(KeyCode.Q, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotation(315f)));
      keyboardBinder.AddBinding(KeyCode.Alpha1, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotationDirection(LightRotationDirection.Counterclockwise)));
      keyboardBinder.AddBinding(KeyCode.Alpha2, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotationDirection(LightRotationDirection.Automatic)));
      keyboardBinder.AddBinding(KeyCode.Alpha3, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeRotationDirection(LightRotationDirection.Clockwise)));
      keyboardBinder.AddBinding(KeyCode.R, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleChangeLoops(0)));
      keyboardBinder.AddBinding(KeyCode.T, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleIncrementLoops()));
      keyboardBinder.AddBinding(KeyCode.Alpha4, KeyboardBinder.KeyBindingType.KeyDown, (Action<bool>) (pressed => this.HandleEnableExtension(true)));
    }

    protected void OnEnable()
    {
      this._noneEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.None)));
      this._linearEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.Linear)));
      this._inQuadEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.InQuad)));
      this._outQuadEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.OutQuad)));
      this._inOutQuadEasingButton.onClick.AddListener((UnityAction) (() => this.HandleChangeEasing(EaseType.InOutQuad)));
      this._loopsInputField.onEndEdit.AddListener(new UnityAction<string>(this.HandleLoopsInputOnEndEdit));
      this._0RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(0.0f)));
      this._45RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(45f)));
      this._90RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(90f)));
      this._135RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(135f)));
      this._180RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(180f)));
      this._225RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(225f)));
      this._270RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(270f)));
      this._315RotButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotation(315f)));
      this._autoRotationDirectionButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotationDirection(LightRotationDirection.Automatic)));
      this._cwRotationDirectionButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotationDirection(LightRotationDirection.Clockwise)));
      this._ccwRotationDirectionButton.onClick.AddListener((UnityAction) (() => this.HandleChangeRotationDirection(LightRotationDirection.Counterclockwise)));
      this._extensionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleEnableExtension));
    }

    protected void OnDisable()
    {
      this._noneEasingButton.onClick.RemoveAllListeners();
      this._linearEasingButton.onClick.RemoveAllListeners();
      this._inQuadEasingButton.onClick.RemoveAllListeners();
      this._outQuadEasingButton.onClick.RemoveAllListeners();
      this._inOutQuadEasingButton.onClick.RemoveAllListeners();
      this._loopsInputField.onEndEdit.RemoveListener(new UnityAction<string>(this.HandleLoopsInputOnEndEdit));
      this._0RotButton.onClick.RemoveAllListeners();
      this._45RotButton.onClick.RemoveAllListeners();
      this._90RotButton.onClick.RemoveAllListeners();
      this._135RotButton.onClick.RemoveAllListeners();
      this._180RotButton.onClick.RemoveAllListeners();
      this._225RotButton.onClick.RemoveAllListeners();
      this._270RotButton.onClick.RemoveAllListeners();
      this._315RotButton.onClick.RemoveAllListeners();
      this._autoRotationDirectionButton.onClick.RemoveAllListeners();
      this._cwRotationDirectionButton.onClick.RemoveAllListeners();
      this._ccwRotationDirectionButton.onClick.RemoveAllListeners();
      this._extensionToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleEnableExtension));
    }

    private void HandleChangeEasing(EaseType easeType)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._easeType = easeType;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightRotationEaseTypeSignal>(new ModifyHoveredLightRotationEaseTypeSignal(this._easeType));
      this._signalBus.Fire<ChangeLightRotationEventSignal>(new ChangeLightRotationEventSignal(this._easeType, this._loops, this._rotation, this._rotationDirection, false));
    }

    private void HandleToggleQuadEasing()
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._easeType = this._easeType != EaseType.InQuad ? EaseType.InQuad : EaseType.OutQuad;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightRotationEaseTypeSignal>(new ModifyHoveredLightRotationEaseTypeSignal(this._easeType));
      this._signalBus.Fire<ChangeLightRotationEventSignal>(new ChangeLightRotationEventSignal(this._easeType, this._loops, this._rotation, this._rotationDirection, false));
    }

    private void HandleChangeRotation(float rotation)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._rotation = rotation;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightRotationRotationSignal>(new ModifyHoveredLightRotationRotationSignal(this._rotation));
      this._signalBus.Fire<ChangeLightRotationEventSignal>(new ChangeLightRotationEventSignal(this._easeType, this._loops, this._rotation, this._rotationDirection, false));
    }

    private void HandleChangeRotationDirection(LightRotationDirection rotationDirection)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._rotationDirection = rotationDirection;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightRotationDirectionSignal>(new ModifyHoveredLightRotationDirectionSignal(this._rotationDirection));
      this._signalBus.Fire<ChangeLightRotationEventSignal>(new ChangeLightRotationEventSignal(this._easeType, this._loops, this._rotation, this._rotationDirection, false));
    }

    private void HandleIncrementLoops()
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._loops = (this._loops + 1) % 6;
      this._loops = this._loops == 0 ? 1 : this._loops;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightRotationLoopsCountSignal>(new ModifyHoveredLightRotationLoopsCountSignal(this._loops));
      this._signalBus.Fire<ChangeLightRotationEventSignal>(new ChangeLightRotationEventSignal(this._easeType, this._loops, this._rotation, this._rotationDirection, false));
    }

    private void HandleEnableExtension(bool isOn)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._extension = true;
      this._signalBus.Fire<ChangeLightRotationEventSignal>(new ChangeLightRotationEventSignal(this._easeType, this._loops, this._rotation, this._rotationDirection, isOn));
    }

    private void HandleChangeLoops(int loops)
    {
      if (this._beatmapState.cameraMoving)
        return;
      this._loops = loops;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightRotationLoopsCountSignal>(new ModifyHoveredLightRotationLoopsCountSignal(this._loops));
      this._signalBus.Fire<ChangeLightRotationEventSignal>(new ChangeLightRotationEventSignal(this._easeType, this._loops, this._rotation, this._rotationDirection, false));
    }

    private void HandleLoopsInputOnEndEdit(string loopsInput)
    {
      int result;
      if (this._beatmapState.cameraMoving || !int.TryParse(loopsInput, out result))
        return;
      this._loops = result;
      if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        this._signalBus.Fire<ModifyHoveredLightRotationLoopsCountSignal>(new ModifyHoveredLightRotationLoopsCountSignal(this._loops));
      this._signalBus.Fire<ChangeLightRotationEventSignal>(new ChangeLightRotationEventSignal(this._easeType, this._loops, this._rotation, this._rotationDirection, false));
    }
  }
}
