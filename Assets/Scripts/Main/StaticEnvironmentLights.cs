// Decompiled with JetBrains decompiler
// Type: StaticEnvironmentLights
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class StaticEnvironmentLights : MonoBehaviour
{
  [SerializeField]
  protected Color[] _lightColors;
  [SerializeField]
  protected Material[] _materials;

  public virtual void Awake()
  {
    for (int index = 0; index < this._materials.Length; ++index)
      this._materials[index].color = this._lightColors[index];
  }
}
