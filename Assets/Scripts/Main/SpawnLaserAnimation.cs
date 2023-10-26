// Decompiled with JetBrains decompiler
// Type: SpawnLaserAnimation
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteAlways]
public class SpawnLaserAnimation : MonoBehaviour
{
  [SerializeField]
  protected Transform _centerThresholdTransform;
  [SerializeField]
  protected Transform _horizontalLasersTransform;
  [SerializeField]
  protected TubeBloomPrePassLight _leftHorizontalLaser;
  [SerializeField]
  protected TubeBloomPrePassLight _rightHorizontalLaser;
  [HideInInspector]
  public float _normalizedDistance;
  [HideInInspector]
  public float _alphaMultiplier = 1f;
  [HideInInspector]
  public float _laserLength = 5f;
  protected float _centerDistance;
  protected bool _initialized;

  public virtual void InitIfNeeded()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this._centerDistance = (Object) this._centerThresholdTransform == (Object) null ? 40f : this.transform.InverseTransformPoint(this._centerThresholdTransform.position).z;
  }

    public virtual void LateUpdate()
    {
        this.InitIfNeeded();
        float num = Mathf.LerpUnclamped(0f, this._centerDistance, this._normalizedDistance);
        Vector3 localPosition = this._horizontalLasersTransform.localPosition;
        localPosition.z = num;
        this._horizontalLasersTransform.localPosition = localPosition;
        this._leftHorizontalLaser.length = (this._rightHorizontalLaser.length = Mathf.Max(0f, Mathf.Min(this._laserLength, this._centerDistance - num)));
        Color color = this._leftHorizontalLaser.color;
        color.a = ((this._leftHorizontalLaser.length > 0f) ? this._alphaMultiplier : 0f);
        this._leftHorizontalLaser.color = (this._rightHorizontalLaser.color = color);
    }
}
