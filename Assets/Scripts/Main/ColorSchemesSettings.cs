// Decompiled with JetBrains decompiler
// Type: ColorSchemesSettings
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class ColorSchemesSettings
{
  public bool overrideDefaultColors;
  protected List<ColorScheme> _colorSchemesList;
  protected Dictionary<string, ColorScheme> _colorSchemesDict;
  protected string _selectedColorSchemeId;

  public string selectedColorSchemeId
  {
    set => this._selectedColorSchemeId = value;
    get => this._selectedColorSchemeId;
  }

  public ColorSchemesSettings(ColorScheme[] colorSchemes)
  {
    this._colorSchemesList = new List<ColorScheme>((IEnumerable<ColorScheme>) colorSchemes);
    this._colorSchemesDict = new Dictionary<string, ColorScheme>(this._colorSchemesList.Count);
    foreach (ColorScheme colorScheme in colorSchemes)
      this._colorSchemesDict[colorScheme.colorSchemeId] = colorScheme;
    this._selectedColorSchemeId = colorSchemes[0].colorSchemeId;
  }

  public ColorSchemesSettings(ColorSchemeSO[] colorSchemeSOs)
    : this(ColorSchemesSettings.ConvertColorSchemeSOs(colorSchemeSOs))
  {
  }

  private static ColorScheme[] ConvertColorSchemeSOs(ColorSchemeSO[] colorSchemeSOs)
  {
    ColorScheme[] colorSchemeArray = new ColorScheme[colorSchemeSOs.Length];
    for (int index = 0; index < colorSchemeSOs.Length; ++index)
      colorSchemeArray[index] = new ColorScheme(colorSchemeSOs[index]);
    return colorSchemeArray;
  }

  public virtual int GetNumberOfColorSchemes() => this._colorSchemesList.Count;

  public virtual ColorScheme GetColorSchemeForIdx(int idx) => this._colorSchemesList[idx];

  public virtual ColorScheme GetColorSchemeForId(string id) => this._colorSchemesDict[id];

  public virtual void SetColorSchemeForId(ColorScheme colorScheme)
  {
    this._colorSchemesDict[colorScheme.colorSchemeId] = colorScheme;
    for (int index = 0; index < this._colorSchemesList.Count; ++index)
    {
      if (this._colorSchemesList[index].colorSchemeId == colorScheme.colorSchemeId)
      {
        this._colorSchemesList[index] = colorScheme;
        break;
      }
    }
  }

  public virtual ColorScheme GetSelectedColorScheme() => this._colorSchemesDict[this._selectedColorSchemeId];

  public virtual int GetSelectedColorSchemeIdx()
  {
    for (int index = 0; index < this._colorSchemesList.Count; ++index)
    {
      if (this._colorSchemesList[index].colorSchemeId == this._selectedColorSchemeId)
        return index;
    }
    return 0;
  }

  public virtual ColorScheme GetOverrideColorScheme() => !this.overrideDefaultColors ? (ColorScheme) null : this.GetSelectedColorScheme();
}
