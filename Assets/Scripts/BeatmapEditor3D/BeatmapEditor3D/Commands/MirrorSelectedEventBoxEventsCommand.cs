// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MirrorSelectedEventBoxEventsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MirrorSelectedEventBoxEventsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly EventBoxesSelectionState _selectionState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>> _oldLightColorListsByGroups = new Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>>();
    private readonly Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>> _newLightColorListsByGroups = new Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>>();

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._selectionState.eventBoxGroupType != EventBoxGroupEditorData.EventBoxGroupType.Color)
        return;
      foreach ((BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId) tuple in (IEnumerable<(BeatmapEditorObjectId eventBoxId, BeatmapEditorObjectId eventId)>) this._selectionState.events)
      {
        LightColorBaseEditorData original = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataById<LightColorBaseEditorData>(tuple.eventBoxId, tuple.eventId).Item1;
        if (!(original == (LightColorBaseEditorData) null))
        {
          BeatmapEditorObjectId eventBoxId = tuple.eventBoxId;
          List<LightColorBaseEditorData> colorBaseEditorDataList1;
          if (!this._oldLightColorListsByGroups.TryGetValue(eventBoxId, out colorBaseEditorDataList1))
            colorBaseEditorDataList1 = this._oldLightColorListsByGroups[eventBoxId] = new List<LightColorBaseEditorData>();
          List<LightColorBaseEditorData> colorBaseEditorDataList2;
          if (!this._newLightColorListsByGroups.TryGetValue(eventBoxId, out colorBaseEditorDataList2))
            colorBaseEditorDataList2 = this._newLightColorListsByGroups[eventBoxId] = new List<LightColorBaseEditorData>();
          colorBaseEditorDataList1.Add(original);
          colorBaseEditorDataList2.Add(LightColorBaseEditorData.CopyWithModifications(original, colorType: new LightColorBaseEditorData.EnvironmentColorType?(original.colorType.MirrorColor())));
        }
      }
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this.ReplaceEvents(this._oldLightColorListsByGroups, this._newLightColorListsByGroups);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>(new BeatmapLevelUpdatedSignal(true));
    }

    public void Redo()
    {
      this.ReplaceEvents(this._newLightColorListsByGroups, this._oldLightColorListsByGroups);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>(new BeatmapLevelUpdatedSignal(true));
    }

    private void ReplaceEvents(
      Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>> toAdd,
      Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>> toRemove)
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, List<LightColorBaseEditorData>> keyValuePair in toRemove)
      {
        foreach (LightColorBaseEditorData colorBaseEditorData in keyValuePair.Value)
          this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(keyValuePair.Key, (BaseEditorData) colorBaseEditorData);
      }
      foreach (KeyValuePair<BeatmapEditorObjectId, List<LightColorBaseEditorData>> keyValuePair in toAdd)
      {
        foreach (LightColorBaseEditorData data in keyValuePair.Value)
          this._beatmapEventBoxGroupsDataModel.InsertBaseEditorData(keyValuePair.Key, (BaseEditorData) data);
      }
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Mirrored {0} event box events", (object) this._selectionState.events.Count));
  }
}
