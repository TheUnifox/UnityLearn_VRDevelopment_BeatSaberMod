// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyCenterStageLayoutAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class MultiplayerLobbyCenterStageLayoutAnimator : MonoBehaviour
{
  [SerializeField]
  protected RectTransform _nextLevelTransform;
  [Space]
  [SerializeField]
  protected RectTransform _nextLevelBasePosition;
  [SerializeField]
  protected RectTransform _nextLevelCountdownPosition;
  [Space]
  [SerializeField]
  protected float _transitionDuration;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;

  public virtual void StartCountdown()
  {
    this._nextLevelTransform.gameObject.SetActive(true);
    this.Move((Vector3) this._nextLevelTransform.anchoredPosition, (Vector3) this._nextLevelCountdownPosition.anchoredPosition, this._transitionDuration);
  }

  public virtual void StopCountdown(bool instant)
  {
    if (instant)
      this._nextLevelTransform.anchoredPosition = this._nextLevelBasePosition.anchoredPosition;
    else
      this.Move((Vector3) this._nextLevelTransform.anchoredPosition, (Vector3) this._nextLevelBasePosition.anchoredPosition, this._transitionDuration);
  }

  public virtual void Move(Vector3 from, Vector3 to, float duration)
  {
    this._tweeningManager.KillAllTweens((object) this);
    this._tweeningManager.AddTween((Tween) new Vector3Tween(from, to, (System.Action<Vector3>) (pos => this._nextLevelTransform.anchoredPosition = (Vector2) pos), duration, EaseType.OutQuad), (object) this);
  }

  [CompilerGenerated]
  public virtual void m_CMovem_Eb__7_0(Vector3 pos) => this._nextLevelTransform.anchoredPosition = (Vector2) pos;
}
