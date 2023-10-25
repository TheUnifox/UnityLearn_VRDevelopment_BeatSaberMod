// Decompiled with JetBrains decompiler
// Type: VFXAnimationEvents
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using UnityEngine;

[ExecuteAlways]
public class VFXAnimationEvents : MonoBehaviour
{
  [SerializeField]
  protected Animation _animation;
  [SerializeField]
  protected VFXAnimationEvents.VFXAnimationEvent[] _animationEvents;

  public event System.Action animationDidPauseEvent;

  public event System.Action spawnCharacterEvent;

  public event System.Action despawnCharacterEvent;

  public virtual void PlayEvent(string eventName)
  {
    foreach (VFXAnimationEvents.VFXAnimationEvent animationEvent in this._animationEvents)
    {
      if (animationEvent.name == eventName)
      {
        foreach (ParticleSystem particleSystem in animationEvent.particleSystems)
          particleSystem.Play(false);
        break;
      }
    }
  }

  public virtual void PauseAnimation()
  {
    foreach (AnimationState animationState in this._animation)
      animationState.speed = 0.0f;
    System.Action animationDidPauseEvent = this.animationDidPauseEvent;
    if (animationDidPauseEvent == null)
      return;
    animationDidPauseEvent();
  }

  public virtual void SpawnCharacterEvent()
  {
    System.Action spawnCharacterEvent = this.spawnCharacterEvent;
    if (spawnCharacterEvent == null)
      return;
    spawnCharacterEvent();
  }

  public virtual void DeSpawnCharacterEvent()
  {
    System.Action despawnCharacterEvent = this.despawnCharacterEvent;
    if (despawnCharacterEvent == null)
      return;
    despawnCharacterEvent();
  }

  public virtual void ResumeAnimation()
  {
    foreach (AnimationState animationState in this._animation)
      animationState.speed = 1f;
  }

  [Serializable]
  public class VFXAnimationEvent
  {
    [SerializeField]
    protected string _name;
    [SerializeField]
    protected ParticleSystem[] _particleSystems;

    public string name => this._name;

    public ParticleSystem[] particleSystems => this._particleSystems;
  }
}
