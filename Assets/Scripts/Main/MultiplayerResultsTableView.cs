// Decompiled with JetBrains decompiler
// Type: MultiplayerResultsTableView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class MultiplayerResultsTableView : MonoBehaviour, TableView.IDataSource
{
  protected const string kCellIdentifier = "Cell";
  [SerializeField]
  protected TableView _tableView;
  [SerializeField]
  protected RectTransform _tableViewRectTransform;
  [SerializeField]
  protected MultiplayerResultsTableCell _winnerTableCell;
  [SerializeField]
  protected MultiplayerResultsTableCell _cellPrefab;
  [Space]
  [SerializeField]
  protected float _startRowXPosition = -500f;
  [SerializeField]
  protected float _rowHeight = 6.2f;
  [SerializeField]
  protected float _rowXOffset = 1.8f;
  [SerializeField]
  protected float _animationDuration = 0.3f;
  [SerializeField]
  protected float _animationSeparationTime;
  [SerializeField]
  protected float _winnerAnimationDuration = 1.5f;
  [SerializeField]
  protected float _duelTablePosXOffset;
  [SerializeField]
  protected MultiplayerOffsetPositionByLocalPlayerPosition _multiplayerOffsetByLocalPlayerPosition;
  [Header("Audio")]
  [SerializeField]
  protected AudioSource _outroSfxAudioSource;
  [SerializeField]
  protected AudioClip[] _rowSlideAudioClips;
  [SerializeField]
  protected AudioClip _avatarSlideAudioClip;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;
  [Inject]
  protected readonly DiContainer _container;
  protected IReadOnlyList<MultiplayerPlayerResultsData> _dataList;
  protected Vector3 _positionOffset;
  protected Quaternion _rotationOffset;
  protected Vector3 _lastParentPosition;
  protected Quaternion _lastParentRotation;

  public virtual float CellSize() => this._rowHeight;

  public virtual int NumberOfCells() => this._layoutProvider.layout != MultiplayerPlayerLayout.Duel ? this._dataList.Count<MultiplayerPlayerResultsData>() - 1 : this._dataList.Count<MultiplayerPlayerResultsData>();

  public virtual void Awake()
  {
    Transform transform = this.transform;
    this._positionOffset = transform.position;
    this._rotationOffset = transform.rotation;
    this.enabled = false;
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  public virtual TableCell CellForIdx(TableView tableView, int idx)
  {
    MultiplayerResultsTableCell resultsTableCell = tableView.DequeueReusableCellForIdentifier("Cell") as MultiplayerResultsTableCell;
    if ((UnityEngine.Object) resultsTableCell == (UnityEngine.Object) null)
    {
      resultsTableCell = this._container.InstantiatePrefab((UnityEngine.Object) this._cellPrefab).GetComponent<MultiplayerResultsTableCell>();
      resultsTableCell.reuseIdentifier = "Cell";
    }
    if (this._layoutProvider.layout != MultiplayerPlayerLayout.Duel)
    {
      MultiplayerPlayerResultsData data = this._dataList[idx + 1];
      IConnectedPlayer connectedPlayer = data.connectedPlayer;
      resultsTableCell.SetData(data.connectedPlayer, idx + 2, connectedPlayer.userName, data.multiplayerLevelCompletionResults.levelCompletionResults, connectedPlayer.isMe, this._dataList.Count<MultiplayerPlayerResultsData>());
    }
    else
    {
      MultiplayerPlayerResultsData data = this._dataList[idx];
      IConnectedPlayer connectedPlayer = data.connectedPlayer;
      resultsTableCell.SetData(data.connectedPlayer, idx + 1, connectedPlayer.userName, data.multiplayerLevelCompletionResults.levelCompletionResults, connectedPlayer.isMe, this._dataList.Count<MultiplayerPlayerResultsData>());
    }
    return (TableCell) resultsTableCell;
  }

  public virtual void SetData(
    IReadOnlyList<MultiplayerPlayerResultsData> dataList)
  {
    this._dataList = dataList;
    this._tableView.SetDataSource((TableView.IDataSource) this, true);
    if (dataList.Count > 0 && this._layoutProvider.layout != MultiplayerPlayerLayout.Duel)
    {
      MultiplayerPlayerResultsData data = dataList[0];
      this._winnerTableCell.SetData(data.connectedPlayer, 1, data.connectedPlayer.userName, data.multiplayerLevelCompletionResults.levelCompletionResults, data.connectedPlayer.isMe, 1);
    }
    else
      this._winnerTableCell.gameObject.SetActive(false);
  }

  [ContextMenu("Animate Cells")]
  public virtual float StartAnimation()
  {
    this._tweeningManager.KillAllTweens((object) this);
    if (this._layoutProvider.layout == MultiplayerPlayerLayout.Duel)
    {
            Vector2 anchoredPosition1 = this._tableViewRectTransform.anchoredPosition;
            anchoredPosition1.x = this._duelTablePosXOffset;
            this._tableViewRectTransform.anchoredPosition = anchoredPosition1;
            this._tableViewRectTransform.localRotation = Quaternion.identity;
        }
    this._multiplayerOffsetByLocalPlayerPosition.SetEnabled(true);
    foreach (TableCell visibleCell in this._tableView.visibleCells)
    {
      MultiplayerResultsTableCell cell = (MultiplayerResultsTableCell) visibleCell;
      int num = this._tableView.visibleCells.Count<TableCell>() - 1 - cell.idx;
      RectTransform rectTransform = (RectTransform) cell.transform;
      float y = rectTransform.anchoredPosition.y;
      Vector2Tween vector2Tween = new Vector2Tween(new Vector2(this._startRowXPosition, y), new Vector2((float) num * this._rowXOffset, y), (System.Action<Vector2>) (val => rectTransform.anchoredPosition = val), this._animationDuration, EaseType.OutQuint, (float) num * this._animationSeparationTime);
      vector2Tween.onStart = (System.Action) (() => this.StartCoroutine(this.PlayRandomRowSlideInSound(0.12f)));
      this._tweeningManager.RestartTween((Tween) vector2Tween, (object) this);
      this._tweeningManager.RestartTween((Tween) new FloatTween(0.0f, 1f, (System.Action<float>) (val => cell.alpha = val), this._animationDuration, EaseType.OutQuint, (float) num * this._animationSeparationTime), (object) this);
    }
    if (this._layoutProvider.layout == MultiplayerPlayerLayout.Duel)
      return (float) (this._tableView.visibleCells.Count<TableCell>() - 1) * this._animationSeparationTime + this._animationDuration;
    RectTransform winnerRectTransform = (RectTransform) this._winnerTableCell.transform;
    Vector2 anchoredPosition = winnerRectTransform.anchoredPosition;
    float delay = (float) this._tableView.visibleCells.Count<TableCell>() * this._animationSeparationTime;
    Vector2Tween vector2Tween1 = new Vector2Tween(anchoredPosition + new Vector2(-this._startRowXPosition, 0.0f), anchoredPosition, (System.Action<Vector2>) (val => winnerRectTransform.anchoredPosition = val), this._winnerAnimationDuration, EaseType.OutQuint, delay);
    vector2Tween1.onStart = (System.Action) (() => this.StartCoroutine(this.PlayAvatarSlideInSound(0.3f)));
    this._tweeningManager.RestartTween((Tween) vector2Tween1, (object) this);
    this._tweeningManager.RestartTween((Tween) new FloatTween(0.0f, 1f, (System.Action<float>) (val => this._winnerTableCell.alpha = val), this._winnerAnimationDuration, EaseType.OutQuint, delay), (object) this);
    return delay + this._winnerAnimationDuration;
  }

  public virtual IEnumerator PlayRandomRowSlideInSound(float delay)
  {
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    this._outroSfxAudioSource.PlayOneShot(this._rowSlideAudioClips[UnityEngine.Random.Range(0, this._rowSlideAudioClips.Length)]);
  }

  public virtual IEnumerator PlayAvatarSlideInSound(float delay)
  {
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    this._outroSfxAudioSource.PlayOneShot(this._avatarSlideAudioClip);
  }

  [CompilerGenerated]
  public virtual void m_CStartAnimationm_Eb__30_1() => this.StartCoroutine(this.PlayRandomRowSlideInSound(0.12f));

  [CompilerGenerated]
  public virtual void m_CStartAnimationm_Eb__30_4() => this.StartCoroutine(this.PlayAvatarSlideInSound(0.3f));

  [CompilerGenerated]
  public virtual void m_CStartAnimationm_Eb__30_5(float val) => this._winnerTableCell.alpha = val;
}
