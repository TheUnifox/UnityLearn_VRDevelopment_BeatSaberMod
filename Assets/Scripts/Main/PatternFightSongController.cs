// Decompiled with JetBrains decompiler
// Type: PatternFightSongController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PatternFightSongController : SongController
{
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly PatternFightSongController.InitData _initData;
  protected const float kBPM = 120f;
  protected const float kPhaseLengthInBeats = 8f;
  protected BeatmapData _beatmapData;
  protected PatternFightSongController.GameplayPhase _gameplayPhase;
  protected int _gameplayPhaseNumber;
  protected readonly HashSet<NoteData> _thisPlayerSourcePatternNoteData = new HashSet<NoteData>();
  protected readonly HashSet<NoteData> _thisPlayerDefinedPatternNoteData = new HashSet<NoteData>();

  public virtual void Start()
  {
    this._beatmapData = this._initData.beatmapData;
    this._beatmapData.InsertBeatmapEventData((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event0, 1, 1f));
    this._beatmapData.InsertBeatmapEventData((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event4, 1, 1f));
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._gameplayPhase = PatternFightSongController.GameplayPhase.Start;
    this.StartSong();
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
  }

  public virtual void Update()
  {
    if ((double) this._audioTimeSyncController.songTime < (double) this._gameplayPhaseNumber * 8.0 * 60.0 / 120.0 - 1.0 / 1000.0)
      return;
    ++this._gameplayPhaseNumber;
    if (this._gameplayPhase == PatternFightSongController.GameplayPhase.ReplayThisPlayerPattern || this._gameplayPhase == PatternFightSongController.GameplayPhase.Start)
    {
      this.CreatePattern((float) ((double) this._gameplayPhaseNumber * 8.0 * 60.0 / 120.0));
      this._gameplayPhase = PatternFightSongController.GameplayPhase.DefineThisPlayerPattern;
    }
    else
    {
      if (this._gameplayPhase != PatternFightSongController.GameplayPhase.DefineThisPlayerPattern)
        return;
      this._gameplayPhase = PatternFightSongController.GameplayPhase.ReplayThisPlayerPattern;
    }
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    GameNoteController gameNoteController = noteController as GameNoteController;
    if ((Object) gameNoteController == (Object) null)
      return;
    NoteData noteData1 = gameNoteController.noteData;
    if (this._thisPlayerSourcePatternNoteData.Contains(noteData1))
    {
      NoteData noteData2 = noteData1.CopyWith(new float?(noteData1.time + 4f));
      noteData2.ChangeNoteCutDirection(NoteCutDirectionExtensions.MainNoteCutDirectionFromCutDirAngle(noteCutInfo.cutAngle));
      this._thisPlayerDefinedPatternNoteData.Add(noteData2);
      this._beatmapData.AddBeatmapObjectData((BeatmapObjectData) noteData2);
      this._thisPlayerSourcePatternNoteData.Remove(gameNoteController.noteData);
    }
    else
    {
      if (!this._thisPlayerDefinedPatternNoteData.Contains(gameNoteController.noteData))
        return;
      this._thisPlayerDefinedPatternNoteData.Remove(gameNoteController.noteData);
    }
  }

  public virtual void NoteWasMissed(NoteController noteController)
  {
    GameNoteController gameNoteController = noteController as GameNoteController;
    if ((Object) gameNoteController == (Object) null)
      return;
    this._thisPlayerDefinedPatternNoteData.Remove(gameNoteController.noteData);
    this._thisPlayerSourcePatternNoteData.Remove(gameNoteController.noteData);
  }

  public virtual void CreatePattern(float time)
  {
    foreach (NoteData noteData in new List<NoteData>(8)
    {
      NoteData.CreateBasicNoteData(time, 1, NoteLineLayer.Base, ColorType.ColorA, NoteCutDirection.Any),
      NoteData.CreateBasicNoteData(time, 1, NoteLineLayer.Base, ColorType.ColorA, NoteCutDirection.Any),
      NoteData.CreateBasicNoteData(time, 1, NoteLineLayer.Base, ColorType.ColorA, NoteCutDirection.Any),
      NoteData.CreateBasicNoteData(time, 1, NoteLineLayer.Base, ColorType.ColorA, NoteCutDirection.Any),
      NoteData.CreateBasicNoteData(time, 1, NoteLineLayer.Base, ColorType.ColorA, NoteCutDirection.Any),
      NoteData.CreateBasicNoteData(time, 1, NoteLineLayer.Base, ColorType.ColorA, NoteCutDirection.Any),
      NoteData.CreateBasicNoteData(time, 1, NoteLineLayer.Base, ColorType.ColorA, NoteCutDirection.Any),
      NoteData.CreateBasicNoteData(time, 1, NoteLineLayer.Base, ColorType.ColorA, NoteCutDirection.Any)
    })
    {
      this._thisPlayerSourcePatternNoteData.Add(noteData);
      this._beatmapData.AddBeatmapObjectData((BeatmapObjectData) noteData);
    }
  }

  public virtual void StartSong(float startTimeOffset = 0.0f) => this._audioTimeSyncController.StartSong(startTimeOffset);

  public override void StopSong() => this._audioTimeSyncController.StopSong();

  public override void PauseSong() => this._audioTimeSyncController.Pause();

  public override void ResumeSong() => this._audioTimeSyncController.Resume();

  public class InitData
  {
    public readonly BeatmapData beatmapData;

    public InitData(BeatmapData beatmapData) => this.beatmapData = beatmapData;
  }

  public enum GameplayPhase
  {
    Undefined = -1, // 0xFFFFFFFF
    Start = 0,
    DefineThisPlayerPattern = 1,
    ReplayThisPlayerPattern = 2,
  }
}
