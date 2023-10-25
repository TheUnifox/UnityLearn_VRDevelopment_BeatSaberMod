// Decompiled with JetBrains decompiler
// Type: PlayingMultiplayerRichPresenceData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;

public class PlayingMultiplayerRichPresenceData : InMultiplayerRichPresenceData
{
  [LocalizationKey]
  protected const string kPlayingMultiplayerLobbyRichPresenceLocalizationKey = "PLAYING_MULTIPLAYER_PRESENCE";

  public PlayingMultiplayerRichPresenceData(
    IDifficultyBeatmap difficultyBeatmap,
    bool atMaxPartySize)
    : base((string) null, false, atMaxPartySize)
  {
    this.apiName = "multiplayer";
    this.localizedDescription = Localization.Get("PLAYING_MULTIPLAYER_PRESENCE");
  }
}
