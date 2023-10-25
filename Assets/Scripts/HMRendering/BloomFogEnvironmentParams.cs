// Decompiled with JetBrains decompiler
// Type: BloomFogEnvironmentParams
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class BloomFogEnvironmentParams : PersistentScriptableObject
{
  public float attenuation = 0.1f;
  public float offset;
  public float heightFogStartY = -300f;
  public float heightFogHeight = 10f;
  [Tooltip("Limits the maximum multiplication of the bloom at low intensities")]
  public float autoExposureLimit = 1000f;
  [Min(0.0f)]
  public float noteSpawnIntensity = 1f;
}
