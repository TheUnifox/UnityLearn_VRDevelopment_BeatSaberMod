// Decompiled with JetBrains decompiler
// Type: HMUI.Touchable
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class Touchable : Graphic
  {
    [SerializeField]
    protected float _skew;

    public float skew => this._skew;

    protected override void OnPopulateMesh(VertexHelper vh) => vh.Clear();
  }
}
