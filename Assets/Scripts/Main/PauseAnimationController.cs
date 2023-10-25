// Decompiled with JetBrains decompiler
// Type: PauseAnimationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class PauseAnimationController : MonoBehaviour
{
  [SerializeField]
  protected Animator _animator;

  public event System.Action resumeFromPauseAnimationDidFinishEvent;

  public virtual void Awake()
  {
    this.enabled = false;
    this._animator.enabled = false;
  }

  public virtual void StartEnterPauseAnimation()
  {
    this.enabled = true;
    this._animator.enabled = true;
    this._animator.SetTrigger("EnterPause");
  }

  public virtual void StartResumeFromPauseAnimation()
  {
    this.enabled = true;
    this._animator.enabled = true;
    this._animator.SetTrigger("ResumeFromPause");
  }

  public virtual void EnterPauseAnimationDidFinish()
  {
    this.enabled = false;
    this._animator.enabled = false;
  }

  public virtual void ResumeFromPauseAnimationDidFinish()
  {
    this.enabled = false;
    this._animator.enabled = false;
    System.Action animationDidFinishEvent = this.resumeFromPauseAnimationDidFinishEvent;
    if (animationDidFinishEvent == null)
      return;
    animationDidFinishEvent();
  }
}
