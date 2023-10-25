// Decompiled with JetBrains decompiler
// Type: FireEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public abstract class FireEffect : MonoBehaviour
{
  [SerializeField]
  private int _groupId;
  [SerializeField]
  private int _elementId;
  [SerializeField]
  private int _lightId;
  [SerializeField]
  protected MaterialPropertyBlockController _flipBookPropertyBlockController;
  [SerializeField]
  protected MaterialPropertyBlockController _bloomPropertyBlockController;
  [SerializeField]
  protected MaterialPropertyBlockController _privatePointLightPropertyBlockController;
  [SerializeField]
  protected BloomPrePassBackgroundNonLightRenderer _bloomPrePassRenderer;
  [SerializeField]
  protected float _bloomIntensityMultiplier = 1f;
  [SerializeField]
  [ColorUsage(false, true)]
  protected Color _pointLightColor = Color.yellow;
  [Space]
  [SerializeField]
  private bool _contributeCustomLightColor = true;
  [SerializeField]
  [DrawIf("_contributeCustomLightColor", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed]
  private ColorSO _customLightColorContribution;
  [Inject]
  private readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  private readonly LightWithIdManager _lightWithIdManager;
  private BeatmapDataCallbackWrapper _lightColorBeatmapEventCallbackWrapper;

  protected virtual void Start() => this._lightColorBeatmapEventCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<LightColorBeatmapEventData>(new BeatmapDataCallback<LightColorBeatmapEventData>(this.HandleColorChangeBeatmapEvent), LightColorBeatmapEventData.SubtypeIdentifier(this._groupId, this._elementId));

  protected virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._lightColorBeatmapEventCallbackWrapper);
  }

  protected abstract void HandleColorChangeBeatmapEvent(LightColorBeatmapEventData e);

  protected void SetRenderersEnabled(bool enabled)
  {
    foreach (Renderer renderer in this._flipBookPropertyBlockController.renderers)
      renderer.enabled = enabled;
    this._bloomPrePassRenderer.enabled = enabled;
  }

  protected void NotifyAlphaWasChanged(float currentAlpha)
  {
    if (!this._contributeCustomLightColor)
      return;
    this._lightWithIdManager.SetColorForId(this._lightId, this._customLightColorContribution.color.ColorWithAlpha(currentAlpha));
  }
}
