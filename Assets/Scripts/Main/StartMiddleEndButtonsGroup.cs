// Decompiled with JetBrains decompiler
// Type: StartMiddleEndButtonsGroup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class StartMiddleEndButtonsGroup : MonoBehaviour, ILayoutController
{
  public virtual void SetLayoutHorizontal()
  {
    StartMiddleEndButtonBackgroundController[] componentsInChildren = this.transform.GetComponentsInChildren<StartMiddleEndButtonBackgroundController>(false);
    if (componentsInChildren.Length == 1)
    {
      componentsInChildren[0].SetMiddleSprite();
    }
    else
    {
      for (int index = 0; index < componentsInChildren.Length; ++index)
      {
        if (index == 0)
          componentsInChildren[index].SetStartSprite();
        else if (index == componentsInChildren.Length - 1)
          componentsInChildren[index].SetEndSprite();
        else
          componentsInChildren[index].SetMiddleSprite();
      }
    }
  }

  public virtual void SetLayoutVertical()
  {
  }
}
