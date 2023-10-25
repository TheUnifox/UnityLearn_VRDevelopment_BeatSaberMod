// Decompiled with JetBrains decompiler
// Type: GameplayModifiersPanelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayModifiersPanelController : MonoBehaviour, IRefreshable
{
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModel;
  [SerializeField]
  protected TextMeshProUGUI _totalMultiplierValueText;
  [SerializeField]
  protected TextMeshProUGUI _maxRankValueText;
  [Space]
  [SerializeField]
  protected Color _positiveColor;
  [SerializeField]
  protected Color _negativeColor;
  protected GameplayModifiers _gameplayModifiers;
  protected ToggleBinder _toggleBinder;
  protected GameplayModifierToggle[] _gameplayModifierToggles;
  protected bool _changingGameplayModifierToggles;
  protected readonly Dictionary<GameplayModifierParamsSO, Toggle> _toggleForGameplayModifierParam = new Dictionary<GameplayModifierParamsSO, Toggle>(20);

  public event System.Action didChangeGameplayModifiersEvent;

  public GameplayModifiers gameplayModifiers => this._gameplayModifiers;

  public virtual void SetData(GameplayModifiers newGameplayModifiers) => this._gameplayModifiers = newGameplayModifiers;

  public virtual void Awake()
  {
    this._toggleBinder = new ToggleBinder();
    this._gameplayModifierToggles = this.GetComponentsInChildren<GameplayModifierToggle>();
    foreach (GameplayModifierToggle gameplayModifierToggle1 in this._gameplayModifierToggles)
    {
      GameplayModifierToggle gameplayModifierToggle = gameplayModifierToggle1;
      this._toggleBinder.AddBinding(gameplayModifierToggle.toggle, (System.Action<bool>) (on =>
      {
        if (this._changingGameplayModifierToggles)
          return;
        this._changingGameplayModifierToggles = true;
        if (on)
        {
          foreach (GameplayModifierParamsSO mutuallyExclusive in gameplayModifierToggle.gameplayModifier.mutuallyExclusives)
            this.SetToggleValueWithGameplayModifierParams(mutuallyExclusive, false);
          foreach (GameplayModifierParamsSO require in gameplayModifierToggle.gameplayModifier.requires)
            this.SetToggleValueWithGameplayModifierParams(require, true);
        }
        else
        {
          foreach (GameplayModifierParamsSO gameplayModifierParams in gameplayModifierToggle.gameplayModifier.requiredBy)
            this.SetToggleValueWithGameplayModifierParams(gameplayModifierParams, false);
        }
        this._gameplayModifiers = this._gameplayModifiersModel.CreateGameplayModifiers(new Func<GameplayModifierParamsSO, bool>(this.GetToggleValueWithGameplayModifierParams));
        this.RefreshTotalMultiplierAndRankUI();
        System.Action gameplayModifiersEvent = this.didChangeGameplayModifiersEvent;
        if (gameplayModifiersEvent != null)
          gameplayModifiersEvent();
        this._changingGameplayModifierToggles = false;
      }));
    }
  }

  public virtual void OnDestroy() => this._toggleBinder?.ClearBindings();

  public virtual void SetToggleValueWithGameplayModifierParams(
    GameplayModifierParamsSO gameplayModifierParams,
    bool value)
  {
    Toggle toggle;
    if (!this._toggleForGameplayModifierParam.TryGetValue(gameplayModifierParams, out toggle))
      return;
    toggle.isOn = value;
  }

  public virtual bool GetToggleValueWithGameplayModifierParams(
    GameplayModifierParamsSO gameplayModifierParams)
  {
    Toggle toggle;
    return this._toggleForGameplayModifierParam.TryGetValue(gameplayModifierParams, out toggle) && toggle.isOn;
  }

  public virtual void RefreshTotalMultiplierAndRankUI()
  {
    float totalMultiplier = this._gameplayModifiersModel.GetTotalMultiplier(this._gameplayModifiersModel.CreateModifierParamsList(this._gameplayModifiers), 1f);
    Color color = (double) totalMultiplier >= 1.0 ? this._positiveColor : this._negativeColor;
    this._totalMultiplierValueText.text = string.Format((IFormatProvider) Localization.Instance.SelectedCultureInfo, "{0:P0}", (object) totalMultiplier);
    this._totalMultiplierValueText.color = color;
    this._maxRankValueText.text = RankModel.GetRankName(RankModelHelper.MaxRankForGameplayModifiers(this.gameplayModifiers, this._gameplayModifiersModel, 1f));
    this._maxRankValueText.color = color;
  }

  void IRefreshable.Refresh()
  {
    this._changingGameplayModifierToggles = true;
    foreach (GameplayModifierToggle gameplayModifierToggle in this._gameplayModifierToggles)
    {
      gameplayModifierToggle.toggle.isOn = this._gameplayModifiersModel.GetModifierBoolValue(this._gameplayModifiers, gameplayModifierToggle.gameplayModifier);
      this._toggleForGameplayModifierParam[gameplayModifierToggle.gameplayModifier] = gameplayModifierToggle.toggle;
    }
    this._changingGameplayModifierToggles = false;
    System.Action gameplayModifiersEvent = this.didChangeGameplayModifiersEvent;
    if (gameplayModifiersEvent != null)
      gameplayModifiersEvent();
    this.RefreshTotalMultiplierAndRankUI();
  }
}
