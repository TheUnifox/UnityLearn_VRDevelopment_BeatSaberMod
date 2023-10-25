// Decompiled with JetBrains decompiler
// Type: MaterialPropertyBlockColorAnimator
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteAlways]
public class MaterialPropertyBlockColorAnimator : MaterialPropertyBlockAnimator
{
  [Space]
  [SerializeField]
  protected Color _color = Color.black;

  public Color color
  {
    get => this._color;
    set => this._color = value;
  }

  protected override void SetProperty() => this._materialPropertyBlockController.materialPropertyBlock.SetColor(this.propertyId, this._color);
}
