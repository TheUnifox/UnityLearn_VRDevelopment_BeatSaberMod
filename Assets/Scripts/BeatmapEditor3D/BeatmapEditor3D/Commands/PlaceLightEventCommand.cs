// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.PlaceLightEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public abstract class PlaceLightEventCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;
    private BaseEditorData _event;

    public bool shouldAddToHistory { get; private set; }

    protected abstract float GetBeat();

    protected abstract BeatmapEditorObjectId GetEventBoxId();

    protected abstract BaseEditorData CreateEventData(float beat);

    public void Execute()
    {
      float beat = this._beatmapState.beat - this.GetBeat();
      if (this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataAt(this.GetEventBoxId(), beat) != null)
        return;
      this._event = this.CreateEventData(beat);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(this.GetEventBoxId(), this._event);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataAtBeat(this.GetEventBoxId(), this._event);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
