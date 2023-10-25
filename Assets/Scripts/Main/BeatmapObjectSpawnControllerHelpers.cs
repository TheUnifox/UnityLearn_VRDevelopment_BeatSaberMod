// Decompiled with JetBrains decompiler
// Type: BeatmapObjectSpawnControllerHelpers
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class BeatmapObjectSpawnControllerHelpers
{
  public static void GetNoteJumpValues(
    PlayerSpecificSettings playerSpecificSettings,
    float defaultNoteJumpStartBeatOffset,
    out BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType,
    out float noteJumpValue)
  {
    noteJumpValueType = playerSpecificSettings.noteJumpDurationTypeSettings == NoteJumpDurationTypeSettings.Dynamic ? BeatmapObjectSpawnMovementData.NoteJumpValueType.BeatOffset : BeatmapObjectSpawnMovementData.NoteJumpValueType.JumpDuration;
    float num = defaultNoteJumpStartBeatOffset + playerSpecificSettings.noteJumpStartBeatOffset;
    float jumpFixedDuration = playerSpecificSettings.noteJumpFixedDuration;
    noteJumpValue = noteJumpValueType == BeatmapObjectSpawnMovementData.NoteJumpValueType.BeatOffset ? num : jumpFixedDuration;
  }
}
