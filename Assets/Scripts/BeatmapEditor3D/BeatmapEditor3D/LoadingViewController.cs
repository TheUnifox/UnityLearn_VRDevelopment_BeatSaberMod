// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LoadingViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class LoadingViewController : SimpleEditorDialogViewController
  {
    [SerializeField]
    private GameObject _loadingIndicatorGO;

    public void Init(string title, string message, bool showLoadingIndicator)
    {
      this.Init(title, message, (string) null, (Action<int>) null);
      this._loadingIndicatorGO.SetActive(showLoadingIndicator);
    }
  }
}
