// Decompiled with JetBrains decompiler
// Type: TubeBloomPrePassLight
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using UnityEngine;

[SelectionBase]
public class TubeBloomPrePassLight : BloomPrePassLight
{
  [SerializeField]
  protected BoolSO _mainEffectPostProcessEnabled;
  [SerializeField]
  protected float _width = 0.5f;
  [SerializeField]
  protected bool _overrideChildrenLength = true;
  [SerializeField]
  protected float _length = 1f;
  [SerializeField]
  [Range(0.0f, 1f)]
  protected float _center = 0.5f;
  [SerializeField]
  protected Color _color;
  [SerializeField]
  protected float _colorAlphaMultiplier = 1f;
  [SerializeField]
  protected float _bloomFogIntensityMultiplier = 1f;
  [SerializeField]
  protected float _fakeBloomIntensityMultiplier = 1f;
  [SerializeField]
  protected float _boostToWhite;
  [SerializeField]
  [Min(1f)]
  protected float _lightWidthMultiplier = 1f;
  [SerializeField]
  protected bool _addWidthToLength;
  [SerializeField]
  protected bool _thickenWithDistance;
  [SerializeField]
  [DrawIf("_thickenWithDistance", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected AnimationCurve _thickenCurve = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
  [SerializeField]
  [DrawIf("_thickenWithDistance", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _minDistance = 30f;
  [SerializeField]
  [DrawIf("_thickenWithDistance", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _maxDistance = 200f;
  [SerializeField]
  [DrawIf("_thickenWithDistance", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _minWidthMultiplier = 1f;
  [SerializeField]
  [DrawIf("_thickenWithDistance", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _maxWidthMultiplier = 10f;
  [Space]
  [SerializeField]
  protected float _bakedGlowWidthScale = 1f;
  [SerializeField]
  protected bool _forceUseBakedGlow;
  [Space]
  [Tooltip("Use when this light is updated with animations")]
  [SerializeField]
  protected bool _updateAlways;
  [Space]
  [SerializeField]
  protected bool _limitAlpha;
  [SerializeField]
  [DrawIf("_limitAlpha", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _minAlpha;
  [SerializeField]
  [DrawIf("_limitAlpha", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _maxAlpha = 1f;
  [Space]
  [SerializeField]
  protected bool _overrideChildrenAlpha = true;
  [SerializeField]
  [Min(0.0f)]
  protected float _startAlpha = 1f;
  [SerializeField]
  [Min(0.0f)]
  protected float _endAlpha = 1f;
  [Space]
  [SerializeField]
  protected bool _overrideChildrenWidth;
  [SerializeField]
  [Min(1f)]
  protected float _startWidth = 1f;
  [SerializeField]
  [Min(1f)]
  protected float _endWidth = 1f;
  [Space]
  [SerializeField]
  [NullAllowed]
  protected ParametricBoxController _parametricBoxController;
  [SerializeField]
  [NullAllowed]
  protected Parametric3SliceSpriteController _dynamic3SliceSprite;
  protected bool _isDirty = true;
  protected Transform _transform;

  public event Action didRefreshEvent;

  public float colorAlphaMultiplier => this._colorAlphaMultiplier;

  public float center => this._center;

  public override bool isDirty => this._isDirty || this._updateAlways;

  public virtual void MarkDirty() => this._isDirty = true;

  public float length
  {
    get => this._length;
    set
    {
      this._length = value;
      this._isDirty = true;
    }
  }

  public float width
  {
    get => this._width;
    set
    {
      this._width = value;
      this._isDirty = true;
    }
  }

  public float lightWidthMultiplier
  {
    get => this._lightWidthMultiplier;
    set => this._lightWidthMultiplier = value;
  }

  public float bloomFogIntensityMultiplier
  {
    get => this._bloomFogIntensityMultiplier;
    set => this._bloomFogIntensityMultiplier = value;
  }

  public Color color
  {
    set
    {
      this._color = value;
      if ((UnityEngine.Object) this._parametricBoxController != (UnityEngine.Object) null)
      {
        this._parametricBoxController.color = value;
        this._isDirty = true;
      }
      if ((bool) (ObservableVariableSO<bool>) this._mainEffectPostProcessEnabled && !this._forceUseBakedGlow || !((UnityEngine.Object) this._dynamic3SliceSprite != (UnityEngine.Object) null))
        return;
      this._dynamic3SliceSprite.color = value;
      this._isDirty = true;
    }
    get => this._color;
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.Refresh();
  }

  protected override void DidRegisterLight() => this._transform = this.transform;

  public override void Refresh()
  {
    if (!this.isDirty)
      return;
    float num1 = 1f;
    if (this._thickenWithDistance)
      num1 = Mathf.Lerp(this._minWidthMultiplier, this._maxWidthMultiplier, this._thickenCurve.Evaluate(Mathf.InverseLerp(this._minDistance, this._maxDistance, this.transform.position.z)));
    if ((UnityEngine.Object) this._parametricBoxController != (UnityEngine.Object) null)
    {
      this._parametricBoxController.enabled = true;
      float num2 = this._thickenWithDistance ? this._width * num1 : this._width;
      this._parametricBoxController.width = num2;
      if (this._overrideChildrenLength)
        this._parametricBoxController.height = this._length + (this._addWidthToLength ? this._width : 0.0f);
      this._parametricBoxController.length = num2;
      this._parametricBoxController.heightCenter = this._center;
      this._parametricBoxController.color = this._color;
      this._parametricBoxController.alphaMultiplier = this._colorAlphaMultiplier;
      this._parametricBoxController.minAlpha = this._limitAlpha ? this._minAlpha : 0.0f;
      if (this._overrideChildrenAlpha)
      {
        this._parametricBoxController.alphaStart = this._startAlpha;
        this._parametricBoxController.alphaEnd = this._endAlpha;
      }
      if (this._overrideChildrenWidth)
      {
        this._parametricBoxController.widthStart = this._startWidth;
        this._parametricBoxController.widthEnd = this._endWidth;
      }
      this._parametricBoxController.Refresh();
    }
    if (!(bool) (ObservableVariableSO<bool>) this._mainEffectPostProcessEnabled || this._forceUseBakedGlow)
    {
      if ((UnityEngine.Object) this._dynamic3SliceSprite != (UnityEngine.Object) null)
      {
        this._dynamic3SliceSprite.enabled = true;
        float num3 = this._thickenWithDistance ? this._width * this._bakedGlowWidthScale * num1 : this._width * this._bakedGlowWidthScale;
        this._dynamic3SliceSprite.width = num3;
        if (this._overrideChildrenLength)
          this._dynamic3SliceSprite.length = this._length + (this._addWidthToLength ? num3 : 0.0f);
        this._dynamic3SliceSprite.center = this._center;
        this._dynamic3SliceSprite.color = this._color;
        this._dynamic3SliceSprite.alphaMultiplier = this._colorAlphaMultiplier * this._fakeBloomIntensityMultiplier;
        this._dynamic3SliceSprite.minAlpha = this._limitAlpha ? this._minAlpha : 0.0f;
        if (this._overrideChildrenAlpha)
        {
          this._dynamic3SliceSprite.alphaStart = this._startAlpha;
          this._dynamic3SliceSprite.alphaEnd = this._endAlpha;
        }
        if (this._overrideChildrenWidth)
        {
          this._dynamic3SliceSprite.widthStart = this._startWidth;
          this._dynamic3SliceSprite.widthEnd = this._endWidth;
        }
        this._dynamic3SliceSprite.Refresh();
      }
    }
    else if ((UnityEngine.Object) this._dynamic3SliceSprite != (UnityEngine.Object) null)
      this._dynamic3SliceSprite.enabled = false;
    Action didRefreshEvent = this.didRefreshEvent;
    if (didRefreshEvent != null)
      didRefreshEvent();
    this._isDirty = false;
  }

  public override void FillMeshData(
    ref int lightNum,
    BloomPrePassLight.QuadData[] lightQuads,
    Matrix4x4 viewMatrix,
    Matrix4x4 projectionMatrix,
    float lineWidth)
  {
    float y1 = -this._length * this._center;
    float y2 = this._length * (1f - this._center);
    Matrix4x4 localToWorldMatrix = this._transform.localToWorldMatrix;
    Vector3 point1 = localToWorldMatrix.MultiplyPoint3x4(new Vector3(0.0f, y1, 0.0f));
    Vector3 point2 = localToWorldMatrix.MultiplyPoint3x4(new Vector3(0.0f, y2, 0.0f));
    Vector3 fromPointViewPos = viewMatrix.MultiplyPoint3x4(point1);
    Vector3 toPointViewPos = viewMatrix.MultiplyPoint3x4(point2);
    Vector4 fromPointClipPos = projectionMatrix * new Vector4(fromPointViewPos.x, fromPointViewPos.y, fromPointViewPos.z, 1f);
    Vector4 toPointClipPos = projectionMatrix * new Vector4(toPointViewPos.x, toPointViewPos.y, toPointViewPos.z, 1f);
    bool fromPointInside1 = (double) fromPointClipPos.x >= -(double) fromPointClipPos.w;
    bool flag1 = (double) toPointClipPos.x >= -(double) toPointClipPos.w;
    if (!fromPointInside1 && !flag1)
      return;
    if (fromPointInside1 != flag1)
    {
      float t = (float) ((-(double) fromPointClipPos.w - (double) fromPointClipPos.x) / ((double) toPointClipPos.x - (double) fromPointClipPos.x + (double) toPointClipPos.w - (double) fromPointClipPos.w));
      TubeBloomPrePassLight.ClipPoints(ref fromPointClipPos, ref toPointClipPos, ref fromPointViewPos, ref toPointViewPos, fromPointInside1, t);
    }
    bool fromPointInside2 = (double) fromPointClipPos.x <= (double) fromPointClipPos.w;
    bool flag2 = (double) toPointClipPos.x <= (double) toPointClipPos.w;
    if (!fromPointInside2 && !flag2)
      return;
    if (fromPointInside2 != flag2)
    {
      float t = (float) (((double) fromPointClipPos.w - (double) fromPointClipPos.x) / ((double) toPointClipPos.x - (double) fromPointClipPos.x - (double) toPointClipPos.w + (double) fromPointClipPos.w));
      TubeBloomPrePassLight.ClipPoints(ref fromPointClipPos, ref toPointClipPos, ref fromPointViewPos, ref toPointViewPos, fromPointInside2, t);
    }
    bool fromPointInside3 = (double) fromPointClipPos.y >= -(double) fromPointClipPos.w;
    bool flag3 = (double) toPointClipPos.y >= -(double) toPointClipPos.w;
    if (!fromPointInside3 && !flag3)
      return;
    if (fromPointInside3 != flag3)
    {
      float t = (float) ((-(double) fromPointClipPos.w - (double) fromPointClipPos.y) / ((double) toPointClipPos.y - (double) fromPointClipPos.y + (double) toPointClipPos.w - (double) fromPointClipPos.w));
      TubeBloomPrePassLight.ClipPoints(ref fromPointClipPos, ref toPointClipPos, ref fromPointViewPos, ref toPointViewPos, fromPointInside3, t);
    }
    bool fromPointInside4 = (double) fromPointClipPos.y <= (double) fromPointClipPos.w;
    bool flag4 = (double) toPointClipPos.y <= (double) toPointClipPos.w;
    if (!fromPointInside4 && !flag4)
      return;
    if (fromPointInside4 != flag4)
    {
      float t = (float) (((double) fromPointClipPos.w - (double) fromPointClipPos.y) / ((double) toPointClipPos.y - (double) fromPointClipPos.y - (double) toPointClipPos.w + (double) fromPointClipPos.w));
      TubeBloomPrePassLight.ClipPoints(ref fromPointClipPos, ref toPointClipPos, ref fromPointViewPos, ref toPointViewPos, fromPointInside4, t);
    }
    bool fromPointInside5 = (double) fromPointClipPos.z <= (double) fromPointClipPos.w;
    bool flag5 = (double) toPointClipPos.z <= (double) toPointClipPos.w;
    if (!fromPointInside5 && !flag5)
      return;
    if (fromPointInside5 != flag5)
    {
      float t = (float) (((double) fromPointClipPos.w - (double) fromPointClipPos.z) / ((double) toPointClipPos.z - (double) fromPointClipPos.z - (double) toPointClipPos.w + (double) fromPointClipPos.w));
      TubeBloomPrePassLight.ClipPoints(ref fromPointClipPos, ref toPointClipPos, ref fromPointViewPos, ref toPointViewPos, fromPointInside5, t);
    }
    bool fromPointInside6 = (double) fromPointClipPos.z >= -(double) fromPointClipPos.w - 9.9999997473787516E-05;
    bool flag6 = (double) toPointClipPos.z >= -(double) toPointClipPos.w - 9.9999997473787516E-05;
    if (!fromPointInside6 && !flag6)
      return;
    if (fromPointInside6 != flag6)
    {
      float t = (float) ((-(double) fromPointClipPos.w - (double) fromPointClipPos.z) / ((double) toPointClipPos.z - (double) fromPointClipPos.z + (double) toPointClipPos.w - (double) fromPointClipPos.w));
      TubeBloomPrePassLight.ClipPoints(ref fromPointClipPos, ref toPointClipPos, ref fromPointViewPos, ref toPointViewPos, fromPointInside6, t);
    }
    float num1 = (float) ((double) fromPointClipPos.x / (double) fromPointClipPos.w * 0.5 + 0.5);
    float num2 = (float) ((double) fromPointClipPos.y / (double) fromPointClipPos.w * 0.5 + 0.5);
    float num3 = (float) ((double) toPointClipPos.x / (double) toPointClipPos.w * 0.5 + 0.5);
    float num4 = (float) ((double) toPointClipPos.y / (double) toPointClipPos.w * 0.5 + 0.5);
    float num5 = num3 - num1;
    float num6 = num4 - num2;
    float num7 = Mathf.Sqrt((float) ((double) num5 * (double) num5 + (double) num6 * (double) num6));
    if ((double) num7 == 0.0)
      num7 = 1E-06f;
    float num8 = num5 / num7;
    float num9 = num6 / num7;
    float num10 = num8 * (1f / 64f);
    float num11 = num9 * (1f / 64f);
    float num12 = num3 + num10;
    float num13 = num4 + num11;
    float num14 = num1 - num10;
    float num15 = num2 - num11;
    float num16 = lineWidth * this._lightWidthMultiplier;
    double num17 = -(double) num9 * (double) num16;
    float num18 = num8 * num16;
    float num19 = (float) num17 * this._startWidth;
    float num20 = num18 * this._startWidth;
    float num21 = (float) num17 * this._endWidth;
    float num22 = num18 * this._endWidth;
    float num23 = this._color.r + this._boostToWhite;
    float num24 = this._color.g + this._boostToWhite;
    float num25 = this._color.b + this._boostToWhite;
    float num26 = this._color.a * this._bloomFogIntensityMultiplier;
    if (this._limitAlpha)
      num26 = Mathf.Clamp(num26, this._minAlpha, this._maxAlpha);
    float gammaSpace = Mathf.LinearToGammaSpace(num26);
    float num27 = this._startAlpha * num23;
    float num28 = this._startAlpha * num24;
    float num29 = this._startAlpha * num25;
    float num30 = this._startAlpha * gammaSpace;
    float num31 = this._endAlpha * num23;
    float num32 = this._endAlpha * num24;
    float num33 = this._endAlpha * num25;
    float num34 = this._endAlpha * gammaSpace;
    ref BloomPrePassLight.QuadData local = ref lightQuads[lightNum];
    ++lightNum;
    local.vertex0.vertex.x = num14 - num19;
    local.vertex0.vertex.y = num15 - num20;
    local.vertex0.vertex.z = 0.0f;
    local.vertex0.viewPos.x = fromPointViewPos.x;
    local.vertex0.viewPos.y = fromPointViewPos.y;
    local.vertex0.viewPos.z = fromPointViewPos.z;
    local.vertex0.color.r = num27;
    local.vertex0.color.g = num28;
    local.vertex0.color.b = num29;
    local.vertex0.color.a = num30;
    local.vertex0.uv.x = 0.0f;
    local.vertex0.uv.y = 0.0f;
    local.vertex0.uv.z = this._startWidth;
    local.vertex1.vertex.x = num14 + num19;
    local.vertex1.vertex.y = num15 + num20;
    local.vertex1.vertex.z = 0.0f;
    local.vertex1.viewPos.x = fromPointViewPos.x;
    local.vertex1.viewPos.y = fromPointViewPos.y;
    local.vertex1.viewPos.z = fromPointViewPos.z;
    local.vertex1.color.r = num27;
    local.vertex1.color.g = num28;
    local.vertex1.color.b = num29;
    local.vertex1.color.a = num30;
    local.vertex1.uv.x = this._startWidth;
    local.vertex1.uv.y = 0.0f;
    local.vertex1.uv.z = this._startWidth;
    local.vertex2.vertex.x = num12 + num21;
    local.vertex2.vertex.y = num13 + num22;
    local.vertex2.vertex.z = 0.0f;
    local.vertex2.viewPos.x = toPointViewPos.x;
    local.vertex2.viewPos.y = toPointViewPos.y;
    local.vertex2.viewPos.z = toPointViewPos.z;
    local.vertex2.color.r = num31;
    local.vertex2.color.g = num32;
    local.vertex2.color.b = num33;
    local.vertex2.color.a = num34;
    local.vertex2.uv.x = this._endWidth;
    local.vertex2.uv.y = 1f;
    local.vertex2.uv.z = this._endWidth;
    local.vertex3.vertex.x = num12 - num21;
    local.vertex3.vertex.y = num13 - num22;
    local.vertex3.vertex.z = 0.0f;
    local.vertex3.viewPos.x = toPointViewPos.x;
    local.vertex3.viewPos.y = toPointViewPos.y;
    local.vertex3.viewPos.z = toPointViewPos.z;
    local.vertex3.color.r = num31;
    local.vertex3.color.g = num32;
    local.vertex3.color.b = num33;
    local.vertex3.color.a = num34;
    local.vertex3.uv.x = 0.0f;
    local.vertex3.uv.y = 1f;
    local.vertex3.uv.z = this._endWidth;
  }

  private static void ClipPoints(
    ref Vector4 fromPointClipPos,
    ref Vector4 toPointClipPos,
    ref Vector3 fromPointViewPos,
    ref Vector3 toPointViewPos,
    bool fromPointInside,
    float t)
  {
    Vector4 vector4 = fromPointClipPos + (toPointClipPos - fromPointClipPos) * t;
    Vector3 vector3 = fromPointViewPos + (toPointViewPos - fromPointViewPos) * t;
    if (fromPointInside)
    {
      toPointClipPos = vector4;
      toPointViewPos = vector3;
    }
    else
    {
      fromPointClipPos = vector4;
      fromPointViewPos = vector3;
    }
  }

  public virtual void OnDrawGizmos()
  {
    Gizmos.color = this._color;
    Gizmos.matrix = Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.lossyScale);
    Gizmos.DrawWireCube(new Vector3(0.0f, (float) (-((double) this._length - 0.0099999997764825821) * ((double) this._center - 0.5)), 0.0f), new Vector3(this._width - 0.01f, this._length - 0.01f, this._width - 0.01f));
  }
}
