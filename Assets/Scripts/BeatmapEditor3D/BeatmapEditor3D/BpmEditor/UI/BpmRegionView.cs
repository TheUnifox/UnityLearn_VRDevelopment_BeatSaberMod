// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.BpmRegionView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class BpmRegionView : MonoBehaviour
  {
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private GameObject _textsContainer;
    [SerializeField]
    private TextMeshProUGUI _label;
    [SerializeField]
    private GameObject _backgroundGo;
    [SerializeField]
    private TextMeshProUGUI _beatText;
    [Space]
    [SerializeField]
    private float _fullWidth;
    [SerializeField]
    private float _bpmOnlyWidth;

    public RectTransform rectTransform => this._rectTransform;

    public void SetSampleData(int startSample, int endSample)
    {
      float x = this._rectTransform.sizeDelta.x;
      this._textsContainer.SetActive((double) x > (double) this._bpmOnlyWidth);
      if ((double) x > (double) this._fullWidth)
      {
        this._label.text = string.Format("{0} / {1}", (object) startSample, (object) endSample);
      }
      else
      {
        if ((double) x <= (double) this._bpmOnlyWidth)
          return;
        this._label.text = string.Format("{0}", (object) endSample);
      }
    }

    public void SetBeatData(float startBeat, float bpm, float beats)
    {
      float x = this._rectTransform.sizeDelta.x;
      this._beatText.text = string.Format("{0:F2}", (object) startBeat);
      this._beatText.gameObject.SetActive((double) x > (double) this._bpmOnlyWidth);
      this._textsContainer.SetActive((double) x > (double) this._bpmOnlyWidth);
      if ((double) x > (double) this._fullWidth)
      {
        this._label.text = string.Format("{0:F2} bpm / {1:F2} beats", (object) bpm, (object) beats);
      }
      else
      {
        if ((double) x <= (double) this._bpmOnlyWidth)
          return;
        this._label.text = string.Format("{0:F2}", (object) bpm);
      }
    }

    public void SetState(bool active) => this._backgroundGo.SetActive(active);

    public class Pool : MonoMemoryPool<BpmRegionView>
    {
    }
  }
}
