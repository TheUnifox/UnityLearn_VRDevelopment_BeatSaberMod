// Decompiled with JetBrains decompiler
// Type: AvatarHeadOffset
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class AvatarHeadOffset : MonoBehaviour
{
  [SerializeField]
  protected Vector3 _positionOffset = new Vector3(0.0f, 0.5f, 0.0f);
  [Inject]
  protected readonly AvatarPoseController _avatarPoseController;

  public virtual void Start() => this._avatarPoseController.didUpdatePoseEvent += new System.Action<Vector3>(this.HandleMultiplayerAvatarPoseControllerDidUpdatePose);

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._avatarPoseController != (UnityEngine.Object) null))
      return;
    this._avatarPoseController.didUpdatePoseEvent -= new System.Action<Vector3>(this.HandleMultiplayerAvatarPoseControllerDidUpdatePose);
  }

  public virtual void HandleMultiplayerAvatarPoseControllerDidUpdatePose(Vector3 headLocalPosition) => this.transform.localPosition = headLocalPosition + this._positionOffset;
}
