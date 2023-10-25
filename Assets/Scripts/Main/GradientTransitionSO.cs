// Decompiled with JetBrains decompiler
// Type: GradientTransitionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class GradientTransitionSO : BaseTransitionSO
{
  [SerializeField]
  protected ColorSO _normalColor1;
  [SerializeField]
  protected ColorSO _normalColor2;
  [SerializeField]
  protected ColorSO _highlightColor1;
  [SerializeField]
  protected ColorSO _highlightColor2;
  [SerializeField]
  protected ColorSO _pressedColor1;
  [SerializeField]
  protected ColorSO _pressedColor2;
  [SerializeField]
  protected ColorSO _disabledColor1;
  [SerializeField]
  protected ColorSO _disabledColor2;
  [SerializeField]
  protected ColorSO _selectedColor1;
  [SerializeField]
  protected ColorSO _selectedColor2;
  [SerializeField]
  protected ColorSO _selectedAndHighlightedColor1;
  [SerializeField]
  protected ColorSO _selectedAndHighlightedColor2;

  public Color normalColor1 => (Color) this._normalColor1;

  public Color normalColor2 => (Color) this._normalColor2;

  public Color highlightColor1 => (Color) this._highlightColor1;

  public Color highlightColor2 => (Color) this._highlightColor2;

  public Color pressedColor1 => (Color) this._pressedColor1;

  public Color pressedColor2 => (Color) this._pressedColor2;

  public Color disabledColor1 => (Color) this._disabledColor1;

  public Color disabledColor2 => (Color) this._disabledColor2;

  public Color selectedColor1 => (Color) this._selectedColor1;

  public Color selectedColor2 => (Color) this._selectedColor2;

  public Color selectedAndHighlightedColor1 => (Color) this._selectedAndHighlightedColor1;

  public Color selectedAndHighlightedColor2 => (Color) this._selectedAndHighlightedColor2;
}
