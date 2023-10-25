// Decompiled with JetBrains decompiler
// Type: MultiplayerScoreDiffText
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using TMPro;
using Tweening;
using UnityEngine;
using Zenject;

public class MultiplayerScoreDiffText : MonoBehaviour
{
  [SerializeField]
  protected Color _activeTextColor = Color.white;
  [SerializeField]
  protected Color _normalBackgroundColor = Color.white;
  [SerializeField]
  protected Color _leadingBackgroundColor = Color.white;
  [SerializeField]
  protected bool _useAutomaticLeadPlayerSelection = true;
  [Space]
  [SerializeField]
  protected TextMeshPro _onPlatformText;
  [SerializeField]
  protected SpriteRenderer _backgroundSpriteRenderer;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;
  [Inject]
  protected readonly MultiplayerLeadPlayerProvider _leadPlayerProvider;
  protected Color _currentBackgroundColor;
  protected MultiplayerScoreDiffText.State _state;
  protected Tweening.FloatTween _onPlatformTextAlphaTween;

  public virtual void Start()
  {
    this._currentBackgroundColor = this._normalBackgroundColor;
    this._onPlatformTextAlphaTween = new Tweening.FloatTween(0.0f, 1f, (System.Action<float>) (val =>
    {
      this._onPlatformText.color = this._activeTextColor.ColorWithAlpha(this._activeTextColor.a * val);
      this._backgroundSpriteRenderer.color = this._currentBackgroundColor.ColorWithAlpha(this._currentBackgroundColor.a * val);
    }), 0.5f, EaseType.OutQuad);
    this.gameObject.SetActive(false);
    this._state = MultiplayerScoreDiffText.State.Hidden;
    if (!this._useAutomaticLeadPlayerSelection)
      return;
    this._leadPlayerProvider.newLeaderWasSelectedEvent += new System.Action<string>(this.HandleNewLeaderWasSelected);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null)
      this._tweeningManager.KillAllTweens((object) this);
    if (!((UnityEngine.Object) this._leadPlayerProvider != (UnityEngine.Object) null))
      return;
    this._leadPlayerProvider.newLeaderWasSelectedEvent -= new System.Action<string>(this.HandleNewLeaderWasSelected);
  }

  public virtual void SetHorizontalPositionRelativeToLocalPlayer(
    MultiplayerScoreDiffText.HorizontalPosition relativePosition)
  {
    this._onPlatformText.text = relativePosition.ToString();
    this.transform.localRotation = Quaternion.Euler(0.0f, relativePosition == MultiplayerScoreDiffText.HorizontalPosition.Right ? 0.0f : 180f, 0.0f);
  }

  public virtual void AnimateScoreDiff(int scoreDiff)
  {
    this._onPlatformText.text = ((float) scoreDiff / 1000f).ToString("+0.0k;-0.0k");
    if (this._state == MultiplayerScoreDiffText.State.Displayed || this._state == MultiplayerScoreDiffText.State.AnimatingDisplay || scoreDiff == 0)
      return;
    if (this._state == MultiplayerScoreDiffText.State.Hidden)
    {
      this.gameObject.SetActive(true);
      this._onPlatformText.color = this._activeTextColor.ColorWithAlpha(0.0f);
      this._backgroundSpriteRenderer.color = this._currentBackgroundColor.ColorWithAlpha(0.0f);
    }
    this._onPlatformTextAlphaTween.fromValue = this._onPlatformText.color.a / this._activeTextColor.a;
    this._onPlatformTextAlphaTween.toValue = 1f;
    this._onPlatformTextAlphaTween.onCompleted = (System.Action) (() => this._state = MultiplayerScoreDiffText.State.Displayed);
    this._state = MultiplayerScoreDiffText.State.AnimatingDisplay;
    this._tweeningManager.RestartTween((Tween) this._onPlatformTextAlphaTween, (object) this);
  }

  public virtual void AnimateHide()
  {
    if (this._state == MultiplayerScoreDiffText.State.Hidden)
      return;
    this._onPlatformTextAlphaTween.toValue = 0.0f;
    this._onPlatformTextAlphaTween.fromValue = this._onPlatformText.color.a / this._activeTextColor.a;
    this._onPlatformTextAlphaTween.onCompleted = (System.Action) (() =>
    {
      this.gameObject.SetActive(false);
      this._state = MultiplayerScoreDiffText.State.Hidden;
    });
    this._state = MultiplayerScoreDiffText.State.AnimatingHide;
    this._tweeningManager.RestartTween((Tween) this._onPlatformTextAlphaTween, (object) this);
  }

  public virtual void AnimateIsLeadPlayer(bool isLeader)
  {
    float a = this._currentBackgroundColor.a;
    this._currentBackgroundColor = isLeader ? this._leadingBackgroundColor : this._normalBackgroundColor;
    this._backgroundSpriteRenderer.color = this._currentBackgroundColor.ColorWithAlpha(a);
  }

  public virtual void HandleNewLeaderWasSelected(string userId) => this.AnimateIsLeadPlayer(this._connectedPlayer.userId == userId);

  [CompilerGenerated]
  public virtual void m_CStartm_Eb__14_0(float val)
  {
    this._onPlatformText.color = this._activeTextColor.ColorWithAlpha(this._activeTextColor.a * val);
    this._backgroundSpriteRenderer.color = this._currentBackgroundColor.ColorWithAlpha(this._currentBackgroundColor.a * val);
  }

  [CompilerGenerated]
  public virtual void m_CAnimateScoreDiffm_Eb__17_0() => this._state = MultiplayerScoreDiffText.State.Displayed;

  [CompilerGenerated]
  public virtual void m_CAnimateHidem_Eb__18_0()
  {
    this.gameObject.SetActive(false);
    this._state = MultiplayerScoreDiffText.State.Hidden;
  }

  public enum HorizontalPosition
  {
    Left,
    Right,
  }

  public enum State
  {
    Hidden,
    Displayed,
    AnimatingDisplay,
    AnimatingHide,
  }
}
