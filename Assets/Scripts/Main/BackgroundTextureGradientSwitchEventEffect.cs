// Decompiled with JetBrains decompiler
// Type: BackgroundTextureGradientSwitchEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BackgroundTextureGradientSwitchEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BloomPrePassBackgroundTextureGradient _defaultTextureGradient;
  [SerializeField]
  protected BloomPrePassBackgroundTextureGradient _boostTextureGradient;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start() => this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<ColorBoostBeatmapEventData>(new BeatmapDataCallback<ColorBoostBeatmapEventData>(this.HandleBeatmapEvent));

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void HandleBeatmapEvent(ColorBoostBeatmapEventData eventData)
  {
    bool boostColorsAreOn = eventData.boostColorsAreOn;
    this._defaultTextureGradient.enabled = !boostColorsAreOn;
    this._boostTextureGradient.enabled = boostColorsAreOn;
  }
}
