// Decompiled with JetBrains decompiler
// Type: MultiplayerMenuClient
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerMenuClient : MonoBehaviour
{
  [Inject]
  protected readonly MenuPlayerController _menuPlayerController;
  [Inject]
  protected readonly INodePoseSyncStateManager _nodePoseSyncStateManager;

  public virtual void LateUpdate()
  {
    this._nodePoseSyncStateManager.localState?.SetState(NodePoseSyncState.NodePose.Head, new PoseSerializable((Vector3Serializable) this._menuPlayerController.headPos, (QuaternionSerializable) this._menuPlayerController.headRot));
    this._nodePoseSyncStateManager.localState?.SetState(NodePoseSyncState.NodePose.LeftController, new PoseSerializable((Vector3Serializable) this._menuPlayerController.leftController.position, (QuaternionSerializable) this._menuPlayerController.leftController.rotation));
    this._nodePoseSyncStateManager.localState?.SetState(NodePoseSyncState.NodePose.RightController, new PoseSerializable((Vector3Serializable) this._menuPlayerController.rightController.position, (QuaternionSerializable) this._menuPlayerController.rightController.rotation));
  }
}
