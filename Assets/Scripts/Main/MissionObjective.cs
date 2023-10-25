// Decompiled with JetBrains decompiler
// Type: MissionObjective
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class MissionObjective
{
  [SerializeField]
  protected MissionObjectiveTypeSO _type;
  [SerializeField]
  protected MissionObjective.ReferenceValueComparisonType _referenceValueComparisonType;
  [SerializeField]
  protected int _referenceValue;

  public MissionObjectiveTypeSO type => this._type;

  public MissionObjective.ReferenceValueComparisonType referenceValueComparisonType => this._referenceValueComparisonType;

  public int referenceValue => this._referenceValue;

  public static bool operator ==(MissionObjective obj1, MissionObjective obj2)
  {
    if ((object) obj1 == (object) obj2)
      return true;
    return (object) obj1 != null && (object) obj2 != null && (UnityEngine.Object) obj1.type == (UnityEngine.Object) obj2.type && obj1.referenceValueComparisonType == obj2.referenceValueComparisonType && obj1._referenceValue == obj2._referenceValue;
  }

  public static bool operator !=(MissionObjective obj1, MissionObjective obj2) => !(obj1 == obj2);

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if ((object) this == obj)
      return true;
    return obj.GetType() == this.GetType() && this == (MissionObjective) obj;
  }

  public override int GetHashCode() => (((object) this._type).GetHashCode() * 397 ^ this._referenceValueComparisonType.GetHashCode()) * 397 ^ this._referenceValue.GetHashCode();

  public enum ReferenceValueComparisonType
  {
    None,
    Equal,
    Max,
    Min,
  }
}
