// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorAutoExposureController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorAutoExposureController : MonoBehaviour
  {
    [SerializeField]
    private BloomPrePassEffectContainerSO _container;
    [Space]
    [SerializeField]
    private BloomPrePassEffectSO _onEffect;
    [SerializeField]
    private BloomPrePassEffectSO _offEffect;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

    protected void Start()
    {
      this._signalBus.Subscribe<LevelEditorStateAutoExposureUpdatedSignal>(new Action(this.HandleLevelEditorStateAutoExposureUpdated));
      this.UpdateEffectState();
    }

    protected void OnDestroy() => this._signalBus.TryUnsubscribe<LevelEditorStateAutoExposureUpdatedSignal>(new Action(this.HandleLevelEditorStateAutoExposureUpdated));

    private void HandleLevelEditorStateAutoExposureUpdated() => this.UpdateEffectState();

    private void UpdateEffectState() => this._container.Init(this._beatmapEditorSettingsDataModel.autoExposure ? this._onEffect : this._offEffect);
  }
}
