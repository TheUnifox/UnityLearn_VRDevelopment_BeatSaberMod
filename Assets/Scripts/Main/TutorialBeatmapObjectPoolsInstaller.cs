// Decompiled with JetBrains decompiler
// Type: TutorialBeatmapObjectPoolsInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class TutorialBeatmapObjectPoolsInstaller : MonoInstaller
{
  [SerializeField]
  protected TutorialNoteController _basicNotePrefab;
  [SerializeField]
  protected BombNoteController _bombNotePrefab;
  [SerializeField]
  protected ObstacleController _obstaclePrefab;
  [SerializeField]
  protected NoteLineConnectionController _noteLineConnectionControllerPrefab;

  public override void InstallBindings()
  {
    this.Container.BindMemoryPool<TutorialNoteController, TutorialNoteController.Pool>().WithInitialSize(20).FromComponentInNewPrefab((Object) this._basicNotePrefab);
    this.Container.BindMemoryPool<BombNoteController, BombNoteController.Pool>().WithInitialSize(20).FromComponentInNewPrefab((Object) this._bombNotePrefab);
    this.Container.BindMemoryPool<ObstacleController, ObstacleController.Pool>().WithInitialSize(4).FromComponentInNewPrefab((Object) this._obstaclePrefab);
    this.Container.BindMemoryPool<NoteLineConnectionController, NoteLineConnectionController.Pool>().WithInitialSize(10).FromComponentInNewPrefab((Object) this._noteLineConnectionControllerPrefab);
  }
}
