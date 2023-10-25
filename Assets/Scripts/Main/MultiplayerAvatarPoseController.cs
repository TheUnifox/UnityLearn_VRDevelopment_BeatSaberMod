// Decompiled with JetBrains decompiler
// Type: MultiplayerAvatarPoseController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerAvatarPoseController : MonoBehaviour
{
  [SerializeField]
  protected AvatarPoseController _avatarPoseController;
  [Inject]
  protected readonly INodePoseSyncStateManager _nodePoseSyncStateManager;
  [InjectOptional]
  protected IConnectedPlayer _connectedPlayer;

  public IConnectedPlayer connectedPlayer
  {
    set => this._connectedPlayer = value;
  }

  public virtual void Start()
  {
    if (this._connectedPlayer != null)
      return;
    this.enabled = false;
  }

  public virtual void Update()
  {
    if (this._connectedPlayer == null)
      return;
    MultiplayerSyncState<NodePoseSyncState, NodePoseSyncState.NodePose, PoseSerializable> syncStateForPlayer = this._nodePoseSyncStateManager.GetSyncStateForPlayer(this._connectedPlayer);
    if (syncStateForPlayer == null)
      return;
    PoseSerializable state1 = syncStateForPlayer.GetState(NodePoseSyncState.NodePose.Head, this._connectedPlayer.offsetSyncTime);
    PoseSerializable state2 = syncStateForPlayer.GetState(NodePoseSyncState.NodePose.LeftController, this._connectedPlayer.offsetSyncTime);
    PoseSerializable state3 = syncStateForPlayer.GetState(NodePoseSyncState.NodePose.RightController, this._connectedPlayer.offsetSyncTime);
    this._avatarPoseController.UpdateTransforms((Vector3) state1.position, (Vector3) state2.position, (Vector3) state3.position, (Quaternion) state1.rotation, (Quaternion) state2.rotation, (Quaternion) state3.rotation);
  }
}
