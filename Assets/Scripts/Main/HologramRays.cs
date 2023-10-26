// Decompiled with JetBrains decompiler
// Type: HologramRays
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class HologramRays : MonoBehaviour
{
  [SerializeField]
  protected TubeBloomPrePassLight _bloomLight;
  [SerializeField]
  protected MeshRenderer _raysMeshRenderer;
  [SerializeField]
  protected Transform _targetTransform;
  [SerializeField]
  protected Transform _laserHolderTransform;
  [SerializeField]
  protected Transform _hologramRaysTransform;
  [SerializeField]
  protected Mesh _hologramRaysMesh;
  [SerializeField]
  protected float _topYPosition = 2f;
  [SerializeField]
  protected float _bottomYPosition = -2f;
  [Tooltip("Updates automatically")]
  [SerializeField]
  protected float cachedExtent = 86.2f;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _materialBottomPositionID = Shader.PropertyToID("_BottomPosition");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _materialTopPositionID = Shader.PropertyToID("_TopPosition");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _materialAlphaID = Shader.PropertyToID("Alpha");
  protected FloatTween _transitionTween;
  protected Color _bloomColor;
  protected Color _bloomTransparentColor;
  protected float _alpha;
  protected float _raysDistance;
  protected Vector3 _raysLocalScale;
  protected bool boundsInitialized;
  protected static MaterialPropertyBlock _materialPropertyBlock = (MaterialPropertyBlock) null;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
  private static void RuntimeInit() => HologramRays._materialPropertyBlock = (MaterialPropertyBlock) null;

  public virtual void Awake()
  {
    this._bloomLight.gameObject.SetActive(false);
    this._raysMeshRenderer.gameObject.SetActive(false);
    this._transitionTween = new FloatTween(0.0f, 1f, (System.Action<float>) (f =>
    {
      this._bloomLight.color = Color.Lerp(this._bloomTransparentColor, this._bloomColor, f);
      this._alpha = f;
    }), 1f, EaseType.Linear);
    this._bloomColor = this._bloomTransparentColor = this._bloomLight.color;
    this._bloomTransparentColor.a = 0.0f;
    this._raysLocalScale = this._hologramRaysTransform.localScale;
  }

  public virtual void OnDisable() => this._tweeningManager.KillAllTweens((object) this);

  public virtual void OnValidate()
  {
    if (Application.isPlaying)
      return;
    this._alpha = 1f;
    this.Refresh();
    this.cachedExtent = this._hologramRaysTransform.InverseTransformPoint(this._targetTransform.position + new Vector3(0.0f, this._topYPosition, 0.0f)).x * 0.5f;
  }

  public virtual void Update() => this.Refresh();

  public virtual void Refresh()
  {
    if (HologramRays._materialPropertyBlock == null)
      HologramRays._materialPropertyBlock = new MaterialPropertyBlock();
    if ((double) this._raysLocalScale.x == 0.0 || (double) this._raysLocalScale.z == 0.0)
      this._raysLocalScale = this._hologramRaysTransform.localScale;
    Vector3 position = this._targetTransform.position;
    HologramRays._materialPropertyBlock.SetFloat(HologramRays._materialBottomPositionID, position.y + this._bottomYPosition * this._alpha);
    HologramRays._materialPropertyBlock.SetFloat(HologramRays._materialTopPositionID, position.y + this._topYPosition * this._alpha);
    HologramRays._materialPropertyBlock.SetFloat(HologramRays._materialAlphaID, this._alpha);
    this._raysMeshRenderer.SetPropertyBlock(HologramRays._materialPropertyBlock);
    this._laserHolderTransform.LookAt(this._targetTransform);
    this._raysDistance = Vector3.Distance(this._laserHolderTransform.position, position);
    this._bloomLight.length = this._raysDistance;
    this._raysLocalScale.y = this._raysDistance;
    this._hologramRaysTransform.localScale = this._raysLocalScale;
  }

  public virtual void Animate(bool turningOn, float duration, EaseType easeType)
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.FadingCoroutine(turningOn, duration, easeType));
    if (!(!this.boundsInitialized & turningOn))
      return;
    if ((double) this._hologramRaysMesh.bounds.extents.x <= 5.0)
      this.UpdateBounds();
    this.boundsInitialized = true;
  }

  public virtual IEnumerator FadingCoroutine(bool turningOn, float duration, EaseType easeType)
  {
    HologramRays owner = this;
    if (turningOn)
    {
      owner._transitionTween.fromValue = 0.0f;
      owner._transitionTween.toValue = 1f;
    }
    else
    {
      owner._transitionTween.fromValue = 1f;
      owner._transitionTween.toValue = 0.0f;
    }
    owner._transitionTween.easeType = easeType;
    owner._transitionTween.duration = duration;
    owner._tweeningManager.RestartTween((Tween) owner._transitionTween, (object) owner);
    if (turningOn)
    {
      owner._bloomLight.gameObject.SetActive(true);
      owner._raysMeshRenderer.gameObject.SetActive(true);
    }
    yield return (object) new WaitForSeconds(duration);
    if (!turningOn)
    {
      owner._bloomLight.gameObject.SetActive(false);
      owner._raysMeshRenderer.gameObject.SetActive(false);
    }
  }

    public virtual void UpdateBounds()
    {
        Vector3 one = Vector3.one;
        Vector3 center = this._hologramRaysTransform.InverseTransformPoint(this._targetTransform.position) * 0.5f;
        center.x = (one.x = this.cachedExtent);
        this._hologramRaysMesh.bounds = new Bounds(center, one);
    }

    [CompilerGenerated]
  public virtual void m_CAwakem_Eb__22_0(float f)
  {
    this._bloomLight.color = Color.Lerp(this._bloomTransparentColor, this._bloomColor, f);
    this._alpha = f;
  }
}
