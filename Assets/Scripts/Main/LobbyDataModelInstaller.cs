// Decompiled with JetBrains decompiler
// Type: LobbyDataModelInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using Zenject;

public class LobbyDataModelInstaller : MonoInstaller
{
  public override void InstallBindings()
  {
    this.Container.Bind<MultiplayerLobbyConnectionController>().AsSingle();
    this.Container.Bind(typeof (ILobbyStateDataModel), typeof (LobbyStateDataModel), typeof (IDisposable)).To<LobbyStateDataModel>().AsSingle();
    this.Container.Bind(typeof (ILobbyPlayersDataModel), typeof (LobbyPlayersDataModel), typeof (IDisposable)).To<LobbyPlayersDataModel>().AsSingle();
    this.Container.Bind(typeof (ILobbyGameStateController), typeof (LobbyGameStateController), typeof (IDisposable)).To<LobbyGameStateController>().AsSingle();
    this.Container.Bind<LobbyDataModelsManager>().AsSingle();
  }
}
