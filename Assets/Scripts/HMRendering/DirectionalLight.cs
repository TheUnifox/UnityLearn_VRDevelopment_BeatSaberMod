// Decompiled with JetBrains decompiler
// Type: DirectionalLight
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DirectionalLight : MonoBehaviour
{
  public const int kMaxLights = 5;
  [ColorUsage(false)]
  public Color color;
  public float intensity;
  public float radius = 50f;
  protected static List<DirectionalLight> _lights = new List<DirectionalLight>(10);
  protected static DirectionalLight _mainLight;

  public static List<DirectionalLight> lights => DirectionalLight._lights;

  public virtual void OnEnable() => DirectionalLight._lights.Add(this);

  public virtual void OnDisable() => DirectionalLight._lights.Remove(this);
}
