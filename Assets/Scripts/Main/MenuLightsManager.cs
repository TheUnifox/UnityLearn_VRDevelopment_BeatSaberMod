// Decompiled with JetBrains decompiler
// Type: MenuLightsManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MenuLightsManager : MonoBehaviour
{
  [SerializeField]
  protected MenuLightsPresetSO _defaultPreset;
  [SerializeField]
  protected float _smooth = 8f;
  [Inject]
  protected LightWithIdManager _lightManager;
  protected MenuLightsPresetSO _preset;

  public virtual IEnumerator Start()
  {
    MenuLightsManager menuLightsManager = this;
    menuLightsManager.enabled = false;
    yield return (object) null;
    if ((Object) menuLightsManager._preset == (Object) null)
    {
      menuLightsManager.SetColorPreset(menuLightsManager._defaultPreset, false);
    }
  }

  public virtual void Update()
  {
    if (this.SetColorsFromPreset(this._preset, Time.deltaTime * this._smooth))
      return;
    this.enabled = false;
  }

  public virtual bool IsColorVeryCloseToColor(Color color0, Color color1) => (double) Mathf.Abs(color0.r - color1.r) < 1.0 / 500.0 && (double) Mathf.Abs(color0.g - color1.g) < 1.0 / 500.0 && (double) Mathf.Abs(color0.b - color1.b) < 1.0 / 500.0 && (double) Mathf.Abs(color0.a - color1.a) < 1.0 / 500.0;

  public virtual void SetColor(int lightId, Color color) => this._lightManager.SetColorForId(lightId, color);

  public virtual Color CurrentColorForID(int lightId) => this._lightManager.GetColorForId(lightId);

  public virtual bool SetColorsFromPreset(MenuLightsPresetSO preset, float interpolationFactor = 1f)
  {
    bool flag = true;
    for (int index = 0; index < this._preset.lightIdColorPairs.Length; ++index)
    {
      MenuLightsPresetSO.LightIdColorPair lightIdColorPair = this._preset.lightIdColorPairs[index];
      Color a = this.CurrentColorForID(lightIdColorPair.lightId);
      Color lightColor = lightIdColorPair.lightColor;
      Color b = lightColor;
      double t = (double) interpolationFactor;
      Color color = Color.Lerp(a, b, (float) t);
      this.SetColor(lightIdColorPair.lightId, color);
      if (!this.IsColorVeryCloseToColor(color, lightColor))
        flag = false;
    }
    return !flag;
  }

  public virtual void RefreshLightsDictForPreset(MenuLightsPresetSO preset)
  {
    HashSet<int> intSet = new HashSet<int>();
    foreach (MenuLightsPresetSO.LightIdColorPair lightIdColorPair in preset.lightIdColorPairs)
      intSet.Add(lightIdColorPair.lightId);
  }

  public virtual void SetColorPreset(MenuLightsPresetSO preset, bool animated)
  {
    if ((Object) this._preset == (Object) preset)
      return;
    this.RefreshLightsDictForPreset(preset);
    this._preset = preset;
    if (animated)
      this.enabled = true;
    else
      this.SetColorsFromPreset(this._preset);
  }

  public virtual void RefreshColors()
  {
    if ((Object) this._preset != (Object) this._defaultPreset)
      this.SetColorPreset(this._defaultPreset, false);
    else
      this.SetColorsFromPreset(this._preset);
  }
}
