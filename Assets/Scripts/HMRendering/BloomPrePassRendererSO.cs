// Decompiled with JetBrains decompiler
// Type: BloomPrePassRendererSO
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BloomPrePassRendererSO : PersistentScriptableObject
{
  [SerializeField]
  protected BloomFogSO _bloomFog;
  [Space]
  [SerializeField]
  protected BloomPrePassRendererSO.PreallocationData[] _preallocationData;
  protected readonly Dictionary<BloomPrePassLightTypeSO, BloomPrePassRendererSO.LightsRenderingData> _lightsRenderingData = new Dictionary<BloomPrePassLightTypeSO, BloomPrePassRendererSO.LightsRenderingData>();
  protected CommandBuffer _commandBuffer;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _vertexTransformMatrixID = Shader.PropertyToID("_VertexTransformMatrix");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _bloomPrePassTextureID = Shader.PropertyToID("_BloomPrePassTexture");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _stereoCameraEyeOffsetID = Shader.PropertyToID("_StereoCameraEyeOffset");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _customFogTextureToScreenRatioID = Shader.PropertyToID("_CustomFogTextureToScreenRatio");
  protected bool _initialized;
  protected Texture2D _blackTexture;
  protected RenderTexture _lowestResBloomTexture;

  protected override void OnEnable()
  {
    base.OnEnable();
    this.Init();
  }

  public virtual void OnDisable() => this.Cleanup();

  public virtual void Init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    foreach (BloomPrePassRendererSO.PreallocationData preallocationData in this._preallocationData)
    {
      BloomPrePassRendererSO.LightsRenderingData data;
      if (!this._lightsRenderingData.TryGetValue(preallocationData.lightType, out data))
      {
        data = new BloomPrePassRendererSO.LightsRenderingData();
        this._lightsRenderingData[preallocationData.lightType] = data;
      }
      this.PrepareLightsMeshRendering(preallocationData.lightType, data, preallocationData.preallocateCount);
    }
    this._blackTexture = Texture2D.blackTexture;
  }

  public virtual void Cleanup()
  {
    if (!this._initialized)
      return;
    foreach (KeyValuePair<BloomPrePassLightTypeSO, BloomPrePassRendererSO.LightsRenderingData> keyValuePair in this._lightsRenderingData)
      EssentialHelpers.SafeDestroy((UnityEngine.Object) keyValuePair.Value.mesh);
    this._lightsRenderingData.Clear();
    this._initialized = false;
  }

  public virtual void RenderAndSetData(
    Vector3 cameraPos,
    Matrix4x4 projectionMatrix,
    Matrix4x4 viewMatrix,
    float stereoCameraEyeOffset,
    IBloomPrePassParams bloomPrePassParams,
    RenderTexture dest,
    out Vector2 textureToScreenRatio,
    out ToneMapping toneMapping)
  {
    this._bloomFog.UpdateShaderParams();
    textureToScreenRatio.x = Mathf.Clamp01((float) (1.0 / ((double) Mathf.Tan((float) ((double) bloomPrePassParams.fov.x * 0.5 * (Math.PI / 180.0))) * (double) projectionMatrix.m00)));
    textureToScreenRatio.y = Mathf.Clamp01((float) (1.0 / ((double) Mathf.Tan((float) ((double) bloomPrePassParams.fov.y * 0.5 * (Math.PI / 180.0))) * (double) projectionMatrix.m11)));
    projectionMatrix.m00 *= textureToScreenRatio.x;
    projectionMatrix.m02 *= textureToScreenRatio.x;
    projectionMatrix.m11 *= textureToScreenRatio.y;
    projectionMatrix.m12 *= textureToScreenRatio.y;
    this.EnableBloomFog();
    BloomPrePassRendererSO.SetDataToShaders(0.0f, Vector2.one, (Texture) this._blackTexture, bloomPrePassParams.toneMapping);
    RenderTexture temporary = RenderTexture.GetTemporary(bloomPrePassParams.textureWidth, bloomPrePassParams.textureHeight, 0, RenderTextureFormat.RGB111110Float, RenderTextureReadWrite.Linear);
    Graphics.SetRenderTarget(temporary);
    GL.Clear(true, true, Color.black);
    this.RenderAllLights(viewMatrix, projectionMatrix, bloomPrePassParams.linesWidth);
    if (!SystemInfo.usesReversedZBuffer)
    {
      projectionMatrix.m11 *= -1f;
      projectionMatrix.m12 *= -1f;
    }
    foreach (BloomPrePassNonLightPass prePassBeforeBlur in BloomPrePassNonLightPass.bloomPrePassBeforeBlurList)
      prePassBeforeBlur.Render(temporary, viewMatrix, projectionMatrix);
    dest.DiscardContents();
    bloomPrePassParams.textureEffect.Render(temporary, dest);
    RenderTexture.ReleaseTemporary(temporary);
    toneMapping = bloomPrePassParams.toneMapping;
    toneMapping.SetShaderKeyword();
    foreach (BloomPrePassNonLightPass prePassAfterBlur in BloomPrePassNonLightPass.bloomPrePassAfterBlurList)
      prePassAfterBlur.Render(dest, viewMatrix, projectionMatrix);
  }

  public static void SetDataToShaders(
    float stereoCameraEyeOffset,
    Vector2 textureToScreenRatio,
    Texture bloomFogTexture,
    ToneMapping toneMapping)
  {
    Shader.SetGlobalTexture(BloomPrePassRendererSO._bloomPrePassTextureID, bloomFogTexture);
    Shader.SetGlobalFloat(BloomPrePassRendererSO._stereoCameraEyeOffsetID, stereoCameraEyeOffset);
    Shader.SetGlobalVector(BloomPrePassRendererSO._customFogTextureToScreenRatioID, (Vector4) textureToScreenRatio);
    toneMapping.SetShaderKeyword();
  }

  public virtual void SetCustomStereoCameraEyeOffset(float stereoCameraEyeOffset) => Shader.SetGlobalFloat(BloomPrePassRendererSO._stereoCameraEyeOffsetID, stereoCameraEyeOffset);

  public virtual RenderTexture CreateBloomPrePassRenderTextureIfNeeded(
    RenderTexture renderTexture,
    IBloomPrePassParams bloomPrePassParams)
  {
    if ((UnityEngine.Object) renderTexture != (UnityEngine.Object) null && renderTexture.width == bloomPrePassParams.textureWidth && renderTexture.height == bloomPrePassParams.textureHeight)
      return renderTexture;
    if ((UnityEngine.Object) renderTexture != (UnityEngine.Object) null)
    {
      renderTexture.Release();
      EssentialHelpers.SafeDestroy((UnityEngine.Object) renderTexture);
    }
    renderTexture = new RenderTexture(bloomPrePassParams.textureWidth, bloomPrePassParams.textureHeight, 0, RenderTextureFormat.RGB111110Float, RenderTextureReadWrite.Linear);
    renderTexture.name = "BloomRenderTexture";
    return renderTexture;
  }

  public virtual void EnableBloomFog() => this._bloomFog.bloomFogEnabled = true;

  public virtual void DisableBloomFog() => this._bloomFog.bloomFogEnabled = false;

  public virtual void UpdateBloomFogParams() => this._bloomFog.UpdateShaderParams();

  public virtual void GetCameraParams(
    Camera camera,
    out Matrix4x4 projectionMatrix,
    out Matrix4x4 viewMatrix,
    out float stereoCameraEyeOffset)
  {
    viewMatrix = camera.worldToCameraMatrix;
    if (camera.stereoEnabled)
    {
      Matrix4x4 projectionMatrix1 = camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
      Matrix4x4 projectionMatrix2 = camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
      stereoCameraEyeOffset = (float) (-((double) projectionMatrix1.m02 - (double) projectionMatrix2.m02) * 0.25);
      projectionMatrix = this.MatrixLerp(projectionMatrix1, projectionMatrix2, 0.5f);
    }
    else
    {
      stereoCameraEyeOffset = 0.0f;
      projectionMatrix = camera.projectionMatrix;
    }
  }

  public virtual void RenderAllLights(
    Matrix4x4 viewMatrix,
    Matrix4x4 projectionMatrix,
    float linesWidth)
  {
    List<BloomPrePassLight.LightsDataItem> lightsDataItems = BloomPrePassLight.lightsDataItems;
    if (lightsDataItems == null)
      return;
    if (this._commandBuffer == null)
      this._commandBuffer = new CommandBuffer()
      {
        name = "Bloom Pre Pass Render"
      };
    else
      this._commandBuffer.Clear();
    foreach (BloomPrePassLight.LightsDataItem lightsDataItem in lightsDataItems)
    {
      BloomPrePassLightTypeSO lightType = lightsDataItem.lightType;
      HashSet<BloomPrePassLight> lights = lightsDataItem.lights;
      int count = lights.Count;
      BloomPrePassRendererSO.LightsRenderingData data;
      if (!this._lightsRenderingData.TryGetValue(lightType, out data))
      {
        Debug.LogWarning((object) string.Format("Did not preallocate mesh for {0}, generating on the fly.", (object) lightType));
        data = new BloomPrePassRendererSO.LightsRenderingData();
        this._lightsRenderingData[lightType] = data;
      }
      this.PrepareLightsMeshRendering(lightType, data, count);
      this._commandBuffer.DrawMesh(data.mesh, Matrix4x4.identity, lightType.material);
      lightType.material.SetMatrix(BloomPrePassRendererSO._vertexTransformMatrixID, Matrix4x4.Ortho(0.0f, 1f, 1f, 0.0f, -1f, 1f));
      int lightNum = 0;
      foreach (BloomPrePassLight bloomPrePassLight in lights)
        bloomPrePassLight.FillMeshData(ref lightNum, data.lightQuads, viewMatrix, projectionMatrix, linesWidth);
      data.subMeshDescriptor.indexCount = 6 * lightNum;
      data.mesh.SetSubMesh(0, data.subMeshDescriptor, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
      data.mesh.SetVertexBufferData<BloomPrePassLight.QuadData>(data.lightQuads, 0, 0, data.lightQuads.Length, flags: MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
    }
    Graphics.ExecuteCommandBuffer(this._commandBuffer);
  }

  public virtual void PrepareLightsMeshRendering(
    BloomPrePassLightTypeSO lightType,
    BloomPrePassRendererSO.LightsRenderingData data,
    int numberOfLights)
  {
    if (data.lightQuads == null || data.lightQuads.Length < numberOfLights)
    {
      numberOfLights = (numberOfLights + 64 - 1) / 64 * 64;
      data.lightQuads = new BloomPrePassLight.QuadData[numberOfLights];
    }
    if ((bool) (UnityEngine.Object) data.mesh && data.mesh.vertexCount >= numberOfLights * 4)
      return;
    if ((bool) (UnityEngine.Object) data.mesh)
    {
      Debug.LogWarning((object) (string.Format("Reallocating BloomPrePass mesh for {0}. ", (object) lightType) + string.Format("Current Mesh supports {0} lights, ", (object) (data.mesh.vertexCount / 4)) + string.Format("but need to support {0}", (object) numberOfLights)));
      data.mesh.Clear();
    }
    else
    {
      data.mesh = new Mesh();
      data.mesh.name = "BloomPrePassRenderer";
      data.mesh.MarkDynamic();
    }
    ushort[] data1 = new ushort[numberOfLights * 2 * 3];
    for (int index = 0; index < numberOfLights; ++index)
    {
      data1[index * 6] = (ushort) (index * 4);
      data1[index * 6 + 1] = (ushort) (index * 4 + 1);
      data1[index * 6 + 2] = (ushort) (index * 4 + 2);
      data1[index * 6 + 3] = (ushort) (index * 4 + 2);
      data1[index * 6 + 4] = (ushort) (index * 4 + 3);
      data1[index * 6 + 5] = (ushort) (index * 4);
    }
    data.mesh.SetVertexBufferParams(4 * data.lightQuads.Length, new VertexAttributeDescriptor(), new VertexAttributeDescriptor(VertexAttribute.Tangent), new VertexAttributeDescriptor(VertexAttribute.Color, dimension: 4), new VertexAttributeDescriptor(VertexAttribute.TexCoord0));
    data.mesh.SetIndexBufferParams(data1.Length, IndexFormat.UInt16);
    data.mesh.SetIndexBufferData<ushort>(data1, 0, 0, data1.Length);
  }

  public virtual Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float t)
  {
    Matrix4x4 matrix4x4 = new Matrix4x4();
    for (int index = 0; index < 16; ++index)
      matrix4x4[index] = Mathf.Lerp(from[index], to[index], t);
    return matrix4x4;
  }

  [Serializable]
  public class PreallocationData
  {
    public BloomPrePassLightTypeSO lightType;
    public int preallocateCount = 10;
  }

  public class LightsRenderingData
  {
    public Mesh mesh;
    public BloomPrePassLight.QuadData[] lightQuads;
    public SubMeshDescriptor subMeshDescriptor;
  }
}
