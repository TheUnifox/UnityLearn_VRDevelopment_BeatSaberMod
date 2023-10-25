// Decompiled with JetBrains decompiler
// Type: NoteJumpStartBeatOffsetDropdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Collections.Generic;

public class NoteJumpStartBeatOffsetDropdown : ValueDropdownController<float>
{
  protected override IReadOnlyList<Tuple<float, string>> GetNamedValues()
  {
    List<Tuple<float, string>> list = new List<Tuple<float, string>>();
    list.Add<float, string>(-0.5f, Localization.Get("PLAYER_SETTINGS_JUMP_START_CLOSE"));
    list.Add<float, string>(-0.25f, Localization.Get("PLAYER_SETTINGS_JUMP_START_CLOSER"));
    list.Add<float, string>(0.0f, Localization.Get("PLAYER_SETTINGS_JUMP_START_DEFAULT"));
    list.Add<float, string>(0.25f, Localization.Get("PLAYER_SETTINGS_JUMP_START_FURTHER"));
    list.Add<float, string>(0.5f, Localization.Get("PLAYER_SETTINGS_JUMP_START_FAR"));
    return (IReadOnlyList<Tuple<float, string>>) list;
  }
}
