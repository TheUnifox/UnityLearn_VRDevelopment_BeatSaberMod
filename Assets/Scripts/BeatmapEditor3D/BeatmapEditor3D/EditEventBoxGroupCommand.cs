// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EditEventBoxGroupCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D
{
  public class EditEventBoxGroupCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly EditEventBoxGroupSignal _signal;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      EventBoxGroupEditorData eventBoxGroupById = this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(this._signal.id);
      if (eventBoxGroupById == (EventBoxGroupEditorData) null)
        return;
      this._eventBoxGroupsState.eventBoxGroupContext = eventBoxGroupById;
      this._beatmapState.beatOffset = eventBoxGroupById.beat;
      this._signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(eventBoxGroupById.beat, UpdatePlayHeadSignal.SnapType.None));
      if (this._beatmapState.editingMode == BeatmapEditingMode.EventBoxes)
        this._signalBus.Fire<EditingEventBoxGroupChangedSignal>();
      else
        this._signalBus.Fire<SwitchBeatmapEditingModeSignal>(new SwitchBeatmapEditingModeSignal(BeatmapEditingMode.EventBoxes));
    }
  }
}
