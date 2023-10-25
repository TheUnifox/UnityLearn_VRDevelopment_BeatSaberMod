// Decompiled with JetBrains decompiler
// Type: SliderInteractionManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class SliderInteractionManager : MonoBehaviour
{
  [SerializeField]
  protected ColorType _colorType;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [CompilerGenerated]
  protected float m_CsaberInteractionParam;
  protected readonly List<SliderController> _activeSliders = new List<SliderController>();

  public ColorType colorType => this._colorType;

  public float saberInteractionParam
  {
    get => this.m_CsaberInteractionParam;
    private set => this.m_CsaberInteractionParam = value;
  }

  public event System.Action<float> sliderWasAddedToActiveSlidersEvent;

  public event System.Action allSliderWereRemovedFromActiveSlidersEvent;

  public virtual void Start()
  {
    this._beatmapObjectManager.sliderWasSpawnedEvent += new System.Action<SliderController>(this.HandleSliderWasSpawned);
    this._beatmapObjectManager.sliderWasDespawnedEvent += new System.Action<SliderController>(this.HandleSliderWasDespawned);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.sliderWasSpawnedEvent -= new System.Action<SliderController>(this.HandleSliderWasSpawned);
    this._beatmapObjectManager.sliderWasDespawnedEvent -= new System.Action<SliderController>(this.HandleSliderWasDespawned);
  }

  public virtual void Update()
  {
    this.saberInteractionParam = 0.0f;
    foreach (SliderController activeSlider in this._activeSliders)
      this.saberInteractionParam = Mathf.Max(this.saberInteractionParam, activeSlider.saberInteractionParam);
  }

  public virtual void AddActiveSlider(SliderController newSliderController)
  {
    if (!this._activeSliders.Contains(newSliderController))
      this._activeSliders.Add(newSliderController);
    this.saberInteractionParam = 0.0f;
    foreach (SliderController activeSlider in this._activeSliders)
      this.saberInteractionParam = Mathf.Max(this.saberInteractionParam, activeSlider.saberInteractionParam);
    System.Action<float> activeSlidersEvent = this.sliderWasAddedToActiveSlidersEvent;
    if (activeSlidersEvent == null)
      return;
    activeSlidersEvent(this.saberInteractionParam);
  }

  public virtual void RemoveActiveSlider(SliderController sliderController)
  {
    this._activeSliders.Remove(sliderController);
    if (this._activeSliders.Count != 0)
      return;
    System.Action activeSlidersEvent = this.allSliderWereRemovedFromActiveSlidersEvent;
    if (activeSlidersEvent == null)
      return;
    activeSlidersEvent();
  }

  public virtual void HandleSliderWasSpawned(SliderController sliderController)
  {
    if (sliderController.sliderData.colorType != this._colorType)
      return;
    this.AddActiveSlider(sliderController);
  }

  public virtual void HandleSliderWasDespawned(SliderController sliderController)
  {
    if (sliderController.sliderData.colorType != this._colorType)
      return;
    this.RemoveActiveSlider(sliderController);
  }
}
