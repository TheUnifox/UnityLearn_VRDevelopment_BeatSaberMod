// Decompiled with JetBrains decompiler
// Type: TrailElement
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class TrailElement
{
  public Vector3 position;
  public Vector3 normal;
  public float distance;
  public float localDistance;
  public float time;

  public virtual void SetData(Vector3 start, Vector3 end, float time)
  {
    this.position = (start + end) * 0.5f;
    this.normal = end - start;
    this.distance = 0.0f;
    this.localDistance = 0.0f;
    this.time = time;
  }

  public virtual void CopyFrom(TrailElement other)
  {
    this.position = other.position;
    this.normal = other.normal;
    this.time = other.time;
    this.distance = 0.0f;
    this.localDistance = 0.0f;
  }

  public virtual void SetDistance(float value) => this.distance = value;

  public virtual void UpdateLocalDistance(TrailElement prev) => this.localDistance = (this.position - prev.position).magnitude;
}
