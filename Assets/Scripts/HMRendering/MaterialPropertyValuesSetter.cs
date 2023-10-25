// Decompiled with JetBrains decompiler
// Type: MaterialPropertyValuesSetter
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MaterialPropertyValuesSetter : MonoBehaviour
{
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [SerializeField]
  protected MaterialPropertyValuesSetter.PropertyNameFloatValuePair[] _floats;
  [SerializeField]
  protected MaterialPropertyValuesSetter.PropertyNameVectorValuePair[] _vectors;
  [SerializeField]
  protected MaterialPropertyValuesSetter.PropertyNameColorValuePair[] _colors;
  [SerializeField]
  protected MaterialPropertyValuesSetter.PropertyNameIntValuePair[] _ints;

  public virtual void Start()
  {
    this.RefreshPropertyIds();
    this.ApplyParams();
  }

  public virtual void OnValidate()
  {
    if ((UnityEngine.Object) this._materialPropertyBlockController == (UnityEngine.Object) null)
      this._materialPropertyBlockController = this.GetComponent<MaterialPropertyBlockController>();
    this.RefreshPropertyIds();
    this.ApplyParams();
  }

  public virtual void RefreshPropertyIds()
  {
    if (this._floats != null)
    {
      foreach (MaterialPropertyValuesSetter.PropertyValuePairBase propertyValuePairBase in this._floats)
        propertyValuePairBase.RefreshPropertyId();
    }
    if (this._vectors != null)
    {
      foreach (MaterialPropertyValuesSetter.PropertyValuePairBase vector in this._vectors)
        vector.RefreshPropertyId();
    }
    if (this._colors != null)
    {
      foreach (MaterialPropertyValuesSetter.PropertyValuePairBase color in this._colors)
        color.RefreshPropertyId();
    }
    if (this._ints == null)
      return;
    foreach (MaterialPropertyValuesSetter.PropertyValuePairBase propertyValuePairBase in this._ints)
      propertyValuePairBase.RefreshPropertyId();
  }

  public virtual void ApplyParams()
  {
    if (this._floats != null)
    {
      foreach (MaterialPropertyValuesSetter.PropertyNameFloatValuePair nameFloatValuePair in this._floats)
        this._materialPropertyBlockController.materialPropertyBlock.SetFloat(nameFloatValuePair.propertyId, nameFloatValuePair.value);
    }
    if (this._vectors != null)
    {
      foreach (MaterialPropertyValuesSetter.PropertyNameVectorValuePair vector in this._vectors)
        this._materialPropertyBlockController.materialPropertyBlock.SetVector(vector.propertyId, vector.vector);
    }
    if (this._colors != null)
    {
      foreach (MaterialPropertyValuesSetter.PropertyNameColorValuePair color in this._colors)
        this._materialPropertyBlockController.materialPropertyBlock.SetVector(color.propertyId, (Vector4) color.color);
    }
    if (this._ints != null)
    {
      foreach (MaterialPropertyValuesSetter.PropertyNameIntValuePair nameIntValuePair in this._ints)
        this._materialPropertyBlockController.materialPropertyBlock.SetInt(nameIntValuePair.propertyId, nameIntValuePair.value);
    }
    this._materialPropertyBlockController.ApplyChanges();
  }

  [Serializable]
  public class PropertyValuePairBase
  {
    [SerializeField]
    protected string _propertyName;
    [CompilerGenerated]
    protected int m_propertyId;

    public int propertyId
    {
      get => this.m_propertyId;
      private set => this.m_propertyId = value;
    }

    public PropertyValuePairBase() => this.RefreshPropertyId();

    public virtual void RefreshPropertyId() => this.propertyId = Shader.PropertyToID(this._propertyName);
  }

  [Serializable]
  public class PropertyNameFloatValuePair : MaterialPropertyValuesSetter.PropertyValuePairBase
  {
    public float value;
  }

  [Serializable]
  public class PropertyNameIntValuePair : MaterialPropertyValuesSetter.PropertyValuePairBase
  {
    public int value;
  }

  [Serializable]
  public class PropertyNameVectorValuePair : MaterialPropertyValuesSetter.PropertyValuePairBase
  {
    public Vector4 vector;
  }

  [Serializable]
  public class PropertyNameColorValuePair : MaterialPropertyValuesSetter.PropertyValuePairBase
  {
    public Color color;
  }
}
