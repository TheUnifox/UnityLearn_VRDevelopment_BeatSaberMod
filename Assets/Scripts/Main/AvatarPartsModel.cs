// Decompiled with JetBrains decompiler
// Type: AvatarPartsModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AvatarPartsModel
{
  [CompilerGenerated]
  protected readonly AvatarPartCollection<AvatarMeshPartSO> m_CheadTopCollection;
  [CompilerGenerated]
  protected readonly AvatarPartCollection<AvatarSpritePartSO> m_CeyesCollection;
  [CompilerGenerated]
  protected readonly AvatarPartCollection<AvatarSpritePartSO> m_CmouthCollection;
  [CompilerGenerated]
  protected readonly AvatarPartCollection<AvatarMeshPartSO> m_CglassesCollection;
  [CompilerGenerated]
  protected readonly AvatarPartCollection<AvatarMeshPartSO> m_CfacialHairCollection;
  [CompilerGenerated]
  protected readonly AvatarPartCollection<AvatarMeshPartSO> m_ChandsCollection;
  [CompilerGenerated]
  protected readonly AvatarPartCollection<AvatarMeshPartSO> m_CclothesCollection;
  [CompilerGenerated]
  protected readonly SkinColorSO[] m_CskinColors;
  protected readonly Dictionary<string, int> _indexById = new Dictionary<string, int>();

  public AvatarPartCollection<AvatarMeshPartSO> headTopCollection => this.m_CheadTopCollection;

  public AvatarPartCollection<AvatarSpritePartSO> eyesCollection => this.m_CeyesCollection;

  public AvatarPartCollection<AvatarSpritePartSO> mouthCollection => this.m_CmouthCollection;

  public AvatarPartCollection<AvatarMeshPartSO> glassesCollection => this.m_CglassesCollection;

  public AvatarPartCollection<AvatarMeshPartSO> facialHairCollection => this.m_CfacialHairCollection;

  public AvatarPartCollection<AvatarMeshPartSO> handsCollection => this.m_ChandsCollection;

  public AvatarPartCollection<AvatarMeshPartSO> clothesCollection => this.m_CclothesCollection;

  public SkinColorSO[] skinColors => this.m_CskinColors;

  public AvatarPartsModel(AvatarPartsModelSO avatarPartData, SkinColorSetSO skinColorSet)
  {
    this.m_CheadTopCollection = new AvatarPartCollection<AvatarMeshPartSO>(avatarPartData.headTops);
    this.m_CeyesCollection = new AvatarPartCollection<AvatarSpritePartSO>(avatarPartData.Eyes);
    this.m_CmouthCollection = new AvatarPartCollection<AvatarSpritePartSO>(avatarPartData.Mouths);
    this.m_CglassesCollection = new AvatarPartCollection<AvatarMeshPartSO>(avatarPartData.Glasses);
    this.m_CfacialHairCollection = new AvatarPartCollection<AvatarMeshPartSO>(avatarPartData.FacialHair);
    this.m_CclothesCollection = new AvatarPartCollection<AvatarMeshPartSO>(avatarPartData.Clothes);
    this.m_ChandsCollection = new AvatarPartCollection<AvatarMeshPartSO>(avatarPartData.Hands);
    this.m_CskinColors = skinColorSet.colors;
    for (int index = 0; index < this.skinColors.Length; ++index)
      this._indexById[this.skinColors[index].id] = index;
  }

  public virtual int GetColorIndexById(string id)
  {
    int num;
    return id != null && this._indexById.TryGetValue(id, out num) ? num : 0;
  }

  public virtual SkinColorSO GetSkinColorById(string id) => this.skinColors[this.GetColorIndexById(id)];

  public virtual SkinColorSO GetRandomColor() => this.skinColors[Random.Range(0, this.skinColors.Length)];
}
