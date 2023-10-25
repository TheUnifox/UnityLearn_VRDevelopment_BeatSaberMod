// Decompiled with JetBrains decompiler
// Type: ConditionalMaterialSwitcher
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ConditionalMaterialSwitcher : MonoBehaviour
{
  [Header("False")]
  [SerializeField]
  protected Material _material0;
  [Header("True")]
  [SerializeField]
  protected Material _material1;
  [Space]
  [SerializeField]
  protected BoolSO _value;
  [SerializeField]
  protected Renderer _renderer;

  public virtual void Awake()
  {
    if ((bool) (ObservableVariableSO<bool>) this._value)
      this._renderer.sharedMaterial = this._material1;
    else
      this._renderer.sharedMaterial = this._material0;
  }
}
