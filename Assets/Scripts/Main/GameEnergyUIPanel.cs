// Decompiled with JetBrains decompiler
// Type: GameEnergyUIPanel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Zenject;

public class GameEnergyUIPanel : MonoBehaviour
{
  [SerializeField]
  protected Image _energyBar;
  [SerializeField]
  protected PlayableDirector _playableDirector;
  [Space]
  [SerializeField]
  protected Image _batteryLifeSegmentPrefab;
  [Space]
  [SerializeField]
  protected float _batterySegmentSeparatorWidth = 1f;
  [SerializeField]
  protected float _batterySegmentHorizontalPadding = 1f;
  [Inject]
  protected readonly IGameEnergyCounter _gameEnergyCounter;
  protected List<Image> _batteryLifeSegments;
  protected int _activeBatteryLifeSegmentsCount;
  protected RectTransform _energyBarRectTransform;

  public virtual void Start()
  {
    this._energyBarRectTransform = this._energyBar.rectTransform;
    if (this._gameEnergyCounter.isInitialized)
      this.Init();
    else
      this._gameEnergyCounter.didInitEvent += new System.Action(this.HandleGameEnergyCounterDidInit);
  }

  public virtual void Init()
  {
    this._gameEnergyCounter.didInitEvent -= new System.Action(this.HandleGameEnergyCounterDidInit);
    if (this._gameEnergyCounter.noFail)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      if (this._gameEnergyCounter.energyType == GameplayModifiers.EnergyType.Battery)
      {
        this._energyBar.gameObject.SetActive(false);
        this.CreateUIForBatteryEnergyType(this._gameEnergyCounter.batteryLives);
      }
      this.RefreshEnergyUI(this._gameEnergyCounter.energy);
      this._gameEnergyCounter.gameEnergyDidChangeEvent += new System.Action<float>(this.HandleGameEnergyDidChange);
    }
  }

  public virtual void OnDestroy()
  {
    if (this._gameEnergyCounter == null)
      return;
    this._gameEnergyCounter.didInitEvent -= new System.Action(this.HandleGameEnergyCounterDidInit);
    this._gameEnergyCounter.gameEnergyDidChangeEvent -= new System.Action<float>(this.HandleGameEnergyDidChange);
  }

  public virtual void CreateUIForBatteryEnergyType(int batteryLives)
  {
    this._batteryLifeSegments = new List<Image>(batteryLives);
    for (int index = 0; index < batteryLives; ++index)
      this._batteryLifeSegments.Add(UnityEngine.Object.Instantiate<Image>(this._batteryLifeSegmentPrefab));
    Rect rect = this._energyBar.rectTransform.rect;
    float width = rect.width;
    float height = rect.height;
    float x = (float) ((double) width - 2.0 * (double) this._batterySegmentHorizontalPadding - (double) this._batterySegmentSeparatorWidth * (double) (batteryLives - 1)) / (float) batteryLives;
    for (int index = 0; index < batteryLives; ++index)
    {
      RectTransform rectTransform = this._batteryLifeSegments[index].rectTransform;
      rectTransform.SetParent(this.transform, false);
      rectTransform.pivot = new Vector2(0.0f, 0.5f);
      rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
      rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
      rectTransform.sizeDelta = new Vector2(x, height);
      rectTransform.anchoredPosition3D = new Vector3((float) (((double) x + (double) this._batterySegmentSeparatorWidth) * (double) index + (double) this._batterySegmentHorizontalPadding - (double) width * 0.5), -0.5f, 0.0f);
    }
    this._activeBatteryLifeSegmentsCount = batteryLives;
  }

  public virtual void RefreshEnergyUI(float energy)
  {
    if (this._gameEnergyCounter.energyType == GameplayModifiers.EnergyType.Battery)
    {
      int batteryEnergy = this._gameEnergyCounter.batteryEnergy;
      if (batteryEnergy < this._activeBatteryLifeSegmentsCount)
      {
        for (int index = batteryEnergy; index < this._activeBatteryLifeSegmentsCount; ++index)
          this._batteryLifeSegments[index].enabled = false;
        this._activeBatteryLifeSegmentsCount = batteryEnergy;
      }
      else if (batteryEnergy > this._activeBatteryLifeSegmentsCount)
      {
        for (int lifeSegmentsCount = this._activeBatteryLifeSegmentsCount; lifeSegmentsCount < batteryEnergy; ++lifeSegmentsCount)
          this._batteryLifeSegments[lifeSegmentsCount].enabled = true;
        this._activeBatteryLifeSegmentsCount = batteryEnergy;
      }
    }
    else
      this._energyBarRectTransform.anchorMax = new Vector2(energy, 1f);
    if ((double) energy > 9.9999997473787516E-06)
      return;
    this._energyBar.enabled = false;
    this._playableDirector.Play();
    if (this._gameEnergyCounter == null)
      return;
    this._gameEnergyCounter.gameEnergyDidChangeEvent -= new System.Action<float>(this.HandleGameEnergyDidChange);
  }

  public virtual void HandleGameEnergyCounterDidInit() => this.Init();

  public virtual void HandleGameEnergyDidChange(float energy) => this.RefreshEnergyUI(energy);
}
