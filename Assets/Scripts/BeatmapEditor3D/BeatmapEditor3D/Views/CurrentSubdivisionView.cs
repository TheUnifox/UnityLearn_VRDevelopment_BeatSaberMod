// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.CurrentSubdivisionView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using TMPro;
using UnityEngine;

namespace BeatmapEditor3D.Views
{
  public class CurrentSubdivisionView : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _subdivisionText;
    [SerializeField]
    private TextMeshProUGUI _prevSubdivisionText;

    public void SetSubdivision(int subdivision, int prevSubdivision)
    {
      int num1 = subdivision / 4;
      int num2 = prevSubdivision / 4;
      this._subdivisionText.text = string.Format("1/{0}", (object) num1);
      this._prevSubdivisionText.text = string.Format("1/{0}", (object) num2);
    }
  }
}
