// Decompiled with JetBrains decompiler
// Type: AudioHelpers
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public abstract class AudioHelpers
{
  public static float NormalizedVolumeToDB(float normalizedVolume) => Mathf.Max(-100f, Mathf.Log(normalizedVolume, 1.1f));

  public static float DBToNormalizedVolume(float db) => Mathf.Pow(1.1f, db);
}
