// Decompiled with JetBrains decompiler
// Type: TutorialSongController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class TutorialSongController : SongController
{
  [SerializeField]
  protected AudioTimeSyncController _audioTimeSyncController;
  [SerializeField]
  protected int _startWaitTimeInBeats = 8;
  [SerializeField]
  protected int _numberOfBeatsToSnap = 8;
  [SerializeField]
  protected int _obstacleDurationInBeats = 2;
  [Space]
  [FormerlySerializedAs("noteCuttingTutorialPartDidStartEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _noteCuttingTutorialPartDidStartSignal;
  [FormerlySerializedAs("noteCuttingInAnyDirectionDidStartEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _noteCuttingInAnyDirectionDidStartSignal;
  [FormerlySerializedAs("bombCuttingTutorialPartDidStartEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _bombCuttingTutorialPartDidStartSignal;
  [FormerlySerializedAs("leftObstacleTutorialPartDidStartEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _leftObstacleTutorialPartDidStartSignal;
  [FormerlySerializedAs("rightObstacleTutorialPartDidStartEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _rightObstacleTutorialPartDidStartSignal;
  [FormerlySerializedAs("topObstacleTutorialPartDidStartEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _topObstacleTutorialPartDidStartSignal;
  [FormerlySerializedAs("noteWasCutOKEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _noteWasCutOKSignal;
  [FormerlySerializedAs("noteWasCutTooSoonEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _noteWasCutTooSoonSignal;
  [FormerlySerializedAs("noteWasCutWithWrongColorEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _noteWasCutWithWrongColorSignal;
  [FormerlySerializedAs("noteWasCutFromDifferentDirectionEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _noteWasCutFromDifferentDirectionSignal;
  [FormerlySerializedAs("noteWasCutWithSlowSpeedEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _noteWasCutWithSlowSpeedSignal;
  [FormerlySerializedAs("bombWasCutEvent")]
  [SerializeField]
  [SignalSender]
  protected Signal _bombWasCutSignal;
  [Inject]
  protected readonly TutorialSongController.InitData _initData;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  protected int _tutorialBeatmapObjectIndex;
  protected int _prevSpawnedBeatmapObjectIndex = -1;
  protected float _songBpm;
  protected BeatmapData _beatmapData;
  protected TutorialSongController.TutorialObjectSpawnData[] _normalModeTutorialObjectsSpawnData;

  public virtual void Awake() => this._normalModeTutorialObjectsSpawnData = new TutorialSongController.TutorialObjectSpawnData[13]
  {
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBasicNoteSpawnData(this._noteCuttingTutorialPartDidStartSignal, 9, 9, 2, NoteLineLayer.Base, NoteCutDirection.Down, ColorType.ColorB),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBasicNoteSpawnData((Signal) null, 9, 9, 1, NoteLineLayer.Base, NoteCutDirection.Down, ColorType.ColorA),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBasicNoteSpawnData((Signal) null, 9, 9, 2, NoteLineLayer.Base, NoteCutDirection.Right, ColorType.ColorB),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBasicNoteSpawnData((Signal) null, 9, 9, 2, NoteLineLayer.Upper, NoteCutDirection.Right, ColorType.ColorA),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBasicNoteSpawnData((Signal) null, 9, 9, 1, NoteLineLayer.Upper, NoteCutDirection.Left, ColorType.ColorB),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBasicNoteSpawnData((Signal) null, 9, 9, 1, NoteLineLayer.Base, NoteCutDirection.Up, ColorType.ColorA),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBasicNoteSpawnData(this._noteCuttingInAnyDirectionDidStartSignal, 9, 9, 2, NoteLineLayer.Top, NoteCutDirection.Any, ColorType.ColorB),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBasicNoteSpawnData((Signal) null, 9, 9, 1, NoteLineLayer.Top, NoteCutDirection.Any, ColorType.ColorA),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBombNoteSpawnData(this._bombCuttingTutorialPartDidStartSignal, 17, 9, 2, NoteLineLayer.Base),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialBombNoteSpawnData((Signal) null, 9, 9, 1, NoteLineLayer.Base),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialObstacleSpawnData(this._rightObstacleTutorialPartDidStartSignal, 9, 9, 2, 1, 3, NoteLineLayer.Base),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialObstacleSpawnData(this._leftObstacleTutorialPartDidStartSignal, 9, 9, 1, 1, 3, NoteLineLayer.Base),
    (TutorialSongController.TutorialObjectSpawnData) new TutorialSongController.TutorialObstacleSpawnData(this._topObstacleTutorialPartDidStartSignal, 9, 9, 0, 4, 1, NoteLineLayer.Top)
  };

  public virtual void Start()
  {
    this._songBpm = this._initData.songBpm;
    this._beatmapData = this._initData.beatmapData;
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent += new System.Action<NoteController>(this.HandleNoteWasMissed);
    this._beatmapObjectManager.obstacleDidPassThreeQuartersOfMove2Event += new System.Action<ObstacleController>(this.HandleObstacleDidPassThreeQuartersOfMove2);
  }

  public virtual void OnDestroy()
  {
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent -= new System.Action<NoteController>(this.HandleNoteWasMissed);
    this._beatmapObjectManager.obstacleDidPassThreeQuartersOfMove2Event -= new System.Action<ObstacleController>(this.HandleObstacleDidPassThreeQuartersOfMove2);
  }

  public virtual void StartSong(float startTimeOffset = 0.0f)
  {
    this.UpdateBeatmapData(this._songBpm.OneBeatDuration() * (float) this._startWaitTimeInBeats);
    this._audioTimeSyncController.StartSong(startTimeOffset);
  }

  public override void StopSong()
  {
    this.StopAllCoroutines();
    this._audioTimeSyncController.StopSong();
  }

  public override void PauseSong()
  {
    this.StopAllCoroutines();
    this._audioTimeSyncController.Pause();
  }

  public override void ResumeSong() => this._audioTimeSyncController.Resume();

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if (noteController.noteData.colorType == ColorType.None)
    {
      this._bombWasCutSignal.Raise();
      this.UpdateBeatmapData(-1f);
    }
    else if (!noteCutInfo.allIsOK)
    {
      if (noteCutInfo.wasCutTooSoon)
        this._noteWasCutTooSoonSignal.Raise();
      else if (!noteCutInfo.saberTypeOK)
        this._noteWasCutWithWrongColorSignal.Raise();
      else if (!noteCutInfo.speedOK)
        this._noteWasCutWithSlowSpeedSignal.Raise();
      else if (!noteCutInfo.directionOK)
        this._noteWasCutFromDifferentDirectionSignal.Raise();
      this.UpdateBeatmapData(-1f);
    }
    else
    {
      this._noteWasCutOKSignal.Raise();
      ++this._tutorialBeatmapObjectIndex;
      this.UpdateBeatmapData(-1f);
    }
  }

  public virtual void HandleNoteWasMissed(NoteController noteController)
  {
    if (noteController.noteData.colorType != ColorType.None)
    {
      this.UpdateBeatmapData(-1f);
    }
    else
    {
      this._noteWasCutOKSignal.Raise();
      ++this._tutorialBeatmapObjectIndex;
      this.UpdateBeatmapData(-1f);
    }
  }

  public virtual void HandleObstacleDidPassThreeQuartersOfMove2(
    ObstacleController obstacleController)
  {
    ++this._tutorialBeatmapObjectIndex;
    this.UpdateBeatmapData(-1f);
  }

  public virtual void UpdateBeatmapData(float noteTime)
  {
    TutorialSongController.TutorialObjectSpawnData[] objectsSpawnData = this._normalModeTutorialObjectsSpawnData;
    if (this._tutorialBeatmapObjectIndex >= objectsSpawnData.Length)
    {
      this.SendSongDidFinishEvent();
    }
    else
    {
      TutorialSongController.TutorialObjectSpawnData tutorialObjectSpawnData = objectsSpawnData[this._tutorialBeatmapObjectIndex];
      bool flag = this._prevSpawnedBeatmapObjectIndex != this._tutorialBeatmapObjectIndex;
      if (flag && (UnityEngine.Object) tutorialObjectSpawnData.signal != (UnityEngine.Object) null)
        tutorialObjectSpawnData.signal.Raise();
      float time = (double) noteTime > 0.0 ? noteTime : this.GetNextBeatmapObjectTime(flag ? tutorialObjectSpawnData.firstTimeBeatOffset : tutorialObjectSpawnData.beatOffset);
      switch (tutorialObjectSpawnData)
      {
        case TutorialSongController.TutorialBasicNoteSpawnData tutorialBasicNoteSpawnData:
          this._beatmapData.AddBeatmapObjectDataInOrder((BeatmapObjectData) this.CreateBasicNoteData(time, tutorialBasicNoteSpawnData));
          break;
        case TutorialSongController.TutorialBombNoteSpawnData tutorialBombNoteSpawnData:
          this._beatmapData.AddBeatmapObjectDataInOrder((BeatmapObjectData) this.CreateBombNoteData(time, tutorialBombNoteSpawnData));
          break;
        case TutorialSongController.TutorialObstacleSpawnData tutorialObstacleSpawnData:
          this._beatmapData.AddBeatmapObjectDataInOrder((BeatmapObjectData) this.CreateObstacleData(time, tutorialObstacleSpawnData));
          break;
      }
      this._prevSpawnedBeatmapObjectIndex = this._tutorialBeatmapObjectIndex;
    }
  }

  public virtual float GetNextBeatmapObjectTime(int beatOffset)
  {
    float num = this._songBpm.OneBeatDuration();
    return (float) ((int) ((double) this._audioTimeSyncController.songTime / ((double) num * (double) this._numberOfBeatsToSnap) + 0.5) * this._numberOfBeatsToSnap + 1 + (beatOffset - 1) - 1) * num;
  }

  public virtual ObstacleData CreateObstacleData(
    float time,
    TutorialSongController.TutorialObstacleSpawnData tutorialObstacleSpawnData)
  {
    float num = this._songBpm.OneBeatDuration();
    int lineIndex = tutorialObstacleSpawnData.lineIndex;
    NoteLineLayer noteLineLayer = tutorialObstacleSpawnData.noteLineLayer;
    return new ObstacleData(time, lineIndex, noteLineLayer, (float) this._obstacleDurationInBeats * num, tutorialObstacleSpawnData.width, tutorialObstacleSpawnData.height);
  }

  public virtual NoteData CreateBasicNoteData(
    float time,
    TutorialSongController.TutorialBasicNoteSpawnData tutorialBasicNoteSpawnData)
  {
    int lineIndex = tutorialBasicNoteSpawnData.lineIndex;
    NoteLineLayer noteLineLayer = tutorialBasicNoteSpawnData.noteLineLayer;
    return NoteData.CreateBasicNoteData(time, lineIndex, noteLineLayer, tutorialBasicNoteSpawnData.colorType, tutorialBasicNoteSpawnData.cutDirection);
  }

  public virtual NoteData CreateBombNoteData(
    float time,
    TutorialSongController.TutorialBombNoteSpawnData tutorialBombNoteSpawnData)
  {
    int lineIndex = tutorialBombNoteSpawnData.lineIndex;
    NoteLineLayer noteLineLayer = tutorialBombNoteSpawnData.noteLineLayer;
    return NoteData.CreateBombNoteData(time, lineIndex, noteLineLayer);
  }

  public class InitData
  {
    public readonly float songBpm;
    public readonly BeatmapData beatmapData;

    public InitData(float songBpm, BeatmapData beatmapData)
    {
      this.songBpm = songBpm;
      this.beatmapData = beatmapData;
    }
  }

  public abstract class TutorialObjectSpawnData
  {
    public readonly Signal signal;
    public readonly int beatOffset;
    public readonly int firstTimeBeatOffset;
    public readonly int lineIndex;

    protected TutorialObjectSpawnData(
      Signal signal,
      int firstTimeBeatOffset,
      int beatOffset,
      int lineIndex)
    {
      this.signal = signal;
      this.firstTimeBeatOffset = firstTimeBeatOffset;
      this.beatOffset = beatOffset;
      this.lineIndex = lineIndex;
    }
  }

  public abstract class TutorialJumpingNoteSpawnData : TutorialSongController.TutorialObjectSpawnData
  {
    public readonly NoteLineLayer noteLineLayer;

    protected TutorialJumpingNoteSpawnData(
      Signal signal,
      int firstTimeBeatOffset,
      int beatOffset,
      int lineIndex,
      NoteLineLayer noteLineLayer)
      : base(signal, firstTimeBeatOffset, beatOffset, lineIndex)
    {
      this.noteLineLayer = noteLineLayer;
    }
  }

  public class TutorialBasicNoteSpawnData : TutorialSongController.TutorialJumpingNoteSpawnData
  {
    public readonly NoteCutDirection cutDirection;
    public readonly ColorType colorType;

    public TutorialBasicNoteSpawnData(
      Signal signal,
      int firstTimeBeatOffset,
      int beatOffset,
      int lineIndex,
      NoteLineLayer noteLineLayer,
      NoteCutDirection cutDirection,
      ColorType colorType)
      : base(signal, firstTimeBeatOffset, beatOffset, lineIndex, noteLineLayer)
    {
      this.cutDirection = cutDirection;
      this.colorType = colorType;
    }
  }

  public class TutorialBombNoteSpawnData : TutorialSongController.TutorialJumpingNoteSpawnData
  {
    public TutorialBombNoteSpawnData(
      Signal signal,
      int firstTimeBeatOffset,
      int beatOffset,
      int lineIndex,
      NoteLineLayer noteLineLayer)
      : base(signal, firstTimeBeatOffset, beatOffset, lineIndex, noteLineLayer)
    {
    }
  }

  public class TutorialObstacleSpawnData : TutorialSongController.TutorialObjectSpawnData
  {
    public readonly NoteLineLayer noteLineLayer;
    public readonly int width;
    public readonly int height;

    public TutorialObstacleSpawnData(
      Signal signal,
      int firstTimeBeatOffset,
      int beatOffset,
      int lineIndex,
      int width,
      int height,
      NoteLineLayer noteLineLayer)
      : base(signal, firstTimeBeatOffset, beatOffset, lineIndex)
    {
      this.width = width;
      this.height = height;
      this.noteLineLayer = noteLineLayer;
    }
  }
}
