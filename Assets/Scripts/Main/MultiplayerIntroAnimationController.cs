// Decompiled with JetBrains decompiler
// Type: MultiplayerIntroAnimationController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

public class MultiplayerIntroAnimationController : MonoBehaviour
{
  [Header("Timeline References")]
  [SerializeField]
  protected PlayableDirector _introPlayableDirector;
  [SerializeField]
  protected string[] _playerTimelineTrackNames;
  [SerializeField]
  protected string[] _ringTimelineTrackNames;
  [HideInInspector]
  [SerializeField]
  protected PropertyName[] _playerTimelinePropertyNames;
  [Header("Local Player")]
  [SerializeField]
  protected string _localPlayerTrackName;
  [SerializeField]
  protected string _localPlayerRingTrackName;
  [HideInInspector]
  [SerializeField]
  protected PropertyName _localPlayerTimelinePropertyName;
  [Header("Timing")]
  [SerializeField]
  protected float _firstConnectedPlayerStart = 2f;
  [SerializeField]
  protected float _spawnDuration = 1f;
  [SerializeField]
  protected string _endMarkerName = "IntroEnd";
  [Space]
  [SerializeField]
  protected MultiplayerScoreRingManager _scoreRingManager;
  [Inject]
  protected readonly MultiplayerPlayersManager _multiplayerPlayersManager;
  [Inject]
  protected readonly IMultiplayerSessionManager _multiplayerSessionManager;
  [Inject]
  protected readonly MultiplayerLayoutProvider _layoutProvider;
  protected System.Action _onCompleted;
  protected float _introDuration;
  protected bool _bindingFinished;

  public virtual void OnValidate()
  {
    if (!(this._introPlayableDirector.playableAsset is TimelineAsset playableAsset))
      return;
    this._playerTimelinePropertyNames = new PropertyName[this._playerTimelineTrackNames.Length];
    foreach (TrackAsset outputTrack in playableAsset.GetOutputTracks())
    {
      if (outputTrack is ControlTrack controlTrack)
      {
        for (int index = 0; index < this._playerTimelineTrackNames.Length; ++index)
        {
          if (this._playerTimelineTrackNames[index] == controlTrack.name)
          {
            ControlPlayableAsset asset = controlTrack.GetClips().First<TimelineClip>().asset as ControlPlayableAsset;
            this._playerTimelinePropertyNames[index] = asset.sourceGameObject.exposedName;
            break;
          }
          if (this._localPlayerTrackName == controlTrack.name)
          {
            this._localPlayerTimelinePropertyName = (controlTrack.GetClips().First<TimelineClip>().asset as ControlPlayableAsset).sourceGameObject.exposedName;
            break;
          }
        }
      }
    }
  }

  public virtual void SetBeforeIntroValue()
  {
    if ((UnityEngine.Object) this._multiplayerPlayersManager.activeLocalPlayerFacade != (UnityEngine.Object) null)
      this._multiplayerPlayersManager.activeLocalPlayerFacade.introAnimator.SetBeforeIntroValues();
    foreach (IConnectedPlayer atGameStartPlayer in (IEnumerable<IConnectedPlayer>) this._multiplayerPlayersManager.allActiveAtGameStartPlayers)
    {
      MultiplayerConnectedPlayerFacade connectedPlayerController;
      if (!atGameStartPlayer.isMe && this._multiplayerPlayersManager.TryGetConnectedPlayerController(atGameStartPlayer.userId, out connectedPlayerController))
        connectedPlayerController.introAnimator.SetBeforeIntroValues();
    }
  }

  public virtual void PlayIntroAnimation(float maxDesiredIntroAnimationDuration, System.Action onCompleted)
  {
    this.BindTimeline();
    this._onCompleted = new System.Action(onCompleted.Invoke);
    this._introPlayableDirector.Play();
    float num = this.GetFullIntroAnimationTime() / maxDesiredIntroAnimationDuration;
    if ((double) num >= 1.0)
      return;
    this._introPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed<Playable>((double) num);
  }

  public virtual float GetFullIntroAnimationTime()
  {
    if ((double) this._introDuration > 0.0)
      return this._introDuration;
    if (!(this._introPlayableDirector.playableAsset is TimelineAsset playableAsset))
      return 0.0f;
    foreach (IMarker marker in playableAsset.markerTrack.GetMarkers())
    {
      if (marker is SignalEmitter signalEmitter && signalEmitter.name == this._endMarkerName)
        this._introDuration = (float) signalEmitter.time;
    }
    return this._introDuration;
  }

  public virtual void BindTimeline()
  {
    if (this._bindingFinished)
      return;
    IReadOnlyList<IConnectedPlayer> gameStartPlayers = this._multiplayerPlayersManager.allActiveAtGameStartPlayers;
    Queue<int> playerIndexSequence = this.CalculatePlayerIndexSequence(gameStartPlayers);
    int count = playerIndexSequence.Count;
    bool flag = this._layoutProvider.layout == MultiplayerPlayerLayout.Duel;
    float num = this._spawnDuration / (float) Mathf.Max(count - 1, 1);
    float[] numArray = new float[count + 1];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = this._firstConnectedPlayerStart + (float) index * num;
    GameObject[] connectedRings = new GameObject[count];
    int index1 = 0;
    while (playerIndexSequence.Count > 0)
    {
      int index2 = playerIndexSequence.Dequeue();
      string userId = gameStartPlayers[index2].userId;
      MultiplayerConnectedPlayerFacade connectedPlayerController;
      if (!this._multiplayerPlayersManager.TryGetConnectedPlayerController(userId, out connectedPlayerController))
      {
        Debug.LogWarning(this._multiplayerSessionManager.localPlayer.userId == userId ? (object) "Tried to animate local active player as connected player" : (object) ("Unable to find ConnectedPlayerController for userId \"" + userId + "\", skipping animation"));
      }
      else
      {
        GameObject gameObject = connectedPlayerController.introAnimator.gameObject;
        this._introPlayableDirector.SetReferenceValue(this._playerTimelinePropertyNames[index1], (UnityEngine.Object) gameObject);
        connectedRings[index1] = flag ? (GameObject) null : this._scoreRingManager.GetScoreRingItem(userId).gameObject;
        ++index1;
      }
    }
    GameObject gameObject1 = (flag ? 1 : (!this._multiplayerSessionManager.localPlayer.WasActiveAtLevelStart() ? 1 : 0)) != 0 ? (GameObject) null : this._scoreRingManager.GetScoreRingItem(this._multiplayerSessionManager.localPlayer.userId).gameObject;
    this.BindRingsAndSetTiming(count, connectedRings, gameObject1);
    if ((UnityEngine.Object) this._multiplayerPlayersManager.inactivePlayerFacade != (UnityEngine.Object) null)
      this._introPlayableDirector.SetReferenceValue(this._localPlayerTimelinePropertyName, (UnityEngine.Object) this._multiplayerPlayersManager.inactivePlayerFacade.introAnimator);
    else if ((UnityEngine.Object) this._multiplayerPlayersManager.activeLocalPlayerFacade != (UnityEngine.Object) null)
      this._introPlayableDirector.SetReferenceValue(this._localPlayerTimelinePropertyName, (UnityEngine.Object) this._multiplayerPlayersManager.activeLocalPlayerFacade.introAnimator.gameObject);
    else
      Debug.LogError((object) "Neither active or inactive facade exists, this should not happen");
    this._bindingFinished = true;
  }

  public virtual void BindRingsAndSetTiming(
    int connectedPlayersCount,
    GameObject[] connectedRings = null,
    GameObject localRing = null)
  {
    float[] numArray = new float[connectedPlayersCount + 1];
    float num = this._spawnDuration / (float) Mathf.Max(connectedPlayersCount - 1, 1);
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = this._firstConnectedPlayerStart + (float) index * num;
    TimelineAsset playableAsset = this._introPlayableDirector.playableAsset as TimelineAsset;
    if ((UnityEngine.Object) playableAsset == (UnityEngine.Object) null)
      return;
    foreach (TrackAsset outputTrack in playableAsset.GetOutputTracks())
    {
      if (outputTrack != null)
      {
        if (!(outputTrack is AnimationTrack key))
        {
          if (outputTrack is ControlTrack controlTrack)
          {
            if (this._localPlayerTrackName == controlTrack.name)
            {
              controlTrack.GetClips().First<TimelineClip>().start = (double) numArray[numArray.Length - 1];
            }
            else
            {
              for (int index = 0; index < connectedPlayersCount; ++index)
              {
                if (this._playerTimelineTrackNames[index] == controlTrack.name)
                {
                  controlTrack.GetClips().First<TimelineClip>().start = (double) numArray[index];
                  break;
                }
              }
            }
          }
        }
        else if (connectedPlayersCount != 1 && connectedRings != null)
        {
          if (this._localPlayerRingTrackName == key.name)
          {
            this._introPlayableDirector.SetGenericBinding((UnityEngine.Object) key, (UnityEngine.Object) localRing);
            key.GetClips().First<TimelineClip>().start = (double) numArray[numArray.Length - 1];
          }
          else
          {
            for (int index = 0; index < connectedPlayersCount; ++index)
            {
              if (this._ringTimelineTrackNames[index] == key.name)
              {
                this._introPlayableDirector.SetGenericBinding((UnityEngine.Object) key, (UnityEngine.Object) connectedRings[index]);
                key.GetClips().First<TimelineClip>().start = (double) numArray[index];
                break;
              }
            }
          }
        }
      }
    }
  }

  public virtual void TransitionToAfterIntroAnimationState()
  {
    this._introPlayableDirector.time = this._introPlayableDirector.duration;
    this.BindTimeline();
    this._introPlayableDirector.Evaluate();
    if (!((UnityEngine.Object) this._multiplayerPlayersManager.activeLocalPlayerFacade != (UnityEngine.Object) null))
      return;
    this._multiplayerPlayersManager.activeLocalPlayerFacade.introAnimator.SetAfterIntroValues();
  }

  public virtual Queue<int> CalculatePlayerIndexSequence(
    IReadOnlyList<IConnectedPlayer> allActivePlayer)
  {
    int count = allActivePlayer.Count;
    if (count == 0)
    {
      Debug.LogWarning((object) "0 active players, this should only happen in some edge cases");
      return new Queue<int>();
    }
    int num1 = allActivePlayer.IndexOf<IConnectedPlayer>(this._multiplayerSessionManager.localPlayer);
    int num2 = 1;
    int num3;
    if (num1 == -1)
    {
      num1 = allActivePlayer.Count - 1;
      num3 = (num1 + count) % count;
      num2 = 0;
    }
    else
      num3 = (num1 - 1 + count) % count;
    int num4 = (num1 + 1) % count;
    List<int> list1 = new List<int>();
    List<int> list2 = new List<int>();
    bool flag = true;
    for (; num2 < allActivePlayer.Count; ++num2)
    {
      if (flag)
      {
        list1.Add(num3);
        num3 = (num3 - 1 + count) % count;
      }
      else
      {
        list2.Add(num4);
        num4 = (num4 + 1) % count;
      }
      flag = !flag;
    }
    list1.ShuffleInPlace<int>();
    list2.ShuffleInPlace<int>();
    int index1 = 0;
    int index2 = 0;
    Queue<int> playerIndexSequence = new Queue<int>();
    while (index1 < list1.Count || index2 < list2.Count)
    {
      if (index1 < list1.Count)
      {
        playerIndexSequence.Enqueue(list1[index1]);
        ++index1;
      }
      if (index2 < list2.Count)
      {
        playerIndexSequence.Enqueue(list2[index2]);
        ++index2;
      }
    }
    return playerIndexSequence;
  }

  public virtual void SetTimelineMock(
    MultiplayerTimelineMock multiplayerIntroTimelineMock,
    bool isDuel = false)
  {
    if (isDuel)
    {
      this.BindRingsAndSetTiming(1);
      this._introPlayableDirector.SetReferenceValue(this._localPlayerTimelinePropertyName, (UnityEngine.Object) multiplayerIntroTimelineMock.localDuelIntroAnimator);
      this._introPlayableDirector.SetReferenceValue(this._playerTimelinePropertyNames[0], (UnityEngine.Object) multiplayerIntroTimelineMock.connectedDuelIntroAnimator);
    }
    else
    {
      this.BindRingsAndSetTiming(4, multiplayerIntroTimelineMock.connectedPlayerScoreRings, multiplayerIntroTimelineMock.localPlayerScoreRingItem);
      this._introPlayableDirector.SetReferenceValue(this._localPlayerTimelinePropertyName, (UnityEngine.Object) multiplayerIntroTimelineMock.localPlayerIntroAnimator);
      GameObject[] playerIntroAnimators = multiplayerIntroTimelineMock.connectedPlayerIntroAnimators;
      for (int index = 0; index < playerIntroAnimators.Length; ++index)
        this._introPlayableDirector.SetReferenceValue(this._playerTimelinePropertyNames[index], (UnityEngine.Object) playerIntroAnimators[index]);
    }
  }

  public virtual void Completed()
  {
    this._onCompleted();
    this._introPlayableDirector.Pause();
  }
}
