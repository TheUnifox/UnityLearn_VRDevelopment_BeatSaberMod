// Decompiled with JetBrains decompiler
// Type: ReflectionProbeDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ReflectionProbeDataSO : PersistentScriptableObject
{
  [SerializeField]
  protected Cubemap _reflectionProbeCubemap1;
  [SerializeField]
  protected Cubemap _reflectionProbeCubemap2;

  public Cubemap reflectionProbeCubemap1
  {
    get => this._reflectionProbeCubemap1;
    set => this._reflectionProbeCubemap1 = value;
  }

  public Cubemap reflectionProbeCubemap2
  {
    get => this._reflectionProbeCubemap2;
    set => this._reflectionProbeCubemap2 = value;
  }
}
