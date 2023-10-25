// Decompiled with JetBrains decompiler
// Type: HeadBodyOffsetSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class HeadBodyOffsetSO : PersistentScriptableObject
{
  [SerializeField]
  protected Vector3 _headNeckOffset = new Vector3(0.0f, 0.35f, 0.136f);
  [SerializeField]
  protected float _verticalOffset = 0.03f;

  public Vector3 headNeckOffset => this._headNeckOffset;

  public float verticalOffset => this._verticalOffset;
}
