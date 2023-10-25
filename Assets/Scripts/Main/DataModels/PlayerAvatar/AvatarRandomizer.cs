// Decompiled with JetBrains decompiler
// Type: DataModels.PlayerAvatar.AvatarRandomizer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

namespace DataModels.PlayerAvatar
{
  public abstract class AvatarRandomizer
  {
    [DoesNotRequireDomainReloadInit]
    private static readonly RandomizeAvatarColorMap[] _randomizeColorsParamsCollection = new RandomizeAvatarColorMap[10]
    {
      new RandomizeAvatarColorMap(0, 0, 0, 0, 1, 0, 2, 0),
      new RandomizeAvatarColorMap(0, 1, 0, 2, 0, 1, 0, 1),
      new RandomizeAvatarColorMap(0, 1, 0, 2, 0, 1, 0, 1),
      new RandomizeAvatarColorMap(0, 1, 0, 0, 0, 3, 2, 3),
      new RandomizeAvatarColorMap(0, 1, 0, 2, 0, 3, 0, 3),
      new RandomizeAvatarColorMap(0, 1, 0, 2, 4, 3, 0, 3),
      new RandomizeAvatarColorMap(0, 1, 0, 2, 0, 3, 0, 4),
      new RandomizeAvatarColorMap(0, 2, 1, 1, 0, 1, 0, 2),
      new RandomizeAvatarColorMap(0, 0, 1, 2, 3, 4, 5, 5),
      new RandomizeAvatarColorMap(0, 1, 2, 3, 4, 1, 5, 1)
    };

    public static void RandomizeAll(AvatarData avatarData, AvatarPartsModel avatarPartsModel)
    {
      AvatarRandomizer.RandomizeModels(avatarData, avatarPartsModel);
      AvatarRandomizer.RandomizeColors(avatarData);
    }

    public static void RandomizeModels(AvatarData avatarData, AvatarPartsModel avatarPartsModel)
    {
      avatarData.headTopId = avatarPartsModel.headTopCollection.GetRandom().id;
      avatarData.eyesId = avatarPartsModel.eyesCollection.GetRandom().id;
      avatarData.mouthId = avatarPartsModel.mouthCollection.GetRandom().id;
      avatarData.glassesId = avatarPartsModel.glassesCollection.GetDefault().id;
      avatarData.facialHairId = avatarPartsModel.facialHairCollection.GetDefault().id;
      avatarData.handsId = avatarPartsModel.handsCollection.GetRandom().id;
      avatarData.clothesId = avatarPartsModel.clothesCollection.GetRandom().id;
    }

    public static void RandomizeColors(AvatarData avatarData)
    {
      int index1 = Random.Range(0, AvatarRandomizer._randomizeColorsParamsCollection.Length - 1);
      RandomizeAvatarColorMap randomizeColorsParams = AvatarRandomizer._randomizeColorsParamsCollection[index1];
      Color[] colorArray = new Color[randomizeColorsParams.totalIndices];
      for (int index2 = 0; index2 < randomizeColorsParams.totalIndices; ++index2)
        colorArray[index2] = Random.ColorHSV();
      avatarData.headTopPrimaryColor = colorArray[randomizeColorsParams.colorIndices[0]];
      avatarData.headTopSecondaryColor = colorArray[randomizeColorsParams.colorIndices[1]];
      avatarData.glassesColor = colorArray[randomizeColorsParams.colorIndices[2]];
      avatarData.facialHairColor = colorArray[randomizeColorsParams.colorIndices[3]];
      avatarData.handsColor = colorArray[randomizeColorsParams.colorIndices[4]];
      avatarData.clothesPrimaryColor = colorArray[randomizeColorsParams.colorIndices[5]];
      avatarData.clothesSecondaryColor = colorArray[randomizeColorsParams.colorIndices[6]];
      avatarData.clothesDetailColor = colorArray[randomizeColorsParams.colorIndices[7]];
    }
  }
}
