// Decompiled with JetBrains decompiler
// Type: SliderInteractionEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class SliderInteractionEffect : MonoBehaviour
{
  [SerializeField]
  private SliderInteractionManager _sliderInteractionManager;

  protected float saberInteractionParam => this._sliderInteractionManager.saberInteractionParam;

  protected ColorType colorType => this._sliderInteractionManager.colorType;

  protected virtual void Start()
  {
    this._sliderInteractionManager.sliderWasAddedToActiveSlidersEvent += new System.Action<float>(this.HandleSliderWasAddedToActiveSliders);
    this._sliderInteractionManager.allSliderWereRemovedFromActiveSlidersEvent += new System.Action(this.HandleAllSliderWereRemovedFromActiveSliders);
  }

  protected void OnDestroy()
  {
    this._sliderInteractionManager.sliderWasAddedToActiveSlidersEvent -= new System.Action<float>(this.HandleSliderWasAddedToActiveSliders);
    this._sliderInteractionManager.allSliderWereRemovedFromActiveSlidersEvent -= new System.Action(this.HandleAllSliderWereRemovedFromActiveSliders);
  }

  protected abstract void StartEffect(float saberInteractionParam);

  protected abstract void EndEffect();

  private void HandleSliderWasAddedToActiveSliders(float saberInteractionParam) => this.StartEffect(saberInteractionParam);

  private void HandleAllSliderWereRemovedFromActiveSliders() => this.EndEffect();
}
