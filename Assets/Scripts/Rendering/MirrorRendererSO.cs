// Decompiled with JetBrains decompiler
// Type: MirrorRendererSO
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRendererSO : PersistentScriptableObject
{
  [SerializeField]
  private LayerMask _reflectLayers = (LayerMask)(-1);
  [SerializeField]
  private int _stereoTextureWidth = 2048;
  [SerializeField]
  private int _stereoTextureHeight = 1024;
  [SerializeField]
  private int _monoTextureWidth = 256;
  [SerializeField]
  private int _monoTextureHeight = 256;
  [SerializeField]
  private int _maxAntiAliasing = 1;
  [SerializeField]
  private bool _disableDepthTexture = true;
  [SerializeField]
  private bool _enableBloomPrePass;
  [Space]
  [SerializeField]
  private BloomPrePassRendererSO _bloomPrePassRenderer;
  [SerializeField]
  private BloomPrePassEffectSO _bloomPrePassEffect;
  [SerializeField]
  private Shader _clearDepthShader;
  private RenderTexture _bloomPrePassRenderTexture;
  private Camera _mirrorCamera;
  private int _antialiasing;
  private Dictionary<MirrorRendererSO.CameraTransformData, RenderTexture> _renderTextures = new Dictionary<MirrorRendererSO.CameraTransformData, RenderTexture>(4);
  private readonly Rect kLeftRect = new Rect(0.0f, 0.0f, 0.5f, 1f);
  private readonly Rect kRightRect = new Rect(0.5f, 0.0f, 0.5f, 1f);
  private readonly Rect kFullRect = new Rect(0.0f, 0.0f, 1f, 1f);
  private const int kWaterLayer = 4;

  private void OnValidate() => this.ValidateParams();

  protected void Awake() => this.ValidateParams();

  private void ValidateParams()
  {
    if (this._maxAntiAliasing != 1 && this._maxAntiAliasing != 2 && this._maxAntiAliasing != 4 && this._maxAntiAliasing != 8)
      this._maxAntiAliasing = 1;
    this._antialiasing = Mathf.Min(QualitySettings.antiAliasing, this._maxAntiAliasing);
    if (this._antialiasing == 1 || this._antialiasing == 2 || this._antialiasing == 4 || this._antialiasing == 8)
      return;
    this._antialiasing = 1;
  }

  public void Init(
    LayerMask reflectLayers,
    int stereoTextureWidth,
    int stereoTextureHeight,
    int monoTextureWidth,
    int monoTextureHeight,
    int maxAntiAliasing,
    bool enableBloomPrePass)
  {
    this._reflectLayers = reflectLayers;
    this._stereoTextureWidth = stereoTextureWidth;
    this._stereoTextureHeight = stereoTextureHeight;
    this._monoTextureWidth = monoTextureWidth;
    this._monoTextureHeight = monoTextureHeight;
    this._maxAntiAliasing = maxAntiAliasing;
    this._enableBloomPrePass = enableBloomPrePass;
  }

  public void PrepareForNextFrame()
  {
    foreach (RenderTexture temp in this._renderTextures.Values)
      RenderTexture.ReleaseTemporary(temp);
    this._renderTextures.Clear();
  }

  public Texture GetMirrorTexture(Vector3 reflectionPlanePos, Vector3 reflectionPlaneNormal)
  {
    if ((int) this._reflectLayers == 0)
      return (Texture) null;
    Camera current = Camera.current;
    if (!(bool) (UnityEngine.Object) current || (UnityEngine.Object) current == (UnityEngine.Object) this._mirrorCamera)
      return (Texture) null;
    int frameCount = Time.frameCount;
    Transform transform = current.transform;
    Vector3 position = transform.position;
    Quaternion rotation = transform.rotation;
    bool stereoEnabled = current.stereoEnabled;
    if ((double) new UnityEngine.Plane(reflectionPlaneNormal, reflectionPlanePos).GetDistanceToPoint(position) <= (double) Mathf.Epsilon || current.orthographic && (double) Mathf.Abs(Vector3.Dot(transform.forward, reflectionPlaneNormal)) <= (double) Mathf.Epsilon)
      return (Texture) null;
    MirrorRendererSO.CameraTransformData key = new MirrorRendererSO.CameraTransformData(position, rotation, current.fieldOfView, stereoEnabled);
    RenderTexture renderTexture;
    if (this._renderTextures.TryGetValue(key, out renderTexture))
      return (Texture) renderTexture;
    renderTexture = !stereoEnabled ? RenderTexture.GetTemporary(this._monoTextureWidth, this._monoTextureHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, this._antialiasing) : RenderTexture.GetTemporary(this._stereoTextureWidth, this._stereoTextureHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, this._antialiasing);
    this._renderTextures[key] = renderTexture;
    this.CreateOrUpdateMirrorCamera(current, renderTexture);
    float stereoCameraEyeOffset = 0.0f;
    if (this._enableBloomPrePass)
    {
      Matrix4x4 projectionMatrix;
      Matrix4x4 viewMatrix1;
      this._bloomPrePassRenderer.GetCameraParams(current, out projectionMatrix, out viewMatrix1, out stereoCameraEyeOffset);
      Matrix4x4 viewMatrix2 = viewMatrix1 * MirrorRendererSO.CalculateReflectionMatrix(MirrorRendererSO.Plane(reflectionPlanePos, reflectionPlaneNormal));
      Vector2 textureToScreenRatio;
      ToneMapping toneMapping;
      this._bloomPrePassRenderer.RenderAndSetData(position, projectionMatrix, viewMatrix2, stereoCameraEyeOffset, (IBloomPrePassParams) this._bloomPrePassEffect, this._bloomPrePassRenderTexture, out textureToScreenRatio, out toneMapping);
      this._bloomPrePassRenderer.EnableBloomFog();
      BloomPrePassRendererSO.SetDataToShaders(stereoCameraEyeOffset, textureToScreenRatio, (Texture) this._bloomPrePassRenderTexture, toneMapping);
    }
    int num = GL.invertCulling ? 1 : 0;
    GL.invertCulling = num == 0;
    if (current.stereoEnabled)
    {
      Quaternion camRotation = rotation;
      if (current.stereoTargetEye == StereoTargetEyeMask.Both || current.stereoTargetEye == StereoTargetEyeMask.Left)
      {
        Vector3 worldPoint = current.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f), Camera.MonoOrStereoscopicEye.Left);
        Matrix4x4 projectionMatrix = current.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
        this._bloomPrePassRenderer.SetCustomStereoCameraEyeOffset(stereoCameraEyeOffset);
        this.RenderMirror(worldPoint, camRotation, projectionMatrix, this.kLeftRect, reflectionPlanePos, reflectionPlaneNormal);
      }
      if (current.stereoTargetEye == StereoTargetEyeMask.Both || current.stereoTargetEye == StereoTargetEyeMask.Right)
      {
        Vector3 worldPoint = current.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f), Camera.MonoOrStereoscopicEye.Right);
        Matrix4x4 projectionMatrix = current.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
        this._bloomPrePassRenderer.SetCustomStereoCameraEyeOffset(-stereoCameraEyeOffset);
        this.RenderMirror(worldPoint, camRotation, projectionMatrix, this.kRightRect, reflectionPlanePos, reflectionPlaneNormal);
      }
    }
    else
      this.RenderMirror(position, rotation, current.projectionMatrix, this.kFullRect, reflectionPlanePos, reflectionPlaneNormal);
    GL.invertCulling = num != 0;
    GL.Flush();
    return (Texture) renderTexture;
  }

  private void RenderMirror(
    Vector3 camPosition,
    Quaternion camRotation,
    Matrix4x4 camProjectionMatrix,
    Rect screenRect,
    Vector3 reclectionPlanePos,
    Vector3 reflectionPlaneNormal)
  {
    this._mirrorCamera.rect = screenRect;
    this._mirrorCamera.projectionMatrix = camProjectionMatrix;
    Matrix4x4 reflectionMatrix = MirrorRendererSO.CalculateReflectionMatrix(MirrorRendererSO.Plane(reclectionPlanePos, reflectionPlaneNormal));
    this._mirrorCamera.ResetWorldToCameraMatrix();
    this._mirrorCamera.transform.SetPositionAndRotation(camPosition, camRotation);
    this._mirrorCamera.worldToCameraMatrix *= reflectionMatrix;
    this._mirrorCamera.projectionMatrix = this._mirrorCamera.CalculateObliqueMatrix(MirrorRendererSO.CameraSpacePlane(this._mirrorCamera.worldToCameraMatrix, reclectionPlanePos, reflectionPlaneNormal));
    this._mirrorCamera.Render();
  }

  protected void OnDisable()
  {
    if ((bool) (UnityEngine.Object) this._mirrorCamera)
    {
      EssentialHelpers.SafeDestroy((UnityEngine.Object) this._mirrorCamera.gameObject);
      this._mirrorCamera = (Camera) null;
    }
    foreach (RenderTexture temp in this._renderTextures.Values)
      RenderTexture.ReleaseTemporary(temp);
    this._renderTextures.Clear();
    if (!(bool) (UnityEngine.Object) this._bloomPrePassRenderTexture)
      return;
    this._bloomPrePassRenderTexture.Release();
    EssentialHelpers.SafeDestroy((UnityEngine.Object) this._bloomPrePassRenderTexture);
    this._bloomPrePassRenderTexture = (RenderTexture) null;
  }

  private void CreateOrUpdateMirrorCamera(Camera currentCamera, RenderTexture renderTexture)
  {
    if (!(bool) (UnityEngine.Object) this._mirrorCamera)
    {
      GameObject gameObject = new GameObject("MirrorCam" + (object) this.GetInstanceID(), new Type[1]
      {
        typeof (Camera)
      });
      gameObject.hideFlags = HideFlags.HideAndDontSave;
      this._mirrorCamera = gameObject.GetComponent<Camera>();
      this._mirrorCamera.enabled = false;
    }
    if (this._enableBloomPrePass)
      this._bloomPrePassRenderTexture = this._bloomPrePassRenderer.CreateBloomPrePassRenderTextureIfNeeded(this._bloomPrePassRenderTexture, (IBloomPrePassParams) this._bloomPrePassEffect);
    this._mirrorCamera.CopyFrom(currentCamera);
    this._mirrorCamera.targetTexture = renderTexture;
    if (this._disableDepthTexture)
      this._mirrorCamera.depthTextureMode = DepthTextureMode.None;
    this._mirrorCamera.cullingMask = -17 & this._reflectLayers.value & currentCamera.cullingMask;
    this._mirrorCamera.clearFlags = CameraClearFlags.Color;
  }

  private static Vector4 Plane(Vector3 pos, Vector3 normal) => new Vector4(normal.x, normal.y, normal.z, -Vector3.Dot(pos, normal));

  private static Vector4 CameraSpacePlane(
    Matrix4x4 worldToCameraMatrix,
    Vector3 pos,
    Vector3 normal)
  {
    return MirrorRendererSO.Plane(worldToCameraMatrix.MultiplyPoint(pos), worldToCameraMatrix.MultiplyVector(normal).normalized);
  }

    private static Matrix4x4 CalculateReflectionMatrix(Vector4 plane)
    {
        Matrix4x4 identity = Matrix4x4.identity;
        identity.m00 = 1f - 2f * plane[0] * plane[0];
        identity.m01 = -2f * plane[0] * plane[1];
        identity.m02 = -2f * plane[0] * plane[2];
        identity.m03 = -2f * plane[3] * plane[0];
        identity.m10 = -2f * plane[1] * plane[0];
        identity.m11 = 1f - 2f * plane[1] * plane[1];
        identity.m12 = -2f * plane[1] * plane[2];
        identity.m13 = -2f * plane[3] * plane[1];
        identity.m20 = -2f * plane[2] * plane[0];
        identity.m21 = -2f * plane[2] * plane[1];
        identity.m22 = 1f - 2f * plane[2] * plane[2];
        identity.m23 = -2f * plane[3] * plane[2];
        identity.m30 = 0f;
        identity.m31 = 0f;
        identity.m32 = 0f;
        identity.m33 = 1f;
        return identity;
    }

    private struct CameraTransformData
  {
    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly float fov;
    public readonly bool stereoEnabled;

    public CameraTransformData(
      Vector3 position,
      Quaternion rotation,
      float fov,
      bool stereoEnabled)
    {
      this.position = position;
      this.rotation = rotation;
      this.fov = fov;
      this.stereoEnabled = stereoEnabled;
    }

    public override bool Equals(object obj) => obj is MirrorRendererSO.CameraTransformData cameraTransformData && cameraTransformData.position == this.position && cameraTransformData.rotation == this.rotation && (double) cameraTransformData.fov == (double) this.fov && cameraTransformData.stereoEnabled == this.stereoEnabled;

    public override int GetHashCode() => this.position.GetHashCode() ^ this.rotation.GetHashCode() ^ this.fov.GetHashCode() ^ this.stereoEnabled.GetHashCode();
  }
}
