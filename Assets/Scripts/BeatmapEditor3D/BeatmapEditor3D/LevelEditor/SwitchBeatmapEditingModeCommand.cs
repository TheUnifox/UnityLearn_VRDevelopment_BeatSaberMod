// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.SwitchBeatmapEditingModeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class SwitchBeatmapEditingModeCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SwitchBeatmapEditingModeSignal _signal;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      if (this._beatmapState.editingMode == this._signal.mode)
        return;
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Place));
      if (this._beatmapState.editingMode == BeatmapEditingMode.EventBoxes)
        this._signalBus.Fire<ClearEditedEventBoxGroupSignal>();
      this._beatmapState.editingMode = this._signal.mode;
      this._signalBus.Fire<ToggleStaticLightSignal>(new ToggleStaticLightSignal()
      {
        staticLightIsOn = this._beatmapEditorSettingsDataModel.staticLights
      });
      this._signalBus.Fire<BeatmapEditingModeSwitched>(new BeatmapEditingModeSwitched(this._beatmapState.editingMode));
    }
  }
}
