// Decompiled with JetBrains decompiler
// Type: FlyingGameHUDRotation
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class FlyingGameHUDRotation : MonoBehaviour
{
  [SerializeField]
  protected float _smooth = 8f;
  [InjectOptional]
  protected readonly BeatLineManager _beatLineManager;
  [InjectOptional]
  protected readonly EnvironmentSpawnRotation _environmentSpawnRotation;
  protected float _prevYAngle;
  protected float _yAngle;

  public virtual void Start()
  {
    if ((Object) this._beatLineManager == (Object) null || (Object) this._environmentSpawnRotation == (Object) null)
    {
      this.enabled = false;
    }
    else
    {
      this._yAngle = this._beatLineManager.midRotation;
      this.transform.eulerAngles = new Vector3(0.0f, this._yAngle, 0.0f);
    }
  }

  public virtual void FixedUpdate()
  {
    float b;
    if (this._beatLineManager.isMidRotationValid)
    {
      double midRotation = (double) this._beatLineManager.midRotation;
      float num1 = Mathf.DeltaAngle((float) midRotation, this._environmentSpawnRotation.targetRotation);
      float num2 = (float) (-(double) this._beatLineManager.rotationRange * 0.5);
      float num3 = this._beatLineManager.rotationRange * 0.5f;
      if ((double) num1 > (double) num3)
        num3 = num1;
      else if ((double) num1 < (double) num2)
        num2 = num1;
      b = this._yAngle + Mathf.DeltaAngle(this._yAngle, (float) (midRotation + ((double) num2 + (double) num3) * 0.5));
    }
    else
      b = this._yAngle + Mathf.DeltaAngle(this._yAngle, this._environmentSpawnRotation.targetRotation);
    this._prevYAngle = this._yAngle;
    this._yAngle = Mathf.LerpUnclamped(this._yAngle, b, TimeHelper.fixedDeltaTime * this._smooth);
  }

  public virtual void LateUpdate() => this.transform.eulerAngles = new Vector3(0.0f, Mathf.LerpUnclamped(this._prevYAngle, this._yAngle, TimeHelper.interpolationFactor), 0.0f);
}
