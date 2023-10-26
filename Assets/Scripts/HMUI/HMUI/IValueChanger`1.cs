// Decompiled with JetBrains decompiler
// Type: HMUI.IValueChanger`1
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;

namespace HMUI
{
  public interface IValueChanger<T>
  {
    event Action<T> valueChangedEvent;
  }
}
