// Decompiled with JetBrains decompiler
// Type: GoodCutsMissionObjectiveChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class GoodCutsMissionObjectiveChecker : SimpleValueMissionObjectiveChecker
{
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;

  protected override void Init()
  {
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    if (this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Min || this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Equal)
      this.status = MissionObjectiveChecker.Status.NotClearedYet;
    else
      this.status = MissionObjectiveChecker.Status.NotFailedYet;
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if (noteController.noteData.colorType != ColorType.None && noteCutInfo.allIsOK)
      ++this.checkedValue;
    this.CheckAndUpdateStatus();
  }
}
