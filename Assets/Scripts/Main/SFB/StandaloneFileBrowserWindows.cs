// Decompiled with JetBrains decompiler
// Type: SFB.StandaloneFileBrowserWindows
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Ookii.Dialogs;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SFB
{
  public class StandaloneFileBrowserWindows : IStandaloneFileBrowser
  {
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    public virtual string[] OpenFilePanel(
      string title,
      string directory,
      ExtensionFilter[] extensions,
      bool multiselect)
    {
      VistaOpenFileDialog vistaOpenFileDialog = new VistaOpenFileDialog();
      vistaOpenFileDialog.Title = title;
      if (extensions != null)
      {
        vistaOpenFileDialog.Filter = StandaloneFileBrowserWindows.GetFilterFromFileExtensionList(extensions);
        vistaOpenFileDialog.FilterIndex = 1;
      }
      else
        vistaOpenFileDialog.Filter = string.Empty;
      vistaOpenFileDialog.Multiselect = multiselect;
      if (!string.IsNullOrEmpty(directory))
        vistaOpenFileDialog.FileName = StandaloneFileBrowserWindows.GetDirectoryPath(directory);
      string[] strArray = vistaOpenFileDialog.ShowDialog((IWin32Window) new WindowWrapper(StandaloneFileBrowserWindows.GetActiveWindow())) == DialogResult.OK ? vistaOpenFileDialog.FileNames : new string[0];
      ((Component) vistaOpenFileDialog).Dispose();
      return strArray;
    }

    public virtual void OpenFilePanelAsync(
      string title,
      string directory,
      ExtensionFilter[] extensions,
      bool multiselect,
      Action<string[]> cb)
    {
      cb(this.OpenFilePanel(title, directory, extensions, multiselect));
    }

    public virtual string[] OpenFolderPanel(string title, string directory, bool multiselect)
    {
      VistaFolderBrowserDialog folderBrowserDialog = new VistaFolderBrowserDialog();
      folderBrowserDialog.Description = title;
      if (!string.IsNullOrEmpty(directory))
        folderBrowserDialog.SelectedPath = StandaloneFileBrowserWindows.GetDirectoryPath(directory);
      string[] strArray;
      if (folderBrowserDialog.ShowDialog((IWin32Window) new WindowWrapper(StandaloneFileBrowserWindows.GetActiveWindow())) != DialogResult.OK)
        strArray = new string[0];
      else
        strArray = new string[1]
        {
          folderBrowserDialog.SelectedPath
        };
      ((Component) folderBrowserDialog).Dispose();
      return strArray;
    }

    public virtual void OpenFolderPanelAsync(
      string title,
      string directory,
      bool multiselect,
      Action<string[]> cb)
    {
      cb(this.OpenFolderPanel(title, directory, multiselect));
    }

    public virtual string SaveFilePanel(
      string title,
      string directory,
      string defaultName,
      ExtensionFilter[] extensions)
    {
      VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog();
      vistaSaveFileDialog.Title = title;
      string str = "";
      if (!string.IsNullOrEmpty(directory))
        str = StandaloneFileBrowserWindows.GetDirectoryPath(directory);
      if (!string.IsNullOrEmpty(defaultName))
        str += defaultName;
      vistaSaveFileDialog.FileName = str;
      if (extensions != null)
      {
        vistaSaveFileDialog.Filter = StandaloneFileBrowserWindows.GetFilterFromFileExtensionList(extensions);
        vistaSaveFileDialog.FilterIndex = 1;
        vistaSaveFileDialog.DefaultExt = extensions[0]._extensions[0];
        vistaSaveFileDialog.AddExtension = true;
      }
      else
      {
        vistaSaveFileDialog.DefaultExt = string.Empty;
        vistaSaveFileDialog.Filter = string.Empty;
        vistaSaveFileDialog.AddExtension = false;
      }
      string fileName = vistaSaveFileDialog.ShowDialog((IWin32Window) new WindowWrapper(StandaloneFileBrowserWindows.GetActiveWindow())) == DialogResult.OK ? vistaSaveFileDialog.FileName : "";
      ((Component) vistaSaveFileDialog).Dispose();
      return fileName;
    }

    public virtual void SaveFilePanelAsync(
      string title,
      string directory,
      string defaultName,
      ExtensionFilter[] extensions,
      Action<string> cb)
    {
      cb(this.SaveFilePanel(title, directory, defaultName, extensions));
    }

    private static string GetFilterFromFileExtensionList(ExtensionFilter[] extensions)
    {
      string str1 = "";
      foreach (ExtensionFilter extension1 in extensions)
      {
        string str2 = str1 + extension1._name + "(";
        foreach (string extension2 in extension1._extensions)
          str2 = str2 + "*." + extension2 + ",";
        string str3 = str2.Remove(str2.Length - 1) + ") |";
        foreach (string extension3 in extension1._extensions)
          str3 = str3 + "*." + extension3 + "; ";
        str1 = str3 + "|";
      }
      return str1.Remove(str1.Length - 1);
    }

    private static string GetDirectoryPath(string directory)
    {
      string fullPath = Path.GetFullPath(directory);
      if (!fullPath.EndsWith("\\"))
        fullPath += "\\";
      return Path.GetDirectoryName(fullPath) + Path.DirectorySeparatorChar.ToString();
    }
  }
}
