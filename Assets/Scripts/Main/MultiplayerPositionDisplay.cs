// Decompiled with JetBrains decompiler
// Type: MultiplayerPositionDisplay
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using Zenject;

public class MultiplayerPositionDisplay : MonoBehaviour
{
  [SerializeField]
  protected TextMeshPro _text;
  [SerializeField]
  protected Color _normalColor;
  [SerializeField]
  protected Color _leadingColor;
  [SerializeField]
  protected Color _failedColor;
  [SerializeField]
  protected float _fadeInDuration = 0.3f;
  [SerializeField]
  protected float _crossFadeDuration = 0.3f;
  [SerializeField]
  protected float _fadeOutDuration = 0.7f;
  [Inject]
  protected readonly MultiplayerScoreProvider _scoreProvider;
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;
  protected bool _wasFirst;
  protected bool _isFailed;
  protected bool _updatingColor;
  protected Color _startColor;
  protected Color _targetColor;
  protected float _colorAnimationStartTime;
  protected float _colorAnimationStartDuration;

  public virtual void Start()
  {
    this._text.color = this._normalColor;
    this._text.text = "1";
    if (this._scoreProvider.scoresAvailable)
      return;
    this.enabled = false;
    this._scoreProvider.firstPlayerDidChangeEvent += new System.Action<MultiplayerScoreProvider.RankedPlayer>(this.HandleFirstPlayerDidChange);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._scoreProvider != (UnityEngine.Object) null)
      this._scoreProvider.firstPlayerDidChangeEvent -= new System.Action<MultiplayerScoreProvider.RankedPlayer>(this.HandleFirstPlayerDidChange);
    else
      this.ChangeColor(this._normalColor, this._fadeInDuration);
  }

  public virtual void Update()
  {
    if (!this._scoreProvider.scoresAvailable)
      return;
    this.UpdateColors();
    if (this._isFailed)
      return;
    this.UpdatePosition();
  }

  public virtual void UpdateColors()
  {
    if (!this._updatingColor)
      return;
    float t = (Time.time - this._colorAnimationStartTime) / this._colorAnimationStartDuration;
    if ((double) t < 0.0 || (double) t > 1.0)
      this._updatingColor = false;
    this._text.color = Color.Lerp(this._startColor, this._targetColor, t);
  }

  public virtual void UpdatePosition()
  {
    if (!this._connectedPlayer.IsActiveOrFinished())
    {
      this.HandlePlayerFailed();
    }
    else
    {
      int positionOfPlayer = this._scoreProvider.GetPositionOfPlayer(this._connectedPlayer.userId);
      if (positionOfPlayer > 0)
      {
        if (positionOfPlayer == 1 && !this._wasFirst)
        {
          this._wasFirst = true;
          this.ChangeColor(this._leadingColor, this._crossFadeDuration);
        }
        else if (positionOfPlayer != 1 && this._wasFirst)
        {
          this._wasFirst = false;
          this.ChangeColor(this._normalColor, this._crossFadeDuration);
        }
        this._text.text = positionOfPlayer.ToString();
      }
      else
        this.HandlePlayerFailed();
    }
  }

  public virtual void HandlePlayerFailed()
  {
    this._isFailed = true;
    this.ChangeColor(this._failedColor, this._fadeOutDuration);
  }

  public virtual void HandleFirstPlayerDidChange(MultiplayerScoreProvider.RankedPlayer obj)
  {
    this.enabled = true;
    this._scoreProvider.firstPlayerDidChangeEvent -= new System.Action<MultiplayerScoreProvider.RankedPlayer>(this.HandleFirstPlayerDidChange);
    this.ChangeColor(this._normalColor, this._fadeInDuration);
  }

  public virtual void ChangeColor(Color toColor, float duration)
  {
    this._updatingColor = true;
    this._colorAnimationStartTime = Time.time;
    this._colorAnimationStartDuration = duration;
    this._startColor = this._text.color;
    this._targetColor = toColor;
  }
}
