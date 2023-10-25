// Decompiled with JetBrains decompiler
// Type: BookmarksFoldersModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class BookmarksFoldersModel : PersistentScriptableObject
{
  [SerializeField]
  protected string[] myFolders;
  protected FileBrowserItem[] _bookmarksFolders;

  public FileBrowserItem[] bookmarksFolders
  {
    get
    {
      if (this._bookmarksFolders == null)
      {
        List<FileBrowserItem> fileBrowserItemList = new List<FileBrowserItem>();
        foreach (string logicalDrive in Directory.GetLogicalDrives())
          fileBrowserItemList.Add(new FileBrowserItem(logicalDrive, logicalDrive, true));
        IEnumerable<string> collection = ((IEnumerable<Environment.SpecialFolder>) new Environment.SpecialFolder[4]
        {
          Environment.SpecialFolder.MyComputer,
          Environment.SpecialFolder.Personal,
          Environment.SpecialFolder.MyMusic,
          Environment.SpecialFolder.Desktop
        }).Select<Environment.SpecialFolder, string>((Func<Environment.SpecialFolder, string>) (specialFolder => Environment.GetFolderPath(specialFolder)));
        List<string> stringList = new List<string>((IEnumerable<string>) this.myFolders);
        stringList.AddRange(collection);
        stringList.Add(Application.dataPath);
        foreach (string str in stringList)
        {
          if (Directory.Exists(str))
            fileBrowserItemList.Add(new FileBrowserItem(new DirectoryInfo(str).Name, str, true));
        }
        this._bookmarksFolders = fileBrowserItemList.ToArray();
      }
      return this._bookmarksFolders;
    }
  }
}
