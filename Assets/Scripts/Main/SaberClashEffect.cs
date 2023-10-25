// Decompiled with JetBrains decompiler
// Type: SaberClashEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Libraries.HM.HMLib.VR;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class SaberClashEffect : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _sparkleParticleSystem;
  [SerializeField]
  protected ParticleSystem _glowParticleSystem;
  [SerializeField]
  protected HapticPresetSO _rumblePreset;
  [Inject]
  protected SaberClashChecker _saberClashChecker;
  [Inject]
  protected HapticFeedbackController _hapticFeedbackController;
  [Inject]
  protected ColorManager _colorManager;
  protected ParticleSystem.EmissionModule _sparkleParticleSystemEmmisionModule;
  protected ParticleSystem.EmissionModule _glowParticleSystemEmmisionModule;
  protected bool _sabersAreClashing;

  public virtual void Start()
  {
    this._sparkleParticleSystemEmmisionModule = this._sparkleParticleSystem.emission;
    this._sparkleParticleSystemEmmisionModule.enabled = false;
    this._glowParticleSystemEmmisionModule = this._glowParticleSystem.emission;
    this._glowParticleSystemEmmisionModule.enabled = false;
    Color color = Color.Lerp(this._colorManager.EffectsColorForSaberType(SaberType.SaberA), this._colorManager.EffectsColorForSaberType(SaberType.SaberB), 0.5f);
    this._sparkleParticleSystem.main.startColor = (ParticleSystem.MinMaxGradient) color;
    this._glowParticleSystem.main.startColor = (ParticleSystem.MinMaxGradient) color;
  }

  public virtual void OnDisable()
  {
    if (!this._sabersAreClashing)
      return;
    this._sabersAreClashing = false;
  }

  public virtual void LateUpdate()
  {
    Vector3 clashingPoint;
    if (this._saberClashChecker.AreSabersClashing(out clashingPoint))
    {
      this.transform.position = clashingPoint;
      this._hapticFeedbackController.PlayHapticFeedback(XRNode.LeftHand, this._rumblePreset);
      this._hapticFeedbackController.PlayHapticFeedback(XRNode.RightHand, this._rumblePreset);
      if (this._sabersAreClashing)
        return;
      this._sabersAreClashing = true;
      this._sparkleParticleSystemEmmisionModule.enabled = true;
      this._glowParticleSystemEmmisionModule.enabled = true;
    }
    else
    {
      if (!this._sabersAreClashing)
        return;
      this._sabersAreClashing = false;
      this._sparkleParticleSystemEmmisionModule.enabled = false;
      this._glowParticleSystemEmmisionModule.enabled = false;
      this._glowParticleSystem.Clear();
    }
  }
}
