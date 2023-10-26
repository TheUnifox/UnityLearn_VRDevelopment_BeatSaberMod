// Decompiled with JetBrains decompiler
// Type: AvatarDataMultiplayerAvatarDataConverter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public static class AvatarDataMultiplayerAvatarDataConverter
{
  public static MultiplayerAvatarData CreateMultiplayerAvatarData(this AvatarData avatarData) => new MultiplayerAvatarData(avatarData.headTopId, (Color32) avatarData.headTopPrimaryColor, (Color32) avatarData.headTopSecondaryColor, avatarData.glassesId, (Color32) avatarData.glassesColor, avatarData.facialHairId, (Color32) avatarData.facialHairColor, avatarData.handsId, (Color32) avatarData.handsColor, avatarData.clothesId, (Color32) avatarData.clothesPrimaryColor, (Color32) avatarData.clothesSecondaryColor, (Color32) avatarData.clothesDetailColor, avatarData.skinColorId, avatarData.eyesId, avatarData.mouthId);

  public static AvatarData CreateAvatarData(this MultiplayerAvatarData multiplayerAvatarData) => new AvatarData(multiplayerAvatarData.headTopId, (Color32)multiplayerAvatarData.headTopPrimaryColor, (Color32)multiplayerAvatarData.headTopSecondaryColor, multiplayerAvatarData.glassesId, (Color32)multiplayerAvatarData.glassesColor, multiplayerAvatarData.facialHairId, (Color32)multiplayerAvatarData.facialHairColor, multiplayerAvatarData.handsId, (Color32)multiplayerAvatarData.handsColor, multiplayerAvatarData.clothesId, (Color32)multiplayerAvatarData.clothesPrimaryColor, (Color32)multiplayerAvatarData.clothesSecondaryColor, (Color32)multiplayerAvatarData.clothesDetailColor, multiplayerAvatarData.skinColorId, multiplayerAvatarData.eyesId, multiplayerAvatarData.mouthId);
}
