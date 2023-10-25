// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.TurnOffValueDurationDataTransformer
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace BeatmapEditor3D.DataModels
{
  public class TurnOffValueDurationDataTransformer : IEventDataTransformer
  {
    public List<BasicEventEditorData> TransformTo(
      List<BasicEventEditorData> events,
      BpmData bpmData)
    {
      List<BasicEventEditorData> basicEventEditorDataList = new List<BasicEventEditorData>();
      for (int index = 0; index < events.Count; ++index)
      {
        BasicEventEditorData basicEventData = events[index];
        if (basicEventData.value == 0)
        {
          basicEventEditorDataList.Add(BasicEventEditorData.Copy(basicEventData));
        }
        else
        {
          BasicEventEditorData basicEventEditorData = index < events.Count - 1 ? events[index + 1] : (BasicEventEditorData) null;
          float endTime = (object) basicEventEditorData != null ? basicEventEditorData.beat : 3000f;
          int endValue = (object) basicEventEditorData != null ? basicEventEditorData.value : 0;
          float endFloatValue = (object) basicEventEditorData != null ? basicEventEditorData.floatValue : 1f;
          basicEventEditorDataList.Add(BasicEventEditorData.CreateNewWithId(basicEventData.id, basicEventData.type, basicEventData.beat, basicEventData.value, basicEventData.floatValue, endTime, endValue, endFloatValue));
        }
      }
      return basicEventEditorDataList;
    }

    public List<BasicEventEditorData> TransformFrom(List<BasicEventEditorData> events) => events.Select<BasicEventEditorData, BasicEventEditorData>((Func<BasicEventEditorData, BasicEventEditorData>) (current => BasicEventEditorData.CreateNewWithId(current.id, current.type, current.beat, current.value, current.floatValue))).ToList<BasicEventEditorData>();
  }
}
