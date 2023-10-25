// Decompiled with JetBrains decompiler
// Type: IntroTutorialRing
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class IntroTutorialRing : MonoBehaviour
{
  [SerializeField]
  protected Image[] _progressImages;
  [SerializeField]
  protected SaberType _saberType;
  [SerializeField]
  protected ParticleSystem _particleSystem;
  [SerializeField]
  protected CanvasGroup _canvasGroup;
  [SerializeField]
  protected float _activationDuration = 0.7f;
  [SerializeField]
  protected Image[] _ringGLowImages;
  [Inject]
  protected readonly ColorManager _colorManager;
  protected bool _highlighted;
  protected float _emitNextParticleTimer;
  protected float _activationProgress;
  protected readonly HashSet<SaberType> _sabersInside = new HashSet<SaberType>();
  protected bool _sabersInsideAfterOnEnable;

  public float alpha
  {
    set => this._canvasGroup.alpha = value;
  }

  public bool fullyActivated => this._highlighted && (double) this._activationProgress == 1.0;

  public SaberType saberType
  {
    get => this._saberType;
    set => this._saberType = value;
  }

  public virtual void Start()
  {
    foreach (Graphic ringGlowImage in this._ringGLowImages)
      ringGlowImage.color = this._colorManager.ColorForSaberType(this._saberType);
  }

  public virtual void OnEnable() => this._sabersInside.Clear();

  public virtual void Update()
  {
    bool flag = this._sabersInside.Contains(this._saberType);
    if (flag && !this._highlighted)
    {
      this._highlighted = true;
      this._emitNextParticleTimer = 0.0f;
    }
    else if (!flag && this._highlighted)
      this._highlighted = false;
    if (this._highlighted)
    {
      this._activationProgress = Mathf.Min(this._activationProgress + Time.deltaTime / this._activationDuration, 1f);
      this.SetProgressImagesfillAmount(this._activationProgress);
      if ((double) this._emitNextParticleTimer <= 0.0)
      {
        this._particleSystem.Emit(1);
        this._emitNextParticleTimer = 0.25f;
      }
      this._emitNextParticleTimer -= Time.deltaTime;
    }
    else
    {
      this._activationProgress = Mathf.Max(this._activationProgress - Time.deltaTime / this._activationDuration, 0.0f);
      this.SetProgressImagesfillAmount(this._activationProgress);
    }
  }

  public virtual void SetProgressImagesfillAmount(float fillAmount)
  {
    foreach (Image progressImage in this._progressImages)
      progressImage.fillAmount = fillAmount;
  }

  public virtual void OnTriggerEnter(Collider other)
  {
    if (!LayerMasks.saberLayerMask.ContainsLayer(other.gameObject.layer))
      return;
    this._sabersInside.Add(other.gameObject.GetComponent<Saber>().saberType);
  }

  public virtual void OnTriggerExit(Collider other)
  {
    if (!LayerMasks.saberLayerMask.ContainsLayer(other.gameObject.layer))
      return;
    this._sabersInside.Remove(other.gameObject.GetComponent<Saber>().saberType);
  }

  public virtual void OnTriggerStay(Collider other)
  {
    if (!this._sabersInsideAfterOnEnable)
      return;
    this._sabersInsideAfterOnEnable = false;
    if (!LayerMasks.saberLayerMask.ContainsLayer(other.gameObject.layer))
      return;
    this._sabersInside.Add(other.gameObject.GetComponent<Saber>().saberType);
  }
}
