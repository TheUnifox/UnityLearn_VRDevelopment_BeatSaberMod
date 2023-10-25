// Decompiled with JetBrains decompiler
// Type: FillIndicator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class FillIndicator : MonoBehaviour
{
  [SerializeField]
  protected Image _bgImage;
  [SerializeField]
  protected Image _image;

  public float fillAmount
  {
    set
    {
      this._image.fillAmount = value;
      this._bgImage.fillAmount = 1f - value;
    }
    get => this._image.fillAmount;
  }
}
