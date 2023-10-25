// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LightEventMarkerObject
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class LightEventMarkerObject : EventMarkerObject
  {
    [SerializeField]
    private TextMeshPro _sideText;
    [SerializeField]
    private TextMeshPro _topText;
    [Header("Top")]
    [SerializeField]
    private GameObject _topOnSprite;
    [SerializeField]
    private GameObject _topFadeOutSprite;
    [SerializeField]
    private GameObject _topFlashOnSprite;
    [Header("Side")]
    [SerializeField]
    private GameObject _sideOnSprite;
    [SerializeField]
    private GameObject _sideFadeOutSprite;
    [SerializeField]
    private GameObject _sideFlashOnSprite;
    private GameObject _topCurrentSprite;
    private GameObject _sideCurrentSprite;

    public void SetEventValue(int value, float floatValue)
    {
      string str = string.Format("{0:F0}", (object) ((double) floatValue * 100.0));
      this._sideText.text = str;
      this._topText.text = str;
      if ((Object) this._sideCurrentSprite != (Object) null && (Object) this._topCurrentSprite != (Object) null)
      {
        this._topCurrentSprite.SetActive(false);
        this._sideCurrentSprite.SetActive(false);
        this._topCurrentSprite = (GameObject) null;
        this._sideCurrentSprite = (GameObject) null;
      }
      switch (value)
      {
        case 1:
        case 5:
        case 9:
          this._sideCurrentSprite = this._sideOnSprite;
          this._topCurrentSprite = this._topOnSprite;
          break;
        case 2:
        case 4:
        case 6:
        case 8:
        case 10:
        case 12:
          this._sideCurrentSprite = this._sideFadeOutSprite;
          this._topCurrentSprite = this._topFadeOutSprite;
          break;
        case 3:
        case 7:
        case 11:
          this._sideCurrentSprite = this._sideFlashOnSprite;
          this._topCurrentSprite = this._topFlashOnSprite;
          break;
      }
      if (!((Object) this._sideCurrentSprite != (Object) null) || !((Object) this._topCurrentSprite != (Object) null))
        return;
      this._topCurrentSprite.SetActive(true);
      this._sideCurrentSprite.SetActive(true);
    }

    public class Pool : MonoMemoryPool<LightEventMarkerObject>
    {
    }
  }
}
