// Decompiled with JetBrains decompiler
// Type: ArcVisibilityTypeSettingsDropdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Collections.Generic;

public class ArcVisibilityTypeSettingsDropdown : ValueDropdownController<ArcVisibilityType>
{
  protected override IReadOnlyList<Tuple<ArcVisibilityType, string>> GetNamedValues()
  {
    List<Tuple<ArcVisibilityType, string>> list = new List<Tuple<ArcVisibilityType, string>>();
    list.Add<ArcVisibilityType, string>(ArcVisibilityType.Standard, Localization.Get("PLAYER_SETTINGS_ARC_VISIBILITY_STANDARD"));
    list.Add<ArcVisibilityType, string>(ArcVisibilityType.None, Localization.Get("PLAYER_SETTINGS_ARC_VISIBILITY_NONE"));
    list.Add<ArcVisibilityType, string>(ArcVisibilityType.Low, Localization.Get("PLAYER_SETTINGS_ARC_VISIBILITY_LOW"));
    list.Add<ArcVisibilityType, string>(ArcVisibilityType.High, Localization.Get("PLAYER_SETTINGS_ARC_VISIBILITY_HIGH"));
    return (IReadOnlyList<Tuple<ArcVisibilityType, string>>) list;
  }
}
