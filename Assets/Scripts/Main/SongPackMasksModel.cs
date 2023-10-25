// Decompiled with JetBrains decompiler
// Type: SongPackMasksModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Linq;

public class SongPackMasksModel
{
  protected readonly SongPackMaskModelSO _songPackMaskModel;
  protected readonly SongPackMask _allSongPackMask;

  public SongPackMasksModel(SongPackMaskModelSO songPackMasks)
  {
    this._songPackMaskModel = songPackMasks;
    this._allSongPackMask = songPackMasks.ToSongPackMask(songPackMasks.defaultSongPackMaskItems.First<string>());
  }

  public virtual SongPackMask GetAllSongsMask() => this._allSongPackMask;

  public virtual string GetSongPackMaskText(in SongPackMask songPackMask) => this.GetSongPackMaskText(in songPackMask, out bool _);

  public virtual string GetSongPackMaskText(in SongPackMask songPackMask, out bool plural)
  {
    string serializedName;
    if (this._songPackMaskModel.ToSerializedName(songPackMask, out serializedName))
      return this._songPackMaskModel.ToLocalizedName(serializedName, out plural);
    plural = false;
    return string.Empty;
  }
}
