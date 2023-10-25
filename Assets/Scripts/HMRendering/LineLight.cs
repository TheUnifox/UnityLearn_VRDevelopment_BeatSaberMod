// Decompiled with JetBrains decompiler
// Type: LineLight
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineLight : MonoBehaviour
{
  [SerializeField]
  protected Vector3 _p0;
  [SerializeField]
  protected Vector3 _p1;
  [SerializeField]
  protected Color _color;
  protected static List<LineLight> _lineLights = new List<LineLight>(16);

  public Vector3 p0 => this._p0;

  public Vector3 p1 => this._p1;

  public Color color => this._color;

  public static List<LineLight> lineLights => LineLight._lineLights;

  public virtual void OnEnable() => LineLight._lineLights.Add(this);

  public virtual void OnDisable() => LineLight._lineLights.Remove(this);

  public virtual void OnDrawGizmos() => Gizmos.DrawLine(this.transform.TransformPoint(this._p0), this.transform.TransformPoint(this._p1));
}
