// Decompiled with JetBrains decompiler
// Type: MultiplayerAvatarAudioController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class MultiplayerAvatarAudioController : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  [InjectOptional]
  protected IConnectedPlayer _connectedPlayer;

  public IConnectedPlayer connectedPlayer
  {
    set => this._connectedPlayer = value;
  }

  public virtual IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.m_Cm_E1__state;
    MultiplayerAvatarAudioController avatarAudioController = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.m_Cm_E1__state = -1;
    if (avatarAudioController._connectedPlayer != null && !avatarAudioController._connectedPlayer.isMe)
      return false;
    avatarAudioController.enabled = false;
    return false;
  }
}
