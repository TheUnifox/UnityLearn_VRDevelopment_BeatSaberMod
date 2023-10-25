// Decompiled with JetBrains decompiler
// Type: BoolWithDefaultValueSO
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class BoolWithDefaultValueSO : BoolSO
{
  [SerializeField]
  protected bool _defaultValue;

  protected override void OnEnable()
  {
    base.OnEnable();
    this.value = this._defaultValue;
  }
}
