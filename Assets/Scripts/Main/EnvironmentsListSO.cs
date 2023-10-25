// Decompiled with JetBrains decompiler
// Type: EnvironmentsListSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class EnvironmentsListSO : PersistentScriptableObject
{
  [SerializeField]
  protected EnvironmentInfoSO[] _environmentInfos;

  public EnvironmentInfoSO[] environmentInfos => this._environmentInfos;

  public virtual EnvironmentInfoSO GetEnvironmentInfoBySerializedName(
    string environmentSerializedName)
  {
    foreach (EnvironmentInfoSO environmentInfo in this._environmentInfos)
    {
      if (environmentInfo.serializedName == environmentSerializedName)
        return environmentInfo;
    }
    return (EnvironmentInfoSO) null;
  }

  public virtual List<EnvironmentInfoSO> GetAllEnvironmentInfosWithType(
    EnvironmentTypeSO environmentType)
  {
    List<EnvironmentInfoSO> environmentInfosWithType = new List<EnvironmentInfoSO>();
    foreach (EnvironmentInfoSO environmentInfo in this._environmentInfos)
    {
      if ((Object) environmentInfo.environmentType == (Object) environmentType)
        environmentInfosWithType.Add(environmentInfo);
    }
    return environmentInfosWithType;
  }

  public virtual EnvironmentInfoSO GetFirstEnvironmentInfoWithType(EnvironmentTypeSO environmentType)
  {
    List<EnvironmentInfoSO> environmentInfosWithType = this.GetAllEnvironmentInfosWithType(environmentType);
    return environmentInfosWithType != null && environmentInfosWithType.Count > 0 ? environmentInfosWithType[0] : (EnvironmentInfoSO) null;
  }

  public virtual EnvironmentInfoSO GetLastEnvironmentInfoWithType(EnvironmentTypeSO environmentType)
  {
    List<EnvironmentInfoSO> environmentInfosWithType = this.GetAllEnvironmentInfosWithType(environmentType);
    return environmentInfosWithType != null && environmentInfosWithType.Count > 0 ? environmentInfosWithType[environmentInfosWithType.Count - 1] : (EnvironmentInfoSO) null;
  }
}
