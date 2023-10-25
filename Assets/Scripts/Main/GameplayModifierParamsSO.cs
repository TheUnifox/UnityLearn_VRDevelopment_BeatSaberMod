// Decompiled with JetBrains decompiler
// Type: GameplayModifierParamsSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class GameplayModifierParamsSO : PersistentScriptableObject
{
  [SerializeField]
  [LocalizationKey]
  protected string _modifierNameLocalizationKey;
  [SerializeField]
  [LocalizationKey]
  protected string _descriptionLocalizationKey;
  [SerializeField]
  protected float _multiplier;
  [SerializeField]
  protected bool _multiplierConditionallyValid;
  [SerializeField]
  protected Sprite _icon;
  [SerializeField]
  protected GameplayModifierParamsSO[] _mutuallyExclusives;
  [SerializeField]
  protected GameplayModifierParamsSO[] _requires;
  [SerializeField]
  protected GameplayModifierParamsSO[] _requiredBy;
  [SerializeField]
  protected bool _isInBeta;

  public string modifierNameLocalizationKey => this._modifierNameLocalizationKey;

  public string descriptionLocalizationKey => this._descriptionLocalizationKey;

  public float multiplier => this._multiplier;

  public bool multiplierConditionallyValid => this._multiplierConditionallyValid;

  public Sprite icon => this._icon;

  public GameplayModifierParamsSO[] mutuallyExclusives => this._mutuallyExclusives;

  public GameplayModifierParamsSO[] requires => this._requires;

  public GameplayModifierParamsSO[] requiredBy => this._requiredBy;

  public bool isInBeta => this._isInBeta;
}
