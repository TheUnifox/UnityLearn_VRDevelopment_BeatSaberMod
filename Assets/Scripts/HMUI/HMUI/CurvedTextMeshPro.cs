// Decompiled with JetBrains decompiler
// Type: HMUI.CurvedTextMeshPro
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using TMPro;
using UnityEngine;

namespace HMUI
{
  [DisallowMultipleComponent]
  [ExecuteAlways]
  public class CurvedTextMeshPro : TextMeshProUGUI, IComponentRefresher
  {
    [SerializeField]
    protected bool _useScriptableObjectColors;
    [SerializeField]
    [NullAllowed]
    protected ColorSO _colorSo;
    protected readonly CurvedCanvasSettingsHelper _curvedCanvasSettingsHelper = new CurvedCanvasSettingsHelper();

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

    protected override void OnEnable()
    {
      base.OnEnable();
      this._curvedCanvasSettingsHelper.Reset();
    }

    protected override void GenerateTextMesh()
    {
      base.GenerateTextMesh();
      
      CurvedCanvasSettings curvedCanvasSettings = this._curvedCanvasSettingsHelper.GetCurvedCanvasSettings(this.canvas);
      Vector2 vector2 = new Vector2((Object) curvedCanvasSettings == (Object) null ? 0.0f : curvedCanvasSettings.radius, 0.0f);
      int vertexCount1 = this.m_mesh.vertexCount;
      Vector2[] vector2Array1 = new Vector2[vertexCount1];
      for (int index = 0; index < vertexCount1; ++index)
        vector2Array1[index] = vector2;
      this.m_mesh.uv3 = vector2Array1;
      if (this._useScriptableObjectColors)
      {
        Color[] colors = this.m_mesh.colors;
        for (int index = 0; index < colors.Length; ++index)
          colors[index] = this.color;
        this.m_mesh.colors = colors;
      }
      this.canvasRenderer.SetMesh(this.m_mesh);
      for (int index1 = 1; index1 < this.m_textInfo.materialCount; ++index1)
      {
        if (!((Object) this.m_subTextObjects[index1] == (Object) null))
        {
          Mesh mesh = this.m_textInfo.meshInfo[index1].mesh;
          int vertexCount2 = mesh.vertexCount;
          Vector2[] vector2Array2 = new Vector2[vertexCount2];
          for (int index2 = 0; index2 < vertexCount2; ++index2)
            vector2Array2[index2] = vector2;
          mesh.uv3 = vector2Array2;
          if (this._useScriptableObjectColors)
          {
            Color[] colors = mesh.colors;
            for (int index3 = 0; index3 < mesh.colors.Length; ++index3)
              colors[index3] = this.color;
            mesh.colors = colors;
          }
          this.m_subTextObjects[index1].canvasRenderer.SetMesh(mesh);
        }
      }
    }

    public virtual void __Refresh() => this.SetAllDirty();
  }
}
