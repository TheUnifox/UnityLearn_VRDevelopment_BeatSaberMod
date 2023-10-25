// Decompiled with JetBrains decompiler
// Type: MenuNeonLightsGenerator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteInEditMode]
public class MenuNeonLightsGenerator : MonoBehaviour
{
  [SerializeField]
  protected bool _generate;
  [SerializeField]
  protected float _radius = 8f;
  [SerializeField]
  protected float _angle = 90f;
  [SerializeField]
  protected int _numberOfElements = 4;
  [SerializeField]
  protected AnimationCurve _intensityCurve;
  [SerializeField]
  protected float _intensityMultiplier = 1f;
  [SerializeField]
  protected AnimationCurve _lengthCurve;
  [SerializeField]
  protected float _lengthMultiplier = 1f;
  [SerializeField]
  protected AnimationCurve _widthCurve;
  [SerializeField]
  protected float _widthMultiplier = 1f;
  [SerializeField]
  protected TubeBloomPrePassLight _neonLightPrefab;
  [SerializeField]
  protected Vector3 _afterSpawnRotation;
}
