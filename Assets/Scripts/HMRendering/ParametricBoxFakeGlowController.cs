// Decompiled with JetBrains decompiler
// Type: ParametricBoxFakeGlowController
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[RequireComponent(typeof (MeshRenderer))]
[RequireComponent(typeof (MeshFilter))]
[ExecuteInEditMode]
public class ParametricBoxFakeGlowController : MonoBehaviour
{
  public float width = 1f;
  public float height = 1f;
  public float length = 1f;
  public float edgeSize = 0.1f;
  public float edgeSizeMultiplier = 4f;
  public Color color;
  [SerializeField]
  protected MeshRenderer _meshRenderer;
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorID = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _sizeParamsID = Shader.PropertyToID("_SizeParams");

  public Vector3 localPosition
  {
    set => this.transform.localPosition = value;
  }

  public virtual void Awake() => this._meshRenderer.enabled = false;

  public virtual void OnEnable()
  {
    this.Refresh();
    this._meshRenderer.enabled = true;
  }

  public virtual void OnDisable() => this._meshRenderer.enabled = false;

  public virtual void Refresh()
  {
    Vector4 vector4 = new Vector4(this.width * 0.5f, this.height * 0.5f, this.length * 0.5f, this.edgeSize * 0.5f * this.edgeSizeMultiplier);
    this.transform.localScale = (Vector3) vector4;
    MaterialPropertyBlock materialPropertyBlock = this._materialPropertyBlockController.materialPropertyBlock;
    materialPropertyBlock.SetColor(ParametricBoxFakeGlowController._colorID, this.color);
    materialPropertyBlock.SetVector(ParametricBoxFakeGlowController._sizeParamsID, vector4);
    this._materialPropertyBlockController.ApplyChanges();
  }
}
