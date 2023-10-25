// Decompiled with JetBrains decompiler
// Type: CannotStartGameReasonMethods
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;

public static class CannotStartGameReasonMethods
{
  [LocalizationKey]
  private const string kAllPlayersSpectating = "LABEL_CANT_START_GAME_ALL_PLAYERS_SPECTATING";
  [LocalizationKey]
  private const string kNoSongSelected = "LABEL_CANT_START_GAME_NO_SONG_SELECTED";
  [LocalizationKey]
  private const string kAllPlayersNotInLobby = "LABEL_CANT_START_GAME_ALL_PLAYERS_NOT_IN_LOBBY";
  [LocalizationKey]
  private const string kDoNotOwnSong = "LABEL_CANT_START_GAME_DO_NOT_OWN_SONG";

  public static string LocalizedKey(this CannotStartGameReason cannotStartGameReason)
  {
    switch (cannotStartGameReason)
    {
      case CannotStartGameReason.AllPlayersSpectating:
        return Localization.Get("LABEL_CANT_START_GAME_ALL_PLAYERS_SPECTATING");
      case CannotStartGameReason.NoSongSelected:
        return Localization.Get("LABEL_CANT_START_GAME_NO_SONG_SELECTED");
      case CannotStartGameReason.AllPlayersNotInLobby:
        return Localization.Get("LABEL_CANT_START_GAME_ALL_PLAYERS_NOT_IN_LOBBY");
      case CannotStartGameReason.DoNotOwnSong:
        return Localization.Get("LABEL_CANT_START_GAME_DO_NOT_OWN_SONG");
      default:
        return "";
    }
  }
}
