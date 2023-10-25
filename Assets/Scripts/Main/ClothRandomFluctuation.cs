// Decompiled with JetBrains decompiler
// Type: ClothRandomFluctuation
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ClothRandomFluctuation : MonoBehaviour
{
  [SerializeField]
  protected Cloth _cloth;
  [Header("External Fluctuations")]
  [SerializeField]
  protected bool _useLocalExternalFluctuations;
  [SerializeField]
  protected Vector3 _externalFluctuations;
  [Header("Random Fluctuations")]
  [SerializeField]
  protected bool _useLocalRandomFluctuations;
  [SerializeField]
  protected Vector3 _minFluctuations;
  [SerializeField]
  protected Vector3 _maxFluctuations;
  [SerializeField]
  protected List<ClothRandomFluctuation.SineLayer> _compoundSins;
  [SerializeField]
  protected float _speed;

  public virtual void Update() => this.FluctuateCloth(this._cloth);

  public virtual void FluctuateCloth(Cloth cloth)
  {
    Vector3 direction = new Vector3(Mathf.Lerp(this._minFluctuations.x, this._maxFluctuations.x, this.GetNoise(Time.time)), Mathf.Lerp(this._minFluctuations.y, this._maxFluctuations.y, this.GetNoise(Time.time, 30f)), Mathf.Lerp(this._minFluctuations.z, this._maxFluctuations.z, this.GetNoise(Time.time, 0.5f)));
    if (this._useLocalRandomFluctuations)
      direction = cloth.transform.TransformDirection(direction);
    Vector3 vector3 = this._externalFluctuations;
    if (this._useLocalExternalFluctuations)
      vector3 = cloth.transform.TransformDirection(this._externalFluctuations);
    cloth.externalAcceleration = direction + vector3;
  }

  public virtual float GetNoise(float time, float offset = 0.0f)
  {
    float num1 = 0.0f;
    for (int index = 0; index < this._compoundSins.Count; ++index)
    {
      float num2 = Mathf.Sin((this._compoundSins[index].multiplier * time + this._compoundSins[index].offset) * this._speed + offset);
      num1 += num2;
    }
    return (double) num1 == 0.0 ? num1 : (float) (((double) num1 / (double) this._compoundSins.Count + 1.0) / 2.0);
  }

  [Serializable]
  public class SineLayer
  {
    public float multiplier;
    public float offset;
  }
}
