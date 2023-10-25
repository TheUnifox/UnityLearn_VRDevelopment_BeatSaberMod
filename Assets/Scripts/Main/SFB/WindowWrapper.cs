// Decompiled with JetBrains decompiler
// Type: SFB.WindowWrapper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Windows.Forms;

namespace SFB
{
  public class WindowWrapper : IWin32Window
  {
    protected IntPtr _hwnd;

    public WindowWrapper(IntPtr handle) => this._hwnd = handle;

    public IntPtr Handle => this._hwnd;
  }
}
