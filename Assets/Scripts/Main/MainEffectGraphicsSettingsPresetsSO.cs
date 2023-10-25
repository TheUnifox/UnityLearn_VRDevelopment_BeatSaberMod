// Decompiled with JetBrains decompiler
// Type: MainEffectGraphicsSettingsPresetsSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class MainEffectGraphicsSettingsPresetsSO : NamedPresetsSO
{
  [SerializeField]
  protected MainEffectGraphicsSettingsPresetsSO.Preset[] _presets;

  public MainEffectGraphicsSettingsPresetsSO.Preset[] presets => this._presets;

  public override NamedPreset[] namedPresets => (NamedPreset[]) this._presets;

  [Serializable]
  public class Preset : NamedPreset
  {
    public MainEffectSO mainEffect;
  }
}
