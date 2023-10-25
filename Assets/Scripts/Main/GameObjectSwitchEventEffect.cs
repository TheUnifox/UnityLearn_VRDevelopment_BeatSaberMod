// Decompiled with JetBrains decompiler
// Type: GameObjectSwitchEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class GameObjectSwitchEventEffect : MonoBehaviour
{
  [SerializeField]
  [NullAllowed]
  protected GameObject[] _deactivateOnBoostObjects;
  [SerializeField]
  [NullAllowed]
  protected GameObject[] _activateOnBoostObjects;
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

  public virtual void HandleBeatmapEvent(ColorBoostBeatmapEventData basicBeatmapEventData) => this.ToggleObjects(basicBeatmapEventData.boostColorsAreOn);

  public virtual void ToggleObjects(bool isBoostOn)
  {
    foreach (GameObject deactivateOnBoostObject in this._deactivateOnBoostObjects)
      deactivateOnBoostObject.SetActive(!isBoostOn);
    foreach (GameObject activateOnBoostObject in this._activateOnBoostObjects)
      activateOnBoostObject.SetActive(isBoostOn);
  }
}
