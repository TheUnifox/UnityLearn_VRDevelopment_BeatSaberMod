// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActiveClient
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLocalActiveClient : MonoBehaviour
{
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly IScoreSyncStateManager _scoreSyncStateManager;
  [Inject]
  protected readonly INodePoseSyncStateManager _nodePoseSyncStateManager;
  [Inject]
  protected readonly IGameplayRpcManager _rpcManager;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly IScoreController _scoreController;
  [Inject]
  protected readonly ComboController _comboController;

  public virtual void Start()
  {
    this._beatmapObjectManager.noteWasAddedEvent += new System.Action<NoteData, BeatmapObjectSpawnMovementData.NoteSpawnData, float>(this.HandleNoteWasAdded);
    this._beatmapObjectManager.obstacleWasAddedEvent += new System.Action<ObstacleData, BeatmapObjectSpawnMovementData.ObstacleSpawnData, float>(this.HandleObstacleWasAdded);
    this._beatmapObjectManager.sliderWasAddedEvent += new System.Action<SliderData, BeatmapObjectSpawnMovementData.SliderSpawnData, float>(this.HandleSliderWasAdded);
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent += new System.Action<NoteController>(this.HandleNoteWasMissed);
    this._scoreController.scoreDidChangeEvent += new System.Action<int, int>(this.HandleScoreDidChange);
    this._scoreController.multiplierDidChangeEvent += new System.Action<int, float>(this.HandleMultiplierDidChange);
    this._comboController.comboDidChangeEvent += new System.Action<int>(this.HandleComboDidChange);
    this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.ModifiedScore, 0);
    this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.MultipliedScore, 0);
    this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.ImmediateMaxPossibleMultipliedScore, this._scoreController.immediateMaxPossibleMultipliedScore);
    this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.Combo, 0);
    this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.Multiplier, 0);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager != null)
    {
      this._beatmapObjectManager.noteWasAddedEvent -= new System.Action<NoteData, BeatmapObjectSpawnMovementData.NoteSpawnData, float>(this.HandleNoteWasAdded);
      this._beatmapObjectManager.obstacleWasAddedEvent -= new System.Action<ObstacleData, BeatmapObjectSpawnMovementData.ObstacleSpawnData, float>(this.HandleObstacleWasAdded);
      this._beatmapObjectManager.sliderWasAddedEvent -= new System.Action<SliderData, BeatmapObjectSpawnMovementData.SliderSpawnData, float>(this.HandleSliderWasAdded);
      this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
      this._beatmapObjectManager.noteWasMissedEvent -= new System.Action<NoteController>(this.HandleNoteWasMissed);
    }
    if (this._scoreController != null)
    {
      this._scoreController.scoreDidChangeEvent -= new System.Action<int, int>(this.HandleScoreDidChange);
      this._scoreController.multiplierDidChangeEvent -= new System.Action<int, float>(this.HandleMultiplierDidChange);
    }
    if (!((UnityEngine.Object) this._comboController != (UnityEngine.Object) null))
      return;
    this._comboController.comboDidChangeEvent -= new System.Action<int>(this.HandleComboDidChange);
  }

  public virtual void LateUpdate()
  {
    this._nodePoseSyncStateManager.localState?.SetState(NodePoseSyncState.NodePose.Head, new PoseSerializable((Vector3Serializable) this._playerTransforms.headPseudoLocalPos, (QuaternionSerializable) this._playerTransforms.headPseudoLocalRot));
    this._nodePoseSyncStateManager.localState?.SetState(NodePoseSyncState.NodePose.LeftController, new PoseSerializable((Vector3Serializable) this._playerTransforms.leftHandPseudoLocalPos, (QuaternionSerializable) this._playerTransforms.leftHandPseudoLocalRot));
    this._nodePoseSyncStateManager.localState?.SetState(NodePoseSyncState.NodePose.RightController, new PoseSerializable((Vector3Serializable) this._playerTransforms.rightHandPseudoLocalPos, (QuaternionSerializable) this._playerTransforms.rightHandPseudoLocalRot));
  }

  public virtual void HandleNoteWasAdded(
    NoteData noteData,
    BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData,
    float rotation)
  {
    this._rpcManager.NoteSpawned(this._audioTimeSyncController.songTime, NoteSpawnInfoNetSerializable.Obtain().Init(noteData.time, noteData.lineIndex, noteData.noteLineLayer, noteData.beforeJumpNoteLineLayer, noteData.gameplayType, noteData.scoringType, noteData.colorType, noteData.cutDirection, noteData.timeToNextColorNote, noteData.timeToPrevColorNote, noteData.lineIndex, noteData.flipYSide, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.jumpGravity, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, rotation, noteData.cutDirectionAngleOffset, noteData.cutSfxVolumeMultiplier));
  }

  public virtual void HandleObstacleWasAdded(
    ObstacleData obstacleData,
    BeatmapObjectSpawnMovementData.ObstacleSpawnData obstacleSpawnData,
    float rotation)
  {
    this._rpcManager.ObstacleSpawned(this._audioTimeSyncController.songTime, ObstacleSpawnInfoNetSerializable.Obtain().Init(obstacleData.time, obstacleData.lineIndex, obstacleData.lineLayer, obstacleData.duration, obstacleData.width, obstacleData.height, obstacleSpawnData.moveStartPos, obstacleSpawnData.moveEndPos, obstacleSpawnData.jumpEndPos, obstacleSpawnData.obstacleHeight, obstacleSpawnData.moveDuration, obstacleSpawnData.jumpDuration, obstacleSpawnData.noteLinesDistance, rotation));
  }

  public virtual void HandleSliderWasAdded(
    SliderData sliderData,
    BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData,
    float rotation)
  {
    this._rpcManager.SliderSpawned(this._audioTimeSyncController.songTime, SliderSpawnInfoNetSerializable.Obtain().Init(sliderData.colorType, sliderData.sliderType, sliderData.hasHeadNote, sliderData.time, sliderData.headLineIndex, sliderData.headLineLayer, sliderData.headBeforeJumpLineLayer, sliderData.headControlPointLengthMultiplier, sliderData.headCutDirection, sliderData.headCutDirectionAngleOffset, sliderData.hasTailNote, sliderData.tailTime, sliderData.tailLineIndex, sliderData.tailLineLayer, sliderData.tailBeforeJumpLineLayer, sliderData.tailControlPointLengthMultiplier, sliderData.tailCutDirection, sliderData.tailCutDirectionAngleOffset, sliderData.midAnchorMode, sliderData.sliceCount, sliderData.squishAmount, sliderSpawnData.headMoveStartPos, sliderSpawnData.headJumpStartPos, sliderSpawnData.headJumpEndPos, sliderSpawnData.headJumpGravity, sliderSpawnData.tailMoveStartPos, sliderSpawnData.tailJumpStartPos, sliderSpawnData.tailJumpEndPos, sliderSpawnData.tailJumpGravity, sliderSpawnData.moveDuration, sliderSpawnData.jumpDuration, rotation));
  }

  public virtual void HandleNoteWasMissed(NoteController noteController) => this._rpcManager.NoteMissed(this._audioTimeSyncController.songTime, NoteMissInfoNetSerializable.Obtain().Init(noteController.noteData));

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo) => this._rpcManager.NoteCut(this._audioTimeSyncController.songTime, NoteCutInfoNetSerializable.Obtain().Init(ref noteCutInfo, noteController.noteData, noteController.noteTransform.position, noteController.noteTransform.rotation, noteController.noteTransform.localScale, noteController.moveVec));

  public virtual void HandleComboDidChange(int combo) => this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.Combo, combo);

  public virtual void HandleMultiplierDidChange(int multiplier, float multiplierProgress) => this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.Multiplier, multiplier);

  public virtual void HandleScoreDidChange(int multipliedScore, int modifiedScore)
  {
    this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.MultipliedScore, multipliedScore);
    this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.ModifiedScore, modifiedScore);
    this._scoreSyncStateManager.localState?.SetState(StandardScoreSyncState.Score.ImmediateMaxPossibleMultipliedScore, this._scoreController.immediateMaxPossibleMultipliedScore);
  }
}
