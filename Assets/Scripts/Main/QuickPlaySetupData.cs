// Decompiled with JetBrains decompiler
// Type: QuickPlaySetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

[Preserve]
[Serializable]
public class QuickPlaySetupData
{
  public QuickPlaySetupData.QuickPlaySongPacksOverride quickPlayAvailablePacksOverride;

  public bool hasOverride
  {
    get
    {
      if (this.quickPlayAvailablePacksOverride == null)
        return false;
      return this.quickPlayAvailablePacksOverride.predefinedPackIds.Count > 0 || this.quickPlayAvailablePacksOverride.localizedCustomPacks.Count > 0;
    }
  }

  [Serializable]
  public class QuickPlaySongPacksOverride
  {
    public List<QuickPlaySetupData.QuickPlaySongPacksOverride.PredefinedPack> predefinedPackIds = new List<QuickPlaySetupData.QuickPlaySongPacksOverride.PredefinedPack>();
    public List<QuickPlaySetupData.QuickPlaySongPacksOverride.LocalizedCustomPack> localizedCustomPacks = new List<QuickPlaySetupData.QuickPlaySongPacksOverride.LocalizedCustomPack>();

    [Serializable]
    public class LocalizedCustomPackName
    {
      public string language;
      public string packName;
    }

    [Serializable]
    public class LocalizedCustomPack
    {
      public string serializedName;
      public int order;
      public QuickPlaySetupData.QuickPlaySongPacksOverride.LocalizedCustomPackName[] localizedNames;
      public List<string> packIds;

      public LocalizedCustomPack() => this.packIds = new List<string>();
    }

    [Serializable]
    public class PredefinedPack
    {
      public int order;
      public string packId;
    }
  }
}
