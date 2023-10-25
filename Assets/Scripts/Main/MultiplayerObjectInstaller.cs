// Decompiled with JetBrains decompiler
// Type: MultiplayerObjectInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerObjectInstaller : MonoInstaller
{
  [SerializeField]
  protected MultiplayerScoreRingItem _inEnvironmentTextsPrefab;
  [SerializeField]
  protected FireworkItemController _fireworkItemControllerPrefab;
  [SerializeField]
  protected MultiplayerResultsPyramidViewAvatar _multiplayerResultsPyramidViewAvatarPrefab;

  public override void InstallBindings()
  {
    this.Container.BindMemoryPool<MultiplayerScoreRingItem, MultiplayerScoreRingItem.Pool>().WithInitialSize(8).FromComponentInNewPrefab((Object) this._inEnvironmentTextsPrefab);
    this.Container.BindMemoryPool<FireworkItemController, FireworkItemController.Pool>().WithInitialSize(8).FromComponentInNewPrefab((Object) this._fireworkItemControllerPrefab);
    this.Container.BindFactory<IConnectedPlayer, MultiplayerResultsPyramidViewAvatar, MultiplayerResultsPyramidViewAvatar.Factory>().FromSubContainerResolve().ByNewContextPrefab<MultiplayerResultsPyramidViewAvatarInstaller>((Object) this._multiplayerResultsPyramidViewAvatarPrefab);
  }
}
