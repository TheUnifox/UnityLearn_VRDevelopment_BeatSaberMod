// Decompiled with JetBrains decompiler
// Type: ColorSchemeTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorSchemeTableCell : TableCell
{
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected ColorSchemeView _colorSchemeView;
  [SerializeField]
  protected Image _editIcon;

  public string text
  {
    set => this._text.text = value;
    get => this._text.text;
  }

  public bool showEditIcon
  {
    set => this._editIcon.enabled = value;
  }

  public virtual void SetColors(
    Color saberAColor,
    Color saberBColor,
    Color environment0Color,
    Color environment1Color,
    Color environmentColor0Boost,
    Color environmentColor1Boost,
    Color obstacleColor)
  {
    this._colorSchemeView.SetColors(saberAColor, saberBColor, environment0Color, environment1Color, environmentColor0Boost, environmentColor1Boost, obstacleColor);
  }
}
