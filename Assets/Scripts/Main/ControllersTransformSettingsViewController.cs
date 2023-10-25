// Decompiled with JetBrains decompiler
// Type: ControllersTransformSettingsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class ControllersTransformSettingsViewController : ViewController
{
  [SerializeField]
  protected Vector3SO _controllerPosition;
  [SerializeField]
  protected Vector3SO _controllerRotation;
  [Space]
  [SerializeField]
  protected RangeValuesTextSlider _posXSlider;
  [SerializeField]
  protected RangeValuesTextSlider _posYSlider;
  [SerializeField]
  protected RangeValuesTextSlider _posZSlider;
  [SerializeField]
  protected RangeValuesTextSlider _rotXSlider;
  [SerializeField]
  protected RangeValuesTextSlider _rotYSlider;
  [SerializeField]
  protected RangeValuesTextSlider _rotZSlider;
  protected const float kPositionStep = 0.1f;
  protected const float kPositionMul = 100f;
  protected const float kRotationStep = 1f;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      float num1 = 10f;
      float num2 = -num1;
      int num3 = Mathf.RoundToInt((float) (((double) num1 - (double) num2) / 0.10000000149011612)) + 1;
      this._posXSlider.minValue = num2;
      this._posXSlider.maxValue = num1;
      this._posXSlider.numberOfSteps = num3;
      this._posYSlider.minValue = num2;
      this._posYSlider.maxValue = num1;
      this._posYSlider.numberOfSteps = num3;
      this._posZSlider.minValue = num2;
      this._posZSlider.maxValue = num1;
      this._posZSlider.numberOfSteps = num3;
      this._posXSlider.valueDidChangeEvent += new System.Action<RangeValuesTextSlider, float>(this.HandlePositionSliderValueDidChange);
      this._posYSlider.valueDidChangeEvent += new System.Action<RangeValuesTextSlider, float>(this.HandlePositionSliderValueDidChange);
      this._posZSlider.valueDidChangeEvent += new System.Action<RangeValuesTextSlider, float>(this.HandlePositionSliderValueDidChange);
      float num4 = 180f;
      float num5 = -num4;
      int num6 = Mathf.RoundToInt((float) (((double) num4 - (double) num5) / 1.0)) + 1;
      this._rotXSlider.minValue = num5;
      this._rotXSlider.maxValue = num4;
      this._rotXSlider.numberOfSteps = num6;
      this._rotYSlider.minValue = num5;
      this._rotYSlider.maxValue = num4;
      this._rotYSlider.numberOfSteps = num6;
      this._rotZSlider.minValue = num5;
      this._rotZSlider.maxValue = num4;
      this._rotZSlider.numberOfSteps = num6;
      this._rotXSlider.valueDidChangeEvent += new System.Action<RangeValuesTextSlider, float>(this.HandleRotationSliderValueDidChange);
      this._rotYSlider.valueDidChangeEvent += new System.Action<RangeValuesTextSlider, float>(this.HandleRotationSliderValueDidChange);
      this._rotZSlider.valueDidChangeEvent += new System.Action<RangeValuesTextSlider, float>(this.HandleRotationSliderValueDidChange);
    }
    if (!addedToHierarchy)
      return;
    this._posXSlider.value = this._controllerPosition.value.x * 100f;
    this._posYSlider.value = this._controllerPosition.value.y * 100f;
    this._posZSlider.value = this._controllerPosition.value.z * 100f;
    this._rotXSlider.value = this._controllerRotation.value.x;
    this._rotYSlider.value = this._controllerRotation.value.y;
    this._rotZSlider.value = this._controllerRotation.value.z;
  }

  protected override void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) this._posXSlider)
      this._posXSlider.valueDidChangeEvent -= new System.Action<RangeValuesTextSlider, float>(this.HandlePositionSliderValueDidChange);
    if ((bool) (UnityEngine.Object) this._posYSlider)
      this._posXSlider.valueDidChangeEvent -= new System.Action<RangeValuesTextSlider, float>(this.HandlePositionSliderValueDidChange);
    if ((bool) (UnityEngine.Object) this._posZSlider)
      this._posXSlider.valueDidChangeEvent -= new System.Action<RangeValuesTextSlider, float>(this.HandlePositionSliderValueDidChange);
    if ((bool) (UnityEngine.Object) this._rotXSlider)
      this._rotXSlider.valueDidChangeEvent -= new System.Action<RangeValuesTextSlider, float>(this.HandleRotationSliderValueDidChange);
    if ((bool) (UnityEngine.Object) this._rotYSlider)
      this._rotXSlider.valueDidChangeEvent -= new System.Action<RangeValuesTextSlider, float>(this.HandleRotationSliderValueDidChange);
    if ((bool) (UnityEngine.Object) this._rotZSlider)
      this._rotXSlider.valueDidChangeEvent -= new System.Action<RangeValuesTextSlider, float>(this.HandleRotationSliderValueDidChange);
    base.OnDestroy();
  }

  public virtual void HandlePositionSliderValueDidChange(RangeValuesTextSlider slider, float value) => this._controllerPosition.value = new Vector3(this._posXSlider.value / 100f, this._posYSlider.value / 100f, this._posZSlider.value / 100f);

  public virtual void HandleRotationSliderValueDidChange(RangeValuesTextSlider slider, float value) => this._controllerRotation.value = new Vector3(this._rotXSlider.value, this._rotYSlider.value, this._rotZSlider.value);
}
