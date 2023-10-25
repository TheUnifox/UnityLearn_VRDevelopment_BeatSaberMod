// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.SwapLightColorEventBoxesEditorGroupCommand
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
  public class SwapLightColorEventBoxesEditorGroupCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly SwapLightColorEventBoxesEditorGroupSignal _signal;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>> _oldLightColorLists = new Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>>();
    private readonly Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>> _newLightColorLists = new Dictionary<BeatmapEditorObjectId, List<LightColorBaseEditorData>>();

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(this._signal.eventBoxGroupId).type != EventBoxGroupEditorData.EventBoxGroupType.Color)
        return;
      foreach (EventBoxEditorData eventBoxEditorData in this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._signal.eventBoxGroupId))
      {
        List<LightColorBaseEditorData> list1 = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId<LightColorBaseEditorData>(eventBoxEditorData.id).ToList<LightColorBaseEditorData>();
        List<LightColorBaseEditorData> list2 = list1.Select<LightColorBaseEditorData, LightColorBaseEditorData>((Func<LightColorBaseEditorData, LightColorBaseEditorData>) (e => LightColorBaseEditorData.CopyWithModifications(e, new BeatmapEditorObjectId?(BeatmapEditorObjectId.NewId()), colorType: new LightColorBaseEditorData.EnvironmentColorType?(e.colorType.SwapColor())))).ToList<LightColorBaseEditorData>();
        this._oldLightColorLists[eventBoxEditorData.id] = list1;
        this._newLightColorLists[eventBoxEditorData.id] = list2;
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
