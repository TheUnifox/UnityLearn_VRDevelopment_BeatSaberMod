// Decompiled with JetBrains decompiler
// Type: MultiplayerScoreRingItem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using TMPro;
using Tweening;
using UnityEngine;
using Zenject;

public class MultiplayerScoreRingItem : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _scoreText;
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  protected Tween<Color> _nameColorTween;
  protected Tween<Color> _scoreColorTween;

  public virtual void Awake()
  {
    EaseType easeType = EaseType.InQuad;
    this._nameColorTween = (Tween<Color>) new Tweening.ColorTween(Color.white, Color.white, (System.Action<Color>) (val => this._nameText.color = val), 0.0f, easeType);
    this._scoreColorTween = (Tween<Color>) new Tweening.ColorTween(Color.white, Color.white, (System.Action<Color>) (val => this._scoreText.color = val), 0.0f, easeType);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual void SetPositionAndRotation(Vector3 position, Quaternion rotation) => this.transform.SetPositionAndRotation(position, rotation);

  public virtual void AnimateColors(
    Color nameColor,
    Color scoreColor,
    float duration,
    EaseType easeType)
  {
    this._nameColorTween.easeType = this._scoreColorTween.easeType = easeType;
    this._nameColorTween.duration = this._scoreColorTween.duration = duration;
    this._nameColorTween.fromValue = this._nameText.color;
    this._nameColorTween.toValue = nameColor;
    this._scoreColorTween.fromValue = this._scoreText.color;
    this._scoreColorTween.toValue = scoreColor;
    this._tweeningManager.RestartTween((Tween) this._nameColorTween, (object) this);
    this._tweeningManager.RestartTween((Tween) this._scoreColorTween, (object) this);
  }

  public virtual void SetName(string text) => this._nameText.text = text;

  public virtual void SetScore(string text) => this._scoreText.text = text;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__6_0(Color val) => this._nameText.color = val;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__6_1(Color val) => this._scoreText.color = val;

  public class Pool : MonoMemoryPool<MultiplayerScoreRingItem>
  {
  }
}
