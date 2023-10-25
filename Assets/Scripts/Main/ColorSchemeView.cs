// Decompiled with JetBrains decompiler
// Type: ColorSchemeView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class ColorSchemeView : MonoBehaviour
{
  [SerializeField]
  protected Image _saberAColorImage;
  [SerializeField]
  protected Image _saberBColorImage;
  [SerializeField]
  protected Image _environment0ColorImage;
  [SerializeField]
  protected Image _environment1ColorImage;
  [SerializeField]
  protected Image _environmentColor0BoostImage;
  [SerializeField]
  protected Image _environmentColor1BoostImage;
  [SerializeField]
  protected Image _obstacleColorImage;

  public virtual void SetColors(
    Color saberAColor,
    Color saberBColor,
    Color environment0Color,
    Color environment1Color,
    Color environmentColor0Boost,
    Color environmentColor1Boost,
    Color obstacleColor)
  {
    this._saberAColorImage.color = saberAColor;
    this._saberBColorImage.color = saberBColor;
    this._environment0ColorImage.color = environment0Color;
    this._environment1ColorImage.color = environment1Color;
    this._environmentColor0BoostImage.color = environmentColor0Boost;
    this._environmentColor1BoostImage.color = environmentColor1Boost;
    this._obstacleColorImage.color = obstacleColor;
  }
}
