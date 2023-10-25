// Decompiled with JetBrains decompiler
// Type: DeactivateOnInputFocusCapture
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using Zenject;

public class DeactivateOnInputFocusCapture : MonoBehaviour
{
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;

  public virtual void OnEnable()
  {
    if (!this._vrPlatformHelper.hasInputFocus)
    {
      this._vrPlatformHelper.inputFocusWasReleasedEvent -= new System.Action(this.HandleInputFocusReleased);
      this._vrPlatformHelper.inputFocusWasReleasedEvent += new System.Action(this.HandleInputFocusReleased);
      this.gameObject.SetActive(false);
    }
    else
    {
      this._vrPlatformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleInputFocusCaptured);
      this._vrPlatformHelper.inputFocusWasCapturedEvent += new System.Action(this.HandleInputFocusCaptured);
    }
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
    this._vrPlatformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleInputFocusCaptured);
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    this._vrPlatformHelper.inputFocusWasReleasedEvent -= new System.Action(this.HandleInputFocusReleased);
    this._vrPlatformHelper.inputFocusWasReleasedEvent += new System.Action(this.HandleInputFocusReleased);
    this.gameObject.SetActive(false);
  }

  public virtual void HandleInputFocusReleased()
  {
    this._vrPlatformHelper.inputFocusWasReleasedEvent -= new System.Action(this.HandleInputFocusReleased);
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    this.gameObject.SetActive(true);
  }
}
