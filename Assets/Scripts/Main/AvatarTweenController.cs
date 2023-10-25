// Decompiled with JetBrains decompiler
// Type: AvatarTweenController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.Runtime.CompilerServices;
using Tweening;
using UnityEngine;
using Zenject;

public class AvatarTweenController : MonoBehaviour
{
  [Header("Avatar Parts")]
  [SerializeField]
  protected Transform _avatarTransform;
  [SerializeField]
  protected Transform _headParent;
  [SerializeField]
  protected Transform _leftHandTransform;
  [SerializeField]
  protected Transform _rightHandTransform;
  [SerializeField]
  protected Transform _bodyTransform;
  [Header("Inner Parts")]
  [SerializeField]
  protected Transform _headInnerTransform;
  [SerializeField]
  protected Transform _leftHandInnerTransform;
  [SerializeField]
  protected Transform _rightHandInnerTransform;
  [SerializeField]
  protected Transform _bodyInnerTransform;
  [Header("Pop Tween")]
  [SerializeField]
  [Range(0.0f, 1f)]
  protected float _popDuration = 0.3f;
  [SerializeField]
  protected EaseType _popEaseType = EaseType.OutQuad;
  [SerializeField]
  protected float _headPopAmount = 0.3f;
  [SerializeField]
  protected float _handsPopAmount = 0.6f;
  [SerializeField]
  protected float _clothesPopAmount = 0.6f;
  [SerializeField]
  protected float _allPopAmount = 0.2f;
  [Header("Appear Tween")]
  [SerializeField]
  [Range(0.0f, 1f)]
  protected float _appearDuration = 0.4f;
  [SerializeField]
  [Range(0.0f, 0.5f)]
  protected float _appearSpacing = 0.2f;
  [SerializeField]
  protected float _appearHeight = 2f;
  [SerializeField]
  [Min(0.0f)]
  protected Vector3 _squashFactor = new Vector3(0.3f, 2f, 0.3f);
  [Header("Disappear tween")]
  [SerializeField]
  [Range(0.05f, 0.5f)]
  protected float _disappearDuration = 0.2f;
  [SerializeField]
  protected float _disappearHeight = 0.75f;
  [SerializeField]
  protected Vector3 _disappearSquash = new Vector3(0.0f, 1.5f, 0.0f);
  [SerializeField]
  protected EaseType _disappearScaleEase = EaseType.InBack;
  [SerializeField]
  protected EaseType _disappearPositionEase = EaseType.OutCirc;
  [Inject]
  protected readonly TimeTweeningManager _tweeningManager;
  protected Tween<float> _popHeadTween;
  protected Tween<float> _popLeftHandTween;
  protected Tween<float> _popRightHandTween;
  protected Tween<float> _popClothesTween;
  protected Tween<Vector3> _appearHeadPositionTween;
  protected Tween<Vector3> _appearHeadScaleTween;
  protected Tween<Vector3> _appearBodyPositionTween;
  protected Tween<Vector3> _appearBodyScaleTween;
  protected Tween<Vector3> _appearRightHandPositionTween;
  protected Tween<Vector3> _appearRightHandScaleTween;
  protected Tween<Vector3> _appearLeftHandPositionTween;
  protected Tween<Vector3> _appearLeftHandScaleTween;
  protected Tween<Vector3> _disappearScaleTween;
  protected Tween<Vector3> _disappearPositionTween;
  protected Vector3 _avatarLocalPosition;
  protected Vector3 _avatarLocalScale;

  public virtual void Awake()
  {
    this._avatarLocalScale = this._avatarTransform.localScale;
    this._avatarLocalPosition = this._avatarTransform.localPosition;
  }

  public virtual void OnDisable() => this.StopAll();

  public virtual void PresentAvatar()
  {
    this.StopAll();
    this.StartCoroutine(this.AppearAnimation());
  }

  public virtual void HideAvatar()
  {
    this.StopAll();
    this.StartCoroutine(this.DisappearAnimation());
  }

  public virtual void PopAll()
  {
    this.PopHead(this._allPopAmount);
    this.PopClothes(this._allPopAmount);
    this.PopHands(this._allPopAmount);
  }

  public virtual void PopHead() => this.PopHead(this._headPopAmount);

  public virtual void PopHands() => this.PopHands(this._handsPopAmount);

  public virtual void PopClothes() => this.PopClothes(this._clothesPopAmount);

  public virtual void PopHead(float popAmount)
  {
    if (this._popHeadTween == null)
      this._popHeadTween = this.CreatePopTween(this._headParent, this._headPopAmount);
    this._popHeadTween.fromValue = 1f + popAmount;
    this._tweeningManager.RestartTween((Tween) this._popHeadTween, (object) this);
  }

  public virtual void PopHands(float popAmount)
  {
    if (this._popLeftHandTween == null)
      this._popLeftHandTween = this.CreatePopTween(this._leftHandTransform, this._handsPopAmount);
    if (this._popRightHandTween == null)
      this._popRightHandTween = this.CreatePopTween(this._rightHandTransform, this._handsPopAmount);
    this._popLeftHandTween.fromValue = 1f + popAmount;
    this._popRightHandTween.fromValue = 1f + popAmount;
    this._tweeningManager.RestartTween((Tween) this._popLeftHandTween, (object) this);
    this._tweeningManager.RestartTween((Tween) this._popRightHandTween, (object) this);
  }

  public virtual void PopClothes(float popAmount)
  {
    if (this._popClothesTween == null)
      this._popClothesTween = this.CreatePopTween(this._bodyTransform, 0.5f);
    this._popClothesTween.fromValue = 1f + popAmount;
    this._tweeningManager.RestartTween((Tween) this._popClothesTween, (object) this);
  }

  public virtual Tween<float> CreatePopTween(Transform partTransform, float popAmount)
  {
    Vector3 originalScale = partTransform.localScale;
    return (Tween<float>) new FloatTween(1f + popAmount, 1f, (System.Action<float>) (val => partTransform.localScale = originalScale * val), this._popDuration, this._popEaseType);
  }

  public virtual IEnumerator AppearAnimation()
  {
    this._avatarTransform.localPosition = this._avatarLocalPosition;
    this._avatarTransform.localScale = this._avatarLocalScale;
    Vector3 vector3 = new Vector3(0.0f, -100f, 0.0f);
    this._avatarTransform.gameObject.SetActive(true);
    this._bodyInnerTransform.position = this._headInnerTransform.position = this._leftHandInnerTransform.position = this._rightHandInnerTransform.position = vector3;
    WaitForSeconds waitYieldInstruction = new WaitForSeconds(this._appearSpacing);
    this.AppearBody();
    yield return (object) waitYieldInstruction;
    this.AppearHead();
    yield return (object) waitYieldInstruction;
    this.AppearLeftHand();
    yield return (object) waitYieldInstruction;
    this.AppearRightHand();
  }

  public virtual void AppearBody()
  {
    this._bodyInnerTransform.position = this._bodyInnerTransform.parent.position + Vector3.down * this._appearHeight;
    if (this._appearBodyPositionTween == null)
      this._appearBodyPositionTween = (Tween<Vector3>) new Vector3Tween(this._bodyInnerTransform.localPosition, Vector3.zero, (System.Action<Vector3>) (val => this._bodyInnerTransform.localPosition = val), this._appearDuration, EaseType.OutElastic);
    if (this._appearBodyScaleTween == null)
    {
      Vector3 localScale = this._bodyInnerTransform.localScale;
      this._appearBodyScaleTween = (Tween<Vector3>) new Vector3Tween(Vector3.Scale(localScale, this._squashFactor), localScale, (System.Action<Vector3>) (val => this._bodyInnerTransform.localScale = val), this._appearDuration, EaseType.OutElastic);
    }
    this._tweeningManager.RestartTween((Tween) this._appearBodyPositionTween, (object) this);
    this._tweeningManager.RestartTween((Tween) this._appearBodyScaleTween, (object) this);
  }

  public virtual void AppearHead()
  {
    this._headInnerTransform.position = this._headInnerTransform.parent.position + Vector3.down * this._appearHeight;
    if (this._appearHeadPositionTween == null)
      this._appearHeadPositionTween = (Tween<Vector3>) new Vector3Tween(this._headInnerTransform.localPosition, Vector3.zero, (System.Action<Vector3>) (val => this._headInnerTransform.localPosition = val), this._appearDuration, EaseType.OutElastic);
    if (this._appearHeadScaleTween == null)
    {
      Vector3 localScale = this._headInnerTransform.localScale;
      this._appearHeadScaleTween = (Tween<Vector3>) new Vector3Tween(Vector3.Scale(localScale, this._squashFactor), localScale, (System.Action<Vector3>) (val => this._headInnerTransform.localScale = val), this._appearDuration, EaseType.OutElastic);
    }
    this._tweeningManager.RestartTween((Tween) this._appearHeadPositionTween, (object) this);
    this._tweeningManager.RestartTween((Tween) this._appearHeadScaleTween, (object) this);
  }

  public virtual void AppearLeftHand()
  {
    this._leftHandInnerTransform.position = this._leftHandInnerTransform.parent.position + Vector3.down * (this._appearHeight * 0.5f);
    if (this._appearLeftHandPositionTween == null)
      this._appearLeftHandPositionTween = (Tween<Vector3>) new Vector3Tween(this._leftHandInnerTransform.localPosition, Vector3.zero, (System.Action<Vector3>) (val => this._leftHandInnerTransform.localPosition = val), this._appearDuration, EaseType.OutElastic);
    if (this._appearLeftHandScaleTween == null)
    {
      Vector3 localScale = this._leftHandInnerTransform.localScale;
      this._appearLeftHandScaleTween = (Tween<Vector3>) new Vector3Tween(Vector3.Scale(localScale, this._squashFactor), localScale, (System.Action<Vector3>) (val => this._leftHandInnerTransform.localScale = val), this._appearDuration, EaseType.OutElastic);
    }
    this._tweeningManager.RestartTween((Tween) this._appearLeftHandPositionTween, (object) this);
    this._tweeningManager.RestartTween((Tween) this._appearLeftHandScaleTween, (object) this);
  }

  public virtual void AppearRightHand()
  {
    this._rightHandInnerTransform.position = this._rightHandInnerTransform.parent.position + Vector3.down * (this._appearHeight * 0.5f);
    if (this._appearRightHandPositionTween == null)
      this._appearRightHandPositionTween = (Tween<Vector3>) new Vector3Tween(this._rightHandInnerTransform.localPosition, Vector3.zero, (System.Action<Vector3>) (val => this._rightHandInnerTransform.localPosition = val), this._appearDuration, EaseType.OutElastic);
    if (this._appearRightHandScaleTween == null)
    {
      Vector3 localScale = this._rightHandInnerTransform.localScale;
      this._appearRightHandScaleTween = (Tween<Vector3>) new Vector3Tween(Vector3.Scale(localScale, this._squashFactor), localScale, (System.Action<Vector3>) (val => this._rightHandInnerTransform.localScale = val), this._appearDuration, EaseType.OutElastic);
    }
    this._tweeningManager.RestartTween((Tween) this._appearRightHandPositionTween, (object) this);
    this._tweeningManager.RestartTween((Tween) this._appearRightHandScaleTween, (object) this);
  }

  public virtual IEnumerator DisappearAnimation()
  {
    AvatarTweenController owner = this;
    if (owner._disappearPositionTween == null)
      owner._disappearPositionTween = (Tween<Vector3>) new Vector3Tween(owner._avatarLocalPosition, owner._avatarLocalPosition - new Vector3(0.0f, owner._disappearHeight, 0.0f), new System.Action<Vector3>(owner.m_CDisappearAnimationm_Eb__58_0), owner._disappearDuration, owner._disappearPositionEase);
    if (owner._disappearScaleTween == null)
      owner._disappearScaleTween = (Tween<Vector3>) new Vector3Tween(owner._avatarLocalScale, Vector3.Scale(owner._avatarLocalScale, owner._disappearSquash), new System.Action<Vector3>(owner.m_CDisappearAnimationm_Eb__58_1), owner._disappearDuration, owner._disappearScaleEase);
    owner._tweeningManager.RestartTween((Tween) owner._disappearPositionTween, (object) owner);
    owner._tweeningManager.RestartTween((Tween) owner._disappearScaleTween, (object) owner);
    yield return (object) new WaitForSeconds(owner._disappearDuration);
    owner.gameObject.SetActive(false);
  }

  public virtual void StopAll()
  {
    this.StopAllCoroutines();
    if (!((UnityEngine.Object) this._tweeningManager != (UnityEngine.Object) null))
      return;
    this._tweeningManager.KillAllTweens((object) this);
  }

  [CompilerGenerated]
  public virtual void m_CAppearBodym_Eb__54_0(Vector3 val) => this._bodyInnerTransform.localPosition = val;

  [CompilerGenerated]
  public virtual void m_CAppearBodym_Eb__54_1(Vector3 val) => this._bodyInnerTransform.localScale = val;

  [CompilerGenerated]
  public virtual void m_CAppearHeadm_Eb__55_0(Vector3 val) => this._headInnerTransform.localPosition = val;

  [CompilerGenerated]
  public virtual void m_CAppearHeadm_Eb__55_1(Vector3 val) => this._headInnerTransform.localScale = val;

  [CompilerGenerated]
  public virtual void m_CAppearLeftHandm_Eb__56_0(Vector3 val) => this._leftHandInnerTransform.localPosition = val;

  [CompilerGenerated]
  public virtual void m_CAppearLeftHandm_Eb__56_1(Vector3 val) => this._leftHandInnerTransform.localScale = val;

  [CompilerGenerated]
  public virtual void m_CAppearRightHandm_Eb__57_0(Vector3 val) => this._rightHandInnerTransform.localPosition = val;

  [CompilerGenerated]
  public virtual void m_CAppearRightHandm_Eb__57_1(Vector3 val) => this._rightHandInnerTransform.localScale = val;

  [CompilerGenerated]
  public virtual void m_CDisappearAnimationm_Eb__58_0(Vector3 val) => this._avatarTransform.localPosition = val;

  [CompilerGenerated]
  public virtual void m_CDisappearAnimationm_Eb__58_1(Vector3 val) => this._avatarTransform.localScale = val;
}
