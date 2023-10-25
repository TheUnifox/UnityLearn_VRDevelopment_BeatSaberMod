// Decompiled with JetBrains decompiler
// Type: RectangleFakeGlow
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteInEditMode]
public class RectangleFakeGlow : MonoBehaviour
{
  [SerializeField]
  protected Vector2 _size = new Vector2(1f, 1f);
  [SerializeField]
  protected float _edgeSize = 0.1f;
  [SerializeField]
  protected Color _color = Color.white;
  [Space]
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorID = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _sizeParamsID = Shader.PropertyToID("_SizeParams");

  public Color color
  {
    set
    {
      this._color = value;
      this._materialPropertyBlockController.materialPropertyBlock.SetColor(RectangleFakeGlow._colorID, this._color);
      this._materialPropertyBlockController.ApplyChanges();
    }
    get => this._color;
  }

  public virtual void Awake()
  {
    foreach (Renderer renderer in this._materialPropertyBlockController.renderers)
      renderer.enabled = false;
  }

  public virtual void OnEnable()
  {
    this.Refresh();
    foreach (Renderer renderer in this._materialPropertyBlockController.renderers)
      renderer.enabled = true;
  }

  public virtual void OnDisable()
  {
    foreach (Renderer renderer in this._materialPropertyBlockController.renderers)
      renderer.enabled = false;
  }

  public virtual void Refresh()
  {
    Vector4 vector4 = new Vector4(this._size.x * 0.5f, this._size.y * 0.5f, 1f, this._edgeSize * 0.5f);
    this.transform.localScale = (Vector3) vector4;
    MaterialPropertyBlock materialPropertyBlock = this._materialPropertyBlockController.materialPropertyBlock;
    materialPropertyBlock.SetColor(RectangleFakeGlow._colorID, this._color);
    materialPropertyBlock.SetVector(RectangleFakeGlow._sizeParamsID, vector4);
    this._materialPropertyBlockController.ApplyChanges();
  }
}
