// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SelectArcCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D
{
  public class SelectArcCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SelectArcSignal _signal;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      if (this._beatmapLevelDataModel.GetArcById(this._signal.id) == (ArcEditorData) null)
        return;
      if (this._beatmapObjectsSelectionState.IsArcSelected(this._signal.id))
        this._beatmapObjectsSelectionState.RemoveArc(this._signal.id);
      else
        this._beatmapObjectsSelectionState.AddArc(this._signal.id);
      this._signalBus.Fire<BeatmapObjectsSelectionStateUpdatedSignal>();
    }
  }
}
