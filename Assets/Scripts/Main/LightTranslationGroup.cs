// Decompiled with JetBrains decompiler
// Type: LightTranslationGroup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteAlways]
public class LightTranslationGroup : 
  LightTransformGroup<LightGroupTranslationXTransform, LightGroupTranslationYTransform, LightGroupTranslationZTransform>,
  IEditTimeValidated
{
  [Space]
  [SerializeField]
  protected Vector2 _xTranslationLimits;
  [SerializeField]
  protected Vector2 _yTranslationLimits;
  [SerializeField]
  protected Vector2 _zTranslationLimits;
  [Space]
  [SerializeField]
  protected Vector2 _xDistributionLimits;
  [SerializeField]
  protected Vector2 _yDistributionLimits;
  [SerializeField]
  protected Vector2 _zDistributionLimits;

  public Vector2 xTranslationLimits => this._xTranslationLimits;

  public Vector2 yTranslationLimits => this._yTranslationLimits;

  public Vector2 zTranslationLimits => this._zTranslationLimits;

  public Vector2 xDistributionLimits => this._xDistributionLimits;

  public Vector2 yDistributionLimits => this._yDistributionLimits;

  public Vector2 zDistributionLimits => this._zDistributionLimits;

  public virtual bool __Validate()
  {
    int num1 = this.xTransforms.Count == 0 ? 1 : ((double) Mathf.Abs(this._xTranslationLimits.x) + (double) Mathf.Abs(this._xTranslationLimits.y) > 0.0 ? 1 : 0);
    bool flag1 = this.yTransforms.Count == 0 || (double) Mathf.Abs(this._yTranslationLimits.x) + (double) Mathf.Abs(this._yTranslationLimits.y) > 0.0;
    bool flag2 = this.zTransforms.Count == 0 || (double) Mathf.Abs(this._zTranslationLimits.x) + (double) Mathf.Abs(this._zTranslationLimits.y) > 0.0;
    int num2 = this.xTransforms.Count < 2 ? 1 : ((double) Mathf.Abs(this._xTranslationLimits.x) + (double) Mathf.Abs(this._xTranslationLimits.y) > 0.0 ? 1 : 0);
    int num3 = num1 & num2;
    bool flag3 = ((flag1 ? 1 : 0) & (this.yTransforms.Count < 2 ? 1 : ((double) Mathf.Abs(this._yDistributionLimits.x) + (double) Mathf.Abs(this._yDistributionLimits.y) > 0.0 ? 1 : 0))) != 0;
    bool flag4 = ((flag2 ? 1 : 0) & (this.zTransforms.Count < 2 ? 1 : ((double) Mathf.Abs(this._zDistributionLimits.x) + (double) Mathf.Abs(this._zDistributionLimits.y) > 0.0 ? 1 : 0))) != 0;
    int num4 = flag3 ? 1 : 0;
    if ((num3 & num4 & (flag4 ? 1 : 0)) != 0)
      return true;
    Debug.LogError((object) ("Validation for LightTranslationGroup on " + this.gameObject.name + " failed, translation or distribution limits are not set"));
    return false;
  }
}
