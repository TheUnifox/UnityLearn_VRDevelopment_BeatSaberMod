// Decompiled with JetBrains decompiler
// Type: ConditionalActivation
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ConditionalActivation : MonoBehaviour
{
  [SerializeField]
  protected BoolSO _value;
  [SerializeField]
  protected bool _activateOnFalse;

  public virtual void Awake() => this.gameObject.SetActive(this._activateOnFalse ? !(bool) (ObservableVariableSO<bool>) this._value : (bool) (ObservableVariableSO<bool>) this._value);
}
