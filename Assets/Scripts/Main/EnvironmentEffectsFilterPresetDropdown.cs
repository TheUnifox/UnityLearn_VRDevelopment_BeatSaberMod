// Decompiled with JetBrains decompiler
// Type: EnvironmentEffectsFilterPresetDropdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Collections.Generic;

public class EnvironmentEffectsFilterPresetDropdown : 
  ValueDropdownController<EnvironmentEffectsFilterPreset>
{
  protected override IReadOnlyList<Tuple<EnvironmentEffectsFilterPreset, string>> GetNamedValues()
  {
    List<Tuple<EnvironmentEffectsFilterPreset, string>> list = new List<Tuple<EnvironmentEffectsFilterPreset, string>>();
    list.Add<EnvironmentEffectsFilterPreset, string>(EnvironmentEffectsFilterPreset.AllEffects, Localization.Get("PLAYER_SETTINGS_ALL_ENVIRONMENT_EFFECTS"));
    list.Add<EnvironmentEffectsFilterPreset, string>(EnvironmentEffectsFilterPreset.StrobeFilter, Localization.Get("PLAYER_SETTINGS_STROBE_LIGHTS_REDUCTION"));
    list.Add<EnvironmentEffectsFilterPreset, string>(EnvironmentEffectsFilterPreset.NoEffects, Localization.Get("PLAYER_SETTINGS_NO_ENVIRONMENT_EFFECTS"));
    return (IReadOnlyList<Tuple<EnvironmentEffectsFilterPreset, string>>) list;
  }
}
