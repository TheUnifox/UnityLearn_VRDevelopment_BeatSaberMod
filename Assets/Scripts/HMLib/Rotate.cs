// Decompiled with JetBrains decompiler
// Type: Rotate
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class Rotate : MonoBehaviour
{
  public Vector3 _rotationVector = new Vector3(0.0f, 1f, 0.0f);
  public float _speed = 1f;
  public bool _randomize;
  [DrawIf("_randomize", true, DrawIfAttribute.DisablingType.DontDraw)]
  public Vector3 _randomMinMultiplier = new Vector3(-1f, -1f, -1f);
  [DrawIf("_randomize", true, DrawIfAttribute.DisablingType.DontDraw)]
  public Vector3 _randomMaxMultiplier = new Vector3(1f, 1f, 1f);
  protected Transform _transform;
  protected Vector3 _startRotationAngles;
  protected Vector3 _randomizedMultiplier = Vector3.one;

  public virtual void Awake()
  {
    this._transform = this.transform;
    this._startRotationAngles = this._transform.localEulerAngles;
    if (!(bool) (Object) this.GetComponent<Renderer>())
      return;
    this.enabled = false;
  }

  public virtual void OnBecameVisible()
  {
    this.enabled = true;
    this.Randomize();
  }

  public virtual void OnBecameInvisible() => this.enabled = false;

  public virtual void Update()
  {
    if (this._randomize)
      this._transform.localEulerAngles = this._startRotationAngles + Vector3.Scale(this._randomizedMultiplier, this._rotationVector) * (this._speed * Time.timeSinceLevelLoad);
    else
      this._transform.localEulerAngles = this._startRotationAngles + this._rotationVector * (this._speed * Time.timeSinceLevelLoad);
  }

  public virtual void Randomize()
  {
    if (!this._randomize)
      return;
    this._randomizedMultiplier = new Vector3(Random.Range(this._randomMinMultiplier.x, this._randomMaxMultiplier.x), Random.Range(this._randomMinMultiplier.y, this._randomMaxMultiplier.y), Random.Range(this._randomMinMultiplier.z, this._randomMaxMultiplier.z));
  }
}
