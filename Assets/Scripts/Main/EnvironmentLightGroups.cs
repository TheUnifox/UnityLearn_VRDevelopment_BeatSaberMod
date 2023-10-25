// Decompiled with JetBrains decompiler
// Type: EnvironmentLightGroups
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnvironmentLightGroups
{
  [Tooltip("Order of these does not matter. Name entries here to something that is easily identifiable.")]
  [SerializeField]
  protected List<LightGroupSO> _lightGroupSOList;
  protected Dictionary<int, LightGroupSO> _lightGroupSODict;
  protected List<LightGroupSO> _lightGroupSOListForLightGroupDataDict;

  public List<LightGroupSO> lightGroupSOList => this._lightGroupSOList;

  public virtual LightGroupSO GetDataForGroup(int groupId)
  {
    if (this._lightGroupSOListForLightGroupDataDict == null || !this._lightGroupSOListForLightGroupDataDict.Equals((object) this._lightGroupSOList))
    {
      this._lightGroupSODict = new Dictionary<int, LightGroupSO>(this._lightGroupSOList.Count);
      foreach (LightGroupSO lightGroupSo in this._lightGroupSOList)
        this._lightGroupSODict[lightGroupSo.groupId] = lightGroupSo;
      this._lightGroupSOListForLightGroupDataDict = this._lightGroupSOList;
    }
    LightGroupSO lightGroupSo1;
    return !this._lightGroupSODict.TryGetValue(groupId, out lightGroupSo1) ? (LightGroupSO) null : lightGroupSo1;
  }

  public virtual void Sort() => this._lightGroupSOList = this._lightGroupSOList.OrderBy<LightGroupSO, int>((Func<LightGroupSO, int>) (s => s.groupId)).ToList<LightGroupSO>();
}
