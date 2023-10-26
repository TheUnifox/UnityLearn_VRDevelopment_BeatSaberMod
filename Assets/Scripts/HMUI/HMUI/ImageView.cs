// Decompiled with JetBrains decompiler
// Type: HMUI.ImageView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace HMUI
{
  [DisallowMultipleComponent]
  [ExecuteAlways]
  public class ImageView : Image, IComponentRefresher
  {
    [SerializeField]
    protected bool _useScriptableObjectColors;
    [SerializeField]
    [NullAllowed]
    protected ColorSO _colorSo;
    [SerializeField]
    [NullAllowed]
    protected ColorSO _color0So;
    [SerializeField]
    [NullAllowed]
    protected ColorSO _color1So;
    [SerializeField]
    protected float _skew;
    [SerializeField]
    protected bool _gradient;
    [SerializeField]
    protected Color _color0;
    [SerializeField]
    protected Color _color1;
    [SerializeField]
    protected ImageView.GradientDirection _gradientDirection;
    [SerializeField]
    protected bool _flipGradientColors;
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector2 kVec2Zero = new Vector2(0.0f, 0.0f);
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector3 kVec3Zero = new Vector3(0.0f, 0.0f, 0.0f);
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector4 kVec4Zero = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    protected readonly CurvedCanvasSettingsHelper _curvedCanvasSettingsHelper = new CurvedCanvasSettingsHelper();
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector2[] s_VertScratch = new Vector2[4];
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector2[] s_UVScratch = new Vector2[4];
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector2[] s_UV1Scratch = new Vector2[4];
    [DoesNotRequireDomainReloadInit]
    protected static readonly Color[] s_ColorScratch = new Color[4];
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector3[] s_Xy = new Vector3[4];
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector3[] s_Uv = new Vector3[4];

    public bool useScriptableObjectColors
    {
      get => this._useScriptableObjectColors;
      set => this._useScriptableObjectColors = value;
    }

    public override Color color
    {
      get => !this._useScriptableObjectColors || !((Object) this._colorSo != (Object) null) ? base.color : (Color) this._colorSo;
      set => base.color = value;
    }

    public float skew => this._skew;

    public bool gradient
    {
      get => this._gradient;
      set
      {
        this._gradient = value;
        this.SetVerticesDirty();
      }
    }

    public Color color0
    {
      get => !this._useScriptableObjectColors || !((Object) this._color0So != (Object) null) ? this._color0 : (Color) this._color0So;
      set
      {
        this._color0 = value;
        this.SetVerticesDirty();
      }
    }

    public Color color1
    {
      get => !this._useScriptableObjectColors || !((Object) this._color1So != (Object) null) ? this._color1 : (Color) this._color1So;
      set
      {
        this._color1 = value;
        this.SetVerticesDirty();
      }
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this._curvedCanvasSettingsHelper.Reset();
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
      if ((Object) this.overrideSprite == (Object) null)
      {
        base.OnPopulateMesh(toFill);
      }
      else
      {
        CurvedCanvasSettings curvedCanvasSettings = this._curvedCanvasSettingsHelper.GetCurvedCanvasSettings(this.canvas);
        float curvedUIRadius = (Object) curvedCanvasSettings == (Object) null ? 0.0f : curvedCanvasSettings.radius;
        switch (this.type)
        {
          case Image.Type.Sliced:
            this.GenerateSlicedSprite(toFill, curvedUIRadius);
            break;
          case Image.Type.Tiled:
            this.GenerateTiledSprite(toFill, curvedUIRadius);
            break;
          case Image.Type.Filled:
            this.GenerateFilledSprite(toFill, this.preserveAspect, curvedUIRadius);
            break;
          default:
            this.GenerateSimpleSprite(toFill, this.preserveAspect, curvedUIRadius);
            break;
        }
      }
    }

    public virtual void __Refresh() => this.SetAllDirty();

    public virtual void GenerateSimpleSprite(
      VertexHelper vh,
      bool lPreserveAspect,
      float curvedUIRadius)
    {
      vh.Clear();
      Vector4 drawingDimensions = this.GetDrawingDimensions(lPreserveAspect);
      Vector4 vector4 = (Object) this.overrideSprite != (Object) null ? DataUtility.GetOuterUV(this.overrideSprite) : Vector4.zero;
      int numberOfElements = ImageView.GetNumberOfElements(curvedUIRadius, Mathf.Abs(drawingDimensions.z - drawingDimensions.x));
      Vector2 uv2 = new Vector2(curvedUIRadius, 0.0f);
      float num1 = this._skew * (drawingDimensions.w - drawingDimensions.y);
      float y = this.rectTransform.pivot.y;
      Color color1;
      Color color2;
      Color color3;
      if (this._gradient)
      {
        color1 = this.color;
        color2 = (this._flipGradientColors ? this.color1 : this.color0) * color1;
        color3 = (this._flipGradientColors ? this.color0 : this.color1) * color1;
      }
      else
      {
        color2 = new Color();
        color3 = new Color();
        color1 = this.color;
      }
      for (int index = 0; index < numberOfElements + 1; ++index)
      {
        float num2 = (float) index / (float) numberOfElements;
        float num3 = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num2);
        float x = Mathf.Lerp(vector4.x, vector4.z, num2);
        if (this._gradient)
        {
          if (this._gradientDirection == ImageView.GradientDirection.Horizontal)
          {
            color1 = Color.Lerp(color2, color3, num2);
            vh.AddVert(new Vector3(num3 + num1 * (1f - y), drawingDimensions.w), (Color32) color1, new Vector2(x, vector4.w), new Vector2(num2, 0.0f), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
            vh.AddVert(new Vector3(num3 + num1 * -y, drawingDimensions.y), (Color32) color1, new Vector2(x, vector4.y), new Vector2(num2, 1f), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
          }
          else if (this._gradientDirection == ImageView.GradientDirection.Vertical)
          {
            vh.AddVert(new Vector3(num3 + num1 * (1f - y), drawingDimensions.w), (Color32) color2, new Vector2(x, vector4.w), new Vector2(num2, 0.0f), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
            vh.AddVert(new Vector3(num3 + num1 * -y, drawingDimensions.y), (Color32) color3, new Vector2(x, vector4.y), new Vector2(num2, 1f), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
          }
        }
        else
        {
          vh.AddVert(new Vector3(num3 + num1 * (1f - y), drawingDimensions.w), (Color32) color1, new Vector2(x, vector4.w), new Vector2(num2, 0.0f), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
          vh.AddVert(new Vector3(num3 + num1 * -y, drawingDimensions.y), (Color32) color1, new Vector2(x, vector4.y), new Vector2(num2, 1f), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
        }
      }
      for (int index = 0; index < numberOfElements; ++index)
      {
        int idx0 = index * 2;
        vh.AddTriangle(idx0, 1 + idx0, 2 + idx0);
        vh.AddTriangle(2 + idx0, 3 + idx0, 1 + idx0);
      }
    }

    public virtual void GenerateSlicedSprite(VertexHelper toFill, float curvedUIRadius)
    {
      if (!this.hasBorder)
      {
        this.GenerateSimpleSprite(toFill, false, curvedUIRadius);
      }
      else
      {
        Vector4 vector4_1;
        Vector4 vector4_2;
        Vector4 vector4_3;
        Vector4 vector4_4;
        if ((Object) this.overrideSprite != (Object) null)
        {
          vector4_1 = DataUtility.GetOuterUV(this.overrideSprite);
          vector4_2 = DataUtility.GetInnerUV(this.overrideSprite);
          vector4_3 = DataUtility.GetPadding(this.overrideSprite);
          vector4_4 = this.overrideSprite.border;
        }
        else
        {
          vector4_1 = Vector4.zero;
          vector4_2 = Vector4.zero;
          vector4_3 = Vector4.zero;
          vector4_4 = Vector4.zero;
        }
        Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
        Vector4 adjustedBorders = ImageView.GetAdjustedBorders(vector4_4 / this.pixelsPerUnit, pixelAdjustedRect);
        Vector4 vector4_5 = vector4_3 / this.pixelsPerUnit;
        ImageView.s_VertScratch[0] = new Vector2(vector4_5.x, vector4_5.y);
        ImageView.s_VertScratch[3] = new Vector2(pixelAdjustedRect.width - vector4_5.z, pixelAdjustedRect.height - vector4_5.w);
        ImageView.s_VertScratch[1].x = adjustedBorders.x;
        ImageView.s_VertScratch[1].y = adjustedBorders.y;
        ImageView.s_VertScratch[2].x = pixelAdjustedRect.width - adjustedBorders.z;
        ImageView.s_VertScratch[2].y = pixelAdjustedRect.height - adjustedBorders.w;
        Vector2 scale = new Vector2(1f / pixelAdjustedRect.width, 1f / pixelAdjustedRect.height);
        for (int index = 0; index < 4; ++index)
        {
          ImageView.s_UV1Scratch[index] = ImageView.s_VertScratch[index];
          ImageView.s_UV1Scratch[index].Scale(scale);
        }
        for (int index = 0; index < 4; ++index)
        {
          ImageView.s_VertScratch[index].x += pixelAdjustedRect.x;
          ImageView.s_VertScratch[index].y += pixelAdjustedRect.y;
        }
        ImageView.s_UVScratch[0] = new Vector2(vector4_1.x, vector4_1.y);
        ImageView.s_UVScratch[1] = new Vector2(vector4_2.x, vector4_2.y);
        ImageView.s_UVScratch[2] = new Vector2(vector4_2.z, vector4_2.w);
        ImageView.s_UVScratch[3] = new Vector2(vector4_1.z, vector4_1.w);
        float skewFactor = this._skew * (ImageView.s_VertScratch[3].y - ImageView.s_VertScratch[0].y);
        float y1 = this.rectTransform.pivot.y;
        if (this._gradient)
        {
          Color a = (this._flipGradientColors ? this._color1 : this._color0) * this.color;
          Color b = (this._flipGradientColors ? this._color0 : this._color1) * this.color;
          if (this._gradientDirection == ImageView.GradientDirection.Horizontal)
          {
            float x = ImageView.s_VertScratch[0].x;
            float num = ImageView.s_VertScratch[3].x - ImageView.s_VertScratch[0].x;
            ImageView.s_ColorScratch[0] = a;
            ImageView.s_ColorScratch[1] = Color.Lerp(a, b, (ImageView.s_VertScratch[1].x - x) / num);
            ImageView.s_ColorScratch[2] = Color.Lerp(a, b, (ImageView.s_VertScratch[2].x - x) / num);
            ImageView.s_ColorScratch[3] = b;
          }
          else
          {
            float y2 = ImageView.s_VertScratch[0].y;
            float num = ImageView.s_VertScratch[3].y - ImageView.s_VertScratch[0].y;
            ImageView.s_ColorScratch[3] = a;
            ImageView.s_ColorScratch[2] = Color.Lerp(a, b, (ImageView.s_VertScratch[1].y - y2) / num);
            ImageView.s_ColorScratch[1] = Color.Lerp(a, b, (ImageView.s_VertScratch[2].y - y2) / num);
            ImageView.s_ColorScratch[0] = b;
          }
        }
        toFill.Clear();
        float x1 = this.transform.localScale.x;
        if (this._gradient)
        {
          if (this._gradientDirection == ImageView.GradientDirection.Horizontal)
          {
            for (int index1 = 0; index1 < 3; ++index1)
            {
              int index2 = index1 + 1;
              for (int index3 = 0; index3 < 3; ++index3)
              {
                if (this.fillCenter || index1 != 1 || index3 != 1)
                {
                  int index4 = index3 + 1;
                  ImageView.AddQuadWithHorizontalGradient(toFill, new Vector2(ImageView.s_VertScratch[index1].x, ImageView.s_VertScratch[index3].y), new Vector2(ImageView.s_VertScratch[index2].x, ImageView.s_VertScratch[index4].y), (Color32) ImageView.s_ColorScratch[index1], (Color32) ImageView.s_ColorScratch[index2], new Vector2(ImageView.s_UVScratch[index1].x, ImageView.s_UVScratch[index3].y), new Vector2(ImageView.s_UVScratch[index2].x, ImageView.s_UVScratch[index4].y), new Vector2(ImageView.s_UV1Scratch[index1].x, ImageView.s_UV1Scratch[index3].y), new Vector2(ImageView.s_UV1Scratch[index2].x, ImageView.s_UV1Scratch[index4].y), x1, curvedUIRadius, skewFactor, y1);
                }
              }
            }
          }
          else
          {
            for (int index5 = 0; index5 < 3; ++index5)
            {
              int index6 = index5 + 1;
              for (int index7 = 0; index7 < 3; ++index7)
              {
                if (this.fillCenter || index5 != 1 || index7 != 1)
                {
                  int index8 = index7 + 1;
                  ImageView.AddQuadWithVerticalGradient(toFill, new Vector2(ImageView.s_VertScratch[index5].x, ImageView.s_VertScratch[index7].y), new Vector2(ImageView.s_VertScratch[index6].x, ImageView.s_VertScratch[index8].y), (Color32) ImageView.s_ColorScratch[index7], (Color32) ImageView.s_ColorScratch[index8], new Vector2(ImageView.s_UVScratch[index5].x, ImageView.s_UVScratch[index7].y), new Vector2(ImageView.s_UVScratch[index6].x, ImageView.s_UVScratch[index8].y), new Vector2(ImageView.s_UV1Scratch[index5].x, ImageView.s_UV1Scratch[index7].y), new Vector2(ImageView.s_UV1Scratch[index6].x, ImageView.s_UV1Scratch[index8].y), x1, curvedUIRadius, skewFactor, y1);
                }
              }
            }
          }
        }
        else
        {
          for (int index9 = 0; index9 < 3; ++index9)
          {
            int index10 = index9 + 1;
            for (int index11 = 0; index11 < 3; ++index11)
            {
              if (this.fillCenter || index9 != 1 || index11 != 1)
              {
                int index12 = index11 + 1;
                ImageView.AddQuad(toFill, new Vector2(ImageView.s_VertScratch[index9].x, ImageView.s_VertScratch[index11].y), new Vector2(ImageView.s_VertScratch[index10].x, ImageView.s_VertScratch[index12].y), (Color32) this.color, new Vector2(ImageView.s_UVScratch[index9].x, ImageView.s_UVScratch[index11].y), new Vector2(ImageView.s_UVScratch[index10].x, ImageView.s_UVScratch[index12].y), new Vector2(ImageView.s_UV1Scratch[index9].x, ImageView.s_UV1Scratch[index11].y), new Vector2(ImageView.s_UV1Scratch[index10].x, ImageView.s_UV1Scratch[index12].y), x1, curvedUIRadius, skewFactor, y1);
              }
            }
          }
        }
      }
    }

    public virtual void GenerateTiledSprite(VertexHelper toFill, float curvedUIRadius)
    {
      float x1 = this.transform.localScale.x;
      Vector4 vector4_1;
      Vector4 vector4_2;
      Vector4 vector4_3;
      Vector2 vector2_1;
      if ((Object) this.overrideSprite != (Object) null)
      {
        vector4_1 = DataUtility.GetOuterUV(this.overrideSprite);
        vector4_2 = DataUtility.GetInnerUV(this.overrideSprite);
        vector4_3 = this.overrideSprite.border;
        vector2_1 = this.overrideSprite.rect.size;
      }
      else
      {
        vector4_1 = Vector4.zero;
        vector4_2 = Vector4.zero;
        vector4_3 = Vector4.zero;
        vector2_1 = Vector2.one * 100f;
      }
      Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
      float num1 = (vector2_1.x - vector4_3.x - vector4_3.z) / this.pixelsPerUnit;
      float num2 = (vector2_1.y - vector4_3.y - vector4_3.w) / this.pixelsPerUnit;
      Vector4 adjustedBorders = ImageView.GetAdjustedBorders(vector4_3 / this.pixelsPerUnit, pixelAdjustedRect);
      Vector2 uvMin = new Vector2(vector4_2.x, vector4_2.y);
      Vector2 vector2_2 = new Vector2(vector4_2.z, vector4_2.w);
      UIVertex.simpleVert.color = (Color32) this.color;
      float x2 = adjustedBorders.x;
      float x3 = pixelAdjustedRect.width - adjustedBorders.z;
      float y1 = adjustedBorders.y;
      float y2 = pixelAdjustedRect.height - adjustedBorders.w;
      toFill.Clear();
      Vector2 uvMax = vector2_2;
      if ((double) num1 == 0.0)
        num1 = x3 - x2;
      if ((double) num2 == 0.0)
        num2 = y2 - y1;
      if (this.fillCenter)
      {
        for (float y3 = y1; (double) y3 < (double) y2; y3 += num2)
        {
          float y4 = y3 + num2;
          if ((double) y4 > (double) y2)
          {
            uvMax.y = uvMin.y + (float) (((double) vector2_2.y - (double) uvMin.y) * ((double) y2 - (double) y3) / ((double) y4 - (double) y3));
            y4 = y2;
          }
          uvMax.x = vector2_2.x;
          for (float x4 = x2; (double) x4 < (double) x3; x4 += num1)
          {
            float x5 = x4 + num1;
            if ((double) x5 > (double) x3)
            {
              uvMax.x = uvMin.x + (float) (((double) vector2_2.x - (double) uvMin.x) * ((double) x3 - (double) x4) / ((double) x5 - (double) x4));
              x5 = x3;
            }
            ImageView.AddQuad(toFill, new Vector2(x4, y3) + pixelAdjustedRect.position, new Vector2(x5, y4) + pixelAdjustedRect.position, (Color32) this.color, uvMin, uvMax, x1, curvedUIRadius);
          }
        }
      }
      if (!this.hasBorder)
        return;
      Vector2 vector2_3 = vector2_2;
      for (float y5 = y1; (double) y5 < (double) y2; y5 += num2)
      {
        float y6 = y5 + num2;
        if ((double) y6 > (double) y2)
        {
          vector2_3.y = uvMin.y + (float) (((double) vector2_2.y - (double) uvMin.y) * ((double) y2 - (double) y5) / ((double) y6 - (double) y5));
          y6 = y2;
        }
        ImageView.AddQuad(toFill, new Vector2(0.0f, y5) + pixelAdjustedRect.position, new Vector2(x2, y6) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector4_1.x, uvMin.y), new Vector2(uvMin.x, vector2_3.y), x1, curvedUIRadius);
        ImageView.AddQuad(toFill, new Vector2(x3, y5) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, y6) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector2_2.x, uvMin.y), new Vector2(vector4_1.z, vector2_3.y), x1, curvedUIRadius);
      }
      Vector2 vector2_4 = vector2_2;
      for (float x6 = x2; (double) x6 < (double) x3; x6 += num1)
      {
        float x7 = x6 + num1;
        if ((double) x7 > (double) x3)
        {
          vector2_4.x = uvMin.x + (float) (((double) vector2_2.x - (double) uvMin.x) * ((double) x3 - (double) x6) / ((double) x7 - (double) x6));
          x7 = x3;
        }
        ImageView.AddQuad(toFill, new Vector2(x6, 0.0f) + pixelAdjustedRect.position, new Vector2(x7, y1) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(uvMin.x, vector4_1.y), new Vector2(vector2_4.x, uvMin.y), x1, curvedUIRadius);
        ImageView.AddQuad(toFill, new Vector2(x6, y2) + pixelAdjustedRect.position, new Vector2(x7, pixelAdjustedRect.height) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(uvMin.x, vector2_2.y), new Vector2(vector2_4.x, vector4_1.w), x1, curvedUIRadius);
      }
      ImageView.AddQuad(toFill, new Vector2(0.0f, 0.0f) + pixelAdjustedRect.position, new Vector2(x2, y1) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector4_1.x, vector4_1.y), new Vector2(uvMin.x, uvMin.y), x1, curvedUIRadius);
      ImageView.AddQuad(toFill, new Vector2(x3, 0.0f) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, y1) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector2_2.x, vector4_1.y), new Vector2(vector4_1.z, uvMin.y), x1, curvedUIRadius);
      ImageView.AddQuad(toFill, new Vector2(0.0f, y2) + pixelAdjustedRect.position, new Vector2(x2, pixelAdjustedRect.height) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector4_1.x, vector2_2.y), new Vector2(uvMin.x, vector4_1.w), x1, curvedUIRadius);
      ImageView.AddQuad(toFill, new Vector2(x3, y2) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, pixelAdjustedRect.height) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector2_2.x, vector2_2.y), new Vector2(vector4_1.z, vector4_1.w), x1, curvedUIRadius);
    }

    private static void AddQuad(
      VertexHelper vertexHelper,
      Vector3[] quadPositions,
      Color32 color,
      Vector3[] quadUVs)
    {
      int currentVertCount = vertexHelper.currentVertCount;
      for (int index = 0; index < 4; ++index)
        vertexHelper.AddVert(quadPositions[index], color, (Vector2) quadUVs[index]);
      vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
      vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    private static void AddQuad(
      VertexHelper vertexHelper,
      Vector2 posMin,
      Vector2 posMax,
      Color32 color,
      Vector2 uvMin,
      Vector2 uvMax,
      float elementWidthScale,
      float curvedUIRadius)
    {
      int numberOfElements = ImageView.GetNumberOfElements(curvedUIRadius, Mathf.Abs(posMin.x - posMax.x) * elementWidthScale);
      int currentVertCount = vertexHelper.currentVertCount;
      Vector2 uv2 = new Vector2(curvedUIRadius, 0.0f);
      for (int index = 0; index < numberOfElements + 1; ++index)
      {
        float t = (float) index / (float) numberOfElements;
        float x1 = Mathf.Lerp(posMin.x, posMax.x, t);
        float x2 = Mathf.Lerp(uvMin.x, uvMax.x, t);
        vertexHelper.AddVert(new Vector3(x1, posMin.y), color, new Vector2(x2, uvMin.y), ImageView.kVec2Zero, uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
        vertexHelper.AddVert(new Vector3(x1, posMax.y), color, new Vector2(x2, uvMax.y), ImageView.kVec2Zero, uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
      }
      for (int index = 0; index < numberOfElements; ++index)
      {
        int idx0 = index * 2 + currentVertCount;
        vertexHelper.AddTriangle(idx0, 1 + idx0, 2 + idx0);
        vertexHelper.AddTriangle(2 + idx0, 3 + idx0, 1 + idx0);
      }
    }

    private static void AddQuad(
      VertexHelper vertexHelper,
      Vector2 posMin,
      Vector2 posMax,
      Color32 color,
      Vector2 uv0Min,
      Vector2 uv0Max,
      Vector2 uv1Min,
      Vector2 uv1Max,
      float elementWidthScale,
      float curvedUIRadius,
      float skewFactor,
      float skewOffset)
    {
      int numberOfElements = ImageView.GetNumberOfElements(curvedUIRadius, Mathf.Abs(posMin.x - posMax.x) * elementWidthScale);
      int currentVertCount = vertexHelper.currentVertCount;
      Vector2 uv2 = new Vector2(curvedUIRadius, 0.0f);
      for (int index = 0; index < numberOfElements + 1; ++index)
      {
        float t = (float) index / (float) numberOfElements;
        float num = Mathf.Lerp(posMin.x, posMax.x, t);
        float x1 = Mathf.Lerp(uv0Min.x, uv0Max.x, t);
        float x2 = Mathf.Lerp(uv1Min.x, uv1Max.x, t);
        vertexHelper.AddVert(new Vector3(num + skewFactor * (uv1Min.y - skewOffset), posMin.y), color, new Vector2(x1, uv0Min.y), new Vector2(x2, uv1Min.y), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
        vertexHelper.AddVert(new Vector3(num + skewFactor * (uv1Max.y - skewOffset), posMax.y), color, new Vector2(x1, uv0Max.y), new Vector2(x2, uv1Max.y), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
      }
      for (int index = 0; index < numberOfElements; ++index)
      {
        int idx0 = index * 2 + currentVertCount;
        vertexHelper.AddTriangle(idx0, 1 + idx0, 2 + idx0);
        vertexHelper.AddTriangle(2 + idx0, 3 + idx0, 1 + idx0);
      }
    }

    private static void AddQuadWithHorizontalGradient(
      VertexHelper vertexHelper,
      Vector2 posMin,
      Vector2 posMax,
      Color32 color0,
      Color32 color1,
      Vector2 uv0Min,
      Vector2 uv0Max,
      Vector2 uv1Min,
      Vector2 uv1Max,
      float elementWidthScale,
      float curvedUIRadius,
      float skewFactor,
      float skewOffset)
    {
      int numberOfElements = ImageView.GetNumberOfElements(curvedUIRadius, Mathf.Abs(posMin.x - posMax.x) * elementWidthScale);
      int currentVertCount = vertexHelper.currentVertCount;
      Vector2 uv2 = new Vector2(curvedUIRadius, 0.0f);
      for (int index = 0; index < numberOfElements + 1; ++index)
      {
        float t = (float) index / (float) numberOfElements;
        float num = Mathf.Lerp(posMin.x, posMax.x, t);
        float x1 = Mathf.Lerp(uv0Min.x, uv0Max.x, t);
        float x2 = Mathf.Lerp(uv1Min.x, uv1Max.x, t);
        Color color = Color.Lerp((Color) color0, (Color) color1, t);
        vertexHelper.AddVert(new Vector3(num + skewFactor * (uv1Min.y - skewOffset), posMin.y), (Color32) color, new Vector2(x1, uv0Min.y), new Vector2(x2, uv1Min.y), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
        vertexHelper.AddVert(new Vector3(num + skewFactor * (uv1Max.y - skewOffset), posMax.y), (Color32) color, new Vector2(x1, uv0Max.y), new Vector2(x2, uv1Max.y), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
      }
      for (int index = 0; index < numberOfElements; ++index)
      {
        int idx0 = index * 2 + currentVertCount;
        vertexHelper.AddTriangle(idx0, 1 + idx0, 2 + idx0);
        vertexHelper.AddTriangle(2 + idx0, 3 + idx0, 1 + idx0);
      }
    }

    private static void AddQuadWithVerticalGradient(
      VertexHelper vertexHelper,
      Vector2 posMin,
      Vector2 posMax,
      Color32 color0,
      Color32 color1,
      Vector2 uv0Min,
      Vector2 uv0Max,
      Vector2 uv1Min,
      Vector2 uv1Max,
      float elementWidthScale,
      float curvedUIRadius,
      float skewFactor,
      float skewOffset)
    {
      int numberOfElements = ImageView.GetNumberOfElements(curvedUIRadius, Mathf.Abs(posMin.x - posMax.x) * elementWidthScale);
      int currentVertCount = vertexHelper.currentVertCount;
      Vector2 uv2 = new Vector2(curvedUIRadius, 0.0f);
      for (int index = 0; index < numberOfElements + 1; ++index)
      {
        float t = (float) index / (float) numberOfElements;
        float num = Mathf.Lerp(posMin.x, posMax.x, t);
        float x1 = Mathf.Lerp(uv0Min.x, uv0Max.x, t);
        float x2 = Mathf.Lerp(uv1Min.x, uv1Max.x, t);
        vertexHelper.AddVert(new Vector3(num + skewFactor * (uv1Min.y - skewOffset), posMin.y), color0, new Vector2(x1, uv0Min.y), new Vector2(x2, uv1Min.y), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
        vertexHelper.AddVert(new Vector3(num + skewFactor * (uv1Max.y - skewOffset), posMax.y), color1, new Vector2(x1, uv0Max.y), new Vector2(x2, uv1Max.y), uv2, ImageView.kVec2Zero, ImageView.kVec3Zero, ImageView.kVec4Zero);
      }
      for (int index = 0; index < numberOfElements; ++index)
      {
        int idx0 = index * 2 + currentVertCount;
        vertexHelper.AddTriangle(idx0, 1 + idx0, 2 + idx0);
        vertexHelper.AddTriangle(2 + idx0, 3 + idx0, 1 + idx0);
      }
    }

    private static Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
    {
      for (int index = 0; index <= 1; ++index)
      {
        float num1 = border[index] + border[index + 2];
        if ((double) rect.size[index] < (double) num1 && (double) num1 != 0.0)
        {
          float num2 = rect.size[index] / num1;
          border[index] *= num2;
          border[index + 2] *= num2;
        }
      }
      return border;
    }

    public virtual void GenerateFilledSprite(
      VertexHelper toFill,
      bool preserveAspect,
      float curvedUIRadius)
    {
      toFill.Clear();
      if ((double) this.fillAmount < 1.0 / 1000.0)
        return;
      Vector4 drawingDimensions = this.GetDrawingDimensions(preserveAspect);
      Vector4 vector4 = (Object) this.overrideSprite != (Object) null ? DataUtility.GetOuterUV(this.overrideSprite) : Vector4.zero;
      UIVertex.simpleVert.color = (Color32) this.color;
      float num1 = vector4.x;
      float num2 = vector4.y;
      float num3 = vector4.z;
      float num4 = vector4.w;
      if (this.fillMethod == Image.FillMethod.Horizontal || this.fillMethod == Image.FillMethod.Vertical)
      {
        if (this.fillMethod == Image.FillMethod.Horizontal)
        {
          float num5 = (num3 - num1) * this.fillAmount;
          if (this.fillOrigin == 1)
          {
            drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.fillAmount;
            num1 = num3 - num5;
          }
          else
          {
            drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.fillAmount;
            num3 = num1 + num5;
          }
        }
        else if (this.fillMethod == Image.FillMethod.Vertical)
        {
          float num6 = (num4 - num2) * this.fillAmount;
          if (this.fillOrigin == 1)
          {
            drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.fillAmount;
            num2 = num4 - num6;
          }
          else
          {
            drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.fillAmount;
            num4 = num2 + num6;
          }
        }
      }
      ImageView.s_Xy[0] = (Vector3) new Vector2(drawingDimensions.x, drawingDimensions.y);
      ImageView.s_Xy[1] = (Vector3) new Vector2(drawingDimensions.x, drawingDimensions.w);
      ImageView.s_Xy[2] = (Vector3) new Vector2(drawingDimensions.z, drawingDimensions.w);
      ImageView.s_Xy[3] = (Vector3) new Vector2(drawingDimensions.z, drawingDimensions.y);
      ImageView.s_Uv[0] = (Vector3) new Vector2(num1, num2);
      ImageView.s_Uv[1] = (Vector3) new Vector2(num1, num4);
      ImageView.s_Uv[2] = (Vector3) new Vector2(num3, num4);
      ImageView.s_Uv[3] = (Vector3) new Vector2(num3, num2);
      if ((double) this.fillAmount < 1.0 && this.fillMethod != Image.FillMethod.Horizontal && this.fillMethod != Image.FillMethod.Vertical)
      {
        if (this.fillMethod == Image.FillMethod.Radial90)
        {
          if (!ImageView.RadialCut(ImageView.s_Xy, ImageView.s_Uv, this.fillAmount, this.fillClockwise, this.fillOrigin))
            return;
          ImageView.AddQuad(toFill, ImageView.s_Xy, (Color32) this.color, ImageView.s_Uv);
        }
        else if (this.fillMethod == Image.FillMethod.Radial180)
        {
          for (int index = 0; index < 2; ++index)
          {
            int num7 = this.fillOrigin > 1 ? 1 : 0;
            float t1;
            float t2;
            float t3;
            float t4;
            if (this.fillOrigin == 0 || this.fillOrigin == 2)
            {
              t1 = 0.0f;
              t2 = 1f;
              if (index == num7)
              {
                t3 = 0.0f;
                t4 = 0.5f;
              }
              else
              {
                t3 = 0.5f;
                t4 = 1f;
              }
            }
            else
            {
              t3 = 0.0f;
              t4 = 1f;
              if (index == num7)
              {
                t1 = 0.5f;
                t2 = 1f;
              }
              else
              {
                t1 = 0.0f;
                t2 = 0.5f;
              }
            }
            ImageView.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t3);
            ImageView.s_Xy[1].x = ImageView.s_Xy[0].x;
            ImageView.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t4);
            ImageView.s_Xy[3].x = ImageView.s_Xy[2].x;
            ImageView.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t1);
            ImageView.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t2);
            ImageView.s_Xy[2].y = ImageView.s_Xy[1].y;
            ImageView.s_Xy[3].y = ImageView.s_Xy[0].y;
            ImageView.s_Uv[0].x = Mathf.Lerp(num1, num3, t3);
            ImageView.s_Uv[1].x = ImageView.s_Uv[0].x;
            ImageView.s_Uv[2].x = Mathf.Lerp(num1, num3, t4);
            ImageView.s_Uv[3].x = ImageView.s_Uv[2].x;
            ImageView.s_Uv[0].y = Mathf.Lerp(num2, num4, t1);
            ImageView.s_Uv[1].y = Mathf.Lerp(num2, num4, t2);
            ImageView.s_Uv[2].y = ImageView.s_Uv[1].y;
            ImageView.s_Uv[3].y = ImageView.s_Uv[0].y;
            float num8 = this.fillClockwise ? this.fillAmount * 2f - (float) index : this.fillAmount * 2f - (float) (1 - index);
            if (ImageView.RadialCut(ImageView.s_Xy, ImageView.s_Uv, Mathf.Clamp01(num8), this.fillClockwise, (index + this.fillOrigin + 3) % 4))
              ImageView.AddQuad(toFill, ImageView.s_Xy, (Color32) this.color, ImageView.s_Uv);
          }
        }
        else
        {
          if (this.fillMethod != Image.FillMethod.Radial360)
            return;
          for (int index = 0; index < 4; ++index)
          {
            float t5;
            float t6;
            if (index < 2)
            {
              t5 = 0.0f;
              t6 = 0.5f;
            }
            else
            {
              t5 = 0.5f;
              t6 = 1f;
            }
            float t7;
            float t8;
            if (index == 0 || index == 3)
            {
              t7 = 0.0f;
              t8 = 0.5f;
            }
            else
            {
              t7 = 0.5f;
              t8 = 1f;
            }
            ImageView.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t5);
            ImageView.s_Xy[1].x = ImageView.s_Xy[0].x;
            ImageView.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t6);
            ImageView.s_Xy[3].x = ImageView.s_Xy[2].x;
            ImageView.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t7);
            ImageView.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t8);
            ImageView.s_Xy[2].y = ImageView.s_Xy[1].y;
            ImageView.s_Xy[3].y = ImageView.s_Xy[0].y;
            ImageView.s_Uv[0].x = Mathf.Lerp(num1, num3, t5);
            ImageView.s_Uv[1].x = ImageView.s_Uv[0].x;
            ImageView.s_Uv[2].x = Mathf.Lerp(num1, num3, t6);
            ImageView.s_Uv[3].x = ImageView.s_Uv[2].x;
            ImageView.s_Uv[0].y = Mathf.Lerp(num2, num4, t7);
            ImageView.s_Uv[1].y = Mathf.Lerp(num2, num4, t8);
            ImageView.s_Uv[2].y = ImageView.s_Uv[1].y;
            ImageView.s_Uv[3].y = ImageView.s_Uv[0].y;
            float num9 = this.fillClockwise ? this.fillAmount * 4f - (float) ((index + this.fillOrigin) % 4) : this.fillAmount * 4f - (float) (3 - (index + this.fillOrigin) % 4);
            if (ImageView.RadialCut(ImageView.s_Xy, ImageView.s_Uv, Mathf.Clamp01(num9), this.fillClockwise, (index + 2) % 4))
              ImageView.AddQuad(toFill, ImageView.s_Xy, (Color32) this.color, ImageView.s_Uv);
          }
        }
      }
      else
      {
        float x = this.transform.localScale.x;
        ImageView.AddQuad(toFill, (Vector2) ImageView.s_Xy[0], (Vector2) ImageView.s_Xy[2], (Color32) this.color, (Vector2) ImageView.s_Uv[0], (Vector2) ImageView.s_Uv[2], x, curvedUIRadius);
      }
    }

    private static bool RadialCut(
      Vector3[] xy,
      Vector3[] uv,
      float fill,
      bool invert,
      int corner)
    {
      if ((double) fill < 1.0 / 1000.0)
        return false;
      if ((corner & 1) == 1)
        invert = !invert;
      if (!invert && (double) fill > 0.99900001287460327)
        return true;
      float num = Mathf.Clamp01(fill);
      if (invert)
        num = 1f - num;
      float f = num * 1.57079637f;
      float cos = Mathf.Cos(f);
      float sin = Mathf.Sin(f);
      ImageView.RadialCut(xy, cos, sin, invert, corner);
      ImageView.RadialCut(uv, cos, sin, invert, corner);
      return true;
    }

    private static void RadialCut(Vector3[] xy, float cos, float sin, bool invert, int corner)
    {
      int index1 = corner;
      int index2 = (corner + 1) % 4;
      int index3 = (corner + 2) % 4;
      int index4 = (corner + 3) % 4;
      if ((corner & 1) == 1)
      {
        if ((double) sin > (double) cos)
        {
          cos /= sin;
          sin = 1f;
          if (invert)
          {
            xy[index2].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
            xy[index3].x = xy[index2].x;
          }
        }
        else if ((double) cos > (double) sin)
        {
          sin /= cos;
          cos = 1f;
          if (!invert)
          {
            xy[index3].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
            xy[index4].y = xy[index3].y;
          }
        }
        else
        {
          cos = 1f;
          sin = 1f;
        }
        if (!invert)
          xy[index4].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
        else
          xy[index2].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
      }
      else
      {
        if ((double) cos > (double) sin)
        {
          sin /= cos;
          cos = 1f;
          if (!invert)
          {
            xy[index2].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
            xy[index3].y = xy[index2].y;
          }
        }
        else if ((double) sin > (double) cos)
        {
          cos /= sin;
          sin = 1f;
          if (invert)
          {
            xy[index3].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
            xy[index4].x = xy[index3].x;
          }
        }
        else
        {
          cos = 1f;
          sin = 1f;
        }
        if (invert)
          xy[index4].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
        else
          xy[index2].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
      }
    }

    private static int GetNumberOfElements(float curvedUIRadius, float width)
    {
      if ((double) curvedUIRadius <= 9.9999997473787516E-05)
        return 1;
      int numberOfElements = Mathf.CeilToInt(Mathf.Abs(width) / 10f);
      if (numberOfElements < 1)
        numberOfElements = 1;
      return numberOfElements;
    }

    public virtual Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
    {
      Vector4 vector4 = (Object) this.overrideSprite == (Object) null ? Vector4.zero : DataUtility.GetPadding(this.overrideSprite);
      Vector2 vector2_1;
      if (!((Object) this.overrideSprite == (Object) null))
      {
        Rect rect = this.overrideSprite.rect;
        double width = (double) rect.width;
        rect = this.overrideSprite.rect;
        double height = (double) rect.height;
        vector2_1 = new Vector2((float) width, (float) height);
      }
      else
        vector2_1 = Vector2.zero;
      Vector2 vector2_2 = vector2_1;
      Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
      int num1 = Mathf.RoundToInt(vector2_2.x);
      int num2 = Mathf.RoundToInt(vector2_2.y);
      Vector4 drawingDimensions = new Vector4(vector4.x / (float) num1, vector4.y / (float) num2, ((float) num1 - vector4.z) / (float) num1, ((float) num2 - vector4.w) / (float) num2);
      if (shouldPreserveAspect && (double) vector2_2.sqrMagnitude > 0.0)
      {
        float num3 = vector2_2.x / vector2_2.y;
        float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
        if ((double) num3 > (double) num4)
        {
          float height = pixelAdjustedRect.height;
          pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
          pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * this.rectTransform.pivot.y;
        }
        else
        {
          float width = pixelAdjustedRect.width;
          pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
          pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * this.rectTransform.pivot.x;
        }
      }
      drawingDimensions = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * drawingDimensions.x, pixelAdjustedRect.y + pixelAdjustedRect.height * drawingDimensions.y, pixelAdjustedRect.x + pixelAdjustedRect.width * drawingDimensions.z, pixelAdjustedRect.y + pixelAdjustedRect.height * drawingDimensions.w);
      return drawingDimensions;
    }

    public enum GradientDirection
    {
      Horizontal,
      Vertical,
    }
  }
}
