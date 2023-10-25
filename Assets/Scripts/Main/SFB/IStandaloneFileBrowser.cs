// Decompiled with JetBrains decompiler
// Type: SFB.IStandaloneFileBrowser
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;

namespace SFB
{
  public interface IStandaloneFileBrowser
  {
    string[] OpenFilePanel(
      string title,
      string directory,
      ExtensionFilter[] extensions,
      bool multiselect);

    string[] OpenFolderPanel(string title, string directory, bool multiselect);

    string SaveFilePanel(
      string title,
      string directory,
      string defaultName,
      ExtensionFilter[] extensions);

    void OpenFilePanelAsync(
      string title,
      string directory,
      ExtensionFilter[] extensions,
      bool multiselect,
      Action<string[]> cb);

    void OpenFolderPanelAsync(
      string title,
      string directory,
      bool multiselect,
      Action<string[]> cb);

    void SaveFilePanelAsync(
      string title,
      string directory,
      string defaultName,
      ExtensionFilter[] extensions,
      Action<string> cb);
  }
}
