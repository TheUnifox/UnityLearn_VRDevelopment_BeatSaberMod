// Decompiled with JetBrains decompiler
// Type: MissionObjectiveTypeSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using UnityEngine;

public class MissionObjectiveTypeSO : PersistentScriptableObject
{
  [SerializeField]
  [LocalizationKey]
  protected string _objectiveName;
  [SerializeField]
  protected bool _noConditionValue;
  [SerializeField]
  protected ObjectiveValueFormatterSO _objectiveValueFormater;

  public string objectiveName => this._objectiveName;

  public string objectiveNameLocalized => Localization.Get(this._objectiveName);

  public bool noConditionValue => this._noConditionValue;

  public ObjectiveValueFormatterSO objectiveValueFormater => this._objectiveValueFormater;
}
