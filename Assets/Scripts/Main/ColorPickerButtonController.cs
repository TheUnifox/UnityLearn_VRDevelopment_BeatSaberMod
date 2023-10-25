// Decompiled with JetBrains decompiler
// Type: ColorPickerButtonController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class ColorPickerButtonController : MonoBehaviour
{
  [SerializeField]
  protected Button _button;
  [SerializeField]
  protected Image _colorImage;

  public Button button => this._button;

  public virtual void SetColor(Color color)
  {
    color.a = 1f;
    this._colorImage.color = color;
  }
}
