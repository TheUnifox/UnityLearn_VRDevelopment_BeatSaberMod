// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLobbyInstaller : MonoInstaller
{
  [SerializeField]
  protected MultiplayerLobbyAvatarController _multiplayerLobbyAvatarControllerPrefab;
  [SerializeField]
  protected MultiplayerLobbyAvatarPlace _multiplayerAvatarPlacePrefab;

  public override void InstallBindings()
  {
    this.Container.BindMemoryPool<MultiplayerLobbyAvatarPlace, MultiplayerLobbyAvatarPlace.Pool>().WithInitialSize(4).FromComponentInNewPrefab((Object) this._multiplayerAvatarPlacePrefab);
    this.Container.BindFactory<IConnectedPlayer, MultiplayerLobbyAvatarController, MultiplayerLobbyAvatarController.Factory>().FromSubContainerResolve().ByNewContextPrefab<LobbyAvatarInstaller>((Object) this._multiplayerLobbyAvatarControllerPrefab);
  }
}
