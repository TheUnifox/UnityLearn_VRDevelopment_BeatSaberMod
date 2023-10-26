// Decompiled with JetBrains decompiler
// Type: HMUI.EventSystemHelpers
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HMUI
{
  public abstract class EventSystemHelpers
  {
    public static bool IsInputFieldSelected()
    {
      GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
      return !((Object) selectedGameObject == (Object) null) && !((Object) selectedGameObject.GetComponent<InputField>() == (Object) null);
    }
  }
}
