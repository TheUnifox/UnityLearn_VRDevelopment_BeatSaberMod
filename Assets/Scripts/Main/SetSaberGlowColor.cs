// Decompiled with JetBrains decompiler
// Type: SetSaberGlowColor
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

public class SetSaberGlowColor : MonoBehaviour
{
  [SerializeField]
  [NullAllowed]
  protected SaberTypeObject _saberTypeObject;
  [SerializeField]
  protected MeshRenderer _meshRenderer;
  [SerializeField]
  [NullAllowed]
  protected SetSaberGlowColor.PropertyTintColorPair[] _propertyTintColorPairs;
  [Inject]
  protected ColorManager _colorManager;
  protected MaterialPropertyBlock _materialPropertyBlock;
  protected SaberType _saberType;

  public SaberType saberType
  {
    set
    {
      this._saberTypeObject = (SaberTypeObject) null;
      this._saberType = value;
      this.SetColors();
    }
  }

  public virtual void Start()
  {
    if ((UnityEngine.Object) this._saberTypeObject != (UnityEngine.Object) null)
      this._saberType = this._saberTypeObject.saberType;
    this.SetColors();
  }

  public virtual void SetColors()
  {
    if (this._materialPropertyBlock == null)
      this._materialPropertyBlock = new MaterialPropertyBlock();
    Color color = this._colorManager.ColorForSaberType(this._saberType);
    foreach (SetSaberGlowColor.PropertyTintColorPair propertyTintColorPair in this._propertyTintColorPairs)
      this._materialPropertyBlock.SetColor(propertyTintColorPair.property, color * propertyTintColorPair.tintColor);
    this._meshRenderer.SetPropertyBlock(this._materialPropertyBlock);
  }

  [Serializable]
  public class PropertyTintColorPair
  {
    public Color tintColor;
    public string property;
  }
}
