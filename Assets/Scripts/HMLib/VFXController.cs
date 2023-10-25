// Decompiled with JetBrains decompiler
// Type: VFXController
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections;
using UnityEngine;

public class VFXController : MonoBehaviour
{
  [SerializeField]
  [NullAllowed]
  protected ParticleSystem[] _particleSystems;
  [SerializeField]
  [NullAllowed]
  protected Animation _animation;
  [SerializeField]
  protected bool _deactivateAfterAnimationDuration;

  public new Animation animation => this._animation;

  public ParticleSystem[] particleSystems => this._particleSystems;

  public virtual void Awake() => this.gameObject.SetActive(false);

  public virtual void Play()
  {
    this.gameObject.SetActive(true);
    if (this._deactivateAfterAnimationDuration && (Object) this._animation != (Object) null && (Object) this._animation.clip != (Object) null)
      this.StartCoroutine(this.MainCoroutine(true, this._animation.clip.length));
    else
      this.StartCoroutine(this.MainCoroutine(false, 0.0f));
  }

  public virtual IEnumerator MainCoroutine(bool deactivateAfterDuration, float duration)
  {
    VFXController vfxController = this;
    if (vfxController._particleSystems.Length != 0)
    {
      foreach (ParticleSystem particleSystem in vfxController._particleSystems)
        particleSystem.Play(false);
    }
    if ((Object) vfxController._animation != (Object) null)
    {
      vfxController._animation.Rewind();
      vfxController._animation.Play();
    }
    if (deactivateAfterDuration)
    {
      yield return (object) new WaitForSeconds(duration);
      vfxController.gameObject.SetActive(false);
    }
  }
}
