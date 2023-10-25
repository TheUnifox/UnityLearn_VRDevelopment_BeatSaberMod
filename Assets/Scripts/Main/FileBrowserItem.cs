// Decompiled with JetBrains decompiler
// Type: FileBrowserItem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class FileBrowserItem
{
  [CompilerGenerated]
  protected string m_CdisplayName;
  [CompilerGenerated]
  protected string m_CfullPath;
  [CompilerGenerated]
  protected bool m_CisDirectory;

  public string displayName
  {
    get => this.m_CdisplayName;
    private set => this.m_CdisplayName = value;
  }

  public string fullPath
  {
    get => this.m_CfullPath;
    private set => this.m_CfullPath = value;
  }

  public bool isDirectory
  {
    get => this.m_CisDirectory;
    private set => this.m_CisDirectory = value;
  }

  public FileBrowserItem(string displayName, string fullPath, bool isDirectory)
  {
    this.displayName = displayName;
    this.fullPath = fullPath;
    this.isDirectory = isDirectory;
  }
}
