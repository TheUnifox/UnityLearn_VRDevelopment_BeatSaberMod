// Decompiled with JetBrains decompiler
// Type: AnniversaryManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class AnniversaryManager : MonoBehaviour
{
  [SerializeField]
  protected FireworksController _fireworksController;
  [Inject]
  protected readonly MainMenuViewController _mainMenuViewController;

  public virtual void Start()
  {
    this.StartFireworks();
    this._mainMenuViewController.didFinishEvent += new System.Action<MainMenuViewController, MainMenuViewController.MenuButton>(this.HandleMainMenuViewControllerDidFinish);
    this._mainMenuViewController.musicPackPromoButtonWasPressedEvent += new System.Action<IBeatmapLevelPack, IPreviewBeatmapLevel>(this.HandleMainMenuViewControllerMusicPackPromoButtonWasPressed);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._mainMenuViewController != (UnityEngine.Object) null))
      return;
    this._mainMenuViewController.didFinishEvent -= new System.Action<MainMenuViewController, MainMenuViewController.MenuButton>(this.HandleMainMenuViewControllerDidFinish);
    this._mainMenuViewController.musicPackPromoButtonWasPressedEvent -= new System.Action<IBeatmapLevelPack, IPreviewBeatmapLevel>(this.HandleMainMenuViewControllerMusicPackPromoButtonWasPressed);
  }

  public virtual void HandleMainMenuViewControllerDidFinish(
    MainMenuViewController mainMenuViewController,
    MainMenuViewController.MenuButton menuButton)
  {
    this.StopFireworks();
  }

  public virtual void HandleMainMenuViewControllerMusicPackPromoButtonWasPressed(
    IBeatmapLevelPack musicPack,
    IPreviewBeatmapLevel beatmap)
  {
    this.StopFireworks();
  }

  public virtual void StartFireworks() => this._fireworksController.enabled = true;

  public virtual void StopFireworks() => this._fireworksController.enabled = false;
}
