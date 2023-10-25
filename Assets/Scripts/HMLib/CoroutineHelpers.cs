// Decompiled with JetBrains decompiler
// Type: CoroutineHelpers
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections;
using UnityEngine;

public class CoroutineHelpers
{
  public static IEnumerator ExecuteAfterDelayCoroutine(System.Action action, float time)
  {
    yield return (object) new WaitForSeconds(time);
    System.Action action1 = action;
    if (action1 != null)
      action1();
  }
}
