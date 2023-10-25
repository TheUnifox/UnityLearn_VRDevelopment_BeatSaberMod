// Decompiled with JetBrains decompiler
// Type: DisableGCWhileEnabled
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.Scripting;

public class DisableGCWhileEnabled : MonoBehaviour
{
  public virtual void OnEnable() => GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;

  public virtual void OnDisable() => GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
}
