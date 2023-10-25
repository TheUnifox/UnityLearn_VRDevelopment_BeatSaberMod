// Decompiled with JetBrains decompiler
// Type: DataModels.PlayerAvatar.RandomizeAvatarColorMap
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Runtime.CompilerServices;

namespace DataModels.PlayerAvatar
{
  public class RandomizeAvatarColorMap
  {
    [CompilerGenerated]
    protected readonly int m_CtotalIndices;
    [CompilerGenerated]
    protected readonly int[] m_CcolorIndices;

    public int totalIndices => this.m_CtotalIndices;

    public int[] colorIndices => this.m_CcolorIndices;

    public RandomizeAvatarColorMap(
      int headTopPrimaryColorIndex,
      int headTopSecondaryColorIndex,
      int glassesColorIndex,
      int facialHairColorIndex,
      int handsColorIndex,
      int clothesPrimaryColorIndex,
      int clothesSecondaryColorIndex,
      int clothesDetailColorIndex)
    {
      this.m_CcolorIndices = new int[8]
      {
        headTopPrimaryColorIndex,
        headTopSecondaryColorIndex,
        glassesColorIndex,
        facialHairColorIndex,
        handsColorIndex,
        clothesPrimaryColorIndex,
        clothesSecondaryColorIndex,
        clothesDetailColorIndex
      };
      int val1 = 0;
      foreach (int colorIndex in this.colorIndices)
        val1 = Math.Max(val1, colorIndex);
      this.m_CtotalIndices = val1 + 1;
    }
  }
}
