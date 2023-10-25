// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorStandardGameplayInit
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapEditorStandardGameplayInit : MonoBehaviour
  {
    protected void Awake()
    {
      StandardLevelAnalytics objectOfType1 = Object.FindObjectOfType<StandardLevelAnalytics>();
      if ((Object) objectOfType1 != (Object) null)
        Object.Destroy((Object) objectOfType1);
      StandardLevelFailedController objectOfType2 = Object.FindObjectOfType<StandardLevelFailedController>();
      if ((Object) objectOfType2 != (Object) null)
        Object.Destroy((Object) objectOfType2);
      StandardLevelFinishedController objectOfType3 = Object.FindObjectOfType<StandardLevelFinishedController>();
      if (!((Object) objectOfType3 != (Object) null))
        return;
      Object.Destroy((Object) objectOfType3);
    }
  }
}
