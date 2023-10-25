// Decompiled with JetBrains decompiler
// Type: NoteCutSoundEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class NoteCutSoundEffect : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected AnimationCurve _speedToVolumeCurve;
  [SerializeField]
  protected AudioClip[] _badCutSoundEffectAudioClips;
  [SerializeField]
  protected float _badCutVolume = 1f;
  [SerializeField]
  protected float _goodCutVolume = 1f;
  protected Saber _saber;
  protected NoteController _noteController;
  protected bool _isPlaying;
  protected float _volumeMultiplier;
  protected bool _noteWasCut;
  protected float _aheadTime;
  protected float _timeToNextNote;
  protected float _timeToPrevNote;
  protected double _startDSPTime;
  protected double _endDSPtime;
  protected double _fadeOutStartDSPtime;
  protected float _noteMissedTimeOffset;
  protected float _beforeCutVolume;
  protected bool _goodCut;
  protected float _pitch;
  protected RandomObjectPicker<AudioClip> _badCutRandomSoundPicker;
  protected bool _handleWrongSaberTypeAsGood;
  protected bool _ignoreSaberSpeed;
  protected bool _ignoreBadCuts;
  protected readonly LazyCopyHashSet<INoteCutSoundEffectDidFinishEvent> _didFinishEvent = new LazyCopyHashSet<INoteCutSoundEffectDidFinishEvent>();
  protected const float kEndOverlap = 100.01f;
  protected const float kEndFadeLength = 0.01f;

  public ILazyCopyHashSet<INoteCutSoundEffectDidFinishEvent> didFinishEvent => (ILazyCopyHashSet<INoteCutSoundEffectDidFinishEvent>) this._didFinishEvent;

  public float volumeMultiplier
  {
    set => this._volumeMultiplier = value;
    get => this._volumeMultiplier;
  }

  public float time => this._noteController.noteData.time;

  public virtual void Awake() => this._badCutRandomSoundPicker = new RandomObjectPicker<AudioClip>(this._badCutSoundEffectAudioClips, 0.01f);

  public virtual void Start() => this._audioSource.loop = false;

  public virtual void Init(
    AudioClip audioClip,
    NoteController noteController,
    double noteDSPTime,
    float aheadTime,
    float missedTimeOffset,
    float timeToPrevNote,
    float timeToNextNote,
    Saber saber,
    bool handleWrongSaberTypeAsGood,
    float volumeMultiplier,
    bool ignoreSaberSpeed,
    bool ignoreBadCuts)
  {
    this._pitch = Random.Range(0.9f, 1.2f);
    this._ignoreSaberSpeed = ignoreSaberSpeed;
    this._ignoreBadCuts = ignoreBadCuts;
    this._beforeCutVolume = 0.0f;
    this._volumeMultiplier = volumeMultiplier;
    this.enabled = true;
    this._audioSource.clip = audioClip;
    this._noteMissedTimeOffset = missedTimeOffset;
    this._aheadTime = aheadTime / this._pitch;
    this._timeToNextNote = timeToNextNote;
    this._timeToPrevNote = timeToPrevNote;
    this._saber = saber;
    this._noteController = noteController;
    this._handleWrongSaberTypeAsGood = handleWrongSaberTypeAsGood;
    this._noteWasCut = false;
    if (this._ignoreSaberSpeed)
    {
      this._beforeCutVolume = this._goodCutVolume;
      this._audioSource.volume = this._goodCutVolume;
    }
    else
    {
      this._beforeCutVolume = 0.0f;
      this._audioSource.volume = this._speedToVolumeCurve.Evaluate(saber.bladeSpeed);
    }
    this._audioSource.pitch = this._pitch;
    this._audioSource.priority = 128;
    this.transform.position = saber.saberBladeTopPos;
    this.ComputeDSPTimes(noteDSPTime, this._aheadTime, timeToPrevNote, timeToNextNote);
    this._audioSource.PlayScheduled(this._startDSPTime);
  }

  public virtual void ComputeDSPTimes(
    double noteDSPTime,
    float aheadTime,
    float timeToPrevNote,
    float timeToNextNote)
  {
    this._startDSPTime = noteDSPTime - (double) aheadTime;
    this._fadeOutStartDSPtime = noteDSPTime + (double) Mathf.Clamp((float) ((double) timeToNextNote - 0.0099999997764825821 + 100.01000213623047), 0.01f, this._audioSource.clip.length / this._pitch - aheadTime);
    this._endDSPtime = this._fadeOutStartDSPtime + 0.0099999997764825821;
  }

  public virtual void LateUpdate()
  {
    double dspTime = AudioSettings.dspTime;
    if (dspTime - this._endDSPtime > 0.0)
      this.StopPlayingAndFinish();
    else if (!this._noteWasCut)
    {
      if (dspTime > this._startDSPTime + (double) this._aheadTime - 0.05000000074505806)
        this._audioSource.priority = 32;
      this.transform.position = this._saber.saberBladeTopPos;
      float goodCutVolume = this._goodCutVolume;
      if (!this._ignoreSaberSpeed)
        goodCutVolume *= this._speedToVolumeCurve.Evaluate(this._saber.bladeSpeed) * (1f - Mathf.Clamp01((this._audioSource.time / this._pitch - this._aheadTime) / this._noteMissedTimeOffset));
      this._beforeCutVolume = (double) goodCutVolume >= (double) this._beforeCutVolume ? goodCutVolume : Mathf.Lerp(this._beforeCutVolume, goodCutVolume, Time.deltaTime * 4f);
      this._audioSource.volume = this._beforeCutVolume * this._volumeMultiplier;
    }
    else
    {
      if (!this._goodCut)
        return;
      this._audioSource.volume = this._goodCutVolume * this._volumeMultiplier;
    }
  }

  public virtual void StopPlayingAndFinish()
  {
    this.enabled = false;
    this._audioSource.Stop();
    this._isPlaying = false;
    foreach (INoteCutSoundEffectDidFinishEvent effectDidFinishEvent in this._didFinishEvent.items)
      effectDidFinishEvent.HandleNoteCutSoundEffectDidFinish(this);
  }

  public virtual void NoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if ((Object) this._noteController != (Object) noteController)
      return;
    this._noteWasCut = true;
    if (!this._ignoreBadCuts && (!this._handleWrongSaberTypeAsGood && !noteCutInfo.allIsOK || this._handleWrongSaberTypeAsGood && (!noteCutInfo.allExceptSaberTypeIsOK || noteCutInfo.saberTypeOK)))
    {
      this._audioSource.priority = 16;
      this._audioSource.clip = this._badCutRandomSoundPicker.PickRandomObject();
      this._audioSource.time = 0.0f;
      this._audioSource.Play();
      this._goodCut = false;
      this._audioSource.volume = this._badCutVolume;
      this._endDSPtime = AudioSettings.dspTime + (double) this._audioSource.clip.length / (double) this._pitch + 0.10000000149011612;
    }
    else
    {
      this._audioSource.priority = 24;
      this._goodCut = true;
      this._audioSource.volume = this._goodCutVolume;
    }
    this.transform.position = noteCutInfo.cutPoint;
  }

  public class Pool : MonoMemoryPool<NoteCutSoundEffect>
  {
  }
}
