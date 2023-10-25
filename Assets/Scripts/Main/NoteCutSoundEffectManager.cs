// Decompiled with JetBrains decompiler
// Type: NoteCutSoundEffectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class NoteCutSoundEffectManager : MonoBehaviour, INoteCutSoundEffectDidFinishEvent
{
  [SerializeField]
  protected AudioManagerSO _audioManager;
  [Space]
  [SerializeField]
  protected float _audioSamplesBeatAlignOffset = 0.185f;
  [SerializeField]
  protected AudioClip[] _longCutEffectsAudioClips;
  [SerializeField]
  protected AudioClip[] _shortCutEffectsAudioClips;
  [Space]
  [SerializeField]
  protected AudioClip _testAudioClip;
  [InjectOptional]
  protected readonly NoteCutSoundEffectManager.InitData _initData = new NoteCutSoundEffectManager.InitData(false, false);
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly SaberManager _saberManager;
  [Inject]
  protected readonly NoteCutSoundEffect.Pool _noteCutSoundEffectPool;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [CompilerGenerated]
  protected bool m_ChandleWrongSaberTypeAsGood;
  protected const int kMaxNumberOfEffects = 64;
  protected const float kTwoNotesAtTheSameTimeVolumeMul = 0.9f;
  protected const float kDenseNotesVolumeMul = 0.9f;
  protected RandomObjectPicker<AudioClip> _randomLongCutSoundPicker;
  protected RandomObjectPicker<AudioClip> _randomShortCutSoundPicker;
  protected float _prevNoteATime = -1f;
  protected float _prevNoteBTime = -1f;
  protected NoteCutSoundEffect _prevNoteASoundEffect;
  protected NoteCutSoundEffect _prevNoteBSoundEffect;
  protected float _beatAlignOffset;
  protected bool _useTestAudioClips;
  protected MemoryPoolContainer<NoteCutSoundEffect> _noteCutSoundEffectPoolContainer;

  public bool handleWrongSaberTypeAsGood
  {
    get => this.m_ChandleWrongSaberTypeAsGood;
    set => this.m_ChandleWrongSaberTypeAsGood = value;
  }

  public virtual void Start()
  {
    this._useTestAudioClips = this._initData.useTestAudioClips;
    this._noteCutSoundEffectPoolContainer = new MemoryPoolContainer<NoteCutSoundEffect>((IMemoryPool<NoteCutSoundEffect>) this._noteCutSoundEffectPool);
    if (this._initData.useTestAudioClips)
    {
      this._randomLongCutSoundPicker = new RandomObjectPicker<AudioClip>(this._testAudioClip, 0.0f);
      this._randomShortCutSoundPicker = new RandomObjectPicker<AudioClip>(this._testAudioClip, 0.0f);
    }
    else
    {
      this._randomLongCutSoundPicker = new RandomObjectPicker<AudioClip>(this._longCutEffectsAudioClips, 0.0f);
      this._randomShortCutSoundPicker = new RandomObjectPicker<AudioClip>(this._shortCutEffectsAudioClips, 0.0f);
    }
    this._beatAlignOffset = this._audioSamplesBeatAlignOffset + this._audioManager.sfxLatency;
    this._beatmapObjectManager.noteWasSpawnedEvent += new System.Action<NoteController>(this.HandleNoteWasSpawned);
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasSpawnedEvent -= new System.Action<NoteController>(this.HandleNoteWasSpawned);
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
  }

  public virtual void HandleNoteWasSpawned(NoteController noteController)
  {
    NoteData noteData = noteController.noteData;
    if (!this.IsSupportedNote(noteData))
      return;
    float volumeMultiplier = noteData.cutSfxVolumeMultiplier;
    if ((double) volumeMultiplier <= 0.0)
      return;
    float timeScale = this._audioTimeSyncController.timeScale;
    if (noteData.colorType == ColorType.ColorA && (double) noteData.time < (double) this._prevNoteATime + 1.0 / 1000.0 || noteData.colorType == ColorType.ColorB && (double) noteData.time < (double) this._prevNoteBTime + 1.0 / 1000.0)
      return;
    bool flag1 = false;
    if ((double) noteData.time < (double) this._prevNoteATime + 1.0 / 1000.0 || (double) noteData.time < (double) this._prevNoteBTime + 1.0 / 1000.0)
    {
      if (noteData.colorType == ColorType.ColorA && this._prevNoteBSoundEffect.enabled)
      {
        this._prevNoteBSoundEffect.volumeMultiplier = 0.9f;
        flag1 = true;
      }
      else if (noteData.colorType == ColorType.ColorB && this._prevNoteASoundEffect.enabled)
      {
        this._prevNoteASoundEffect.volumeMultiplier = 0.9f;
        flag1 = true;
      }
    }
    NoteCutSoundEffect noteCutSoundEffect1 = this._noteCutSoundEffectPoolContainer.Spawn();
    noteCutSoundEffect1.transform.SetPositionAndRotation(this.transform.localPosition, Quaternion.identity);
    noteCutSoundEffect1.didFinishEvent.Add((INoteCutSoundEffectDidFinishEvent) this);
    Saber saber = (Saber) null;
    if (noteData.colorType == ColorType.ColorA)
    {
      this._prevNoteATime = noteData.time;
      saber = this._saberManager.leftSaber;
      this._prevNoteASoundEffect = noteCutSoundEffect1;
    }
    else if (noteData.colorType == ColorType.ColorB)
    {
      this._prevNoteBTime = noteData.time;
      saber = this._saberManager.rightSaber;
      this._prevNoteBSoundEffect = noteCutSoundEffect1;
    }
    bool flag2 = (double) noteData.timeToPrevColorNote < (double) this._beatAlignOffset;
    AudioClip audioClip = flag2 ? this._randomShortCutSoundPicker.PickRandomObject() : this._randomLongCutSoundPicker.PickRandomObject();
    float num1 = 1f;
    if (flag1)
      num1 = 0.9f;
    else if (flag2)
      num1 = 0.9f;
    noteCutSoundEffect1.Init(audioClip, noteController, (double) noteData.time / (double) timeScale + this._audioTimeSyncController.dspTimeOffset + (double) this._audioTimeSyncController.songTimeOffset, this._beatAlignOffset, 0.15f, noteData.timeToPrevColorNote / timeScale, noteData.timeToNextColorNote / timeScale, saber, this.handleWrongSaberTypeAsGood, num1 * volumeMultiplier, this._useTestAudioClips, this._initData.ignoreBadCuts);
    List<NoteCutSoundEffect> activeItems = this._noteCutSoundEffectPoolContainer.activeItems;
    NoteCutSoundEffect noteCutSoundEffect2 = (NoteCutSoundEffect) null;
    float num2 = float.MaxValue;
    foreach (NoteCutSoundEffect noteCutSoundEffect3 in activeItems)
    {
      if ((double) noteCutSoundEffect3.time < (double) num2)
      {
        num2 = noteCutSoundEffect3.time;
        noteCutSoundEffect2 = noteCutSoundEffect3;
      }
    }
    if (activeItems.Count <= 64 || !((UnityEngine.Object) noteCutSoundEffect2 != (UnityEngine.Object) null))
      return;
    noteCutSoundEffect2.StopPlayingAndFinish();
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if (this._useTestAudioClips || !this.IsSupportedNote(noteController.noteData))
      return;
    foreach (NoteCutSoundEffect activeItem in this._noteCutSoundEffectPoolContainer.activeItems)
      activeItem.NoteWasCut(noteController, in noteCutInfo);
  }

  public virtual void HandleNoteCutSoundEffectDidFinish(NoteCutSoundEffect noteCutSoundEffect)
  {
    noteCutSoundEffect.didFinishEvent.Remove((INoteCutSoundEffectDidFinishEvent) this);
    this._noteCutSoundEffectPoolContainer.Despawn(noteCutSoundEffect);
  }

  public virtual bool IsSupportedNote(NoteData noteData)
  {
    if (noteData.colorType == ColorType.None || noteData.gameplayType == NoteData.GameplayType.BurstSliderElement || noteData.gameplayType == NoteData.GameplayType.BurstSliderElementFill)
      return false;
    int gameplayType = (int) noteData.gameplayType;
    return true;
  }

  public class InitData
  {
    public readonly bool useTestAudioClips;
    public readonly bool ignoreBadCuts;

    public InitData(bool useTestAudioClips, bool ignoreBadCuts)
    {
      this.useTestAudioClips = useTestAudioClips;
      this.ignoreBadCuts = ignoreBadCuts;
    }
  }
}
