// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.NativeFileDialogs
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using SFB;
using System.IO;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class NativeFileDialogs
  {
    private const string kLastUsedDirectoryPathKey = "NativeFileDialogs.LastUsedFilePath";

    private static string lastUsedDirectory => PlayerPrefs.GetString("NativeFileDialogs.LastUsedFilePath", (string) null);

    private static void SetLastUsedDirectory(string path)
    {
      string str = (string) null;
      if (str != null)
      {
        try
        {
          str = Path.GetDirectoryName(path);
        }
        catch
        {
        }
      }
      PlayerPrefs.SetString("NativeFileDialogs.LastUsedFilePath", str);
    }

    public static string SaveFileDialog(
      string title,
      string defaultName,
      string extension,
      string initialDirectory)
    {
      if (initialDirectory == null)
        initialDirectory = NativeFileDialogs.lastUsedDirectory;
      string path = StandaloneFileBrowser.SaveFilePanel(title, initialDirectory, defaultName, extension);
      if (path == null || path == "")
        return (string) null;
      NativeFileDialogs.SetLastUsedDirectory(path);
      return path;
    }

    public static string[] OpenFileDialogMultiselect(
      string title,
      string extension,
      string initialDirectory)
    {
      if (initialDirectory == null)
        initialDirectory = NativeFileDialogs.lastUsedDirectory;
      string[] strArray = StandaloneFileBrowser.OpenFilePanel(title, initialDirectory, extension, true);
      if (strArray == null || strArray.Length == 0)
        return (string[]) null;
      NativeFileDialogs.SetLastUsedDirectory(strArray[0]);
      return strArray;
    }

    public static string OpenFileDialog(string title, string extension, string initialDirectory)
    {
      ExtensionFilter[] extensionFilterArray;
      if (!string.IsNullOrEmpty(extension))
        extensionFilterArray = new ExtensionFilter[1]
        {
          new ExtensionFilter("", new string[1]{ extension })
        };
      else
        extensionFilterArray = (ExtensionFilter[]) null;
      ExtensionFilter[] extensions = extensionFilterArray;
      return NativeFileDialogs.OpenFileDialog(title, extensions, initialDirectory);
    }

    public static string OpenFileDialog(
      string title,
      ExtensionFilter[] extensions,
      string initialDirectory)
    {
      if (initialDirectory == null)
        initialDirectory = NativeFileDialogs.lastUsedDirectory;
      string[] strArray = StandaloneFileBrowser.OpenFilePanel(title, initialDirectory, extensions, false);
      if (strArray == null || strArray.Length == 0)
        return (string) null;
      NativeFileDialogs.SetLastUsedDirectory(strArray[0]);
      return strArray[0];
    }

    public static string OpenDirectoryDialog(string title, string initialDirectory)
    {
      if (initialDirectory == null)
        initialDirectory = NativeFileDialogs.lastUsedDirectory;
      string[] strArray = StandaloneFileBrowser.OpenFolderPanel(title, initialDirectory, false);
      if (strArray == null || strArray.Length == 0)
        return (string) null;
      NativeFileDialogs.SetLastUsedDirectory(strArray[0]);
      return strArray[0];
    }
  }
}
