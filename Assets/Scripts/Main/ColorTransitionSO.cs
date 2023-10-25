// Decompiled with JetBrains decompiler
// Type: ColorTransitionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ColorTransitionSO : BaseTransitionSO
{
  [SerializeField]
  protected ColorSO _normalColor;
  [SerializeField]
  protected ColorSO _highlightedColor;
  [SerializeField]
  protected ColorSO _pressedColor;
  [SerializeField]
  protected ColorSO _disabledColor;
  [SerializeField]
  protected ColorSO _selectedColor;
  [SerializeField]
  protected ColorSO _selectedAndHighlightedColor;

  public Color normalColor => (Color) this._normalColor;

  public Color highlightedColor => (Color) this._highlightedColor;

  public Color pressedColor => (Color) this._pressedColor;

  public Color disabledColor => (Color) this._disabledColor;

  public Color selectedColor => (Color) this._selectedColor;

  public Color selectedAndHighlightedColor => (Color) this._selectedAndHighlightedColor;
}
