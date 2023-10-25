// Decompiled with JetBrains decompiler
// Type: LightColorGroupEffectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LightColorGroupEffectManager : MonoBehaviour
{
  [Inject]
  protected readonly LightGroup[] _lightGroups;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly DiContainer _container;
  protected BeatmapDataCallbackWrapper _colorBoostBeatmapDataCallbackWrapper;
  protected readonly List<LightColorGroupEffect> _lightColorGroupEffects = new List<LightColorGroupEffect>();

  public IReadOnlyCollection<LightGroup> lightGroups => (IReadOnlyCollection<LightGroup>) this._lightGroups;

  public virtual void Start()
  {
    foreach (LightGroup lightGroup in this._lightGroups)
    {
      for (int elementId = 0; elementId < lightGroup.numberOfElements; ++elementId)
        this._lightColorGroupEffects.Add(this._container.Instantiate<LightColorGroupEffect>((IEnumerable<object>) new LightColorGroupEffect.InitData[1]
        {
          new LightColorGroupEffect.InitData(lightGroup.groupId, elementId, lightGroup.startLightId + elementId)
        }));
    }
    this._colorBoostBeatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<ColorBoostBeatmapEventData>(new BeatmapDataCallback<ColorBoostBeatmapEventData>(this.HandleColorBoostBeatmapEvent));
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController != null)
      this._beatmapCallbacksController.RemoveBeatmapCallback(this._colorBoostBeatmapDataCallbackWrapper);
    foreach (LightColorGroupEffect colorGroupEffect in this._lightColorGroupEffects)
      colorGroupEffect.Cleanup();
  }

  public virtual void HandleColorBoostBeatmapEvent(ColorBoostBeatmapEventData eventData)
  {
    foreach (LightColorGroupEffect colorGroupEffect in this._lightColorGroupEffects)
      colorGroupEffect.UseBoostColors(eventData.boostColorsAreOn);
  }
}
