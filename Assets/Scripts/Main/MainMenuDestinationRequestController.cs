// Decompiled with JetBrains decompiler
// Type: MainMenuDestinationRequestController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using Zenject;

public class MainMenuDestinationRequestController : IInitializable, IDisposable
{
  [Inject]
  protected readonly IDestinationRequestManager _destinationRequestManager;
  [Inject]
  protected readonly MenuScenesTransitionSetupDataSO _menuScenesTransitionSetupData;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  protected CancellationTokenSource _cancellationTokenSource;

  public virtual void Initialize()
  {
    this._destinationRequestManager.didSendMenuDestinationRequestEvent += new System.Action<MenuDestination>(this.HandleDestinationRequestManagerDidSendMenuDestinationRequest);
    if (this._destinationRequestManager.currentMenuDestinationRequest == null)
      return;
    this.ProcessDestinationRequest(this._destinationRequestManager.currentMenuDestinationRequest);
  }

  public virtual void Dispose()
  {
    if (this._destinationRequestManager != null)
      this._destinationRequestManager.didSendMenuDestinationRequestEvent -= new System.Action<MenuDestination>(this.HandleDestinationRequestManagerDidSendMenuDestinationRequest);
    if ((UnityEngine.Object) this._gameScenesManager != (UnityEngine.Object) null)
      this._gameScenesManager.installEarlyBindingsEvent -= new System.Action<ScenesTransitionSetupDataSO, DiContainer>(this.HandleGameScenesManagerInstallEarlyBindings);
    this._cancellationTokenSource?.Cancel();
  }

  public virtual void HandleGameScenesManagerInstallEarlyBindings(
    ScenesTransitionSetupDataSO scenesTransitionSetupData,
    DiContainer container)
  {
    if (this._destinationRequestManager.currentMenuDestinationRequest == null || !((UnityEngine.Object) scenesTransitionSetupData == (UnityEngine.Object) this._menuScenesTransitionSetupData))
      return;
    container.Bind<MenuDestination>().FromInstance(this._destinationRequestManager.currentMenuDestinationRequest).AsSingle().NonLazy();
    this._destinationRequestManager.Clear();
    this._gameScenesManager.installEarlyBindingsEvent -= new System.Action<ScenesTransitionSetupDataSO, DiContainer>(this.HandleGameScenesManagerInstallEarlyBindings);
  }

  public virtual void HandleDestinationRequestManagerDidSendMenuDestinationRequest(
    MenuDestination menuDestination)
  {
    this.ProcessDestinationRequest(menuDestination);
  }

  public virtual async void ProcessDestinationRequest(MenuDestination menuDestination)
  {
    MainMenuDestinationRequestController requestController = this;
    requestController._cancellationTokenSource?.Cancel();
    requestController._cancellationTokenSource = new CancellationTokenSource();
    CancellationToken cancellationToken = requestController._cancellationTokenSource.Token;
    try
    {
      while (requestController._gameScenesManager.isInTransition)
        await Task.Delay(100, cancellationToken);
      bool flag = false;
      foreach (SceneInfo scene in requestController._menuScenesTransitionSetupData.scenes)
      {
        if (requestController._gameScenesManager.GetCurrentlyLoadedSceneNames().Contains(scene.sceneName))
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        MenuTransitionsHelper transitionsHelper = requestController._gameScenesManager.currentScenesContainer.TryResolve<MenuTransitionsHelper>();
        if (menuDestination == null)
          return;
        transitionsHelper.RestartGame((System.Action<DiContainer>) (container => container.Resolve<IDestinationRequestManager>().currentMenuDestinationRequest = menuDestination));
      }
      else
      {
        requestController._gameScenesManager.installEarlyBindingsEvent -= new System.Action<ScenesTransitionSetupDataSO, DiContainer>(requestController.HandleGameScenesManagerInstallEarlyBindings);
        requestController._gameScenesManager.installEarlyBindingsEvent += new System.Action<ScenesTransitionSetupDataSO, DiContainer>(requestController.HandleGameScenesManagerInstallEarlyBindings);
      }
    }
    catch (OperationCanceledException ex)
    {
    }
  }
}
