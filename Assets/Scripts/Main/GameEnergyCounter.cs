// Decompiled with JetBrains decompiler
// Type: GameEnergyCounter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class GameEnergyCounter : MonoBehaviour, IGameEnergyCounter
{
  protected const float kBadNoteEnergyDrain = 0.1f;
  protected const float kBadBurstSliderElementEnergyDrain = 0.025f;
  protected const float kMissNoteEnergyDrain = 0.15f;
  protected const float kMissBurstSliderElementEnergyDrain = 0.03f;
  protected const float kHitBombEnergyDrain = 0.15f;
  protected const float kGoodNoteEnergyCharge = 0.01f;
  protected const float kGoodBurstSliderElementCharge = 0.002f;
  protected const float kObstacleEnergyDrainPerSecond = 1.3f;
  protected int _batteryLives = 4;
  [Inject]
  protected readonly GameEnergyCounter.InitData _initData;
  [Inject]
  protected readonly SaberClashChecker _saberClashChecker;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly PlayerHeadAndObstacleInteraction _playerHeadAndObstacleInteraction;
  [CompilerGenerated]
  protected float m_Cenergy = 1f;
  [CompilerGenerated]
  protected GameplayModifiers.EnergyType m_CenergyType;
  [CompilerGenerated]
  protected bool m_CnoFail;
  [CompilerGenerated]
  protected bool m_CinstaFail;
  [CompilerGenerated]
  protected bool m_CfailOnSaberClash;
  protected bool _isInitialized;
  protected bool _didReach0Energy;
  protected float _nextFrameEnergyChange;

  public event System.Action didInitEvent;

  public event System.Action gameEnergyDidReach0Event;

  public event System.Action<float> gameEnergyDidChangeEvent;

  public bool isInitialized => this._isInitialized;

  public float energy
  {
    get => this.m_Cenergy;
    private set => this.m_Cenergy = value;
  }

  public int batteryEnergy => Mathf.FloorToInt((float) ((double) this.energy * (double) this.batteryLives + 0.5));

  public int batteryLives => this._batteryLives;

  public GameplayModifiers.EnergyType energyType
  {
    get => this.m_CenergyType;
    private set => this.m_CenergyType = value;
  }

  public bool noFail
  {
    get => this.m_CnoFail;
    private set => this.m_CnoFail = value;
  }

  public bool instaFail
  {
    get => this.m_CinstaFail;
    private set => this.m_CinstaFail = value;
  }

  public bool failOnSaberClash
  {
    get => this.m_CfailOnSaberClash;
    private set => this.m_CfailOnSaberClash = value;
  }

  public virtual void Start()
  {
    this.energyType = this._initData.energyType;
    this.noFail = this._initData.noFail;
    this.instaFail = this._initData.instaFail;
    this.failOnSaberClash = this._initData.failOnSaberClash;
    this.energy = this.energyType != GameplayModifiers.EnergyType.Battery ? 0.5f : 1f;
    if (this.instaFail)
    {
      this.energyType = GameplayModifiers.EnergyType.Battery;
      this._batteryLives = 1;
      this.energy = 1f;
    }
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent += new System.Action<NoteController>(this.HandleNoteWasMissed);
    this._isInitialized = true;
    System.Action didInitEvent = this.didInitEvent;
    if (didInitEvent != null)
      didInitEvent();
    System.Action<float> energyDidChangeEvent = this.gameEnergyDidChangeEvent;
    if (energyDidChangeEvent == null)
      return;
    energyDidChangeEvent(this.energy);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent -= new System.Action<NoteController>(this.HandleNoteWasMissed);
  }

  public virtual void LateUpdate()
  {
    if (this._playerHeadAndObstacleInteraction.playerHeadIsInObstacle)
      this.ProcessEnergyChange(Time.deltaTime * -1.3f);
    if (this._saberClashChecker.AreSabersClashing(out Vector3 _) && this.failOnSaberClash)
      this.ProcessEnergyChange(-this.energy);
    if (Mathf.Approximately(this._nextFrameEnergyChange, 0.0f))
      return;
    this.ProcessEnergyChange(this._nextFrameEnergyChange);
    this._nextFrameEnergyChange = 0.0f;
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    switch (noteController.noteData.gameplayType)
    {
      case NoteData.GameplayType.Normal:
      case NoteData.GameplayType.BurstSliderHead:
        this._nextFrameEnergyChange += noteCutInfo.allIsOK ? 0.01f : -0.1f;
        break;
      case NoteData.GameplayType.Bomb:
        this._nextFrameEnergyChange -= 0.15f;
        break;
      case NoteData.GameplayType.BurstSliderElement:
        this._nextFrameEnergyChange += noteCutInfo.allIsOK ? 1f / 500f : -0.025f;
        break;
    }
  }

  public virtual void HandleNoteWasMissed(NoteController noteController)
  {
    switch (noteController.noteData.gameplayType)
    {
      case NoteData.GameplayType.Normal:
      case NoteData.GameplayType.BurstSliderHead:
        this._nextFrameEnergyChange -= 0.15f;
        break;
      case NoteData.GameplayType.BurstSliderElement:
        this._nextFrameEnergyChange -= 0.03f;
        break;
    }
  }

  public virtual void ProcessEnergyChange(float energyChange)
  {
    if (this.noFail || this._didReach0Energy)
      return;
    if ((double) energyChange < 0.0)
    {
      if ((double) this.energy <= 0.0)
        return;
      if (this.instaFail)
        this.energy = 0.0f;
      else if (this.energyType == GameplayModifiers.EnergyType.Battery)
        this.energy -= 1f / (float) this._batteryLives;
      else
        this.energy += energyChange;
      if ((double) this.energy <= 9.9999997473787516E-06)
      {
        this.energy = 0.0f;
        this._didReach0Energy = true;
        System.Action energyDidReach0Event = this.gameEnergyDidReach0Event;
        if (energyDidReach0Event != null)
          energyDidReach0Event();
      }
    }
    else if (this.energyType == GameplayModifiers.EnergyType.Bar)
    {
      if ((double) this.energy >= 1.0)
        return;
      this.energy += energyChange;
      if ((double) this.energy >= 1.0)
        this.energy = 1f;
    }
    System.Action<float> energyDidChangeEvent = this.gameEnergyDidChangeEvent;
    if (energyDidChangeEvent == null)
      return;
    energyDidChangeEvent(this.energy);
  }

  public class InitData
  {
    public readonly GameplayModifiers.EnergyType energyType;
    public readonly bool noFail;
    public readonly bool instaFail;
    public readonly bool failOnSaberClash;

    public InitData(
      GameplayModifiers.EnergyType energyType,
      bool noFail,
      bool instaFail,
      bool failOnSaberClash)
    {
      this.energyType = energyType;
      this.noFail = noFail;
      this.instaFail = instaFail;
      this.failOnSaberClash = failOnSaberClash;
    }
  }
}
