// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.RemoveEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class RemoveEventCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly RemoveEventSignal _signal;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    private BasicEventEditorData _basicEventToRemove;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._basicEventToRemove = BasicEventEditorData.Copy(this._signal.basicEventData);
      if (this._beatmapBasicEventsDataModel.GetBasicEventAt(this._basicEventToRemove.type, this._basicEventToRemove.beat) == (BasicEventEditorData) null)
        return;
      if (this._eventsSelectionState.IsSelected(this._basicEventToRemove.id))
        this._signalBus.Fire<DeselectSingleEventSignal>(new DeselectSingleEventSignal()
        {
          basicEventEditorData = this._basicEventToRemove
        });
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapBasicEventsDataModel.Insert(this._basicEventToRemove);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapBasicEventsDataModel.Remove(this._basicEventToRemove);
      this._signalBus.Fire<ChangeHoverEventObjectSignal>(new ChangeHoverEventObjectSignal((BasicEventEditorData) null));
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
