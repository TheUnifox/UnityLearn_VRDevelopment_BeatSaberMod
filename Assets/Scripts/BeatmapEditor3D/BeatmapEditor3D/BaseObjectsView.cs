// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BaseObjectsView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public abstract class BaseObjectsView : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditingMode _mode;
    [SerializeField]
    private GameObject _containerGameObject;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

    protected virtual void Start()
    {
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorStateModeSwitched));
      this.UpdateActiveState();
    }

    protected virtual void OnDestroy() => this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorStateModeSwitched));

    protected virtual void OnEnable() => this._signalBus.Subscribe<LevelEditorStateZenModeUpdatedSignal>(new Action(this.HandleLevelEditorStateZenModeUpdated));

    protected virtual void OnDisable() => this._signalBus.TryUnsubscribe<LevelEditorStateZenModeUpdatedSignal>(new Action(this.HandleLevelEditorStateZenModeUpdated));

    private void HandleLevelEditorStateModeSwitched() => this.UpdateActiveState();

    private void HandleLevelEditorStateZenModeUpdated() => this.UpdateActiveState();

    private void UpdateActiveState()
    {
      this.gameObject.SetActive(this._beatmapState.editingMode == this._mode);
      this._containerGameObject.SetActive(!this._beatmapEditorSettingsDataModel.zenMode && this._beatmapState.editingMode == this._mode);
    }
  }
}
