// Decompiled with JetBrains decompiler
// Type: ColorSchemeColorToggleController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class ColorSchemeColorToggleController : MonoBehaviour
{
  [SerializeField]
  protected Graphic[] _colorGraphics;
  [SerializeField]
  protected Toggle _toggle;

  public Toggle toggle => this._toggle;

  public Color color
  {
    get => this._colorGraphics[0].color;
    set
    {
      foreach (Graphic colorGraphic in this._colorGraphics)
        colorGraphic.color = value;
    }
  }
}
