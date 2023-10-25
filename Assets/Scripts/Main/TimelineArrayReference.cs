// Decompiled with JetBrains decompiler
// Type: TimelineArrayReference
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class TimelineArrayReference : MonoBehaviour
{
  [SerializeField]
  protected TimelineArrayReference.ArrayTypes arrayType;
  public TubeBloomPrePassLight[] _tubeLightArray;
  public CanvasGroup[] _canvasGroupArray;
  public TextMeshPro[] _tmproArray;
  public Transform[] _transformArray;
  public DirectionalLight[] _directionalLights;

  public enum ArrayTypes
  {
    TubeLight,
    Transform,
    Canvas,
    TextMeshPro,
    DirectionalLight,
  }
}
