// Decompiled with JetBrains decompiler
// Type: CustomTimelineTween
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class CustomTimelineTween : MonoBehaviour
{
  public Transform[] transforms;
  [HideInInspector]
  public Vector3[] startPositions;

  public virtual void OnValidate()
  {
    this.startPositions = new Vector3[this.transforms.Length];
    for (int index = 0; index < this.transforms.Length; ++index)
      this.startPositions[index] = this.transforms[index].localPosition;
  }
}
