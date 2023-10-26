// Decompiled with JetBrains decompiler
// Type: CreditsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CreditsController : MonoBehaviour
{
  [SerializeField]
  protected CreditsScenesTransitionSetupDataSO _creditsSceneSetupDataSO;
  [SerializeField]
  protected AudioPlayerBase _audioPlayer;
  [SerializeField]
  protected Button _continueButton;
  [SerializeField]
  protected RectTransform _contentRectTransform;
  [SerializeField]
  protected float _overflowHeight = 60f;
  [SerializeField]
  protected RectTransform _contentWrapper;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;
  protected ButtonBinder _buttonBinder;
  protected bool _didFinish;
  protected bool _isPaused;

  public virtual void Start()
  {
    this._buttonBinder = new ButtonBinder(this._continueButton, new System.Action(this.Finish));
    this._vrPlatformHelper.inputFocusWasCapturedEvent += new System.Action(this.HandleInputFocusCaptured);
    this._vrPlatformHelper.inputFocusWasReleasedEvent += new System.Action(this.HandleInputFocusReleased);
    if (!this._vrPlatformHelper.hasInputFocus)
      this.HandleInputFocusCaptured();
    this.StartCoroutine(this.ScrollCoroutine());
  }

  public virtual void OnDestroy()
  {
    this._buttonBinder?.ClearBindings();
    if (this._vrPlatformHelper == null)
      return;
    this._vrPlatformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleInputFocusCaptured);
    this._vrPlatformHelper.inputFocusWasReleasedEvent -= new System.Action(this.HandleInputFocusReleased);
  }

  public virtual void Finish()
  {
    if (this._didFinish)
      return;
    this._vrPlatformHelper.inputFocusWasCapturedEvent -= new System.Action(this.HandleInputFocusCaptured);
    this._vrPlatformHelper.inputFocusWasReleasedEvent -= new System.Action(this.HandleInputFocusReleased);
    this._didFinish = true;
    this._creditsSceneSetupDataSO.Finish();
  }

  public virtual IEnumerator ScrollCoroutine()
  {
    CreditsController creditsController = this;
    float contentHeight = creditsController._contentRectTransform.rect.height;
    float contentWrapperHeight = creditsController._contentWrapper.rect.height;
    float posY = -contentWrapperHeight;
    creditsController._contentWrapper.anchoredPosition = new Vector2(0.0f, posY);
    yield return (object) null;
    RectTransform transform = (RectTransform) creditsController.transform;
    AudioClip activeAudioClip = creditsController._audioPlayer.activeAudioClip;
    float num = 60f;
    if ((UnityEngine.Object) activeAudioClip != (UnityEngine.Object) null)
      num = activeAudioClip.length;
    float scrollingSpeed = (contentWrapperHeight + transform.rect.height) / num;
    while (true)
    {
      do
      {
        if (!creditsController._isPaused)
        {
          creditsController._contentWrapper.anchoredPosition = new Vector2(0.0f, posY);
          posY += Time.deltaTime * scrollingSpeed;
        }
        yield return (object) null;
      }
      while (creditsController._didFinish || (double) posY <= -(double) contentHeight + (double) contentWrapperHeight + (double) creditsController._overflowHeight);
      creditsController.Finish();
    }
  }

  public virtual void HandleInputFocusCaptured()
  {
    this._audioPlayer.PauseCurrentChannel();
    this._isPaused = true;
  }

  public virtual void HandleInputFocusReleased()
  {
    this._audioPlayer.UnPauseCurrentChannel();
    this._isPaused = false;
  }
}
