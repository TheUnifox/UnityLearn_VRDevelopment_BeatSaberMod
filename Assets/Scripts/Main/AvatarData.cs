// Decompiled with JetBrains decompiler
// Type: AvatarData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class AvatarData
{
  [CompilerGenerated]
  protected string m_CheadTopId;
  [CompilerGenerated]
  protected string m_CglassesId;
  [CompilerGenerated]
  protected string m_CfacialHairId;
  [CompilerGenerated]
  protected string m_ChandsId;
  [CompilerGenerated]
  protected string m_CclothesId;
  [CompilerGenerated]
  protected string m_CeyesId;
  [CompilerGenerated]
  protected string m_CmouthId;
  [CompilerGenerated]
  protected Color m_CheadTopPrimaryColor;
  [CompilerGenerated]
  protected Color m_CheadTopSecondaryColor;
  [CompilerGenerated]
  protected Color m_CglassesColor;
  [CompilerGenerated]
  protected Color m_CfacialHairColor;
  [CompilerGenerated]
  protected Color m_ChandsColor;
  [CompilerGenerated]
  protected Color m_CclothesPrimaryColor;
  [CompilerGenerated]
  protected Color m_CclothesSecondaryColor;
  [CompilerGenerated]
  protected Color m_CclothesDetailColor;
  [CompilerGenerated]
  protected string m_CskinColorId;

  public string headTopId
  {
    get => this.m_CheadTopId;
    set => this.m_CheadTopId = value;
  }

  public string glassesId
  {
    get => this.m_CglassesId;
    set => this.m_CglassesId = value;
  }

  public string facialHairId
  {
    get => this.m_CfacialHairId;
    set => this.m_CfacialHairId = value;
  }

  public string handsId
  {
    get => this.m_ChandsId;
    set => this.m_ChandsId = value;
  }

  public string clothesId
  {
    get => this.m_CclothesId;
    set => this.m_CclothesId = value;
  }

  public string eyesId
  {
    get => this.m_CeyesId;
    set => this.m_CeyesId = value;
  }

  public string mouthId
  {
    get => this.m_CmouthId;
    set => this.m_CmouthId = value;
  }

  public Color headTopPrimaryColor
  {
    get => this.m_CheadTopPrimaryColor;
    set => this.m_CheadTopPrimaryColor = value;
  }

  public Color headTopSecondaryColor
  {
    get => this.m_CheadTopSecondaryColor;
    set => this.m_CheadTopSecondaryColor = value;
  }

  public Color glassesColor
  {
    get => this.m_CglassesColor;
    set => this.m_CglassesColor = value;
  }

  public Color facialHairColor
  {
    get => this.m_CfacialHairColor;
    set => this.m_CfacialHairColor = value;
  }

  public Color handsColor
  {
    get => this.m_ChandsColor;
    set => this.m_ChandsColor = value;
  }

  public Color clothesPrimaryColor
  {
    get => this.m_CclothesPrimaryColor;
    set => this.m_CclothesPrimaryColor = value;
  }

  public Color clothesSecondaryColor
  {
    get => this.m_CclothesSecondaryColor;
    set => this.m_CclothesSecondaryColor = value;
  }

  public Color clothesDetailColor
  {
    get => this.m_CclothesDetailColor;
    set => this.m_CclothesDetailColor = value;
  }

  public string skinColorId
  {
    get => this.m_CskinColorId;
    set => this.m_CskinColorId = value;
  }

  public AvatarData()
  {
  }

  public AvatarData(
    string headTopId,
    Color headTopPrimaryColor,
    Color headTopSecondaryColor,
    string glassesId,
    Color glassesColor,
    string facialHairId,
    Color facialHairColor,
    string handsId,
    Color handsColor,
    string clothesId,
    Color clothesPrimaryColor,
    Color clothesSecondaryColor,
    Color clothesDetailColor,
    string skinColorId,
    string eyesId,
    string mouthId)
  {
    this.headTopId = headTopId;
    this.headTopPrimaryColor = headTopPrimaryColor;
    this.headTopSecondaryColor = headTopSecondaryColor;
    this.glassesId = glassesId;
    this.glassesColor = glassesColor;
    this.facialHairId = facialHairId;
    this.facialHairColor = facialHairColor;
    this.handsId = handsId;
    this.handsColor = handsColor;
    this.clothesId = clothesId;
    this.clothesPrimaryColor = clothesPrimaryColor;
    this.clothesSecondaryColor = clothesSecondaryColor;
    this.clothesDetailColor = clothesDetailColor;
    this.skinColorId = skinColorId;
    this.eyesId = eyesId;
    this.mouthId = mouthId;
  }

  public virtual AvatarData Clone() => new AvatarData(this.headTopId, this.headTopPrimaryColor, this.headTopSecondaryColor, this.glassesId, this.glassesColor, this.facialHairId, this.facialHairColor, this.handsId, this.handsColor, this.clothesId, this.clothesPrimaryColor, this.clothesSecondaryColor, this.clothesDetailColor, this.skinColorId, this.eyesId, this.mouthId);
}
