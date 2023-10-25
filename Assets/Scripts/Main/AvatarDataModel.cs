// Decompiled with JetBrains decompiler
// Type: AvatarDataModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using DataModels.PlayerAvatar;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class AvatarDataModel : MonoBehaviour
{
  [SerializeField]
  protected AvatarDataFileManagerSO _avatarDataFileManager;
  [Inject]
  protected readonly AvatarPartsModel _avatarPartsModel;
  [CompilerGenerated]
  protected AvatarData m_CavatarData;

  public AvatarData avatarData
  {
    get => this.m_CavatarData;
    set => this.m_CavatarData = value;
  }

  public virtual void OnEnable() => this.Load();

  public virtual void Randomize() => this.avatarData = this.CreateDefaultAvatarData();

  public virtual void Save() => this._avatarDataFileManager.Save(this.avatarData);

  public virtual void Load() => this.avatarData = this._avatarDataFileManager.Load() ?? this.CreateDefaultAvatarData();

  public virtual AvatarData CreateDefaultAvatarData()
  {
    UnityEngine.Random.InitState((int) DateTime.Now.Ticks);
    AvatarData avatarData = new AvatarData();
    avatarData.skinColorId = this._avatarPartsModel.GetRandomColor().id;
    AvatarRandomizer.RandomizeAll(avatarData, this._avatarPartsModel);
    return avatarData;
  }
}
