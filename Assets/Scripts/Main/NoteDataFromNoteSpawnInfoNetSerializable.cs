// Decompiled with JetBrains decompiler
// Type: NoteDataFromNoteSpawnInfoNetSerializable
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class NoteDataFromNoteSpawnInfoNetSerializable : NoteData
{
  public NoteDataFromNoteSpawnInfoNetSerializable(NoteSpawnInfoNetSerializable noteSpawnInfo)
    : base(noteSpawnInfo.time, noteSpawnInfo.lineIndex, noteSpawnInfo.noteLineLayer, noteSpawnInfo.beforeJumpNoteLineLayer, noteSpawnInfo.gameplayType, noteSpawnInfo.scoringType, noteSpawnInfo.colorType, noteSpawnInfo.cutDirection, noteSpawnInfo.timeToNextColorNote, noteSpawnInfo.timeToPrevColorNote, noteSpawnInfo.flipLineIndex, noteSpawnInfo.flipYSide, noteSpawnInfo.cutDirectionAngleOffset, noteSpawnInfo.cutSfxVolumeMultiplier)
  {
  }
}
