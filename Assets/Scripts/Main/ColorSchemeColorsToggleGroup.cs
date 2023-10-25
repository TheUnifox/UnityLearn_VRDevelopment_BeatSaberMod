// Decompiled with JetBrains decompiler
// Type: ColorSchemeColorsToggleGroup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ColorSchemeColorsToggleGroup : MonoBehaviour
{
  [SerializeField]
  protected ColorSchemeColorToggleController _saberAColorToggleController;
  [SerializeField]
  protected ColorSchemeColorToggleController _saberBColorToggleController;
  [SerializeField]
  protected ColorSchemeColorToggleController _environmentColor0ToggleController;
  [SerializeField]
  protected ColorSchemeColorToggleController _environmentColor1ToggleController;
  [SerializeField]
  protected ColorSchemeColorToggleController _obstaclesColorToggleController;
  [SerializeField]
  protected ColorSchemeColorToggleController _environmentColor0BoostToggleController;
  [SerializeField]
  protected ColorSchemeColorToggleController _environmentColor1BoostToggleController;
  protected ToggleBinder _toggleBinder;
  protected ColorSchemeColorToggleController _selectedColorToggleController;
  protected ColorScheme _colorScheme;

  public event System.Action<Color> selectedColorDidChangeEvent;

  public Color color
  {
    get => this._selectedColorToggleController.color;
    set => this._selectedColorToggleController.color = value;
  }

  public virtual void SetColorScheme(ColorScheme colorScheme)
  {
    this._colorScheme = colorScheme;
    this._saberAColorToggleController.color = colorScheme.saberAColor;
    this._saberBColorToggleController.color = colorScheme.saberBColor;
    this._environmentColor0ToggleController.color = colorScheme.environmentColor0;
    this._environmentColor1ToggleController.color = colorScheme.environmentColor1;
    this._obstaclesColorToggleController.color = colorScheme.obstaclesColor;
    this._environmentColor0BoostToggleController.color = colorScheme.environmentColor0Boost;
    this._environmentColor1BoostToggleController.color = colorScheme.environmentColor1Boost;
    System.Action<Color> colorDidChangeEvent = this.selectedColorDidChangeEvent;
    if (colorDidChangeEvent == null)
      return;
    colorDidChangeEvent(this.color);
  }

  public virtual void Awake()
  {
    this._selectedColorToggleController = this._saberAColorToggleController;
    this._saberAColorToggleController.toggle.isOn = true;
    this._toggleBinder = new ToggleBinder();
    this._toggleBinder.AddBinding(this._saberAColorToggleController.toggle, (System.Action<bool>) (isOn => this.HandleToggleWasSelected(this._saberAColorToggleController, isOn)));
    this._toggleBinder.AddBinding(this._saberBColorToggleController.toggle, (System.Action<bool>) (isOn => this.HandleToggleWasSelected(this._saberBColorToggleController, isOn)));
    this._toggleBinder.AddBinding(this._environmentColor0ToggleController.toggle, (System.Action<bool>) (isOn => this.HandleToggleWasSelected(this._environmentColor0ToggleController, isOn)));
    this._toggleBinder.AddBinding(this._environmentColor1ToggleController.toggle, (System.Action<bool>) (isOn => this.HandleToggleWasSelected(this._environmentColor1ToggleController, isOn)));
    this._toggleBinder.AddBinding(this._obstaclesColorToggleController.toggle, (System.Action<bool>) (isOn => this.HandleToggleWasSelected(this._obstaclesColorToggleController, isOn)));
    this._toggleBinder.AddBinding(this._environmentColor0BoostToggleController.toggle, (System.Action<bool>) (isOn => this.HandleToggleWasSelected(this._environmentColor0BoostToggleController, isOn)));
    this._toggleBinder.AddBinding(this._environmentColor1BoostToggleController.toggle, (System.Action<bool>) (isOn => this.HandleToggleWasSelected(this._environmentColor1BoostToggleController, isOn)));
  }

  public virtual void OnDestroy() => this._toggleBinder.ClearBindings();

  public virtual void HandleToggleWasSelected(
    ColorSchemeColorToggleController toggleController,
    bool isOn)
  {
    if (!isOn)
      return;
    this._selectedColorToggleController = toggleController;
    System.Action<Color> colorDidChangeEvent = this.selectedColorDidChangeEvent;
    if (colorDidChangeEvent == null)
      return;
    colorDidChangeEvent(toggleController.color);
  }

  public virtual ColorScheme CreateColorSchemeFromEditedColors() => new ColorScheme(this._colorScheme, this._saberAColorToggleController.color, this._saberBColorToggleController.color, this._environmentColor0ToggleController.color, this._environmentColor1ToggleController.color, true, this._environmentColor0BoostToggleController.color, this._environmentColor1BoostToggleController.color, this._obstaclesColorToggleController.color);

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__17_0(bool isOn) => this.HandleToggleWasSelected(this._saberAColorToggleController, isOn);

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__17_1(bool isOn) => this.HandleToggleWasSelected(this._saberBColorToggleController, isOn);

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__17_2(bool isOn) => this.HandleToggleWasSelected(this._environmentColor0ToggleController, isOn);

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__17_3(bool isOn) => this.HandleToggleWasSelected(this._environmentColor1ToggleController, isOn);

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__17_4(bool isOn) => this.HandleToggleWasSelected(this._obstaclesColorToggleController, isOn);

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__17_5(bool isOn) => this.HandleToggleWasSelected(this._environmentColor0BoostToggleController, isOn);

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__17_6(bool isOn) => this.HandleToggleWasSelected(this._environmentColor1BoostToggleController, isOn);
}
