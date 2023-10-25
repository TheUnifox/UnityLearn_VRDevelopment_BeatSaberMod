// Decompiled with JetBrains decompiler
// Type: IncDecColorController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class IncDecColorController : MonoBehaviour
{
  [SerializeField]
  private ColorStepValuePicker _stepValuePicker;

  protected bool enableDec
  {
    set => this._stepValuePicker.decButtonInteractable = value;
  }

  protected bool enableInc
  {
    set => this._stepValuePicker.incButtonInteractable = value;
  }

  protected Color color
  {
    set => this._stepValuePicker.color = value;
  }

  protected virtual void Awake()
  {
    this._stepValuePicker.decButtonWasPressedEvent += new System.Action(this.DecButtonPressed);
    this._stepValuePicker.incButtonWasPressedEvent += new System.Action(this.IncButtonPressed);
  }

  protected void OnDestroy()
  {
    if (!((UnityEngine.Object) this._stepValuePicker != (UnityEngine.Object) null))
      return;
    this._stepValuePicker.decButtonWasPressedEvent -= new System.Action(this.DecButtonPressed);
    this._stepValuePicker.incButtonWasPressedEvent -= new System.Action(this.IncButtonPressed);
  }

  protected abstract void IncButtonPressed();

  protected abstract void DecButtonPressed();
}
