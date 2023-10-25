// Decompiled with JetBrains decompiler
// Type: PlaybackRenderer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

[DefaultExecutionOrder(30400)]
public class PlaybackRenderer : MonoBehaviour
{
  [SerializeField]
  protected Shader _clearBackgroundShader;
  protected bool _isSetup;
  protected Transform _hmd;
  protected Camera _camera;
  protected PosesRecordingData.ExternalCameraCalibration _cameraCalibration;
  protected GameObject _clipQuad;
  protected Material _clipMaterial;
  protected PlaybackRenderer.PlaybackScreenshot[] _screenshots;

  public event System.Action texturesReadyEvent;

  public PlaybackRenderer.PlaybackScreenshot[] screenshots => this._screenshots;

  public virtual void Setup(
    Camera hmdCamera,
    Camera camera,
    PosesRecordingData.ExternalCameraCalibration cameraCalibration,
    int textureWidth,
    int textureHeight,
    PlaybackRenderer.PlaybackScreenshot[] screenshots)
  {
    this._hmd = hmdCamera.transform;
    this.InitCamera(camera, cameraCalibration);
    this.CreateClipQuad();
    this._screenshots = screenshots;
    this.CreateTextures(textureWidth, textureHeight);
    this._isSetup = true;
  }

  public virtual void CreateClipQuad()
  {
    this._clipMaterial = new Material(this._clearBackgroundShader);
    this._clipQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
    this._clipQuad.layer = LayerMask.NameToLayer("MRForegroundClipPlane");
    this._clipQuad.name = "ClipQuad";
    UnityEngine.Object.Destroy((UnityEngine.Object) this._clipQuad.GetComponent<MeshCollider>());
    this._clipQuad.transform.parent = this._camera.transform.parent;
    MeshRenderer component = this._clipQuad.GetComponent<MeshRenderer>();
    component.material = this._clipMaterial;
    component.shadowCastingMode = ShadowCastingMode.Off;
    component.receiveShadows = false;
    component.lightProbeUsage = LightProbeUsage.Off;
    component.reflectionProbeUsage = ReflectionProbeUsage.Off;
    Transform transform = this._clipQuad.transform;
    transform.localScale = new Vector3(1000f, 1000f, 1f);
    transform.localRotation = Quaternion.identity;
    this._clipQuad.SetActive(false);
  }

  public virtual void InitCamera(
    Camera camera,
    PosesRecordingData.ExternalCameraCalibration cameraCalibration)
  {
    this._camera = camera;
    this._camera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
    this._camera.depth = float.MaxValue;
    this._camera.stereoTargetEye = StereoTargetEyeMask.None;
    this._camera.useOcclusionCulling = false;
    this._camera.enabled = false;
    this._cameraCalibration = cameraCalibration;
    this._camera.fieldOfView = this._cameraCalibration.fieldOfVision;
    this._camera.nearClipPlane = this._cameraCalibration.nearClip;
    this._camera.farClipPlane = this._cameraCalibration.farClip;
  }

  public virtual void CreateTextures(int width, int height)
  {
    foreach (PlaybackRenderer.PlaybackScreenshot screenshot in this._screenshots)
      screenshot.CreateTexture(width, height);
  }

  public virtual float GetDistanceToHMD()
  {
    Transform transform = this._camera.transform;
    Vector3 vector3_1 = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
    Vector3 normalized = vector3_1.normalized;
    Vector3 position = this._hmd.position;
    vector3_1 = new Vector3(this._hmd.forward.x, 0.0f, this._hmd.forward.z);
    Vector3 vector3_2 = vector3_1.normalized * this._cameraCalibration.hmdOffset;
    Vector3 inPoint = position + vector3_2;
    return -new Plane(normalized, inPoint).GetDistanceToPoint(transform.position);
  }

  public virtual void OrientClipQuad()
  {
    float num = Mathf.Clamp(this.GetDistanceToHMD() + this._cameraCalibration.nearOffset, this._cameraCalibration.nearClip, this._cameraCalibration.farClip);
    Transform parent = this._clipQuad.transform.parent;
    this._clipQuad.transform.position = parent.position + parent.forward * num;
    this._clipQuad.transform.LookAt(new Vector3(parent.position.x, this._clipQuad.transform.position.y, parent.position.z));
  }

  public virtual void RenderForeground(PlaybackRenderer.PlaybackScreenshot screenshot)
  {
    CameraClearFlags clearFlags = this._camera.clearFlags;
    Color backgroundColor = this._camera.backgroundColor;
    this._camera.clearFlags = CameraClearFlags.Color;
    this._camera.backgroundColor = screenshot.backgroundColor;
    this._clipMaterial.SetColor("_Color", screenshot.backgroundColor);
    this._clipQuad.SetActive(true);
    this._camera.cullingMask = (int) screenshot.layerMask;
    this._camera.targetTexture = screenshot.texture;
    screenshot.texture.DiscardContents(true, true);
    int num = GL.sRGBWrite ? 1 : 0;
    GL.sRGBWrite = true;
    this._camera.Render();
    GL.sRGBWrite = num != 0;
    this._clipQuad.SetActive(false);
    this._camera.clearFlags = clearFlags;
    this._camera.backgroundColor = backgroundColor;
  }

  public virtual void RenderBackground(PlaybackRenderer.PlaybackScreenshot screenshot)
  {
    CameraClearFlags clearFlags = this._camera.clearFlags;
    Color backgroundColor = this._camera.backgroundColor;
    this._camera.clearFlags = CameraClearFlags.Color;
    this._camera.backgroundColor = screenshot.backgroundColor;
    this._camera.cullingMask = (int) screenshot.layerMask;
    this._camera.targetTexture = screenshot.texture;
    screenshot.texture.DiscardContents(true, true);
    int num = GL.sRGBWrite ? 1 : 0;
    GL.sRGBWrite = true;
    this._camera.Render();
    GL.sRGBWrite = num != 0;
    this._camera.clearFlags = clearFlags;
    this._camera.backgroundColor = backgroundColor;
  }

  public virtual void LateUpdate()
  {
    if (!this._isSetup)
      return;
    this.OrientClipQuad();
    foreach (PlaybackRenderer.PlaybackScreenshot screenshot in this._screenshots)
    {
      if (screenshot.type == PlaybackRenderer.PlaybackScreenshot.Type.Foreground)
        this.RenderForeground(screenshot);
      else
        this.RenderBackground(screenshot);
    }
    System.Action texturesReadyEvent = this.texturesReadyEvent;
    if (texturesReadyEvent == null)
      return;
    texturesReadyEvent();
  }

  [Serializable]
  public class PlaybackScreenshot
  {
    [SerializeField]
    protected string _name;
    [SerializeField]
    protected LayerMask _layerMask;
    [SerializeField]
    protected PlaybackRenderer.PlaybackScreenshot.Type _type;
    protected RenderTexture _texture;
    protected string _path;
    protected Color _backgroundColor;

    public string name => this._name;

    public RenderTexture texture => this._texture;

    public LayerMask layerMask => this._layerMask;

    public PlaybackRenderer.PlaybackScreenshot.Type type => this._type;

    public string path
    {
      get => this._path;
      set => this._path = value;
    }

    public Color backgroundColor => this._backgroundColor;

    public PlaybackScreenshot(
      string name,
      LayerMask layerMask,
      PlaybackRenderer.PlaybackScreenshot.Type type,
      Color backgroundColor)
    {
      this._name = name;
      this._layerMask = layerMask;
      this._type = type;
      this._path = name;
      this._backgroundColor = backgroundColor;
    }

    public virtual void CreateTexture(int width, int height)
    {
      RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
      renderTexture.antiAliasing = 1;
      renderTexture.wrapMode = TextureWrapMode.Clamp;
      renderTexture.useMipMap = false;
      renderTexture.anisoLevel = 0;
      this._texture = renderTexture;
    }

    public enum Type
    {
      Foreground = 1,
      Background = 2,
    }
  }
}
