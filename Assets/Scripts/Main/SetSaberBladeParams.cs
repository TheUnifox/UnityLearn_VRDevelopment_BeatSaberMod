// Decompiled with JetBrains decompiler
// Type: SetSaberBladeParams
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

public class SetSaberBladeParams : MonoBehaviour
{
  [SerializeField]
  protected SaberTypeObject _saber;
  [SerializeField]
  protected MeshRenderer _meshRenderer;
  [SerializeField]
  [NullAllowed]
  protected SetSaberBladeParams.PropertyTintColorPair[] _propertyTintColorPairs;
  [Inject]
  protected ColorManager _colorManager;

  public virtual void Start()
  {
    MaterialPropertyBlock properties = new MaterialPropertyBlock();
    foreach (SetSaberBladeParams.PropertyTintColorPair propertyTintColorPair in this._propertyTintColorPairs)
      properties.SetColor(propertyTintColorPair.property, this._colorManager.ColorForSaberType(this._saber.saberType) * propertyTintColorPair.tintColor);
    this._meshRenderer.SetPropertyBlock(properties);
  }

  [Serializable]
  public class PropertyTintColorPair
  {
    public Color tintColor;
    public string property;
  }
}
