// Decompiled with JetBrains decompiler
// Type: RoomAdjustSettingsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class RoomAdjustSettingsViewController : ViewController
{
  [SerializeField]
  protected Vector3SO _roomCenter;
  [SerializeField]
  protected FloatSO _roomRotation;
  [Space]
  [SerializeField]
  protected StepValuePicker _xStepValuePicker;
  [SerializeField]
  protected StepValuePicker _yStepValuePicker;
  [SerializeField]
  protected StepValuePicker _zStepValuePicker;
  [SerializeField]
  protected StepValuePicker _rotStepValuePicker;
  [SerializeField]
  protected Button _resetButton;
  protected const float kHorizontalMoveStep = 0.1f;
  protected const float kVerticalMoveStep = 0.05f;
  protected const float kRotationStep = 5f;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this._xStepValuePicker.incButtonWasPressedEvent += (System.Action) (() => this.Move(new Vector3(-0.1f, 0.0f, 0.0f)));
      this._xStepValuePicker.decButtonWasPressedEvent += (System.Action) (() => this.Move(new Vector3(0.1f, 0.0f, 0.0f)));
      this._yStepValuePicker.incButtonWasPressedEvent += (System.Action) (() => this.Move(new Vector3(0.0f, -0.05f, 0.0f)));
      this._yStepValuePicker.decButtonWasPressedEvent += (System.Action) (() => this.Move(new Vector3(0.0f, 0.05f, 0.0f)));
      this._zStepValuePicker.incButtonWasPressedEvent += (System.Action) (() => this.Move(new Vector3(0.0f, 0.0f, -0.1f)));
      this._zStepValuePicker.decButtonWasPressedEvent += (System.Action) (() => this.Move(new Vector3(0.0f, 0.0f, 0.1f)));
      this._rotStepValuePicker.incButtonWasPressedEvent += (System.Action) (() => this.Rotate(-5f));
      this._rotStepValuePicker.decButtonWasPressedEvent += (System.Action) (() => this.Rotate(5f));
      this.buttonBinder.AddBinding(this._resetButton, (System.Action) (() => this.ResetRoom()));
    }
    this.RefreshTexts();
  }

  public virtual void Move(Vector3 move)
  {
    Vector3SO roomCenter = this._roomCenter;
    roomCenter.value = roomCenter.value + move;
    this.RefreshTexts();
  }

  public virtual void Rotate(float rotation)
  {
    this._roomRotation.value = (float) (((double) rotation + (double) this._roomRotation.value) % 360.0);
    this.RefreshTexts();
  }

  public virtual void ResetRoom()
  {
    this._roomCenter.value = new Vector3(0.0f, 0.0f, 0.0f);
    this._roomRotation.value = 0.0f;
    this.RefreshTexts();
  }

  public virtual void RefreshTexts()
  {
    this._xStepValuePicker.text = (-this._roomCenter.value.x).ToString("F2");
    this._yStepValuePicker.text = (-this._roomCenter.value.y).ToString("F2");
    this._zStepValuePicker.text = (-this._roomCenter.value.z).ToString("F2");
    this._rotStepValuePicker.text = Mathf.RoundToInt(-this._roomRotation.value).ToString();
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_0() => this.Move(new Vector3(-0.1f, 0.0f, 0.0f));

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_1() => this.Move(new Vector3(0.1f, 0.0f, 0.0f));

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_2() => this.Move(new Vector3(0.0f, -0.05f, 0.0f));

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_3() => this.Move(new Vector3(0.0f, 0.05f, 0.0f));

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_4() => this.Move(new Vector3(0.0f, 0.0f, -0.1f));

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_5() => this.Move(new Vector3(0.0f, 0.0f, 0.1f));

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_6() => this.Rotate(-5f);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_7() => this.Rotate(5f);

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__10_8() => this.ResetRoom();
}
