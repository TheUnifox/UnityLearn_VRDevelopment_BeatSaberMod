// Decompiled with JetBrains decompiler
// Type: BloomPrePassLightsUpdateSystem
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using UnityEngine;

public class BloomPrePassLightsUpdateSystem : MonoBehaviour
{
  public virtual void LateUpdate()
  {
    foreach (KeyValuePair<BloomPrePassLightTypeSO, HashSet<BloomPrePassLight>> keyValuePair in BloomPrePassLight.bloomLightsDict)
    {
      foreach (BloomPrePassLight bloomPrePassLight in keyValuePair.Value)
      {
        if (bloomPrePassLight.isDirty)
          bloomPrePassLight.Refresh();
      }
    }
  }
}
