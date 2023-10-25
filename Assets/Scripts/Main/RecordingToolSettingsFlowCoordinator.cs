// Decompiled with JetBrains decompiler
// Type: RecordingToolSettingsFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using Zenject;

public class RecordingToolSettingsFlowCoordinator : FlowCoordinator
{
  [SerializeField]
  protected RecordingToolConfigViewController _recordingToolConfigViewController;
  [SerializeField]
  protected RecordingToolSettingsViewController _recordingToolSettingsViewController;
  [SerializeField]
  protected RecordingToolLoggingViewController _recordingToolLoggingViewController;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  [Inject]
  protected readonly RecordingToolSettingsFlowCoordinator.InitData _initData;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this.showBackButton = false;
    if (!addedToHierarchy)
      return;
    this._recordingToolSettingsViewController.didFinishEvent += new System.Action(this.HandleRecordingToolSettingsViewControllerDidFinish);
    this.SetTitle("Recording tool settings");
    this.ProvideInitialViewControllers((ViewController) this._recordingToolSettingsViewController, (ViewController) this._recordingToolConfigViewController, (ViewController) this._recordingToolLoggingViewController);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._recordingToolSettingsViewController.didFinishEvent -= new System.Action(this.HandleRecordingToolSettingsViewControllerDidFinish);
  }

  public virtual void Update()
  {
    if (!Input.GetKeyDown(KeyCode.Return))
      return;
    this.HandleRecordingToolSettingsViewControllerDidFinish();
  }

  public virtual void HandleRecordingToolSettingsViewControllerDidFinish() => this.GoToNextScene();

  public virtual void GoToNextScene() => this._gameScenesManager.ReplaceScenes(this._initData.nextScenesTransitionSetupData, minDuration: 0.7f);

  public class InitData
  {
    public readonly ScenesTransitionSetupDataSO nextScenesTransitionSetupData;

    public InitData(
      ScenesTransitionSetupDataSO nextScenesTransitionSetupData)
    {
      this.nextScenesTransitionSetupData = nextScenesTransitionSetupData;
    }
  }
}
