// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventEditorDataComparer`1
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections;

namespace BeatmapEditor3D
{
  public class EventEditorDataComparer<TData> : IComparer where TData : BaseEditorData
  {
    public int Compare(object x, object y)
    {
      TData data1 = x as TData;
      TData data2 = y as TData;
      if ((object) data1 == null && (object) data2 == null)
        return 0;
      if ((object) data1 == null)
        return -1;
      if ((object) data2 == null)
        return 1;
      if (AudioTimeHelper.IsBeatSame(data1.beat, data2.beat))
        return 0;
      return (double) data1.beat > (double) data2.beat ? 1 : -1;
    }
  }
}
