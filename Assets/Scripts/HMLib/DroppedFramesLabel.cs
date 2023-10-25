// Decompiled with JetBrains decompiler
// Type: DroppedFramesLabel
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using TMPro;
using UnityEngine;

public class DroppedFramesLabel : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected int _expectedFrameRate = 90;
  [SerializeField]
  protected int _resetInterval = 5;
  protected int _totalNumberOfDroppedFrames;
  protected float _syncedFrameTime;
  protected float _intervalTime;
  protected float _maxFrameTimeInInterval;
  protected int _frameCountInInterval;

  public virtual void Start()
  {
    this._syncedFrameTime = 1f / (float) this._expectedFrameRate;
    this._intervalTime = 0.0f;
    this._text.text = "0";
  }

  public virtual void Update()
  {
    ++this._frameCountInInterval;
    this._maxFrameTimeInInterval = Mathf.Max(this._maxFrameTimeInInterval, Time.unscaledDeltaTime);
    this._intervalTime += Time.unscaledDeltaTime;
    if ((double) this._intervalTime < (double) this._resetInterval)
      return;
    int num = this._resetInterval * this._expectedFrameRate - this._frameCountInInterval;
    if (num < 0)
      num = 0;
    this._totalNumberOfDroppedFrames += num;
    this.RefreshText();
    this._frameCountInInterval = 0;
    this._intervalTime = 0.0f;
    this._maxFrameTimeInInterval = 0.0f;
  }

  public virtual void RefreshText() => this._text.text = string.Format("Dropped: {0}\nFT: {1} : {2}", (object) this._totalNumberOfDroppedFrames, (object) Mathf.CeilToInt(this._maxFrameTimeInInterval / this._syncedFrameTime), (object) this._maxFrameTimeInInterval);
}
