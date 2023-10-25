// Decompiled with JetBrains decompiler
// Type: LightGroupLinearLayouter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteAlways]
public class LightGroupLinearLayouter : LightGroupSubsystem
{
  [Space]
  [SerializeField]
  protected Vector3 _movementStep = new Vector3(1f, 0.0f, 0.0f);
  [SerializeField]
  protected Vector3 _defaultRotation = Vector3.zero;
  [SerializeField]
  protected bool _startFromCenter;
}
