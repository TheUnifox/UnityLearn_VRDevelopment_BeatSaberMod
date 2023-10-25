// Decompiled with JetBrains decompiler
// Type: MissMissionObjectiveChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class MissMissionObjectiveChecker : SimpleValueMissionObjectiveChecker
{
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;

  protected override void Init()
  {
    this._beatmapObjectManager.noteWasMissedEvent -= new System.Action<NoteController>(this.HandleNoteWasMissed);
    this._beatmapObjectManager.noteWasMissedEvent += new System.Action<NoteController>(this.HandleNoteWasMissed);
    if (this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Min || this._missionObjective.referenceValueComparisonType == MissionObjective.ReferenceValueComparisonType.Equal)
      this.status = MissionObjectiveChecker.Status.NotClearedYet;
    else
      this.status = MissionObjectiveChecker.Status.NotFailedYet;
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasMissedEvent -= new System.Action<NoteController>(this.HandleNoteWasMissed);
  }

  public virtual void HandleNoteWasMissed(NoteController noteController)
  {
    if (noteController.noteData.colorType != ColorType.None)
      ++this.checkedValue;
    this.CheckAndUpdateStatus();
  }
}
