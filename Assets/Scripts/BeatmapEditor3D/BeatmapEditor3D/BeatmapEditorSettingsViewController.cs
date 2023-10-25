// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorSettingsViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorSettingsViewController : BeatmapEditorViewController
  {
    [Header("General Settings")]
    [SerializeField]
    private OpenFolderView _openFolderView;
    [SerializeField]
    private WindowResolutionSettingsController _windowResolutionSettingsController;
    [Space]
    [SerializeField]
    private Vector2IntSO _windowResolution;
    [Header("Control Settings")]
    [SerializeField]
    private Toggle _invertSubdivisionScrollToggle;
    [SerializeField]
    private Toggle _invertTimelineScrollToggle;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      this._windowResolution.value = this._beatmapEditorSettingsDataModel.editorWindowResolution;
      this._openFolderView.SetFolderPath(this._beatmapEditorSettingsDataModel.customLevelsFolder);
      this._invertSubdivisionScrollToggle.SetIsOnWithoutNotify(this._beatmapEditorSettingsDataModel.invertSubdivisionScroll);
      this._invertTimelineScrollToggle.SetIsOnWithoutNotify(this._beatmapEditorSettingsDataModel.invertTimelineScroll);
      this._windowResolutionSettingsController.Refresh(true);
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling) => this._signalBus.Fire<BeatmapEditorSettingsSignals.UpdateSettingsSignal>(new BeatmapEditorSettingsSignals.UpdateSettingsSignal(this._openFolderView.path, this._windowResolution.value, this._invertSubdivisionScrollToggle.isOn, this._invertTimelineScrollToggle.isOn));
  }
}
