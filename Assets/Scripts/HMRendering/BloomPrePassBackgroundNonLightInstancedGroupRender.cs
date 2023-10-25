// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundNonLightInstancedGroupRenderer
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class BloomPrePassBackgroundNonLightInstancedGroupRenderer : BloomPrePassNonLightPass
{
  [SerializeField]
  protected BloomPrePassBackgroundNonLightRenderer[] _renderers;
  [Space]
  [SerializeField]
  protected BloomPrePassBackgroundNonLightInstancedGroupRenderer.SupportedProperty[] _supportedProperties;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _worldSpaceCameraPosID = Shader.PropertyToID("_WorldSpaceCameraPos");
  protected const string kInternalMatricesCachingId = "INTERNAL_MATRICES";
  protected readonly Dictionary<string, float[]> _reusableFloatArrays = new Dictionary<string, float[]>();
  protected readonly Dictionary<string, Vector4[]> _reusableVectorArrays = new Dictionary<string, Vector4[]>();
  protected readonly Dictionary<string, Matrix4x4[]> _reusableMatrixArrays = new Dictionary<string, Matrix4x4[]>();
  protected int _reusableArraysSize;
  protected CommandBuffer _commandBuffer;
  protected MaterialPropertyBlock _reusableSetMaterialPropertyBlock;
  protected MaterialPropertyBlock _reusableGetMaterialPropertyBlock;

  public virtual void Awake() => this.InitIfNeeded();

  public virtual void InitIfNeeded()
  {
    if (this._commandBuffer == null)
      this._commandBuffer = new CommandBuffer()
      {
        name = "BloomPrePassBackgroundNonLightInstancedRenderer"
      };
    foreach (BloomPrePassBackgroundNonLightInstancedGroupRenderer.SupportedProperty supportedProperty in this._supportedProperties)
      supportedProperty.propertyId = Shader.PropertyToID(supportedProperty.propertyName);
    foreach (BloomPrePassBackgroundNonLightRenderer renderer in this._renderers)
      renderer.isPartOfInstancedRendering = true;
    if (this._reusableArraysSize != this._renderers.Length)
    {
      this._reusableFloatArrays.Clear();
      this._reusableVectorArrays.Clear();
      this._reusableMatrixArrays.Clear();
      this._reusableArraysSize = this._renderers.Length;
      this._reusableSetMaterialPropertyBlock = new MaterialPropertyBlock();
      this._reusableGetMaterialPropertyBlock = new MaterialPropertyBlock();
    }
    if (this._reusableGetMaterialPropertyBlock != null)
      return;
    this._reusableSetMaterialPropertyBlock = new MaterialPropertyBlock();
    this._reusableGetMaterialPropertyBlock = new MaterialPropertyBlock();
  }

  public override void Render(RenderTexture dest, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix)
  {
    if (this._renderers.Length == 0)
      return;
    Mesh sharedMesh = this._renderers[0].meshFilter.sharedMesh;
    Material sharedMaterial = this._renderers[0].renderer.sharedMaterial;
    this._commandBuffer.Clear();
    this._commandBuffer.SetRenderTarget((RenderTargetIdentifier) (Texture) dest);
    this._commandBuffer.SetViewProjectionMatrices(viewMatrix, projectionMatrix);
    this._commandBuffer.SetGlobalVector(BloomPrePassBackgroundNonLightInstancedGroupRenderer._worldSpaceCameraPosID, viewMatrix.GetColumn(3));
    if (this._renderers.Length == 1)
    {
      Debug.LogWarning((object) "Using BloomPrePassBackgroundNonLightInstancedRenderingSystem to render single Renderer, this add extra overhead with no benefit");
      this._commandBuffer.DrawRenderer(this._renderers[0].renderer, sharedMaterial, 0, 0);
    }
    else
    {
      Matrix4x4[] cachedMatrixArray1 = this.GetCachedMatrixArray("INTERNAL_MATRICES");
      for (int index = 0; index < this._renderers.Length; ++index)
      {
        BloomPrePassBackgroundNonLightRenderer renderer = this._renderers[index];
        cachedMatrixArray1[index] = !renderer.isActiveAndEnabled ? Matrix4x4.zero : renderer.cachedTransform.localToWorldMatrix;
        renderer.renderer.GetPropertyBlock(this._reusableGetMaterialPropertyBlock, 0);
        foreach (BloomPrePassBackgroundNonLightInstancedGroupRenderer.SupportedProperty supportedProperty in this._supportedProperties)
        {
          switch (supportedProperty.propertyType)
          {
            case BloomPrePassBackgroundNonLightInstancedGroupRenderer.PropertyType.Float:
              this.GetCachedFloatArray(supportedProperty.propertyName)[index] = this._reusableGetMaterialPropertyBlock.GetFloat(supportedProperty.propertyId);
              break;
            case BloomPrePassBackgroundNonLightInstancedGroupRenderer.PropertyType.Vector:
              this.GetCachedVectorArray(supportedProperty.propertyName)[index] = this._reusableGetMaterialPropertyBlock.GetVector(supportedProperty.propertyId);
              break;
            case BloomPrePassBackgroundNonLightInstancedGroupRenderer.PropertyType.Color:
              this.GetCachedVectorArray(supportedProperty.propertyName)[index] = (Vector4) this._reusableGetMaterialPropertyBlock.GetColor(supportedProperty.propertyId);
              break;
            case BloomPrePassBackgroundNonLightInstancedGroupRenderer.PropertyType.Matrix4x4:
              this.GetCachedMatrixArray(supportedProperty.propertyName)[index] = this._reusableGetMaterialPropertyBlock.GetMatrix(supportedProperty.propertyId);
              break;
          }
        }
      }
      foreach (BloomPrePassBackgroundNonLightInstancedGroupRenderer.SupportedProperty supportedProperty in this._supportedProperties)
      {
        switch (supportedProperty.propertyType)
        {
          case BloomPrePassBackgroundNonLightInstancedGroupRenderer.PropertyType.Float:
            float[] cachedFloatArray = this.GetCachedFloatArray(supportedProperty.propertyName);
            this._reusableSetMaterialPropertyBlock.SetFloatArray(supportedProperty.propertyId, cachedFloatArray);
            break;
          case BloomPrePassBackgroundNonLightInstancedGroupRenderer.PropertyType.Vector:
          case BloomPrePassBackgroundNonLightInstancedGroupRenderer.PropertyType.Color:
            Vector4[] cachedVectorArray = this.GetCachedVectorArray(supportedProperty.propertyName);
            this._reusableSetMaterialPropertyBlock.SetVectorArray(supportedProperty.propertyId, cachedVectorArray);
            break;
          case BloomPrePassBackgroundNonLightInstancedGroupRenderer.PropertyType.Matrix4x4:
            Matrix4x4[] cachedMatrixArray2 = this.GetCachedMatrixArray(supportedProperty.propertyName);
            this._reusableSetMaterialPropertyBlock.SetMatrixArray(supportedProperty.propertyId, cachedMatrixArray2);
            break;
        }
      }
      this._commandBuffer.DrawMeshInstanced(sharedMesh, 0, sharedMaterial, 0, cachedMatrixArray1, this._renderers.Length, this._reusableSetMaterialPropertyBlock);
    }
    Graphics.ExecuteCommandBuffer(this._commandBuffer);
  }

  public virtual Matrix4x4[] GetCachedMatrixArray(string propertyName)
  {
    Matrix4x4[] matrix4x4Array;
    return this._reusableMatrixArrays.TryGetValue(propertyName, out matrix4x4Array) ? matrix4x4Array : (this._reusableMatrixArrays[propertyName] = new Matrix4x4[this._renderers.Length]);
  }

  public virtual float[] GetCachedFloatArray(string propertyName)
  {
    float[] numArray;
    return this._reusableFloatArrays.TryGetValue(propertyName, out numArray) ? numArray : (this._reusableFloatArrays[propertyName] = new float[this._renderers.Length]);
  }

  public virtual Vector4[] GetCachedVectorArray(string propertyName)
  {
    Vector4[] vector4Array;
    return this._reusableVectorArrays.TryGetValue(propertyName, out vector4Array) ? vector4Array : (this._reusableVectorArrays[propertyName] = new Vector4[this._renderers.Length]);
  }

  [ContextMenu("AutoFill Renderers")]
  public virtual void AutoFillRenderers() => this._renderers = this.GetComponentsInChildren<BloomPrePassBackgroundNonLightRenderer>();

  [Serializable]
  public class SupportedProperty
  {
    public BloomPrePassBackgroundNonLightInstancedGroupRenderer.PropertyType propertyType;
    public string propertyName;
    [NonSerialized]
    public int propertyId;
  }

  public enum PropertyType
  {
    Float,
    Vector,
    Color,
    Matrix4x4,
  }
}
