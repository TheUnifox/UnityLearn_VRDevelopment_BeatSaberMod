// Decompiled with JetBrains decompiler
// Type: BeatmapObjectsInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BeatmapObjectsInstaller : MonoInstaller
{
  [SerializeField]
  protected GameNoteController _normalBasicNotePrefab;
  [SerializeField]
  protected GameNoteController _proModeNotePrefab;
  [SerializeField]
  protected GameNoteController _burstSliderHeadNotePrefab;
  [SerializeField]
  protected BurstSliderGameNoteController _burstSliderNotePrefab;
  [SerializeField]
  protected BurstSliderGameNoteController _burstSliderFillPrefab;
  [SerializeField]
  protected BombNoteController _bombNotePrefab;
  [SerializeField]
  protected ObstacleController _obstaclePrefab;
  [SerializeField]
  protected SliderController _sliderShortPrefab;
  [SerializeField]
  protected SliderController _sliderMediumPrefab;
  [SerializeField]
  protected SliderController _sliderLongPrefab;
  [SerializeField]
  protected NoteLineConnectionController _noteLineConnectionControllerPrefab;
  [SerializeField]
  protected BeatLine _beatLinePrefab;
  [Inject]
  protected readonly GameplayCoreSceneSetupData _sceneSetupData;

  public override void InstallBindings()
  {
    bool proMode = this._sceneSetupData.gameplayModifiers.proMode;
    this.Container.BindMemoryPool<GameNoteController, GameNoteController.Pool>().WithId((object) NoteData.GameplayType.Normal).WithInitialSize(25).FromComponentInNewPrefab(proMode ? (Object) this._proModeNotePrefab : (Object) this._normalBasicNotePrefab);
    this.Container.BindMemoryPool<GameNoteController, GameNoteController.Pool>().WithId((object) NoteData.GameplayType.BurstSliderHead).WithInitialSize(10).FromComponentInNewPrefab((Object) this._burstSliderHeadNotePrefab);
    this.Container.BindMemoryPool<BurstSliderGameNoteController, BurstSliderGameNoteController.Pool>().WithId((object) NoteData.GameplayType.BurstSliderElement).WithInitialSize(40).FromComponentInNewPrefab((Object) this._burstSliderNotePrefab);
    this.Container.BindMemoryPool<BurstSliderGameNoteController, BurstSliderGameNoteController.Pool>().WithId((object) NoteData.GameplayType.BurstSliderElementFill).WithInitialSize(40).FromComponentInNewPrefab((Object) this._burstSliderFillPrefab);
    this.Container.BindMemoryPool<BombNoteController, BombNoteController.Pool>().WithInitialSize(35).FromComponentInNewPrefab((Object) this._bombNotePrefab);
    this.Container.BindMemoryPool<ObstacleController, ObstacleController.Pool>().WithInitialSize(25).FromComponentInNewPrefab((Object) this._obstaclePrefab);
    this.Container.BindMemoryPool<SliderController, SliderController.Pool.Short>().WithInitialSize(10).FromComponentInNewPrefab((Object) this._sliderShortPrefab);
    this.Container.BindMemoryPool<SliderController, SliderController.Pool.Medium>().WithInitialSize(10).FromComponentInNewPrefab((Object) this._sliderMediumPrefab);
    this.Container.BindMemoryPool<SliderController, SliderController.Pool.Long>().WithInitialSize(10).FromComponentInNewPrefab((Object) this._sliderLongPrefab);
    this.Container.Bind<SliderController.Pool>().AsSingle();
    this.Container.BindMemoryPool<NoteLineConnectionController, NoteLineConnectionController.Pool>().WithInitialSize(10).FromComponentInNewPrefab((Object) this._noteLineConnectionControllerPrefab);
    this.Container.BindMemoryPool<BeatLine, BeatLine.Pool>().WithInitialSize(16).FromComponentInNewPrefab((Object) this._beatLinePrefab);
  }
}
