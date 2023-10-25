// Decompiled with JetBrains decompiler
// Type: FlickeringNeonSign
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;

public class FlickeringNeonSign : MonoBehaviour
{
  [SerializeField]
  protected SpriteRenderer _flickeringSprite;
  [SerializeField]
  protected TubeBloomPrePassLight _light;
  [SerializeField]
  protected ParticleSystem[] _particleSystems;
  [SerializeField]
  protected float _minOnDelay = 0.05f;
  [SerializeField]
  protected float _maxOnDelay = 0.4f;
  [SerializeField]
  protected float _minOffDelay = 0.05f;
  [SerializeField]
  protected float _maxOffDelay = 0.4f;
  [SerializeField]
  protected Color _spriteOnColor;
  [SerializeField]
  protected Color _lightOnColor;
  [SerializeField]
  protected Material _onMaterial;
  [SerializeField]
  protected Material _offMaterial;
  [SerializeField]
  protected AudioClip[] _sparksAudioClips;
  protected RandomObjectPicker<AudioClip> _sparksAudioClipPicker;

  public virtual void Awake() => this._sparksAudioClipPicker = new RandomObjectPicker<AudioClip>(this._sparksAudioClips, 0.1f);

  public virtual void Start()
  {
    this._spriteOnColor = this._flickeringSprite.color;
    this._lightOnColor = this._light.color;
  }

  public virtual void OnEnable() => this.StartCoroutine(this.FlickeringCoroutine());

  public virtual IEnumerator FlickeringCoroutine()
  {
    while (true)
    {
      yield return (object) new WaitForSeconds(Random.Range(this._minOnDelay, this._maxOnDelay));
      this.SetOn(false);
      yield return (object) new WaitForSeconds(Random.Range(this._minOffDelay, this._maxOffDelay));
      this.SetOn(true);
    }
  }

  public virtual void SetOn(bool on)
  {
    this._flickeringSprite.material = on ? this._onMaterial : this._offMaterial;
    this._flickeringSprite.color = on ? this._spriteOnColor : Color.black;
    this._light.color = on ? this._lightOnColor : Color.black;
    if (!on)
      return;
    foreach (ParticleSystem particleSystem in this._particleSystems)
      particleSystem.Emit(Random.Range(2, 8));
  }
}
