// Decompiled with JetBrains decompiler
// Type: EssentialHelpers
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using UnityEngine;

public abstract class EssentialHelpers
{
  public static double CurrentTimeStamp => DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

  public static void SafeDestroy(UnityEngine.Object obj)
  {
    if (obj == (UnityEngine.Object) null)
      return;
    if (Application.isPlaying)
      UnityEngine.Object.Destroy(obj);
    else
      UnityEngine.Object.DestroyImmediate(obj);
    obj = (UnityEngine.Object) null;
  }

  public static T GetOrAddComponent<T>(GameObject go) where T : Component
  {
    T orAddComponent = go.GetComponent<T>();
    if (!(bool) (UnityEngine.Object) orAddComponent)
      orAddComponent = go.AddComponent<T>();
    return orAddComponent;
  }
}
