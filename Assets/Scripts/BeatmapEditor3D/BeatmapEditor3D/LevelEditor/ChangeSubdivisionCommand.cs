// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeSubdivisionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeSubdivisionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeSubdivisionSignal _signal;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
        return;
      float num = this._beatmapEditorSettingsDataModel.invertSubdivisionScroll ? this._signal.delta : -this._signal.delta;
      switch (this._signal.type)
      {
        case ChangeSubdivisionSignal.Type.Subdivision:
          if ((double) num > 0.0)
          {
            this._beatmapState.beatSubdivisionsModel.NextSubdivision();
            break;
          }
          this._beatmapState.beatSubdivisionsModel.PreviousSubdivision();
          break;
        case ChangeSubdivisionSignal.Type.Multiplication:
          if ((double) num > 0.0)
          {
            this._beatmapState.beatSubdivisionsModel.NextMultiplication();
            break;
          }
          this._beatmapState.beatSubdivisionsModel.PreviousMultiplication();
          break;
      }
      this._signalBus.Fire<SubdivisionChangedSignal>();
    }
  }
}
