// Decompiled with JetBrains decompiler
// Type: HMUI.GradientImage
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace HMUI
{
  public class GradientImage : Image
  {
    [SerializeField]
    protected Color _color0;
    [SerializeField]
    protected Color _color1;
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
    protected static readonly Color[] s_ColorScratch = new Color[4];
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector3[] s_Xy = new Vector3[4];
    [DoesNotRequireDomainReloadInit]
    protected static readonly Vector3[] s_Uv = new Vector3[4];

    public Color color0
    {
      get => this._color0;
      set
      {
        if (!SetPropertyUtility.SetColor(ref this._color0, value))
          return;
        this.SetAllDirty();
      }
    }

    public Color color1
    {
      get => this._color1;
      set
      {
        if (!SetPropertyUtility.SetColor(ref this._color1, value))
          return;
        this.SetAllDirty();
      }
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
            this.GenerateTiledSprite(toFill);
            break;
          case Image.Type.Filled:
            this.GenerateFilledSprite(toFill, this.preserveAspect);
            break;
          default:
            this.GenerateSimpleSprite(toFill, this.preserveAspect, curvedUIRadius);
            break;
        }
      }
    }

    public virtual void GenerateSimpleSprite(
      VertexHelper vh,
      bool lPreserveAspect,
      float curvedUIRadius)
    {
      vh.Clear();
      Vector4 drawingDimensions = this.GetDrawingDimensions(lPreserveAspect);
      Vector4 vector4 = (Object) this.overrideSprite != (Object) null ? DataUtility.GetOuterUV(this.overrideSprite) : Vector4.zero;
      Color a = this.color * this._color0;
      Color b = this.color * this._color1;
      Vector2 uv2 = new Vector2(curvedUIRadius, 0.0f);
      int num = Mathf.CeilToInt(Mathf.Abs(drawingDimensions.z - drawingDimensions.x) / 10f);
      if (num < 1)
        num = 1;
      for (int index = 0; index < num + 1; ++index)
      {
        float t = (float) index / (float) num;
        float x1 = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t);
        float x2 = Mathf.Lerp(vector4.x, vector4.z, t);
        Color32 color = Color32.Lerp((Color32) a, (Color32) b, t);
        vh.AddVert(new Vector3(x1, drawingDimensions.w), color, new Vector2(x2, vector4.w), new Vector2(1f, 0.0f), uv2, GradientImage.kVec2Zero, GradientImage.kVec3Zero, GradientImage.kVec4Zero);
        vh.AddVert(new Vector3(x1, drawingDimensions.y), color, new Vector2(x2, vector4.y), new Vector2(0.0f, 0.0f), uv2, GradientImage.kVec2Zero, GradientImage.kVec3Zero, GradientImage.kVec4Zero);
      }
      for (int index = 0; index < num; ++index)
      {
        int idx0 = index * 2;
        vh.AddTriangle(idx0, 1 + idx0, 2 + idx0);
        vh.AddTriangle(2 + idx0, 3 + idx0, 1 + idx0);
      }
    }

    public virtual void GenerateSlicedSprite(VertexHelper vh, float curvedUIRadius)
    {
      if (!this.hasBorder)
      {
        this.GenerateSimpleSprite(vh, false, curvedUIRadius);
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
        Vector4 adjustedBorders = this.GetAdjustedBorders(vector4_4 / this.pixelsPerUnit, pixelAdjustedRect);
        Vector4 vector4_5 = vector4_3 / this.pixelsPerUnit;
        GradientImage.s_VertScratch[0] = new Vector2(vector4_5.x, vector4_5.y);
        GradientImage.s_VertScratch[3] = new Vector2(pixelAdjustedRect.width - vector4_5.z, pixelAdjustedRect.height - vector4_5.w);
        GradientImage.s_VertScratch[1].x = adjustedBorders.x;
        GradientImage.s_VertScratch[1].y = adjustedBorders.y;
        GradientImage.s_VertScratch[2].x = pixelAdjustedRect.width - adjustedBorders.z;
        GradientImage.s_VertScratch[2].y = pixelAdjustedRect.height - adjustedBorders.w;
        for (int index = 0; index < 4; ++index)
        {
          GradientImage.s_VertScratch[index].x += pixelAdjustedRect.x;
          GradientImage.s_VertScratch[index].y += pixelAdjustedRect.y;
        }
        GradientImage.s_UVScratch[0] = new Vector2(vector4_1.x, vector4_1.y);
        GradientImage.s_UVScratch[1] = new Vector2(vector4_2.x, vector4_2.y);
        GradientImage.s_UVScratch[2] = new Vector2(vector4_2.z, vector4_2.w);
        GradientImage.s_UVScratch[3] = new Vector2(vector4_1.z, vector4_1.w);
        Color a = this._color0 * this.color;
        Color b = this._color1 * this.color;
        float x1 = GradientImage.s_VertScratch[0].x;
        float num = GradientImage.s_VertScratch[3].x - GradientImage.s_VertScratch[0].x;
        GradientImage.s_ColorScratch[0] = a;
        GradientImage.s_ColorScratch[1] = Color.Lerp(a, b, (GradientImage.s_VertScratch[1].x - x1) / num);
        GradientImage.s_ColorScratch[2] = Color.Lerp(a, b, (GradientImage.s_VertScratch[2].x - x1) / num);
        GradientImage.s_ColorScratch[3] = b;
        vh.Clear();
        float x2 = this.transform.localScale.x;
        for (int index1 = 0; index1 < 3; ++index1)
        {
          int index2 = index1 + 1;
          for (int index3 = 0; index3 < 3; ++index3)
          {
            if (this.fillCenter || index1 != 1 || index3 != 1)
            {
              int index4 = index3 + 1;
              GradientImage.AddQuad(vh, new Vector2(GradientImage.s_VertScratch[index1].x, GradientImage.s_VertScratch[index3].y), new Vector2(GradientImage.s_VertScratch[index2].x, GradientImage.s_VertScratch[index4].y), (Color32) GradientImage.s_ColorScratch[index1], (Color32) GradientImage.s_ColorScratch[index2], new Vector2(GradientImage.s_UVScratch[index1].x, GradientImage.s_UVScratch[index3].y), new Vector2(GradientImage.s_UVScratch[index2].x, GradientImage.s_UVScratch[index4].y), x2, curvedUIRadius);
            }
          }
        }
      }
    }

    public virtual void GenerateTiledSprite(VertexHelper toFill)
    {
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
      Vector4 adjustedBorders = this.GetAdjustedBorders(vector4_3 / this.pixelsPerUnit, pixelAdjustedRect);
      Vector2 uvMin = new Vector2(vector4_2.x, vector4_2.y);
      Vector2 vector2_2 = new Vector2(vector4_2.z, vector4_2.w);
      UIVertex.simpleVert.color = (Color32) this.color;
      float x1 = adjustedBorders.x;
      float x2 = pixelAdjustedRect.width - adjustedBorders.z;
      float y1 = adjustedBorders.y;
      float y2 = pixelAdjustedRect.height - adjustedBorders.w;
      toFill.Clear();
      Vector2 uvMax = vector2_2;
      if ((double) num1 == 0.0)
        num1 = x2 - x1;
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
          for (float x3 = x1; (double) x3 < (double) x2; x3 += num1)
          {
            float x4 = x3 + num1;
            if ((double) x4 > (double) x2)
            {
              uvMax.x = uvMin.x + (float) (((double) vector2_2.x - (double) uvMin.x) * ((double) x2 - (double) x3) / ((double) x4 - (double) x3));
              x4 = x2;
            }
            GradientImage.AddQuad(toFill, new Vector2(x3, y3) + pixelAdjustedRect.position, new Vector2(x4, y4) + pixelAdjustedRect.position, (Color32) this.color, uvMin, uvMax);
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
        GradientImage.AddQuad(toFill, new Vector2(0.0f, y5) + pixelAdjustedRect.position, new Vector2(x1, y6) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector4_1.x, uvMin.y), new Vector2(uvMin.x, vector2_3.y));
        GradientImage.AddQuad(toFill, new Vector2(x2, y5) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, y6) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector2_2.x, uvMin.y), new Vector2(vector4_1.z, vector2_3.y));
      }
      Vector2 vector2_4 = vector2_2;
      for (float x5 = x1; (double) x5 < (double) x2; x5 += num1)
      {
        float x6 = x5 + num1;
        if ((double) x6 > (double) x2)
        {
          vector2_4.x = uvMin.x + (float) (((double) vector2_2.x - (double) uvMin.x) * ((double) x2 - (double) x5) / ((double) x6 - (double) x5));
          x6 = x2;
        }
        GradientImage.AddQuad(toFill, new Vector2(x5, 0.0f) + pixelAdjustedRect.position, new Vector2(x6, y1) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(uvMin.x, vector4_1.y), new Vector2(vector2_4.x, uvMin.y));
        GradientImage.AddQuad(toFill, new Vector2(x5, y2) + pixelAdjustedRect.position, new Vector2(x6, pixelAdjustedRect.height) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(uvMin.x, vector2_2.y), new Vector2(vector2_4.x, vector4_1.w));
      }
      GradientImage.AddQuad(toFill, new Vector2(0.0f, 0.0f) + pixelAdjustedRect.position, new Vector2(x1, y1) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector4_1.x, vector4_1.y), new Vector2(uvMin.x, uvMin.y));
      GradientImage.AddQuad(toFill, new Vector2(x2, 0.0f) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, y1) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector2_2.x, vector4_1.y), new Vector2(vector4_1.z, uvMin.y));
      GradientImage.AddQuad(toFill, new Vector2(0.0f, y2) + pixelAdjustedRect.position, new Vector2(x1, pixelAdjustedRect.height) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector4_1.x, vector2_2.y), new Vector2(uvMin.x, vector4_1.w));
      GradientImage.AddQuad(toFill, new Vector2(x2, y2) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, pixelAdjustedRect.height) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector2_2.x, vector2_2.y), new Vector2(vector4_1.z, vector4_1.w));
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
      Vector2 uvMax)
    {
      int currentVertCount = vertexHelper.currentVertCount;
      vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0.0f), color, new Vector2(uvMin.x, uvMin.y));
      vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0.0f), color, new Vector2(uvMin.x, uvMax.y));
      vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0.0f), color, new Vector2(uvMax.x, uvMax.y));
      vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0.0f), color, new Vector2(uvMax.x, uvMin.y));
      vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
      vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    private static void AddQuad(
      VertexHelper vertexHelper,
      Vector2 posMin,
      Vector2 posMax,
      Color32 color0,
      Color32 color1,
      Vector2 uv0Min,
      Vector2 uv0Max,
      float elementWidthScale,
      float curvedUIRadius)
    {
      int num = Mathf.CeilToInt((float) ((double) Mathf.Abs(posMin.x - posMax.x) * (double) elementWidthScale / 10.0));
      if (num < 1)
        num = 1;
      int currentVertCount = vertexHelper.currentVertCount;
      Vector2 uv2 = new Vector2(curvedUIRadius, 0.0f);
      for (int index = 0; index < num + 1; ++index)
      {
        float t = (float) index / (float) num;
        float x1 = Mathf.Lerp(posMin.x, posMax.x, t);
        float x2 = Mathf.Lerp(uv0Min.x, uv0Max.x, t);
        Color color = Color.Lerp((Color) color0, (Color) color1, t);
        vertexHelper.AddVert(new Vector3(x1, posMin.y), (Color32) color, new Vector2(x2, uv0Min.y), GradientImage.kVec2Zero, uv2, GradientImage.kVec2Zero, GradientImage.kVec3Zero, GradientImage.kVec4Zero);
        vertexHelper.AddVert(new Vector3(x1, posMax.y), (Color32) color, new Vector2(x2, uv0Max.y), GradientImage.kVec2Zero, uv2, GradientImage.kVec2Zero, GradientImage.kVec3Zero, GradientImage.kVec4Zero);
      }
      for (int index = 0; index < num; ++index)
      {
        int idx0 = index * 2 + currentVertCount;
        vertexHelper.AddTriangle(idx0, 1 + idx0, 2 + idx0);
        vertexHelper.AddTriangle(2 + idx0, 3 + idx0, 1 + idx0);
      }
    }

    public virtual Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
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

    public virtual void GenerateFilledSprite(VertexHelper toFill, bool preserveAspect)
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
      GradientImage.s_Xy[0] = (Vector3) new Vector2(drawingDimensions.x, drawingDimensions.y);
      GradientImage.s_Xy[1] = (Vector3) new Vector2(drawingDimensions.x, drawingDimensions.w);
      GradientImage.s_Xy[2] = (Vector3) new Vector2(drawingDimensions.z, drawingDimensions.w);
      GradientImage.s_Xy[3] = (Vector3) new Vector2(drawingDimensions.z, drawingDimensions.y);
      GradientImage.s_Uv[0] = (Vector3) new Vector2(num1, num2);
      GradientImage.s_Uv[1] = (Vector3) new Vector2(num1, num4);
      GradientImage.s_Uv[2] = (Vector3) new Vector2(num3, num4);
      GradientImage.s_Uv[3] = (Vector3) new Vector2(num3, num2);
      if ((double) this.fillAmount < 1.0 && this.fillMethod != Image.FillMethod.Horizontal && this.fillMethod != Image.FillMethod.Vertical)
      {
        if (this.fillMethod == Image.FillMethod.Radial90)
        {
          if (!GradientImage.RadialCut(GradientImage.s_Xy, GradientImage.s_Uv, this.fillAmount, this.fillClockwise, this.fillOrigin))
            return;
          GradientImage.AddQuad(toFill, GradientImage.s_Xy, (Color32) this.color, GradientImage.s_Uv);
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
            GradientImage.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t3);
            GradientImage.s_Xy[1].x = GradientImage.s_Xy[0].x;
            GradientImage.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t4);
            GradientImage.s_Xy[3].x = GradientImage.s_Xy[2].x;
            GradientImage.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t1);
            GradientImage.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t2);
            GradientImage.s_Xy[2].y = GradientImage.s_Xy[1].y;
            GradientImage.s_Xy[3].y = GradientImage.s_Xy[0].y;
            GradientImage.s_Uv[0].x = Mathf.Lerp(num1, num3, t3);
            GradientImage.s_Uv[1].x = GradientImage.s_Uv[0].x;
            GradientImage.s_Uv[2].x = Mathf.Lerp(num1, num3, t4);
            GradientImage.s_Uv[3].x = GradientImage.s_Uv[2].x;
            GradientImage.s_Uv[0].y = Mathf.Lerp(num2, num4, t1);
            GradientImage.s_Uv[1].y = Mathf.Lerp(num2, num4, t2);
            GradientImage.s_Uv[2].y = GradientImage.s_Uv[1].y;
            GradientImage.s_Uv[3].y = GradientImage.s_Uv[0].y;
            float num8 = this.fillClockwise ? this.fillAmount * 2f - (float) index : this.fillAmount * 2f - (float) (1 - index);
            if (GradientImage.RadialCut(GradientImage.s_Xy, GradientImage.s_Uv, Mathf.Clamp01(num8), this.fillClockwise, (index + this.fillOrigin + 3) % 4))
              GradientImage.AddQuad(toFill, GradientImage.s_Xy, (Color32) this.color, GradientImage.s_Uv);
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
            GradientImage.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t5);
            GradientImage.s_Xy[1].x = GradientImage.s_Xy[0].x;
            GradientImage.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t6);
            GradientImage.s_Xy[3].x = GradientImage.s_Xy[2].x;
            GradientImage.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t7);
            GradientImage.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t8);
            GradientImage.s_Xy[2].y = GradientImage.s_Xy[1].y;
            GradientImage.s_Xy[3].y = GradientImage.s_Xy[0].y;
            GradientImage.s_Uv[0].x = Mathf.Lerp(num1, num3, t5);
            GradientImage.s_Uv[1].x = GradientImage.s_Uv[0].x;
            GradientImage.s_Uv[2].x = Mathf.Lerp(num1, num3, t6);
            GradientImage.s_Uv[3].x = GradientImage.s_Uv[2].x;
            GradientImage.s_Uv[0].y = Mathf.Lerp(num2, num4, t7);
            GradientImage.s_Uv[1].y = Mathf.Lerp(num2, num4, t8);
            GradientImage.s_Uv[2].y = GradientImage.s_Uv[1].y;
            GradientImage.s_Uv[3].y = GradientImage.s_Uv[0].y;
            float num9 = this.fillClockwise ? this.fillAmount * 4f - (float) ((index + this.fillOrigin) % 4) : this.fillAmount * 4f - (float) (3 - (index + this.fillOrigin) % 4);
            if (GradientImage.RadialCut(GradientImage.s_Xy, GradientImage.s_Uv, Mathf.Clamp01(num9), this.fillClockwise, (index + 2) % 4))
              GradientImage.AddQuad(toFill, GradientImage.s_Xy, (Color32) this.color, GradientImage.s_Uv);
          }
        }
      }
      else
        GradientImage.AddQuad(toFill, GradientImage.s_Xy, (Color32) this.color, GradientImage.s_Uv);
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
      GradientImage.RadialCut(xy, cos, sin, invert, corner);
      GradientImage.RadialCut(uv, cos, sin, invert, corner);
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
  }
}
