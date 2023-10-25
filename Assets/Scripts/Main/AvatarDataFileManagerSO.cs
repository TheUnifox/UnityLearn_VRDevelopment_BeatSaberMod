// Decompiled with JetBrains decompiler
// Type: AvatarDataFileManagerSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class AvatarDataFileManagerSO : PersistentScriptableObject
{
  protected const string kAvatarDataFileName = "AvatarData.dat";
  protected const string kTempFileName = "AvatarData.dat.tmp";
  protected const string kBackupFileName = "AvatarData.dat.bak";

  public virtual void Save(AvatarData avatarData) => FileHelpers.SaveToJSONFile((object) new AvatarSaveData()
  {
    headTopId = avatarData.headTopId,
    headTopPrimaryColor = avatarData.headTopPrimaryColor,
    headTopSecondaryColor = avatarData.headTopSecondaryColor,
    glassesId = avatarData.glassesId,
    glassesColor = avatarData.glassesColor,
    facialHairId = avatarData.facialHairId,
    facialHairColor = avatarData.facialHairColor,
    handsId = avatarData.handsId,
    handsColor = avatarData.handsColor,
    clothesId = avatarData.clothesId,
    clothesPrimaryColor = avatarData.clothesPrimaryColor,
    clothesSecondaryColor = avatarData.clothesSecondaryColor,
    clothesDetailColor = avatarData.clothesDetailColor,
    skinColorId = avatarData.skinColorId,
    eyesId = avatarData.eyesId,
    mouthId = avatarData.mouthId
  }, Application.persistentDataPath + "/AvatarData.dat", Application.persistentDataPath + "/AvatarData.dat.tmp", Application.persistentDataPath + "/AvatarData.dat.bak");

  public virtual AvatarData Load() => this.LoadFromCurrentVersion(FileHelpers.LoadFromJSONFile<AvatarSaveData>(Application.persistentDataPath + "/AvatarData.dat", Application.persistentDataPath + "/AvatarData.dat.bak"));

  public virtual AvatarData LoadFromCurrentVersion(AvatarSaveData avatarSaveData) => avatarSaveData == null ? (AvatarData) null : new AvatarData(avatarSaveData.headTopId, avatarSaveData.headTopPrimaryColor, avatarSaveData.headTopSecondaryColor, avatarSaveData.glassesId, avatarSaveData.glassesColor, avatarSaveData.facialHairId, avatarSaveData.facialHairColor, avatarSaveData.handsId, avatarSaveData.handsColor, avatarSaveData.clothesId, avatarSaveData.clothesPrimaryColor, avatarSaveData.clothesSecondaryColor, avatarSaveData.clothesDetailColor, avatarSaveData.skinColorId, avatarSaveData.eyesId, avatarSaveData.mouthId);
}
