// Decompiled with JetBrains decompiler
// Type: DeeplinkManagerToDestinationRequestManagerAdapter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class DeeplinkManagerToDestinationRequestManagerAdapter : IDestinationRequestManager
{
  [Inject]
  protected BeatmapLevelsModel _beatmapLevelsModel;
  [Inject]
  protected BeatmapCharacteristicCollectionSO _beatmapCharacteristicCollection;
  protected MenuDestination _currentMenuDestinationRequest;

  public event System.Action<MenuDestination> didSendMenuDestinationRequestEvent;

  public MenuDestination currentMenuDestinationRequest
  {
    get => this._currentMenuDestinationRequest;
    set
    {
      this._currentMenuDestinationRequest = value;
      System.Action<MenuDestination> destinationRequestEvent = this.didSendMenuDestinationRequestEvent;
      if (destinationRequestEvent == null)
        return;
      destinationRequestEvent(value);
    }
  }

  [Inject]
  public virtual void Init(IDeeplinkManager deeplinkManager)
  {
    deeplinkManager.didReceiveDeeplinkEvent += new System.Action<Deeplink>(this.HandleDeeplinkManagerDidReceiveDeeplink);
    if (deeplinkManager.currentDeeplink == null)
      return;
    this.HandleDeeplinkManagerDidReceiveDeeplink(deeplinkManager.currentDeeplink);
  }

  public virtual void Clear() => this._currentMenuDestinationRequest = (MenuDestination) null;

  public virtual void HandleDeeplinkManagerDidReceiveDeeplink(Deeplink deeplink)
  {
    this._currentMenuDestinationRequest = (MenuDestination) null;
    if (!string.IsNullOrWhiteSpace(deeplink.LevelID))
    {
      IBeatmapLevelPack levelPackForLevelId = this._beatmapLevelsModel.GetLevelPackForLevelId(deeplink.LevelID);
      IPreviewBeatmapLevel previewForLevelId = this._beatmapLevelsModel.GetLevelPreviewForLevelId(deeplink.LevelID);
      BeatmapCharacteristicSO characteristicSo1 = (BeatmapCharacteristicSO) null;
      if (!string.IsNullOrWhiteSpace(deeplink.Characteristic))
        characteristicSo1 = this._beatmapCharacteristicCollection.GetBeatmapCharacteristicBySerializedName(deeplink.Characteristic);
      BeatmapDifficulty difficulty;
      deeplink.Difficulty.BeatmapDifficultyFromSerializedName(out difficulty);
      IBeatmapLevelPack beatmapLevelPack = levelPackForLevelId;
      IPreviewBeatmapLevel previewBeatmapLevel = previewForLevelId;
      BeatmapCharacteristicSO characteristicSo2 = characteristicSo1;
      int num = (int) difficulty;
      BeatmapCharacteristicSO beatmapCharacteristic = characteristicSo2;
      this._currentMenuDestinationRequest = (MenuDestination) new SelectLevelDestination(beatmapLevelPack, previewBeatmapLevel, (BeatmapDifficulty) num, beatmapCharacteristic);
    }
    else if (!string.IsNullOrWhiteSpace(deeplink.PackID))
    {
      IBeatmapLevelPack levelPack = this._beatmapLevelsModel.GetLevelPack(deeplink.PackID);
      if (levelPack != null)
        this._currentMenuDestinationRequest = (MenuDestination) new SelectLevelPackDestination(levelPack);
    }
    else if (!string.IsNullOrWhiteSpace(deeplink.MultiplayerLobbyCode))
      this._currentMenuDestinationRequest = (MenuDestination) new SelectMultiplayerLobbyDestination(string.Empty, deeplink.MultiplayerLobbyCode);
    else if (deeplink.MultiplayerRoomId > 0UL)
      this._currentMenuDestinationRequest = (MenuDestination) new SelectMultiplayerLobbyDestination(deeplink.MultiplayerRoomId);
    else if (!string.IsNullOrWhiteSpace(deeplink.MultiplayerSecret))
      this._currentMenuDestinationRequest = (MenuDestination) new SelectMultiplayerLobbyDestination(deeplink.MultiplayerSecret, string.Empty);
    else if (!string.IsNullOrWhiteSpace(deeplink.Destination))
    {
      SelectSubMenuDestination.Destination menuDestination;
      switch (deeplink.Destination.ToLower().Replace(" ", ""))
      {
        case "campaign":
          menuDestination = SelectSubMenuDestination.Destination.Campaign;
          break;
        case "dlc":
        case "level":
        case "pack":
        case "solo":
        case "solofreeplay":
        case "song":
          menuDestination = SelectSubMenuDestination.Destination.SoloFreePlay;
          break;
        case "lobby":
        case "multiplayer":
          menuDestination = SelectSubMenuDestination.Destination.Multiplayer;
          break;
        case "party":
        case "partyfreeplay":
          menuDestination = SelectSubMenuDestination.Destination.PartyFreePlay;
          break;
        case "settings":
          menuDestination = SelectSubMenuDestination.Destination.Settings;
          break;
        case "tutorial":
          menuDestination = SelectSubMenuDestination.Destination.Tutorial;
          break;
        default:
          menuDestination = SelectSubMenuDestination.Destination.MainMenu;
          break;
      }
      this._currentMenuDestinationRequest = (MenuDestination) new SelectSubMenuDestination(menuDestination);
    }
    if (this._currentMenuDestinationRequest == null)
      return;
    System.Action<MenuDestination> destinationRequestEvent = this.didSendMenuDestinationRequestEvent;
    if (destinationRequestEvent == null)
      return;
    destinationRequestEvent(this._currentMenuDestinationRequest);
  }
}
