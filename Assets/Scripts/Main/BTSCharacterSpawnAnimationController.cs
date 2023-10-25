// Decompiled with JetBrains decompiler
// Type: BTSCharacterSpawnAnimationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

public class BTSCharacterSpawnAnimationController : MonoBehaviour
{
  [SerializeField]
  protected PlayableDirector _spawnCharacterPlayableDirector;
  [Space]
  [SerializeField]
  protected JumpReceiver _jumpReceiver;
  [SerializeField]
  protected float _appearAnimationEndTime;
  [SerializeField]
  protected float _disappearAnimationStartTime;
  [Space]
  [SerializeField]
  protected MaterialPropertyBlockColorSetter _rimLightColorSetter;
  [SerializeField]
  protected MaterialPropertyBlockFloatAnimator _rimLightIntensityAnimator;
  [SerializeField]
  protected MaterialPropertyBlockFloatAnimator _rimLightEdgeStartAnimator;
  [Inject]
  protected readonly SongSpeedData _songSpeedData;
  protected const string kCharacterActivationStreamName = "CharacterActivationTrack";
  protected ActivationTrack _characterActivationTrack;
  protected BTSCharacter _currentBtsCharacter;
  protected float _defaultSpawnCharacterDuration;
  protected double _playableDirectorTimeBeforePause;
  protected float _animatorNormalizedTimeBeforePause;

  public bool isCharacterVisible => this._rimLightColorSetter.gameObject.activeInHierarchy;

  public float duration => (float) this._spawnCharacterPlayableDirector.duration * this._songSpeedData.speedMul;

  private ActivationTrack characterActivationTrack
  {
    get
    {
      if ((Object) this._characterActivationTrack == (Object) null)
      {
        foreach (PlayableBinding output in this._spawnCharacterPlayableDirector.playableAsset.outputs)
        {
          if (output.streamName == "CharacterActivationTrack")
            this._characterActivationTrack = (ActivationTrack) output.sourceObject;
        }
      }
      return this._characterActivationTrack;
    }
  }

  public virtual void PlayAnimation()
  {
    this._jumpReceiver.jumpToDestinationValid = false;
    this._spawnCharacterPlayableDirector.Play();
    this._currentBtsCharacter.animator.speed = this._songSpeedData.speedMul;
    this._spawnCharacterPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed<Playable>((double) this._songSpeedData.speedMul);
  }

  public virtual void EndEarlyAnimation()
  {
    double time = this._spawnCharacterPlayableDirector.time;
    if (time > (double) this._appearAnimationEndTime && time < (double) this._disappearAnimationStartTime)
    {
      this._spawnCharacterPlayableDirector.time = (double) this._disappearAnimationStartTime;
    }
    else
    {
      if (time >= (double) this._appearAnimationEndTime)
        return;
      this._jumpReceiver.jumpToDestinationValid = true;
    }
  }

  public virtual void StopAnimation() => this._spawnCharacterPlayableDirector.Stop();

  public virtual void SetCharacter(BTSCharacter btsCharacter)
  {
    this._currentBtsCharacter = btsCharacter;
    this._rimLightColorSetter.materialPropertyBlockController = btsCharacter.materialPropertyBlockController;
    this._rimLightIntensityAnimator.materialPropertyBlockController = btsCharacter.materialPropertyBlockController;
    this._rimLightEdgeStartAnimator.materialPropertyBlockController = btsCharacter.materialPropertyBlockController;
    this._spawnCharacterPlayableDirector.SetGenericBinding((Object) this.characterActivationTrack, (Object) this._currentBtsCharacter.gameObject);
  }

  public virtual void WillResumeAnimation()
  {
    this._spawnCharacterPlayableDirector.time = this._playableDirectorTimeBeforePause;
    this._spawnCharacterPlayableDirector.Play();
    this._spawnCharacterPlayableDirector.Pause();
    this._currentBtsCharacter.animator.enabled = false;
  }

  public virtual void ResumeAnimation()
  {
    this._spawnCharacterPlayableDirector.Play();
    this._currentBtsCharacter.animator.Play(0, -1, this._animatorNormalizedTimeBeforePause);
    this._currentBtsCharacter.animator.enabled = true;
  }

  public virtual void PauseAnimation()
  {
    this._playableDirectorTimeBeforePause = this._spawnCharacterPlayableDirector.time;
    this._animatorNormalizedTimeBeforePause = this._currentBtsCharacter.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    this._currentBtsCharacter.animator.enabled = false;
  }
}
