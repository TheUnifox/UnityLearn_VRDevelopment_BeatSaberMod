// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalInactiveClient
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerLocalInactiveClient : MonoBehaviour
{
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  [Inject]
  protected readonly INodePoseSyncStateManager _nodePoseSyncStateManager;

  public virtual void LateUpdate()
  {
    this._nodePoseSyncStateManager.localState?.SetState(NodePoseSyncState.NodePose.Head, new PoseSerializable((Vector3Serializable) this._playerTransforms.headPseudoLocalPos, (QuaternionSerializable) this._playerTransforms.headPseudoLocalRot));
    this._nodePoseSyncStateManager.localState?.SetState(NodePoseSyncState.NodePose.LeftController, new PoseSerializable((Vector3Serializable) this._playerTransforms.leftHandPseudoLocalPos, (QuaternionSerializable) this._playerTransforms.leftHandPseudoLocalRot));
    this._nodePoseSyncStateManager.localState?.SetState(NodePoseSyncState.NodePose.RightController, new PoseSerializable((Vector3Serializable) this._playerTransforms.rightHandPseudoLocalPos, (QuaternionSerializable) this._playerTransforms.rightHandPseudoLocalRot));
  }
}
