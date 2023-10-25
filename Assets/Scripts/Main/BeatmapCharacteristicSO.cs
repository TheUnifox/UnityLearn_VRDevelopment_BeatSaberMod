// Decompiled with JetBrains decompiler
// Type: BeatmapCharacteristicSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BeatmapCharacteristicSO : PersistentScriptableObject
{
  [SerializeField]
  protected Sprite _icon;
  [SerializeField]
  [LocalizationKey]
  protected string _descriptionLocalizationKey;
  [SerializeField]
  [LocalizationKey]
  protected string _characteristicNameLocalizationKey;
  [SerializeField]
  protected string _serializedName;
  [SerializeField]
  protected string _compoundIdPartName;
  [SerializeField]
  protected int _sortingOrder;
  [SerializeField]
  protected bool _containsRotationEvents;
  [SerializeField]
  protected bool _requires360Movement;
  [SerializeField]
  protected int _numberOfColors = 2;

  public Sprite icon => this._icon;

  public string descriptionLocalizationKey => this._descriptionLocalizationKey;

  public string characteristicNameLocalizationKey => this._characteristicNameLocalizationKey;

  public string serializedName => this._serializedName;

  public string compoundIdPartName => this._compoundIdPartName;

  public int sortingOrder => this._sortingOrder;

  public bool containsRotationEvents => this._containsRotationEvents;

  public bool requires360Movement => this._requires360Movement;

  public int numberOfColors => this._numberOfColors;
}
