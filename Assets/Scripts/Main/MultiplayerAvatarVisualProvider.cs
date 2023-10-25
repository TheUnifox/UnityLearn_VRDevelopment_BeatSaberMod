// Decompiled with JetBrains decompiler
// Type: MultiplayerAvatarVisualProvider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerAvatarVisualProvider : MonoBehaviour
{
  [SerializeField]
  protected AvatarVisualController _avatarVisualController;
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;

  public virtual void Start() => this._avatarVisualController.UpdateAvatarVisual(this._connectedPlayer.multiplayerAvatarData.CreateAvatarData());
}
