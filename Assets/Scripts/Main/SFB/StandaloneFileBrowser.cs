// Decompiled with JetBrains decompiler
// Type: SFB.StandaloneFileBrowser
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

namespace SFB
{
  public class StandaloneFileBrowser
  {
    [DoesNotRequireDomainReloadInit]
    protected static readonly IStandaloneFileBrowser _platformWrapper = (IStandaloneFileBrowser) new StandaloneFileBrowserWindows();

    public static string[] OpenFilePanel(
      string title,
      string directory,
      string extension,
      bool multiselect)
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
      return StandaloneFileBrowser.OpenFilePanel(title, directory, extensions, multiselect);
    }

    public static string[] OpenFilePanel(
      string title,
      string directory,
      ExtensionFilter[] extensions,
      bool multiselect)
    {
      return StandaloneFileBrowser._platformWrapper.OpenFilePanel(title, directory, extensions, multiselect);
    }

    public static void OpenFilePanelAsync(
      string title,
      string directory,
      string extension,
      bool multiselect,
      System.Action<string[]> cb)
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
      StandaloneFileBrowser.OpenFilePanelAsync(title, directory, extensions, multiselect, cb);
    }

    public static void OpenFilePanelAsync(
      string title,
      string directory,
      ExtensionFilter[] extensions,
      bool multiselect,
      System.Action<string[]> cb)
    {
      StandaloneFileBrowser._platformWrapper.OpenFilePanelAsync(title, directory, extensions, multiselect, cb);
    }

    public static string[] OpenFolderPanel(string title, string directory, bool multiselect) => StandaloneFileBrowser._platformWrapper.OpenFolderPanel(title, directory, multiselect);

    public static void OpenFolderPanelAsync(
      string title,
      string directory,
      bool multiselect,
      System.Action<string[]> cb)
    {
      StandaloneFileBrowser._platformWrapper.OpenFolderPanelAsync(title, directory, multiselect, cb);
    }

    public static string SaveFilePanel(
      string title,
      string directory,
      string defaultName,
      string extension)
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
      return StandaloneFileBrowser.SaveFilePanel(title, directory, defaultName, extensions);
    }

    public static string SaveFilePanel(
      string title,
      string directory,
      string defaultName,
      ExtensionFilter[] extensions)
    {
      return StandaloneFileBrowser._platformWrapper.SaveFilePanel(title, directory, defaultName, extensions);
    }

    public static void SaveFilePanelAsync(
      string title,
      string directory,
      string defaultName,
      string extension,
      System.Action<string> cb)
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
      StandaloneFileBrowser.SaveFilePanelAsync(title, directory, defaultName, extensions, cb);
    }

    public static void SaveFilePanelAsync(
      string title,
      string directory,
      string defaultName,
      ExtensionFilter[] extensions,
      System.Action<string> cb)
    {
      StandaloneFileBrowser._platformWrapper.SaveFilePanelAsync(title, directory, defaultName, extensions, cb);
    }
  }
}
