// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.LightEventDataTransformer
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.DataModels
{
  public class LightEventDataTransformer : IEventDataTransformer
  {
    public List<BasicEventEditorData> TransformTo(
      List<BasicEventEditorData> events,
      BpmData bpmData)
    {
      List<BasicEventEditorData> basicEventEditorDataList = new List<BasicEventEditorData>();
      for (int index = 0; index < events.Count; ++index)
      {
        BasicEventEditorData basicEventEditorData1 = events[index];
        float beat = basicEventEditorData1.beat;
        int num1 = basicEventEditorData1.value;
        BasicEventEditorData basicEventEditorData2 = index < events.Count - 1 ? events[index + 1] : (BasicEventEditorData) null;
        float num2 = (object) basicEventEditorData2 != null ? basicEventEditorData2.beat : 3000f;
        int endValue = (object) basicEventEditorData2 != null ? basicEventEditorData2.value : 0;
        float endFloatValue = (object) basicEventEditorData2 != null ? basicEventEditorData2.floatValue : 1f;
        if (num1 == 3 || num1 == 7 || num1 == 11)
        {
          num2 = bpmData.FadeEndBeat(basicEventEditorData1.beat, 2f);
          if (basicEventEditorData2 != (BasicEventEditorData) null)
            num2 = Mathf.Min(num2, basicEventEditorData2.beat);
        }
        basicEventEditorDataList.Add(BasicEventEditorData.CreateNewWithId(basicEventEditorData1.id, basicEventEditorData1.type, beat, num1, basicEventEditorData1.floatValue, num2, endValue, endFloatValue));
      }
      return basicEventEditorDataList;
    }

    public List<BasicEventEditorData> TransformFrom(List<BasicEventEditorData> events) => events.Select<BasicEventEditorData, BasicEventEditorData>((Func<BasicEventEditorData, BasicEventEditorData>) (current => !current.hasEndTime ? current : BasicEventEditorData.CreateNewWithId(current.id, current.type, current.beat, current.value, current.floatValue))).ToList<BasicEventEditorData>();
  }
}
