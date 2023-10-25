// Decompiled with JetBrains decompiler
// Type: FlyingCar
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class FlyingCar : MonoBehaviour
{
  [SerializeField]
  protected float _startZ = -30f;
  [SerializeField]
  protected float _endZ = 100f;
  [SerializeField]
  protected float _speed = 1f;
  protected float _progress;
  protected Vector3 _pos;

  public virtual void Start()
  {
    this._pos = this.transform.localPosition;
    this._progress = (this._pos.z - this._startZ) / Mathf.Abs(this._endZ - this._startZ);
    this.UpdatePos();
  }

  public virtual void Update()
  {
    this._progress += Time.deltaTime * this._speed / Mathf.Abs(this._endZ - this._startZ);
    if ((double) this._progress > 1.0)
      this._progress = -Random.value;
    this.UpdatePos();
  }

  public virtual void UpdatePos()
  {
    this._pos.z = Mathf.LerpUnclamped(this._startZ, this._endZ, this._progress);
    this.transform.localPosition = this._pos;
  }
}
