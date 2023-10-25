// Decompiled with JetBrains decompiler
// Type: LightmapDataSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class LightmapDataSO : PersistentScriptableObject
{
  [SerializeField]
  protected Texture2D _lightmap1;
  [SerializeField]
  protected Texture2D _lightmap2;

  public Texture2D lightmap1
  {
    get => this._lightmap1;
    set => this._lightmap1 = value;
  }

  public Texture2D lightmap2
  {
    get => this._lightmap2;
    set => this._lightmap2 = value;
  }
}
