// Decompiled with JetBrains decompiler
// Type: ScreenBackButtonAnimationController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Collections.Generic;
using UnityEngine;

public class ScreenBackButtonAnimationController : MonoBehaviour
{
  [SerializeField]
  protected Animator _animator;
  protected readonly Dictionary<ScreenBackButtonAnimationController.AnimationType, int> _animationHashes = new Dictionary<ScreenBackButtonAnimationController.AnimationType, int>()
  {
    {
      ScreenBackButtonAnimationController.AnimationType.FadeIn,
      Animator.StringToHash("FadeIn")
    },
    {
      ScreenBackButtonAnimationController.AnimationType.FadeOut,
      Animator.StringToHash("FadeOut")
    },
    {
      ScreenBackButtonAnimationController.AnimationType.MoveIn,
      Animator.StringToHash("MoveIn")
    },
    {
      ScreenBackButtonAnimationController.AnimationType.MoveOut,
      Animator.StringToHash("MoveOut")
    },
    {
      ScreenBackButtonAnimationController.AnimationType.MoveIn2,
      Animator.StringToHash("MoveIn2")
    },
    {
      ScreenBackButtonAnimationController.AnimationType.MoveOut2,
      Animator.StringToHash("MoveOut2")
    }
  };

  public virtual void Awake() => this._animator.keepAnimatorStateOnDisable = true;

  public virtual void StartAnimation(
    ScreenBackButtonAnimationController.AnimationType animationType)
  {
    this._animator.SetTrigger(this._animationHashes[animationType]);
  }

  public enum AnimationType
  {
    FadeIn,
    FadeOut,
    MoveIn,
    MoveOut,
    MoveIn2,
    MoveOut2,
  }
}
