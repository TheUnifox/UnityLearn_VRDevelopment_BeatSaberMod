// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.MirrorSelectedEventBoxGroupsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class MirrorSelectedEventBoxGroupsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>> _oldLightColorLists = new Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>>();
    private readonly Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>> _newLightColorLists = new Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>>();

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._selectionState.eventBoxGroups.Count == 0)
        return;
      List<EventBoxGroupEditorData> list1 = this._selectionState.eventBoxGroups.Select<BeatmapEditorObjectId, EventBoxGroupEditorData>((Func<BeatmapEditorObjectId, EventBoxGroupEditorData>) (id => this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(id))).Where<EventBoxGroupEditorData>((Func<EventBoxGroupEditorData, bool>) (g => g.type == EventBoxGroupEditorData.EventBoxGroupType.Color)).ToList<EventBoxGroupEditorData>();
      if (list1.Count == 0)
        return;
      foreach (BaseEditorData baseEditorData in list1)
      {
        foreach (EventBoxEditorData eventBoxEditorData in this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(baseEditorData.id))
        {
          List<LightColorBaseEditorData> list2 = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId<LightColorBaseEditorData>(eventBoxEditorData.id).ToList<LightColorBaseEditorData>();
          List<LightColorBaseEditorData> list3 = list2.Select<LightColorBaseEditorData, LightColorBaseEditorData>((Func<LightColorBaseEditorData, LightColorBaseEditorData>) (e => LightColorBaseEditorData.CopyWithModifications(e, new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId()), colorType: new LightColorBaseEditorData.EnvironmentColorType?(e.colorType.MirrorColor())))).ToList<LightColorBaseEditorData>();
          this._oldLightColorLists[eventBoxEditorData.id] = list2;
          this._newLightColorLists[eventBoxEditorData.id] = list3;
        }
      }
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, List<LightColorBaseEditorData>> newLightColorList in this._newLightColorLists)
        this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorDataList(newLightColorList.Key);
      foreach (KeyValuePair<BeatmapEditorObjectId, List<LightColorBaseEditorData>> oldLightColorList in this._oldLightColorLists)
        this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataList(oldLightColorList.Key, (IEnumerable<BaseEditorData>) oldLightColorList.Value);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>(new BeatmapLevelUpdatedSignal(true));
    }

    public void Redo()
    {
      foreach (KeyValuePair<BeatmapEditorObjectId, List<LightColorBaseEditorData>> oldLightColorList in this._oldLightColorLists)
        this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorDataList(oldLightColorList.Key);
      foreach (KeyValuePair<BeatmapEditorObjectId, List<LightColorBaseEditorData>> newLightColorList in this._newLightColorLists)
        this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataList(newLightColorList.Key, (IEnumerable<BaseEditorData>) newLightColorList.Value);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>(new BeatmapLevelUpdatedSignal(true));
    }
  }
}
