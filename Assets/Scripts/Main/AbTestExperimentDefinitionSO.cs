// Decompiled with JetBrains decompiler
// Type: AbTestExperimentDefinitionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AbTestExperimentDefinitionSO : PersistentScriptableObject
{
  [SerializeField]
  protected string _experimentName;
  [SerializeField]
  protected float _test1GroupSize = 0.25f;
  [SerializeField]
  protected float _test2GroupSize = 0.25f;
  [SerializeField]
  protected float _controlGroupSize = 0.5f;
  [SerializeField]
  protected string _salt;
  protected AbTestExperimentDefinitionSO.Group _currentUserTreatmentGroup;

  public float test1GroupSize => this._test1GroupSize;

  public float test2GroupSize => this._test2GroupSize;

  public string experimentName => this._experimentName;

  public AbTestExperimentDefinitionSO.Group currentUserTreatmentGroup => this._currentUserTreatmentGroup;

  public virtual void OnValidate()
  {
    this._test1GroupSize = Mathf.Min(1f, this._test1GroupSize);
    this._test2GroupSize = Mathf.Min(this._test2GroupSize, 1f - this._test1GroupSize);
    this._test2GroupSize = Mathf.Min(1f, this._test2GroupSize);
    this._controlGroupSize = 1f - this._test1GroupSize - this._test2GroupSize;
  }

  public virtual void ComputeCurrentUserTreatment(string userId) => this._currentUserTreatmentGroup = this.AbSplit(userId);

  public virtual void ForceSetTreatmentGroup(AbTestExperimentDefinitionSO.Group group) => this._currentUserTreatmentGroup = group;

  public virtual AbTestExperimentDefinitionSO.Group AbSplit(string userId)
  {
    using (SHA256 shA256 = SHA256.Create())
    {
      float num = (float) Math.Abs(BitConverter.ToInt32(shA256.ComputeHash(Encoding.UTF8.GetBytes(userId + this._experimentName + this._salt)), 0)) / (float) int.MaxValue;
      return (double) num < (double) this._test1GroupSize ? AbTestExperimentDefinitionSO.Group.Test1 : ((double) num < (double) this._test1GroupSize + (double) this._test2GroupSize ? AbTestExperimentDefinitionSO.Group.Test2 : AbTestExperimentDefinitionSO.Group.Control);
    }
  }

  public enum Group
  {
    Control,
    Test1,
    Test2,
  }
}
