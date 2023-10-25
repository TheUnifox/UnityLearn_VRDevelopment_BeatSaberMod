// Decompiled with JetBrains decompiler
// Type: DroneHover
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneHover : MonoBehaviour
{
  [SerializeField]
  protected Vector3 _hoverAreaPerAxis;
  [SerializeField]
  protected float _speed = 1f;
  [SerializeField]
  protected List<DroneHover.SineLayer> _compoundSins;
  [Header("Tilt Settings")]
  [SerializeField]
  protected List<Transform> _tiltTransforms;
  [SerializeField]
  protected float _maxTiltAmount;
  [SerializeField]
  protected float _tiltSpeed;
  [SerializeField]
  protected float _tiltAheadOfTime = 1f;
  protected bool _tiltToTarget = true;
  protected Vector3 _startPos;
  protected Cloth _cloth;

  public virtual void Start() => this._startPos = this.transform.position;

  public virtual void Update()
  {
    this.GetNoiseVec3(Time.time);
    this.transform.SetPositionAndRotation(this._startPos + new Vector3(0.0f, 0.0f, Mathf.Sin(Time.timeSinceLevelLoad)), Quaternion.Euler(0.0f, Mathf.Sin(Time.timeSinceLevelLoad) * 5f, 0.0f));
    if (this._tiltTransforms == null)
      return;
    this.UpdateTiltTransform();
  }

  public virtual Vector3 GetNoiseVec3(float time)
  {
    double x = (double) this._hoverAreaPerAxis.x * (double) this.GetNoise(time);
    float num1 = this._hoverAreaPerAxis.y * this.GetNoise(time, 9090.123f);
    float num2 = this._hoverAreaPerAxis.z * this.GetNoise(time, 3234.14f);
    double y = (double) num1;
    double z = (double) num2;
    return new Vector3((float) x, (float) y, (float) z);
  }

  public virtual float GetNoise(float time, float offset = 0.0f)
  {
    float num1 = 0.0f;
    for (int index = 0; index < this._compoundSins.Count; ++index)
    {
      float num2 = Mathf.Sin((this._compoundSins[index].multiplier * time + this._compoundSins[index].offset) * this._speed + offset);
      num1 += num2;
    }
    return (double) num1 == 0.0 ? num1 : num1 / (float) this._compoundSins.Count;
  }

  public virtual void UpdateTiltTransform()
  {
    Vector3 vector3_1 = Vector3.zero;
    if (this._tiltToTarget)
    {
      Vector3 vector3_2 = this.transform.InverseTransformPoint(this.GetNoiseVec3(Time.time + this._tiltAheadOfTime) + this._startPos);
      vector3_1 = this._maxTiltAmount * new Vector3(-vector3_2.y, vector3_2.x, 0.0f);
    }
    for (int index = 0; index < this._tiltTransforms.Count; ++index)
    {
      Transform tiltTransform = this._tiltTransforms[index];
      tiltTransform.transform.localRotation = Quaternion.RotateTowards(tiltTransform.transform.localRotation, Quaternion.Euler(vector3_1.x, vector3_1.y, vector3_1.z), Time.deltaTime * this._tiltSpeed);
    }
  }

  [Serializable]
  public class SineLayer
  {
    public float multiplier;
    public float offset;
  }
}
