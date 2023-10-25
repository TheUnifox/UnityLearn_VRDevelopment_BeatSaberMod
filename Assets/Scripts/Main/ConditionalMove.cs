// Decompiled with JetBrains decompiler
// Type: ConditionalMove
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ConditionalMove : MonoBehaviour
{
  [SerializeField]
  protected Vector3 _offset;
  [SerializeField]
  protected BoolSO _value;
  [SerializeField]
  protected bool _activateOnFalse;

  public virtual void Awake()
  {
    if (!this._activateOnFalse != (bool) (ObservableVariableSO<bool>) this._value)
      return;
    this.transform.localPosition = this.transform.localPosition + this._offset;
  }
}
