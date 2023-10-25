// Decompiled with JetBrains decompiler
// Type: FileBrowserModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.IO;

public abstract class FileBrowserModel
{
  public static void GetContentOfDirectory(
    string direcotryPath,
    string[] extensions,
    System.Action<FileBrowserItem[]> callback)
  {
    FileBrowserItem[] items = (FileBrowserItem[]) null;
    new HMTask((System.Action) (() => items = FileBrowserModel.GetContentOfDirectory(direcotryPath, extensions)), (System.Action) (() => callback(items))).Run();
  }

  private static FileBrowserItem[] GetContentOfDirectory(string directoryPath, string[] extensions)
  {
    List<FileBrowserItem> fileBrowserItemList = new List<FileBrowserItem>();
    string path = directoryPath + "\\..";
    if (Path.GetFullPath(path) != Path.GetFullPath(directoryPath))
      fileBrowserItemList.Add(new FileBrowserItem("..", Path.GetFullPath(path), true));
    if (!FileBrowserModel.CanOpenDirectory(directoryPath))
      return fileBrowserItemList.ToArray();
    directoryPath = Path.GetFullPath(directoryPath);
    foreach (string directory in Directory.GetDirectories(directoryPath))
      fileBrowserItemList.Add(new FileBrowserItem(Path.GetFileName(directory), Path.GetFullPath(directory), true));
    foreach (string file in Directory.GetFiles(directoryPath))
    {
      foreach (string extension in extensions)
      {
        if (file.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
          fileBrowserItemList.Add(new FileBrowserItem(Path.GetFileName(file), Path.GetFullPath(file), false));
      }
    }
    return fileBrowserItemList.ToArray();
  }

  private static bool CanOpenDirectory(string path)
  {
    try
    {
      Directory.GetDirectories(path);
      return true;
    }
    catch
    {
      return false;
    }
  }
}
