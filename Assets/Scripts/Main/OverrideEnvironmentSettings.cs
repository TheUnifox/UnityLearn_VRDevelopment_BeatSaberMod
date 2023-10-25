// Decompiled with JetBrains decompiler
// Type: OverrideEnvironmentSettings
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class OverrideEnvironmentSettings
{
  public bool overrideEnvironments;
  protected readonly Dictionary<EnvironmentTypeSO, EnvironmentInfoSO> _data = new Dictionary<EnvironmentTypeSO, EnvironmentInfoSO>();

  public virtual void SetEnvironmentInfoForType(
    EnvironmentTypeSO environmentType,
    EnvironmentInfoSO environmentInfo)
  {
    this._data[environmentType] = environmentInfo;
  }

  public virtual EnvironmentInfoSO GetOverrideEnvironmentInfoForType(
    EnvironmentTypeSO environmentType)
  {
    EnvironmentInfoSO environmentInfoSo;
    return this._data.TryGetValue(environmentType, out environmentInfoSo) ? environmentInfoSo : (EnvironmentInfoSO) null;
  }
}
