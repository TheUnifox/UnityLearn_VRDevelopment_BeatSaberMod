// Decompiled with JetBrains decompiler
// Type: MeshRendererSwitchEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MeshRendererSwitchEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _beatmapEvent;
  [SerializeField]
  [NullAllowed]
  protected MeshRenderer[] _deactivateOnBoostRenderers;
  [SerializeField]
  [NullAllowed]
  protected MeshRenderer[] _activateOnBoostRenderers;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

  public virtual void Start() => this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(this._beatmapEvent));

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController == null)
      return;
    this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData) => this.ToggleObjects(basicBeatmapEventData.value == 1);

  public virtual void ToggleObjects(bool isBoostOn)
  {
    foreach (Renderer deactivateOnBoostRenderer in this._deactivateOnBoostRenderers)
      deactivateOnBoostRenderer.enabled = !isBoostOn;
    foreach (Renderer activateOnBoostRenderer in this._activateOnBoostRenderers)
      activateOnBoostRenderer.enabled = isBoostOn;
  }
}
