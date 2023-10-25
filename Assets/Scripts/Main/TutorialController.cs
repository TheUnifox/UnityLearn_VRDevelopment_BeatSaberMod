// Decompiled with JetBrains decompiler
// Type: TutorialController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class TutorialController : MonoBehaviour, ILevelStartController
{
  [SerializeField]
  protected TutorialSongController _tutorialSongController;
  [SerializeField]
  protected IntroTutorialController _introTutorialController;
  [SerializeField]
  protected AudioFading _audioFading;
  [SerializeField]
  protected TutorialScenesTransitionSetupDataSO _tutorialSceneSetupData;
  [Space]
  [SerializeField]
  [SignalSender]
  protected Signal _tutorialIntroStartedSignal;
  [SerializeField]
  [SignalSender]
  protected Signal _tutorialFinishedSignal;
  [Inject]
  protected readonly PauseController _pauseController;
  protected bool _paused;
  protected bool _doingOutroTransition;

  public event System.Action levelWillStartIntroEvent;

  public event System.Action levelDidStartEvent;

  public virtual void Start()
  {
    this._introTutorialController.introTutorialDidFinishEvent += new System.Action(this.HandleIntroTutorialDidFinishEvent);
    this._tutorialSongController.songDidFinishEvent += new System.Action(this.HandleTutorialSongControllerSongDidFinishEvent);
    this._pauseController.canPauseEvent += new System.Action<System.Action<bool>>(this.HandlePauseControllerCanPause);
    this._pauseController.didPauseEvent += new System.Action(this.HandlePauseControllerDidPause);
    this._pauseController.didResumeEvent += new System.Action(this.HandlePauseControllerDidResume);
    System.Action willStartIntroEvent = this.levelWillStartIntroEvent;
    if (willStartIntroEvent != null)
      willStartIntroEvent();
    this._tutorialIntroStartedSignal.Raise();
    System.Action levelDidStartEvent = this.levelDidStartEvent;
    if (levelDidStartEvent != null)
      levelDidStartEvent();
    if (!this._pauseController.wantsToPause)
      return;
    this._pauseController.Pause();
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._introTutorialController != (UnityEngine.Object) null)
      this._introTutorialController.introTutorialDidFinishEvent -= new System.Action(this.HandleIntroTutorialDidFinishEvent);
    if ((UnityEngine.Object) this._tutorialSongController != (UnityEngine.Object) null)
      this._tutorialSongController.songDidFinishEvent -= new System.Action(this.HandleTutorialSongControllerSongDidFinishEvent);
    if (!((UnityEngine.Object) this._pauseController != (UnityEngine.Object) null))
      return;
    this._pauseController.canPauseEvent -= new System.Action<System.Action<bool>>(this.HandlePauseControllerCanPause);
    this._pauseController.didPauseEvent -= new System.Action(this.HandlePauseControllerDidPause);
    this._pauseController.didResumeEvent -= new System.Action(this.HandlePauseControllerDidResume);
  }

  public virtual void HandleIntroTutorialDidFinishEvent() => this._tutorialSongController.StartSong();

  public virtual void HandleTutorialSongControllerSongDidFinishEvent()
  {
    this._doingOutroTransition = true;
    this._audioFading.FadeOut();
    this.StartCoroutine(this.OutroCoroutine());
  }

  public virtual IEnumerator OutroCoroutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this._tutorialFinishedSignal.Raise();
    yield return (object) new WaitForSeconds(2.7f);
    this._tutorialSceneSetupData.Finish(TutorialScenesTransitionSetupDataSO.TutorialEndStateType.Completed);
  }

  public virtual void HandlePauseControllerCanPause(System.Action<bool> canPause)
  {
    if (canPause == null)
      return;
    canPause(!this._paused && !this._doingOutroTransition);
  }

  public virtual void HandlePauseControllerDidPause() => this._paused = true;

  public virtual void HandlePauseControllerDidResume() => this._paused = false;
}
