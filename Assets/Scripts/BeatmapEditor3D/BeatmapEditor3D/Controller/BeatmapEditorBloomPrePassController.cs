// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BeatmapEditorBloomPrePassController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class BeatmapEditorBloomPrePassController : MonoBehaviour
  {
    [SerializeField]
    private BloomPrePassBackgroundColor _bloomPrePassBackgroundColor;
    [Space]
    [SerializeField]
    private Color _staticLightsColor;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

    protected void Awake() => this._signalBus.Subscribe<LevelEditorStateStaticLightsUpdatedSignal>(new Action(this.HandleLevelEditorStateStaticLightsUpdated));

    protected void OnDestroy() => this._signalBus.TryUnsubscribe<LevelEditorStateStaticLightsUpdatedSignal>(new Action(this.HandleLevelEditorStateStaticLightsUpdated));

    protected void Start() => this.SetStaticLights();

    private void HandleLevelEditorStateStaticLightsUpdated() => this.SetStaticLights();

    private void SetStaticLights()
    {
      this._bloomPrePassBackgroundColor.enabled = this._beatmapState.editingMode == BeatmapEditingMode.Objects && this._beatmapEditorSettingsDataModel.staticLights;
      this._bloomPrePassBackgroundColor.color = this._staticLightsColor;
    }
  }
}
