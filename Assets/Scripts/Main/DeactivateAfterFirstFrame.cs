// Decompiled with JetBrains decompiler
// Type: DeactivateAfterFirstFrame
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;

public class DeactivateAfterFirstFrame : MonoBehaviour
{
  public virtual IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.m_Cm_E1__state;
    DeactivateAfterFirstFrame deactivateAfterFirstFrame = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Cm_E1__state = -1;
      deactivateAfterFirstFrame.gameObject.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.m_Cm_E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.m_Cm_E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.m_Cm_E1__state = 1;
    return true;
  }
}
