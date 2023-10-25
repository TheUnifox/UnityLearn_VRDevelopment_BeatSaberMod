// Decompiled with JetBrains decompiler
// Type: MultiplayerOffsetPositionByLocalPlayerPosition
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerOffsetPositionByLocalPlayerPosition : MonoBehaviour
{
  [Inject]
  protected readonly MultiplayerPlayersManager _multiplayerPlayersManager;
  protected Vector3 _positionOffset;
  protected Quaternion _rotationOffset;
  protected Vector3 _lastParentPosition;
  protected Quaternion _lastParentRotation;

  public virtual void Awake()
  {
    Transform transform = this.transform;
    this._positionOffset = transform.position;
    this._rotationOffset = transform.rotation;
  }

  public virtual void Update() => this.UpdatePositionAndRotationIfNeeded();

  public virtual void SetEnabled(bool isEnabled) => this.enabled = isEnabled;

  public virtual void UpdatePositionAndRotationIfNeeded()
  {
    Transform localPlayerTransform = this._multiplayerPlayersManager.localPlayerTransform;
    Vector3 position = localPlayerTransform.position;
    Quaternion rotation = localPlayerTransform.rotation;
    if (this._lastParentPosition == position && this._lastParentRotation == rotation)
      return;
    this._lastParentPosition = position;
    this._lastParentRotation = rotation;
    this.transform.SetPositionAndRotation(this._lastParentRotation * this._positionOffset + this._lastParentPosition, this._lastParentRotation * this._rotationOffset);
  }
}
