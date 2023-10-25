// Decompiled with JetBrains decompiler
// Type: Menu.ViewControllers.EditAvatarHistorySnapshot
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

namespace Menu.ViewControllers
{
  public class EditAvatarHistorySnapshot
  {
    [CompilerGenerated]
    protected readonly EditAvatarViewController.AvatarEditPart m_CavatarEditPart;
    [CompilerGenerated]
    protected readonly AvatarData m_CavatarData;

    public EditAvatarViewController.AvatarEditPart avatarEditPart => this.m_CavatarEditPart;

    public AvatarData avatarData => this.m_CavatarData;

    public EditAvatarHistorySnapshot(
      AvatarData avatarData,
      EditAvatarViewController.AvatarEditPart avatarEditPart)
    {
      this.m_CavatarData = avatarData;
      this.m_CavatarEditPart = avatarEditPart;
    }
  }
}
