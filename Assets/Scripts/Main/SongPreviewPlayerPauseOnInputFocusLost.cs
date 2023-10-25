// Decompiled with JetBrains decompiler
// Type: SongPreviewPlayerPauseOnInputFocusLost
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SongPreviewPlayerPauseOnInputFocusLost : MonoBehaviour
{
  [SerializeField]
  protected AudioPlayerBase _songPreviewPlayer;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;

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
    if (!this.gameObject.activeInHierarchy)
      return;
    this._songPreviewPlayer.PauseCurrentChannel();
  }

  public virtual void HandleInputFocusReleased()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this._songPreviewPlayer.UnPauseCurrentChannel();
  }
}
