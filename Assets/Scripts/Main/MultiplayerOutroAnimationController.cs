// Decompiled with JetBrains decompiler
// Type: MultiplayerOutroAnimationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

public class MultiplayerOutroAnimationController : MonoBehaviour
{
  [Header("Timeline References")]
  [SerializeField]
  protected PlayableDirector _outroPlayableDirector;
  [SerializeField]
  protected string[] _playerTimelineTrackNames;
  [SerializeField]
  protected string[] _ringTimelineTrackNames;
  [SerializeField]
  protected string[] _resultsTimelineTrackNames;
  [HideInInspector]
  [SerializeField]
  protected PropertyName[] _playerTimelinePropertyNames;
  [HideInInspector]
  [SerializeField]
  protected PropertyName[] _resultsTimelinePropertyNames;
  [Header("Local Player")]
  [SerializeField]
  protected string _localPlayerTrackName;
  [HideInInspector]
  [SerializeField]
  protected PropertyName _localPlayerTimelinePropertyName;
  [Header("Badges")]
  [SerializeField]
  protected string[] _badgeTimelineTrackNames;
  [HideInInspector]
  [SerializeField]
  protected PropertyName[] _badgeTimelinePropertyNames;
  [SerializeField]
  protected Transform _badgeStartTransform;
  [SerializeField]
  protected Transform _badgeMidTransform;
  [Header("Others")]
  [SerializeField]
  protected string _songPreviewTrackName;
  [SerializeField]
  protected string _resultsMocksActivationTrack;
  [Space]
  [SerializeField]
  protected MultiplayerScoreRingManager _multiplayerScoreRingManager;
  [SerializeField]
  protected MultiplayerResultsPyramidView _multiplayerResultsPyramidView;
  [Inject]
  protected readonly MultiplayerPlayersManager _multiplayerPlayersManager;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly GameplayCoreSceneSetupData _sceneSetupData;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;
  protected System.Action _onCompleted;

  public virtual void Start()
  {
    if (this._multiplayerPlayersManager.playerSpawningFinished)
      this.HandlePlayerSpawningDidFinish();
    else
      this._multiplayerPlayersManager.playerSpawningDidFinishEvent += new System.Action(this.HandlePlayerSpawningDidFinish);
  }

  public virtual void OnValidate()
  {
    if (!(this._outroPlayableDirector.playableAsset is TimelineAsset playableAsset))
      return;
    this._playerTimelinePropertyNames = new PropertyName[this._playerTimelineTrackNames.Length];
    this._resultsTimelinePropertyNames = new PropertyName[this._resultsTimelineTrackNames.Length];
    this._badgeTimelinePropertyNames = new PropertyName[this._badgeTimelineTrackNames.Length];
    foreach (TrackAsset outputTrack in playableAsset.GetOutputTracks())
    {
      if (outputTrack is CustomControlTrack customControlTrack)
      {
        bool flag = false;
        for (int index = 0; index < this._playerTimelineTrackNames.Length; ++index)
        {
          if (this._playerTimelineTrackNames[index] == customControlTrack.name)
          {
            ControlPlayableAsset asset = customControlTrack.GetClips().First<TimelineClip>().asset as ControlPlayableAsset;
            this._playerTimelinePropertyNames[index] = asset.sourceGameObject.exposedName;
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          for (int index = 0; index < this._resultsTimelineTrackNames.Length; ++index)
          {
            if (this._resultsTimelineTrackNames[index] == customControlTrack.name)
            {
              ControlPlayableAsset asset = customControlTrack.GetClips().First<TimelineClip>().asset as ControlPlayableAsset;
              this._resultsTimelinePropertyNames[index] = asset.sourceGameObject.exposedName;
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            for (int index = 0; index < this._badgeTimelineTrackNames.Length; ++index)
            {
              if (this._badgeTimelineTrackNames[index] == customControlTrack.name)
              {
                ControlPlayableAsset asset = customControlTrack.GetClips().First<TimelineClip>().asset as ControlPlayableAsset;
                this._badgeTimelinePropertyNames[index] = asset.sourceGameObject.exposedName;
                break;
              }
            }
            if (this._localPlayerTrackName == customControlTrack.name)
              this._localPlayerTimelinePropertyName = (customControlTrack.GetClips().First<TimelineClip>().asset as ControlPlayableAsset).sourceGameObject.exposedName;
          }
        }
      }
    }
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._multiplayerPlayersManager != (UnityEngine.Object) null))
      return;
    this._multiplayerPlayersManager.playerSpawningDidFinishEvent -= new System.Action(this.HandlePlayerSpawningDidFinish);
  }

  public virtual void AnimateOutro(
    MultiplayerResultsData multiplayerResultsData,
    System.Action onCompleted)
  {
    this.BindOutroTimeline();
    this.BindRingsAndAudio(this._multiplayerScoreRingManager.GetScoreRingItems(), false, this._layoutProvider.layout == MultiplayerPlayerLayout.Duel, (MultiplayerTimelineMock) null);
    this._multiplayerResultsPyramidView.SetupResults(multiplayerResultsData.allPlayersSortedData, this._badgeStartTransform, this._badgeMidTransform);
    for (int index = 0; index < this._multiplayerResultsPyramidView.resultAvatarDirectors.Length; ++index)
      this._outroPlayableDirector.SetReferenceValue(this._resultsTimelinePropertyNames[index], (UnityEngine.Object) this._multiplayerResultsPyramidView.resultAvatarDirectors[index]);
    for (int index = 0; index < this._multiplayerResultsPyramidView.badgeTimelines.Length; ++index)
      this._outroPlayableDirector.SetReferenceValue(this._badgeTimelinePropertyNames[index], (UnityEngine.Object) this._multiplayerResultsPyramidView.badgeTimelines[index]);
    this._onCompleted = new System.Action(onCompleted.Invoke);
    this._outroPlayableDirector.Play();
  }

  public virtual void BindOutroTimeline()
  {
    IReadOnlyList<IConnectedPlayer> gameStartPlayers = this._multiplayerPlayersManager.allActiveAtGameStartPlayers;
    if ((UnityEngine.Object) this._multiplayerPlayersManager.inactivePlayerFacade != (UnityEngine.Object) null)
      this._outroPlayableDirector.SetReferenceValue(this._localPlayerTimelinePropertyName, (UnityEngine.Object) this._multiplayerPlayersManager.inactivePlayerFacade.outroAnimator.gameObject);
    else if ((UnityEngine.Object) this._multiplayerPlayersManager.activeLocalPlayerFacade != (UnityEngine.Object) null)
      this._outroPlayableDirector.SetReferenceValue(this._localPlayerTimelinePropertyName, (UnityEngine.Object) this._multiplayerPlayersManager.activeLocalPlayerFacade.outroAnimator);
    else
      Debug.LogError((object) "Neither active nor inactive player facade exists, this should not happen");
    int index = 0;
    foreach (IConnectedPlayer connectedPlayer in (IEnumerable<IConnectedPlayer>) gameStartPlayers)
    {
      string userId = connectedPlayer.userId;
      MultiplayerConnectedPlayerFacade connectedPlayerController;
      if (!this._multiplayerPlayersManager.TryGetConnectedPlayerController(userId, out connectedPlayerController))
      {
        if (this._multiplayerSessionManager.localPlayer.userId != userId)
          Debug.LogWarning((object) ("Unable to find ConnectedPlayerController for userId \"" + userId + "\", skipping outro animation for this player"));
      }
      else
      {
        this._outroPlayableDirector.SetReferenceValue(this._playerTimelinePropertyNames[index], (UnityEngine.Object) connectedPlayerController.outroAnimator);
        connectedPlayerController.HideBigAvatar();
        ++index;
      }
    }
  }

  public virtual void BindRingsAndAudio(
    GameObject[] rings,
    bool isMock,
    bool isDuel,
    MultiplayerTimelineMock timelineMock)
  {
    TimelineAsset playableAsset = this._outroPlayableDirector.playableAsset as TimelineAsset;
    if ((UnityEngine.Object) playableAsset == (UnityEngine.Object) null)
      return;
    foreach (TrackAsset outputTrack in playableAsset.GetOutputTracks())
    {
      switch (outputTrack)
      {
        case AnimationTrack key1:
          if (!isDuel)
          {
            for (int index = 0; index < rings.Length; ++index)
            {
              if (this._ringTimelineTrackNames[index] == key1.name)
                this._outroPlayableDirector.SetGenericBinding((UnityEngine.Object) key1, (UnityEngine.Object) rings[index]);
            }
            continue;
          }
          continue;
        case AudioTrack audioTrack:
          if (!isMock && this._songPreviewTrackName == audioTrack.name)
          {
            TimelineClip timelineClip = audioTrack.GetClips().First<TimelineClip>();
            AudioPlayableAsset asset = timelineClip.asset as AudioPlayableAsset;
            IBeatmapLevel level = this._sceneSetupData.difficultyBeatmap?.level;
            if (!((UnityEngine.Object) asset == (UnityEngine.Object) null) && level?.beatmapLevelData != null)
            {
              asset.clip = level.beatmapLevelData.audioClip;
              timelineClip.clipIn = (double) level.previewStartTime;
              continue;
            }
            continue;
          }
          continue;
        case ActivationTrack key2:
          if (isMock && this._resultsMocksActivationTrack == key2.name && (UnityEngine.Object) timelineMock != (UnityEngine.Object) null)
          {
            this._outroPlayableDirector.SetGenericBinding((UnityEngine.Object) key2, (UnityEngine.Object) timelineMock.resultsMocks);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  public virtual void SetTimelineMock(
    MultiplayerTimelineMock multiplayerIntroTimelineMock,
    bool isDuel = false)
  {
    if (isDuel)
    {
      this._outroPlayableDirector.SetReferenceValue(this._localPlayerTimelinePropertyName, (UnityEngine.Object) multiplayerIntroTimelineMock.localDuelOutroAnimator);
      this._outroPlayableDirector.SetReferenceValue(this._playerTimelinePropertyNames[0], (UnityEngine.Object) multiplayerIntroTimelineMock.connectedDuelOutroAnimator);
      this.BindRingsAndAudio((GameObject[]) null, true, true, multiplayerIntroTimelineMock);
    }
    else
    {
      this.BindRingsAndAudio(((IEnumerable<GameObject>) multiplayerIntroTimelineMock.connectedPlayerScoreRings).Append<GameObject>(multiplayerIntroTimelineMock.localPlayerScoreRingItem).ToArray<GameObject>(), true, false, multiplayerIntroTimelineMock);
      this._outroPlayableDirector.SetReferenceValue(this._localPlayerTimelinePropertyName, (UnityEngine.Object) multiplayerIntroTimelineMock.localPlayerOutroAnimator);
      GameObject[] playerOutroAnimators = multiplayerIntroTimelineMock.connectedPlayerOutroAnimators;
      for (int index = 0; index < playerOutroAnimators.Length; ++index)
        this._outroPlayableDirector.SetReferenceValue(this._playerTimelinePropertyNames[index], (UnityEngine.Object) playerOutroAnimators[index]);
    }
    GameObject[] resultAvatars = multiplayerIntroTimelineMock.resultAvatars;
    for (int index = 0; index < resultAvatars.Length; ++index)
    {
      this._outroPlayableDirector.SetReferenceValue(this._resultsTimelinePropertyNames[index], (UnityEngine.Object) resultAvatars[index]);
      if (index == 0)
        resultAvatars[index].GetComponent<MultiplayerResultsPyramidViewAvatar>().SetupBadgeTimeline(this._badgeStartTransform, this._badgeMidTransform);
    }
    GameObject[] badgeTimelines = multiplayerIntroTimelineMock.badgeTimelines;
    for (int index = 0; index < badgeTimelines.Length; ++index)
      this._outroPlayableDirector.SetReferenceValue(this._badgeTimelinePropertyNames[index], (UnityEngine.Object) badgeTimelines[index]);
  }

  public virtual void Completed() => this._onCompleted();

  public virtual void HandlePlayerSpawningDidFinish() => this._multiplayerResultsPyramidView.PrespawnAvatars(this._multiplayerPlayersManager.allActiveAtGameStartPlayers);
}
