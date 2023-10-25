// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.GroupInfoView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.SerializedData;
using TMPro;
using UnityEngine;

namespace BeatmapEditor3D.Views
{
  public class GroupInfoView : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _idsInfoText;
    [SerializeField]
    private TextMeshProUGUI _filteredInfoText;
    [SerializeField]
    private TextMeshProUGUI _filteredAndLimitedInfoText;

    public void SetIndexFilter(IndexFilterEditorData indexFilterData, int groupSize)
    {
      IndexFilter indexFilter = BeatmapDataLoader.IndexFilterConvertor.Convert(BeatmapLevelDataModelSaver.CreateIndexFilter(indexFilterData), groupSize);
      this._idsInfoText.text = string.Format("{0}", (object) groupSize);
      this._filteredInfoText.text = string.Format("{0}", (object) indexFilter.Count);
      this._filteredAndLimitedInfoText.text = string.Format("{0}", (object) indexFilter.VisibleCount);
    }
  }
}
