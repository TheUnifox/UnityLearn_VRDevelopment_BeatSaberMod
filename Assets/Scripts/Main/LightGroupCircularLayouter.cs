// Decompiled with JetBrains decompiler
// Type: LightGroupCircularLayouter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteAlways]
public class LightGroupCircularLayouter : LightGroupSubsystem
{
  [Space]
  [SerializeField]
  protected float _radius;
  [SerializeField]
  protected float _angle = 360f;
  [SerializeField]
  protected float _startingAngle;
  [SerializeField]
  protected LightGroupCircularLayouter.RotationDirection _rotationDirection;
  [SerializeField]
  protected bool _staticRotation;
  [SerializeField]
  protected Vector3 _additionalAngle = Vector3.zero;

  public enum RotationDirection
  {
    Clockwise,
    Counterclockwise,
  }
}
