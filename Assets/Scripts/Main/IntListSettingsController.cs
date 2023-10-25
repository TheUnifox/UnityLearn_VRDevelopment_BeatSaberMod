// Decompiled with JetBrains decompiler
// Type: IntListSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class IntListSettingsController : ListSettingsController
{
  [SerializeField]
  protected int _customNumberOfElements;
  [SerializeField]
  protected int _customIndex;

  public event System.Action<int> valueChangedEvent;

  public virtual void InitValues(int numberOfElements, int index)
  {
    this._customNumberOfElements = numberOfElements;
    this._customIndex = Mathf.Clamp(index, 0, this._customNumberOfElements);
    this.Refresh(false);
  }

  protected override bool GetInitValues(out int idx, out int numberOfElements)
  {
    idx = this._customIndex;
    numberOfElements = this._customNumberOfElements;
    return true;
  }

  protected override void ApplyValue(int idx)
  {
    this._customIndex = idx;
    System.Action<int> valueChangedEvent = this.valueChangedEvent;
    if (valueChangedEvent == null)
      return;
    valueChangedEvent(idx);
  }

  protected override string TextForValue(int idx) => idx.ToString();
}
