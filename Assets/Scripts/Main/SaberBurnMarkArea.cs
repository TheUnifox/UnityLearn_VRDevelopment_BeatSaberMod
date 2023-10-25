// Decompiled with JetBrains decompiler
// Type: SaberBurnMarkArea
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

[RequireComponent(typeof (Renderer))]
public class SaberBurnMarkArea : MonoBehaviour
{
  [SerializeField]
  protected LineRenderer _saberBurnMarkLinePrefab;
  [SerializeField]
  protected float _blackMarkLineRandomOffset = 1f / 1000f;
  [SerializeField]
  protected int _textureWidth = 1024;
  [SerializeField]
  protected int _textureHeight = 512;
  [SerializeField]
  protected float _burnMarksFadeOutStrength = 0.3f;
  [SerializeField]
  protected Shader _fadeOutShader;
  [Inject]
  protected readonly ColorManager _colorManager;
  [Inject]
  protected readonly SaberManager _saberManager;
  protected Renderer _renderer;
  protected readonly int _fadeOutStrengthShaderPropertyID = Shader.PropertyToID("_FadeOutStrength");
  protected Saber[] _sabers;
  protected Plane _plane;
  protected Vector3[] _prevBurnMarkPos;
  protected bool[] _prevBurnMarkPosValid;
  protected LineRenderer[] _lineRenderers;
  protected Camera _camera;
  protected Vector3[] _linePoints;
  protected RenderTexture[] _renderTextures;
  protected ParticleSystem.EmitParams _emitParams;
  protected Material _fadeOutMaterial;

  public virtual void Start()
  {
    if ((Object) this._saberManager == (Object) null)
    {
      this.enabled = false;
    }
    else
    {
      this._sabers = new Saber[2];
      this._sabers[0] = this._saberManager.leftSaber;
      this._sabers[1] = this._saberManager.rightSaber;
      this._emitParams = new ParticleSystem.EmitParams();
      this._emitParams.applyShapeToPosition = true;
      this._prevBurnMarkPos = new Vector3[2];
      this._prevBurnMarkPosValid = new bool[2];
      this._renderer = this.GetComponent<Renderer>();
      this._renderer.enabled = true;
      this._plane = new Plane(this.transform.up, this.transform.position);
      this._linePoints = new Vector3[100];
      this._lineRenderers = new LineRenderer[2];
      int num = 31;
      for (int index = 0; index < 2; ++index)
      {
        Color color = this._colorManager.EffectsColorForSaberType(this._sabers[index].saberType);
        this._lineRenderers[index] = Object.Instantiate<LineRenderer>(this._saberBurnMarkLinePrefab, Vector3.zero, Quaternion.identity, (Transform) null);
        this._lineRenderers[index].startColor = color;
        this._lineRenderers[index].endColor = color;
        this._lineRenderers[index].positionCount = 2;
        new Quaternion().eulerAngles = new Vector3(-90f, 0.0f, 0.0f);
        this._prevBurnMarkPosValid[index] = false;
      }
      this._renderTextures = new RenderTexture[2];
      this._renderTextures[0] = new RenderTexture(this._textureWidth, this._textureHeight, 0, RenderTextureFormat.ARGB32);
      this._renderTextures[0].name = "SaberBurnMarkArea Textue 0";
      this._renderTextures[0].hideFlags = HideFlags.DontSave;
      this._renderTextures[1] = new RenderTexture(this._textureWidth, this._textureHeight, 0, RenderTextureFormat.ARGB32);
      this._renderTextures[1].name = "SaberBurnMarkArea Textue 1";
      this._renderTextures[1].hideFlags = HideFlags.DontSave;
      this._camera = new GameObject("BurnMarksCamera").AddComponent<Camera>();
      this._camera.name = "BurnMarksCamera";
      this._camera.orthographic = true;
      this._camera.orthographicSize = 1f;
      this._camera.nearClipPlane = 0.0f;
      this._camera.farClipPlane = 1f;
      this._camera.clearFlags = CameraClearFlags.Nothing;
      this._camera.backgroundColor = Color.black;
      this._camera.cullingMask = 1 << num;
      this._camera.targetTexture = this._renderTextures[0];
      this._camera.allowMSAA = false;
      this._camera.allowHDR = false;
      this._camera.enabled = false;
      this._renderer.sharedMaterial.mainTexture = (Texture) this._renderTextures[1];
      this._fadeOutMaterial = new Material(this._fadeOutShader);
      this._fadeOutMaterial.hideFlags = HideFlags.HideAndDontSave;
      this._fadeOutMaterial.mainTexture = (Texture) this._renderTextures[0];
    }
  }

  public virtual void OnDestroy()
  {
    if ((bool) (Object) this._camera)
      Object.Destroy((Object) this._camera.gameObject);
    if (this._lineRenderers != null && (bool) (Object) this._lineRenderers[0])
      Object.Destroy((Object) this._lineRenderers[0].gameObject);
    if (this._lineRenderers != null && (bool) (Object) this._lineRenderers[1])
      Object.Destroy((Object) this._lineRenderers[1].gameObject);
    EssentialHelpers.SafeDestroy((Object) this._fadeOutMaterial);
    if (this._renderTextures == null)
      return;
    foreach (RenderTexture renderTexture in this._renderTextures)
    {
      if ((bool) (Object) renderTexture)
      {
        renderTexture.Release();
        EssentialHelpers.SafeDestroy((Object) renderTexture);
      }
    }
  }

  public virtual void OnEnable()
  {
    if (this._lineRenderers != null && (bool) (Object) this._lineRenderers[0])
      this._lineRenderers[0].gameObject.SetActive(true);
    if (this._lineRenderers == null || !(bool) (Object) this._lineRenderers[1])
      return;
    this._lineRenderers[1].gameObject.SetActive(true);
  }

  public virtual void OnDisable()
  {
    if (this._lineRenderers != null && (bool) (Object) this._lineRenderers[0])
      this._lineRenderers[0].gameObject.SetActive(false);
    if (this._lineRenderers == null || !(bool) (Object) this._lineRenderers[1])
      return;
    this._lineRenderers[1].gameObject.SetActive(false);
  }

  public virtual bool GetBurnMarkPos(
    Vector3 bladeBottomPos,
    Vector3 bladeTopPos,
    out Vector3 burnMarkPos)
  {
    float num = Vector3.Distance(bladeBottomPos, bladeTopPos);
    Vector3 direction = (bladeTopPos - bladeBottomPos) / num;
    float enter;
    if (this._plane.Raycast(new Ray(bladeBottomPos, direction), out enter) && (double) enter <= (double) num)
    {
      burnMarkPos = bladeBottomPos + direction * enter;
      Bounds bounds = this._renderer.bounds;
      return (double) bounds.min.x < (double) burnMarkPos.x && (double) bounds.max.x > (double) burnMarkPos.x && (double) bounds.min.z < (double) burnMarkPos.z && (double) bounds.max.z > (double) burnMarkPos.z;
    }
    burnMarkPos = Vector3.zero;
    return false;
  }

  public virtual Vector3 WorldToCameraBurnMarkPos(Vector3 pos)
  {
    pos = this.transform.InverseTransformPoint(pos);
    Bounds bounds = this._renderer.bounds;
    Vector3 localScale = this.transform.localScale;
    return new Vector3(pos.x * localScale.x / bounds.extents.x * (float) this._textureWidth / (float) this._textureHeight, pos.z * localScale.z / bounds.extents.z, 0.0f);
  }

  public virtual void LateUpdate()
  {
    if ((Object) this._sabers[0] == (Object) null)
      return;
    for (int index1 = 0; index1 < 2; ++index1)
    {
      Vector3 burnMarkPos = Vector3.zero;
      bool flag = this._sabers[index1].isActiveAndEnabled && this.GetBurnMarkPos(this._sabers[index1].saberBladeBottomPos, this._sabers[index1].saberBladeTopPos, out burnMarkPos);
      if (flag && this._prevBurnMarkPosValid[index1])
      {
        Vector3 vector3 = burnMarkPos - this._prevBurnMarkPos[index1];
        int num1 = (int) ((double) vector3.magnitude / 0.0099999997764825821);
        int num2 = num1 > 0 ? num1 : 1;
        Vector3 normalized = new Vector3(vector3.z, 0.0f, -vector3.x).normalized;
        for (int index2 = 0; index2 <= num2 && index2 < this._linePoints.Length; ++index2)
        {
          Vector3 pos = this._prevBurnMarkPos[index1] + vector3 * (float) index2 / (float) num2 + normalized * Random.Range(-this._blackMarkLineRandomOffset, this._blackMarkLineRandomOffset);
          this._linePoints[index2] = this.WorldToCameraBurnMarkPos(pos);
        }
        this._lineRenderers[index1].positionCount = num2 + 1;
        this._lineRenderers[index1].SetPositions(this._linePoints);
        this._lineRenderers[index1].enabled = true;
      }
      else
        this._lineRenderers[index1].enabled = false;
      this._prevBurnMarkPosValid[index1] = flag;
      this._prevBurnMarkPos[index1] = burnMarkPos;
    }
    if (this._lineRenderers[0].enabled || this._lineRenderers[1].enabled)
      this._camera.Render();
    this._camera.targetTexture = this._renderTextures[1];
    this._renderer.sharedMaterial.mainTexture = (Texture) this._renderTextures[1];
    this._fadeOutMaterial.mainTexture = (Texture) this._renderTextures[0];
    this._fadeOutMaterial.SetFloat(this._fadeOutStrengthShaderPropertyID, Mathf.Max(0.0f, (float) (1.0 - (double) Time.deltaTime * (double) this._burnMarksFadeOutStrength)));
    Graphics.Blit((Texture) this._renderTextures[0], this._renderTextures[1], this._fadeOutMaterial);
    RenderTexture renderTexture = this._renderTextures[0];
    this._renderTextures[0] = this._renderTextures[1];
    this._renderTextures[1] = renderTexture;
  }
}
