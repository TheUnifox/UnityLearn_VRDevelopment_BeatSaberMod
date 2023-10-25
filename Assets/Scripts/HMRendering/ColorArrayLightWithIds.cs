// Decompiled with JetBrains decompiler
// Type: ColorArrayLightWithIds
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorArrayLightWithIds : LightWithIds
{
  [SerializeField]
  protected ColorArrayLightWithIds.ColorArrayLightWithId[] _colorArrayLightWithIds;
  [Space]
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [SerializeField]
  protected string _colorsArrayPropertyName;
  protected int _colorsPropertyId;
  protected Vector4[] _colorsArray;

  protected override void OnEnable()
  {
    base.OnEnable();
    this.RegisterArrayForColorChanges();
  }

  public virtual void OnDestroy() => this.UnregisterArrayFromColorChanges();

  protected override void ProcessNewColorData() => this.SetColorDataToShader();

  protected override IEnumerable<LightWithIds.LightWithId> GetLightWithIds() => (IEnumerable<LightWithIds.LightWithId>) this._colorArrayLightWithIds;

  public virtual void HandleColorLightWithIdDidSetColor(int index, Color color)
  {
    color = color.linear;
    this._colorsArray[index] = new Vector4(color.r, color.g, color.b, color.a);
  }

  public virtual void SetColorDataToShader()
  {
    this._materialPropertyBlockController.materialPropertyBlock.SetVectorArray(this._colorsPropertyId, this._colorsArray);
    this._materialPropertyBlockController.ApplyChanges();
  }

  public virtual void RegisterArrayForColorChanges()
  {
    this._colorsPropertyId = Shader.PropertyToID(this._colorsArrayPropertyName);
    this._colorsArray = new Vector4[this._colorArrayLightWithIds.Length];
    for (int index = 0; index < this._colorsArray.Length; ++index)
    {
      this._colorsArray[index] = Vector4.zero;
      this._colorArrayLightWithIds[index].didSetColorEvent += new Action<int, Color>(this.HandleColorLightWithIdDidSetColor);
    }
    this.SetColorDataToShader();
  }

  public virtual void UnregisterArrayFromColorChanges()
  {
    foreach (ColorArrayLightWithIds.ColorArrayLightWithId arrayLightWithId in this._colorArrayLightWithIds)
      arrayLightWithId.didSetColorEvent -= new Action<int, Color>(this.HandleColorLightWithIdDidSetColor);
  }

  [Serializable]
  public class ColorArrayLightWithId : LightWithIds.LightWithId
  {
    [SerializeField]
    protected int _index;

    public event Action<int, Color> didSetColorEvent;

    public ColorArrayLightWithId(int index, int lightId)
      : base(lightId)
    {
      this._index = index;
    }

    public override void ColorWasSet(Color newColor)
    {
      base.ColorWasSet(newColor);
      Action<int, Color> didSetColorEvent = this.didSetColorEvent;
      if (didSetColorEvent == null)
        return;
      didSetColorEvent(this._index, newColor);
    }
  }
}
