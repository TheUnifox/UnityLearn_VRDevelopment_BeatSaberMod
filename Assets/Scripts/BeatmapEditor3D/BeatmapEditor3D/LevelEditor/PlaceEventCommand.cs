// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.PlaceEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class PlaceEventCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly PlaceEventSignal _placeEventSignal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    private BasicEventEditorData _newEvt;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      BasicBeatmapEventType beatmapEventType = this._placeEventSignal.basicEventTrack.basicBeatmapEventType;
      float beat = this._beatmapState.beat;
      if (this._songPreviewController.isPlaying)
        beat = AudioTimeHelper.RoundToBeat(beat, this._beatmapState.subdivision);
      (int value, float floatValue) = this._basicEventsState.GetSelectedBeatmapTypeValue(this._placeEventSignal.basicEventTrack.trackToolbarType);
      if (this._beatmapBasicEventsDataModel.GetBasicEventAt(beatmapEventType, beat) != (BasicEventEditorData) null)
        return;
      this._newEvt = BasicEventEditorData.CreateNew(beatmapEventType, beat, value, floatValue);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapBasicEventsDataModel.Remove(this._newEvt);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapBasicEventsDataModel.Insert(this._newEvt);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
