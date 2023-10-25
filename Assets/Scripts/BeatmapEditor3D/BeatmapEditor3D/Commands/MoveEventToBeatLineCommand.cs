// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MoveEventToBeatLineCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MoveEventToBeatLineCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly MoveEventToBeatLineSignal _signal;
    [Inject]
    private readonly EventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private BasicEventEditorData _prevBasicEventData;
    private BasicEventEditorData _newBasicEventData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._prevBasicEventData = this._signal.basicEventEditorData;
      if (this._prevBasicEventData == (BasicEventEditorData) null)
        return;
      this._eventsSelectionState.draggedBasicEventData = this._prevBasicEventData;
      float beat = this._beatmapState.beat;
      if (AudioTimeHelper.IsBeatSame(this._signal.basicEventEditorData.beat, beat))
        return;
      this._newBasicEventData = BasicEventEditorData.CreateNewWithId(this._prevBasicEventData.id, this._prevBasicEventData.type, beat, this._prevBasicEventData.value, this._prevBasicEventData.floatValue);
      if (this._beatmapBasicEventsDataModel.GetBasicEventAt(this._newBasicEventData.type, this._newBasicEventData.beat) != (BasicEventEditorData) null)
      {
        this._eventsSelectionState.draggedBasicEventData = (BasicEventEditorData) null;
      }
      else
      {
        this._eventsSelectionState.draggedBasicEventData = this._newBasicEventData;
        this.shouldAddToHistory = true;
        this.Redo();
      }
    }

    public void Undo()
    {
      this._beatmapBasicEventsDataModel.Remove(this._newBasicEventData);
      this._beatmapBasicEventsDataModel.Insert(this._prevBasicEventData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapBasicEventsDataModel.Remove(this._prevBasicEventData);
      this._beatmapBasicEventsDataModel.Insert(this._newBasicEventData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
