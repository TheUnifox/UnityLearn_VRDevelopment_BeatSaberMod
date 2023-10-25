// Decompiled with JetBrains decompiler
// Type: DeactivateAnimatorOnInputFocusCapture
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using Zenject;

public class DeactivateAnimatorOnInputFocusCapture : MonoBehaviour
{
  [SerializeField]
  protected Animator _animator;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;
  protected bool _wasEnabled;

  public virtual void Start()
  {
    this._vrPlatformHelper.inputFocusWasCapturedEvent += new System.Action(this.HandleInputFocusCaptured);
    this._vrPlatformHelper.inputFocusWasReleasedEvent += new System.Action(this.HandleInputFocusReleased);
    if (this._vrPlatformHelper.hasInputFocus)
      return;
    this.HandleInputFocusCaptured();
  }

  public virtual void OnDestroy()
  {
    if (this._vrPlatformHelper == null)
      return;
    this._vrPlatformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleInputFocusCaptured);
    this._vrPlatformHelper.inputFocusWasReleasedEvent -= new System.Action(this.HandleInputFocusReleased);
  }

  public virtual void HandleInputFocusCaptured()
  {
    this._wasEnabled = this._animator.enabled;
    this._animator.enabled = false;
  }

  public virtual void HandleInputFocusReleased() => this._animator.enabled = this._wasEnabled;
}
