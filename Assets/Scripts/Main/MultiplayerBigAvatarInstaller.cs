// Decompiled with JetBrains decompiler
// Type: MultiplayerBigAvatarInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class MultiplayerBigAvatarInstaller : MonoInstaller
{
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;
  [Inject]
  protected readonly SaberManager.InitData _saberManagerInitData;
  [Inject]
  protected readonly PlayersSpecificSettingsAtGameStartModel _playerSpecificSettings;

  public override void InstallBindings()
  {
    this.Container.Bind<IConnectedPlayer>().FromInstance(this._connectedPlayer).AsSingle();
    this.Container.Bind<SaberManager.InitData>().FromInstance(this._saberManagerInitData).AsSingle();
    PlayerSpecificSettingsNetSerializable settingsForUserId = this._playerSpecificSettings.GetPlayerSpecificSettingsForUserId(this._connectedPlayer.userId);
    this.Container.Bind<ColorScheme>().FromInstance(ColorSchemeConverter.FromNetSerializable(settingsForUserId.colorScheme)).AsSingle();
    this.Container.Bind<ColorManager>().AsSingle();
  }
}
