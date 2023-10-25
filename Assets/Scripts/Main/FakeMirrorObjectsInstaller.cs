// Decompiled with JetBrains decompiler
// Type: FakeMirrorObjectsInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class FakeMirrorObjectsInstaller : MonoInstaller
{
  [Space]
  [SerializeField]
  protected MirroredGameNoteController _mirroredGameNoteControllerPrefab;
  [SerializeField]
  protected MirroredGameNoteController _mirroredBurstSliderHeadGameNoteControllerPrefab;
  [SerializeField]
  protected MirroredGameNoteController _mirroredBurstSliderGameNoteControllerPrefab;
  [SerializeField]
  protected MirroredGameNoteController _mirroredBurstSliderFillControllerPrefab;
  [SerializeField]
  protected MirroredBombNoteController _mirroredBombNoteControllerPrefab;
  [SerializeField]
  protected MirroredObstacleController _mirroredObstacleControllerPrefab;
  [SerializeField]
  protected MirroredSliderController _mirroredSliderControllerPrefab;
  [Space]
  [SerializeField]
  protected IntSO _mirrorGraphicsSettings;
  [SerializeField]
  protected MirrorRendererGraphicsSettingsPresets _mirrorRendererGraphicsSettingsPresets;

  public MirroredGameNoteController mirroredGameNoteControllerPrefab => this._mirroredGameNoteControllerPrefab;

  public override void InstallBindings()
  {
    if ((int) (ObservableVariableSO<int>) this._mirrorGraphicsSettings >= this._mirrorRendererGraphicsSettingsPresets.presets.Length)
      this._mirrorGraphicsSettings.value = this._mirrorRendererGraphicsSettingsPresets.presets.Length - 1;
    int num = this._mirrorRendererGraphicsSettingsPresets.presets[(int) (ObservableVariableSO<int>) this._mirrorGraphicsSettings].mirrorType == MirrorRendererGraphicsSettingsPresets.Preset.MirrorType.FakeMirror ? 1 : 0;
    bool flag = this.Container.AllContracts.Any<BindingId>((Func<BindingId, bool>) (t => typeof (BeatmapObjectManager).IsAssignableFrom(t.Type)));
    if (num == 0 || !flag)
    {
      this.Container.Bind<FakeReflectionDynamicObjectsState>().FromInstance(FakeReflectionDynamicObjectsState.Disabled).AsSingle();
    }
    else
    {
      this.Container.Bind<FakeReflectionDynamicObjectsState>().FromInstance(FakeReflectionDynamicObjectsState.Enabled).AsSingle();
      this.Container.BindMemoryPool<MirroredGameNoteController, MirroredGameNoteController.Pool>().WithId((object) NoteData.GameplayType.Normal).WithInitialSize(25).FromComponentInNewPrefab((UnityEngine.Object) this._mirroredGameNoteControllerPrefab);
      this.Container.BindMemoryPool<MirroredGameNoteController, MirroredGameNoteController.Pool>().WithId((object) NoteData.GameplayType.BurstSliderHead).WithInitialSize(10).FromComponentInNewPrefab((UnityEngine.Object) this._mirroredBurstSliderHeadGameNoteControllerPrefab);
      this.Container.BindMemoryPool<MirroredGameNoteController, MirroredGameNoteController.Pool>().WithId((object) NoteData.GameplayType.BurstSliderElement).WithInitialSize(40).FromComponentInNewPrefab((UnityEngine.Object) this._mirroredBurstSliderGameNoteControllerPrefab);
      this.Container.BindMemoryPool<MirroredGameNoteController, MirroredGameNoteController.Pool>().WithId((object) NoteData.GameplayType.BurstSliderElementFill).WithInitialSize(25).FromComponentInNewPrefab((UnityEngine.Object) this._mirroredBurstSliderFillControllerPrefab);
      this.Container.BindMemoryPool<MirroredBombNoteController, MirroredBombNoteController.Pool>().WithInitialSize(35).FromComponentInNewPrefab((UnityEngine.Object) this._mirroredBombNoteControllerPrefab);
      this.Container.BindMemoryPool<MirroredObstacleController, MirroredObstacleController.Pool>().WithInitialSize(25).FromComponentInNewPrefab((UnityEngine.Object) this._mirroredObstacleControllerPrefab);
      this.Container.BindMemoryPool<MirroredSliderController, MirroredSliderController.Pool>().WithInitialSize(10).FromComponentInNewPrefab((UnityEngine.Object) this._mirroredSliderControllerPrefab);
      this.Container.Bind<MirroredBeatmapObjectManager>().To<MirroredBeatmapObjectManager>().AsSingle().NonLazy();
    }
  }
}
