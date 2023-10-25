// Decompiled with JetBrains decompiler
// Type: ComboController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class ComboController : MonoBehaviour, IComboController
{
  [Inject]
  protected readonly PlayerHeadAndObstacleInteraction _playerHeadAndObstacleInteraction;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  protected int _combo;
  protected int _maxCombo;

  public int maxCombo => this._maxCombo;

  public event System.Action<int> comboDidChangeEvent;

  public event System.Action comboBreakingEventHappenedEvent;

  public virtual void Start()
  {
    this._playerHeadAndObstacleInteraction.headDidEnterObstaclesEvent += new System.Action(this.HandlePlayerHeadDidEnterObstacles);
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent += new System.Action<NoteController>(this.HandleNoteWasMissed);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._playerHeadAndObstacleInteraction != (UnityEngine.Object) null)
      this._playerHeadAndObstacleInteraction.headDidEnterObstaclesEvent -= new System.Action(this.HandlePlayerHeadDidEnterObstacles);
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent -= new System.Action<NoteController>(this.HandleNoteWasMissed);
  }

  public virtual void HandlePlayerHeadDidEnterObstacles()
  {
    if (this._combo > 0)
    {
      this._combo = 0;
      System.Action<int> comboDidChangeEvent = this.comboDidChangeEvent;
      if (comboDidChangeEvent != null)
        comboDidChangeEvent(this._combo);
    }
    System.Action eventHappenedEvent = this.comboBreakingEventHappenedEvent;
    if (eventHappenedEvent == null)
      return;
    eventHappenedEvent();
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if (noteCutInfo.allIsOK)
    {
      ++this._combo;
      if (this._maxCombo < this._combo)
        this._maxCombo = this._combo;
      System.Action<int> comboDidChangeEvent = this.comboDidChangeEvent;
      if (comboDidChangeEvent == null)
        return;
      comboDidChangeEvent(this._combo);
    }
    else
    {
      if (this._combo > 0)
      {
        this._combo = 0;
        System.Action<int> comboDidChangeEvent = this.comboDidChangeEvent;
        if (comboDidChangeEvent != null)
          comboDidChangeEvent(this._combo);
      }
      System.Action eventHappenedEvent = this.comboBreakingEventHappenedEvent;
      if (eventHappenedEvent == null)
        return;
      eventHappenedEvent();
    }
  }

  public virtual void HandleNoteWasMissed(NoteController noteController)
  {
    if (noteController.noteData.gameplayType == NoteData.GameplayType.Bomb)
      return;
    if (this._combo > 0)
    {
      this._combo = 0;
      System.Action<int> comboDidChangeEvent = this.comboDidChangeEvent;
      if (comboDidChangeEvent != null)
        comboDidChangeEvent(this._combo);
    }
    System.Action eventHappenedEvent = this.comboBreakingEventHappenedEvent;
    if (eventHappenedEvent == null)
      return;
    eventHappenedEvent();
  }
}
