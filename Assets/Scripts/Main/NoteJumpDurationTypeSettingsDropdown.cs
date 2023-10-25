// Decompiled with JetBrains decompiler
// Type: NoteJumpDurationTypeSettingsDropdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Collections.Generic;

public class NoteJumpDurationTypeSettingsDropdown : 
  ValueDropdownController<NoteJumpDurationTypeSettings>
{
  protected override IReadOnlyList<Tuple<NoteJumpDurationTypeSettings, string>> GetNamedValues()
  {
    List<Tuple<NoteJumpDurationTypeSettings, string>> list = new List<Tuple<NoteJumpDurationTypeSettings, string>>();
    list.Add<NoteJumpDurationTypeSettings, string>(NoteJumpDurationTypeSettings.Dynamic, Localization.Get("PLAYER_SETTINGS_NOTE_JUMP_DURATION_TYPE_DYNAMIC"));
    list.Add<NoteJumpDurationTypeSettings, string>(NoteJumpDurationTypeSettings.Static, Localization.Get("PLAYER_SETTINGS_NOTE_JUMP_DURATION_TYPE_STATIC"));
    return (IReadOnlyList<Tuple<NoteJumpDurationTypeSettings, string>>) list;
  }
}
