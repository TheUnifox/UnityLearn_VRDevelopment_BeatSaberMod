// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorGameObjectDisableController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapEditorGameObjectDisableController : MonoBehaviour
  {
    [SerializeField]
    private string _playerPlaceName;
    [SerializeField]
    private string _originName;

    protected void Start()
    {
      GameObject gameObject1 = GameObject.Find(this._playerPlaceName);
      GameObject gameObject2 = GameObject.Find(this._originName);
      if ((Object) gameObject1 != (Object) null)
        gameObject1.SetActive(false);
      if (!((Object) gameObject2 != (Object) null))
        return;
      gameObject2.SetActive(false);
    }
  }
}
