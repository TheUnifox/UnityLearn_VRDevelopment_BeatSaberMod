// Decompiled with JetBrains decompiler
// Type: BufferedLightColorGroupEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BufferedLightColorGroupEffect
{
  protected readonly ColorManager _colorManager;
  protected readonly MaterialPropertyBlockController _materialPropertyBlockController;
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected readonly BeatmapDataCallbackWrapper _colorBoostBeatmapDataCallbackWrapper;
  protected readonly BeatmapDataCallbackWrapper[] _lightColorBeatmapEventCallbackWrappers;
  protected int _lastIndex;
  protected readonly float[] _timesBuffer = new float[24];
  protected readonly Vector4[] _colorsBuffer = new Vector4[24];
  protected readonly float[] _elementIdsBuffer = new float[24];
  protected bool _useBoostColors;
  protected bool _didReceiveEventThisFrame;
  protected const int kBufferSize = 24;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _timesBufferPropertyId = Shader.PropertyToID("_TimesBuffer");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorBufferPropertyId = Shader.PropertyToID("_ColorsBuffer");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _elementIdBufferPropertyId = Shader.PropertyToID("_ElementIdBuffer");

  public BufferedLightColorGroupEffect(
    BufferedLightColorGroupEffect.InitData initData,
    ColorManager colorManager,
    BeatmapCallbacksController beatmapCallbacksController)
  {
    this._materialPropertyBlockController = initData.materialPropertyBlockController;
    this._beatmapCallbacksController = beatmapCallbacksController;
    this._colorManager = colorManager;
    this._lightColorBeatmapEventCallbackWrappers = new BeatmapDataCallbackWrapper[initData.lightGroup.numberOfElements];
    for (int elementId = 0; elementId < initData.lightGroup.numberOfElements; ++elementId)
      this._lightColorBeatmapEventCallbackWrappers[elementId] = this._beatmapCallbacksController.AddBeatmapCallback<LightColorBeatmapEventData>(new BeatmapDataCallback<LightColorBeatmapEventData>(this.HandleColorChangeBeatmapEvent), LightColorBeatmapEventData.SubtypeIdentifier(initData.lightGroup.groupId, elementId));
    this._colorBoostBeatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<ColorBoostBeatmapEventData>(new BeatmapDataCallback<ColorBoostBeatmapEventData>(this.HandleColorBoostBeatmapEvent));
    this._beatmapCallbacksController.didProcessAllCallbacksThisFrameEvent += new System.Action(this.HandleBeatmapCallbacksControllerDidProcessAllCallbacksThisFrame);
  }

  public virtual void Cleanup()
  {
    foreach (BeatmapDataCallbackWrapper eventCallbackWrapper in this._lightColorBeatmapEventCallbackWrappers)
      this._beatmapCallbacksController?.RemoveBeatmapCallback(eventCallbackWrapper);
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._colorBoostBeatmapDataCallbackWrapper);
    this._beatmapCallbacksController.didProcessAllCallbacksThisFrameEvent -= new System.Action(this.HandleBeatmapCallbacksControllerDidProcessAllCallbacksThisFrame);
  }

  public virtual void HandleColorBoostBeatmapEvent(ColorBoostBeatmapEventData colorBoost) => this._useBoostColors = colorBoost.boostColorsAreOn;

  public virtual void HandleColorChangeBeatmapEvent(LightColorBeatmapEventData currentEvent)
  {
    Color color = this.GetColor(currentEvent.colorType, this._useBoostColors, currentEvent.brightness);
    this._timesBuffer[this._lastIndex] = currentEvent.time;
    this._colorsBuffer[this._lastIndex] = (Vector4) color;
    this._elementIdsBuffer[this._lastIndex] = (float) currentEvent.elementId;
    this._lastIndex = (this._lastIndex + 1) % 24;
    this._didReceiveEventThisFrame = true;
  }

  public virtual void HandleBeatmapCallbacksControllerDidProcessAllCallbacksThisFrame()
  {
    if (!this._didReceiveEventThisFrame)
      return;
    this._didReceiveEventThisFrame = false;
    this._materialPropertyBlockController.materialPropertyBlock.SetFloatArray(BufferedLightColorGroupEffect._timesBufferPropertyId, this._timesBuffer);
    this._materialPropertyBlockController.materialPropertyBlock.SetVectorArray(BufferedLightColorGroupEffect._colorBufferPropertyId, this._colorsBuffer);
    this._materialPropertyBlockController.materialPropertyBlock.SetFloatArray(BufferedLightColorGroupEffect._elementIdBufferPropertyId, this._elementIdsBuffer);
    this._materialPropertyBlockController.ApplyChanges();
  }

  protected virtual Color GetColor(
    EnvironmentColorType colorType,
    bool colorBoost,
    float brightness)
  {
    Color color = this._colorManager.ColorForType(colorType, colorBoost);
    return color.ColorWithAlpha(color.a * brightness);
  }

  public class InitData
  {
    public readonly LightGroup lightGroup;
    public readonly MaterialPropertyBlockController materialPropertyBlockController;

    public InitData(
      LightGroup lightGroup,
      MaterialPropertyBlockController materialPropertyBlockController)
    {
      this.lightGroup = lightGroup;
      this.materialPropertyBlockController = materialPropertyBlockController;
    }
  }
}
