// Decompiled with JetBrains decompiler
// Type: ConditionalImageMaterialSwitcher
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class ConditionalImageMaterialSwitcher : MonoBehaviour
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
  protected Image _image;

  public virtual void Awake()
  {
    if ((bool) (ObservableVariableSO<bool>) this._value)
      this._image.material = this._material1;
    else
      this._image.material = this._material0;
  }
}
