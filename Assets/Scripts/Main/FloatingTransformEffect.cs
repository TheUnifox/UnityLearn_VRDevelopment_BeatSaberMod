// Decompiled with JetBrains decompiler
// Type: FloatingTransformEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class FloatingTransformEffect : MonoBehaviour, ITickable
{
  [SerializeField]
  protected Vector3 _positionMultiplier = Vector3.one;
  [SerializeField]
  protected Vector3 _rotationMultiplier = Vector3.zero;
  [Header("Rotation")]
  [SerializeField]
  [NullAllowed]
  protected Transform _rotationTransform;
  [SerializeField]
  protected float _maxRotationDegrees;
  [Header("X")]
  [SerializeField]
  protected Vector2 _xAmplitude = new Vector2(0.1f, 0.3f);
  [SerializeField]
  protected Vector2 _xFrequency = new Vector2(0.1f, 0.3f);
  [Header("Y")]
  [SerializeField]
  protected Vector2 _yAmplitude = new Vector2(0.1f, 0.3f);
  [SerializeField]
  protected Vector2 _yFrequency = new Vector2(0.1f, 0.3f);
  [Header("Z")]
  [SerializeField]
  protected Vector2 _zAmplitude = new Vector2(0.1f, 0.3f);
  [SerializeField]
  protected Vector2 _zFrequency = new Vector2(0.1f, 0.3f);
  protected Transform _transform;
  protected Vector3 _origin;
  protected float _offsetX;
  protected float _offsetY;
  protected float _offsetZ;
  protected float _amplitudeX;
  protected float _amplitudeY;
  protected float _amplitudeZ;
  protected float _frequencyX;
  protected float _frequencyY;
  protected float _frequencyZ;
  protected Quaternion _targetRotation;

  public virtual void Start()
  {
    this._transform = this.transform;
    this._origin = this._transform.position;
    this.Refresh();
  }

  public virtual void Tick()
  {
    Vector3 point = this.GetPoint(Time.time);
    this._transform.position = this._origin + this._transform.TransformDirection(Vector3.Scale(point, this._positionMultiplier));
    if ((Object) this._rotationTransform == (Object) null)
      return;
    this._rotationTransform.rotation = Quaternion.Euler(Vector3.Scale(point * this._maxRotationDegrees, this._rotationMultiplier));
  }

  public virtual void Refresh()
  {
    this._offsetX = (float) ((double) Random.value * 3.1415927410125732 * 2.0);
    this._offsetY = (float) ((double) Random.value * 3.1415927410125732 * 2.0);
    this._offsetZ = (float) ((double) Random.value * 3.1415927410125732 * 2.0);
    this._amplitudeX = Random.Range(this._xAmplitude.x, this._xAmplitude.y);
    this._amplitudeY = Random.Range(this._yAmplitude.x, this._yAmplitude.y);
    this._amplitudeZ = Random.Range(this._zAmplitude.x, this._xAmplitude.y);
    this._frequencyX = Random.Range(this._xFrequency.x, this._xFrequency.y);
    this._frequencyY = Random.Range(this._yFrequency.x, this._yFrequency.y);
    this._frequencyZ = Random.Range(this._zFrequency.x, this._zFrequency.y);
  }

  public virtual Vector3 GetPoint(float time) => new Vector3(Mathf.Sin((this._offsetX + time) * this._frequencyX) * this._amplitudeX, Mathf.Sin((this._offsetY + time) * this._frequencyY) * this._amplitudeY, Mathf.Sin((this._offsetZ + time) * this._frequencyZ) * this._amplitudeZ);
}
