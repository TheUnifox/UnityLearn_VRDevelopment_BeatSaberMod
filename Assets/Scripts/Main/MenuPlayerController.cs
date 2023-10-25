// Decompiled with JetBrains decompiler
// Type: MenuPlayerController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MenuPlayerController : MonoBehaviour
{
  [SerializeField]
  protected VRController _leftController;
  [SerializeField]
  protected VRController _rightController;
  [SerializeField]
  protected Transform _headTransform;

  public VRController leftController => this._leftController;

  public VRController rightController => this._rightController;

  public Vector3 headPos => this._headTransform.position;

  public Quaternion headRot => this._headTransform.rotation;
}
