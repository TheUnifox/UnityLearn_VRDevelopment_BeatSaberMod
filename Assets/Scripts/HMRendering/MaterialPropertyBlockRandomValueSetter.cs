// Decompiled with JetBrains decompiler
// Type: MaterialPropertyBlockRandomValueSetter
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class MaterialPropertyBlockRandomValueSetter : MonoBehaviour
{
  [SerializeField]
  protected Renderer[] _renderers;
  [SerializeField]
  protected string _propertyName;
  [SerializeField]
  protected float _minValue;
  [SerializeField]
  protected float _maxValue = 1f;
  protected MaterialPropertyBlock[] _materialPropertyBlocks;
  protected int _propertyId;

  public virtual void Start() => this.ApplyParams();

  public virtual void OnValidate()
  {
    this.RefreshPropertyId();
    this.ApplyParams();
  }

  public virtual void RefreshPropertyId() => this._propertyId = Shader.PropertyToID(this._propertyName);

  public virtual void ApplyParams()
  {
    if (this._materialPropertyBlocks == null || this._materialPropertyBlocks.Length != this._renderers.Length)
      this._materialPropertyBlocks = new MaterialPropertyBlock[this._renderers.Length];
    for (int index = 0; index < this._renderers.Length; ++index)
    {
      if (this._materialPropertyBlocks[index] == null)
        this._materialPropertyBlocks[index] = new MaterialPropertyBlock();
      this._materialPropertyBlocks[index].SetFloat(this._propertyId, Random.Range(this._minValue, this._maxValue));
      for (int materialIndex = 0; materialIndex < this._renderers[index].sharedMaterials.Length; ++materialIndex)
        this._renderers[index].SetPropertyBlock(this._materialPropertyBlocks[index], materialIndex);
    }
  }
}
