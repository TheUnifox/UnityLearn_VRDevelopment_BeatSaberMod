// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public abstract class ModifyEventCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private BasicEventEditorData _originalBasicEventData;
    private BasicEventEditorData _modifiedBasicEventData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      BasicEventEditorData originalEventData = this.GetOriginalEventData();
      if (originalEventData == (BasicEventEditorData) null || !this.IsCorrectType(this._beatmapDataModel.environmentTrackDefinition[originalEventData.type].trackToolbarType))
        return;
      this._originalBasicEventData = originalEventData;
      this._modifiedBasicEventData = this.GetModifiedEventData(this._originalBasicEventData);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapBasicEventsDataModel.Remove(this._modifiedBasicEventData);
      this._beatmapBasicEventsDataModel.Insert(this._originalBasicEventData);
      this._signalBus.Fire<ChangeHoverEventObjectSignal>(new ChangeHoverEventObjectSignal(this._originalBasicEventData));
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapBasicEventsDataModel.Remove(this._originalBasicEventData);
      this._beatmapBasicEventsDataModel.Insert(this._modifiedBasicEventData);
      this._signalBus.Fire<ChangeHoverEventObjectSignal>(new ChangeHoverEventObjectSignal(this._modifiedBasicEventData));
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    protected abstract bool IsCorrectType(TrackToolbarType type);

    protected abstract BasicEventEditorData GetOriginalEventData();

    protected abstract BasicEventEditorData GetModifiedEventData(
      BasicEventEditorData originalBasicEventData);
  }
}
