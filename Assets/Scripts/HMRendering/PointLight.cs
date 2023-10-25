// Decompiled with JetBrains decompiler
// Type: PointLight
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PointLight : MonoBehaviour
{
  public const int kMaxLights = 1;
  public Color color;
  public float intensity;
  protected static List<PointLight> _lights = new List<PointLight>(2);

  public static List<PointLight> lights => PointLight._lights;

  public virtual void OnEnable() => PointLight._lights.Add(this);

  public virtual void OnDisable() => PointLight._lights.Remove(this);
}
