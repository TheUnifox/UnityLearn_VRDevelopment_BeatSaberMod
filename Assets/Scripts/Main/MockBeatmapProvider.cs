// Decompiled with JetBrains decompiler
// Type: MockBeatmapProvider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class MockBeatmapProvider : IServerBeatmapProvider
{
  public virtual bool VerifyBeatmapForSelectionMask(
    BeatmapIdentifierNetSerializable beatmapId,
    BeatmapLevelSelectionMask selectionMask)
  {
    return true;
  }

  public virtual BeatmapIdentifierNetSerializable SelectBeatmapFromSuggestionsWithSelectionMaskAndOwnedSongPacks(
    int playerCount,
    Dictionary<string, BeatmapIdentifierNetSerializable> suggestedBeatmaps,
    BeatmapLevelSelectionMask selectionMask,
    Dictionary<string, SongPackMask> ownedSongPacks)
  {
    return new BeatmapIdentifierNetSerializable("100Bills", "Standard", BeatmapDifficulty.Hard);
  }

  public virtual void Dispose()
  {
  }
}
