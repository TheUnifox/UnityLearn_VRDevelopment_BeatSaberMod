// Decompiled with JetBrains decompiler
// Type: MultiplayerTimelineMock
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MultiplayerTimelineMock : MonoBehaviour
{
  [Header("Connected Player References")]
  [SerializeField]
  protected GameObject[] _connectedPlayerIntroAnimators;
  [SerializeField]
  protected GameObject[] _connectedPlayerScoreRingItems;
  [SerializeField]
  protected GameObject[] _connectedPlayerOutroAnimators;
  [Header("Local Player References")]
  [SerializeField]
  protected GameObject _localPlayerIntroAnimator;
  [SerializeField]
  protected GameObject _localPlayerScoreRingItem;
  [SerializeField]
  protected GameObject _localPlayerOutroAnimator;
  [Header("Duel Player References")]
  [SerializeField]
  protected GameObject _connectedDuelIntroAnimator;
  [SerializeField]
  protected GameObject _connectedDuelOutroAnimator;
  [SerializeField]
  protected GameObject _localDuelIntroAnimator;
  [SerializeField]
  protected GameObject _localDuelOutroAnimator;
  [Header("Results References")]
  [SerializeField]
  protected GameObject[] _resultAvatars;
  [SerializeField]
  protected GameObject[] _badgeTimelines;
  [Header("Activation References")]
  [SerializeField]
  protected GameObject _resultsMocks;
  [SerializeField]
  protected GameObject _ringsGroup;
  [SerializeField]
  protected GameObject _playersGroup;
  [SerializeField]
  protected GameObject _duelGroup;

  public GameObject[] connectedPlayerIntroAnimators => this._connectedPlayerIntroAnimators;

  public GameObject[] connectedPlayerScoreRings => this._connectedPlayerScoreRingItems;

  public GameObject[] connectedPlayerOutroAnimators => this._connectedPlayerOutroAnimators;

  public GameObject localPlayerIntroAnimator => this._localPlayerIntroAnimator;

  public GameObject localPlayerScoreRingItem => this._localPlayerScoreRingItem;

  public GameObject localPlayerOutroAnimator => this._localPlayerOutroAnimator;

  public GameObject connectedDuelIntroAnimator => this._connectedDuelIntroAnimator;

  public GameObject connectedDuelOutroAnimator => this._connectedDuelOutroAnimator;

  public GameObject localDuelIntroAnimator => this._localDuelIntroAnimator;

  public GameObject localDuelOutroAnimator => this._localDuelOutroAnimator;

  public GameObject[] resultAvatars => this._resultAvatars;

  public GameObject[] badgeTimelines => this._badgeTimelines;

  public GameObject resultsMocks => this._resultsMocks;

  public virtual void HandleActivations(bool isDuel)
  {
    this._duelGroup.SetActive(isDuel);
    this._ringsGroup.SetActive(!isDuel);
    this._playersGroup.SetActive(!isDuel);
  }
}
