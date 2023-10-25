// Decompiled with JetBrains decompiler
// Type: FileSystemHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

public abstract class FileSystemHelper
{
  public static string FindFirstExistedParentPath(string path)
  {
    while (!string.IsNullOrWhiteSpace(path) && !Directory.Exists(path))
      path = Path.GetDirectoryName(path);
    return path;
  }

  public static bool IsFileWritable(string path) => !new FileInfo(path).Attributes.HasFlag((Enum) FileAttributes.ReadOnly);
}
