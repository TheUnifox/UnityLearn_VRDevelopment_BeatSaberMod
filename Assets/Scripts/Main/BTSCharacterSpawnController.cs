// Decompiled with JetBrains decompiler
// Type: BTSCharacterSpawnController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class BTSCharacterSpawnController : MonoBehaviour
{
  [SerializeField]
  protected BTSCharacterSpawnAnimationController _characterSpawnAnimationController;
  [Space]
  [SerializeField]
  protected BTSCharacterSpawnEventEffect _btsCharacterSpawnEventEffect;
  [Inject]
  protected readonly IGamePause _gamePause;
  [Inject]
  protected readonly ILevelEndActions _levelEndActions;
  protected bool _characterSpawned;
  protected double _playableDirectorTimeBeforePause;
  protected float _animatorNormalizedTimeBeforePause;
  protected Coroutine _despawnCharacterCoroutine;

  public bool isCharacterVisible => this._characterSpawnAnimationController.isCharacterVisible;

  public bool isSpawned => this._characterSpawned;

  public virtual void Start()
  {
    this._btsCharacterSpawnEventEffect.startCharacterAnimationEvent += new System.Action<BTSCharacter>(this.HandleStartCharacterAnimation);
    this._gamePause.didPauseEvent += new System.Action(this.HandleGamePauseDidPause);
    this._gamePause.willResumeEvent += new System.Action(this.HandleGamePauseWillResume);
    this._gamePause.didResumeEvent += new System.Action(this.HandleGamePauseDidResume);
    this._levelEndActions.levelFailedEvent += new System.Action(this.HandleLevelEndActionsLevelFailed);
  }

  public virtual void OnDestroy()
  {
    if (this._despawnCharacterCoroutine != null)
    {
      this.StopCoroutine(this._despawnCharacterCoroutine);
      this._despawnCharacterCoroutine = (Coroutine) null;
    }
    if ((UnityEngine.Object) this._btsCharacterSpawnEventEffect != (UnityEngine.Object) null)
      this._btsCharacterSpawnEventEffect.startCharacterAnimationEvent -= new System.Action<BTSCharacter>(this.HandleStartCharacterAnimation);
    if (this._gamePause != null)
    {
      this._gamePause.didPauseEvent -= new System.Action(this.HandleGamePauseDidPause);
      this._gamePause.willResumeEvent -= new System.Action(this.HandleGamePauseWillResume);
      this._gamePause.didResumeEvent -= new System.Action(this.HandleGamePauseDidResume);
    }
    if (this._levelEndActions == null)
      return;
    this._levelEndActions.levelFailedEvent -= new System.Action(this.HandleLevelEndActionsLevelFailed);
  }

  public virtual void HandleStartCharacterAnimation(BTSCharacter btsCharacter)
  {
    if (this._characterSpawned)
    {
      Debug.LogWarning((object) "Wanting to spawn character while one is already spawned");
    }
    else
    {
      this._characterSpawned = true;
      this._characterSpawnAnimationController.SetCharacter(btsCharacter);
      this._characterSpawnAnimationController.PlayAnimation();
      this._despawnCharacterCoroutine = this.StartCoroutine(this.TimelineStoppedDelayed());
    }
  }

  public virtual void HandleGamePauseDidPause()
  {
    if (this._characterSpawned)
      this._characterSpawnAnimationController.PauseAnimation();
    this.gameObject.SetActive(false);
  }

  public virtual void HandleGamePauseWillResume()
  {
    this.gameObject.SetActive(true);
    if (!this._characterSpawned)
      return;
    this._characterSpawnAnimationController.WillResumeAnimation();
  }

  public virtual void HandleGamePauseDidResume()
  {
    if (!this._characterSpawned)
      return;
    this._characterSpawnAnimationController.ResumeAnimation();
  }

  public virtual void HandleLevelEndActionsLevelFailed()
  {
    if (!this._characterSpawned)
      return;
    this._characterSpawnAnimationController.EndEarlyAnimation();
  }

  public virtual IEnumerator TimelineStoppedDelayed()
  {
    yield return (object) new WaitForSeconds(this._characterSpawnAnimationController.duration);
    this._characterSpawned = false;
    this._despawnCharacterCoroutine = (Coroutine) null;
  }
}
