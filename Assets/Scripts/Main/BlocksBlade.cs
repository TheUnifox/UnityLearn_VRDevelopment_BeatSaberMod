// Decompiled with JetBrains decompiler
// Type: BlocksBlade
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class BlocksBlade : MonoBehaviour
{
  [SerializeField]
  protected Mesh _elementMesh;
  [SerializeField]
  protected Material _material;
  [SerializeField]
  protected int _numberOfElements = 25;
  [SerializeField]
  protected float _radius = 0.3f;
  [SerializeField]
  protected float _length = 1f;
  [SerializeField]
  protected float _minVelocity = 3f;
  [SerializeField]
  protected float _maxVelocity = 5f;
  [SerializeField]
  protected float _elementWidth = 0.01f;
  [SerializeField]
  protected float _minElementLength = 0.1f;
  [SerializeField]
  protected float _maxElementLength = 0.5f;
  [CompilerGenerated]
  protected Color m_Ccolor;
  protected List<BlocksBlade.Element> _elements;
  protected Vector4[] _positions;
  protected Vector4[] _sizes;
  protected Vector4[] _colors;
  protected Matrix4x4[] _matrices;
  protected MaterialPropertyBlock _materialPropertyBlock;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _positionPropertyID = Shader.PropertyToID("_Position");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _sizePropertyID = Shader.PropertyToID("_Size");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorPropertyID = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _zClipPropertyID = Shader.PropertyToID("_ZClip");
  protected int _layer;

  public Color color
  {
    get => this.m_Ccolor;
    set => this.m_Ccolor = value;
  }

  public virtual void Start()
  {
    this._layer = this.gameObject.layer;
    this._positions = new Vector4[this._numberOfElements];
    this._sizes = new Vector4[this._numberOfElements];
    this._matrices = new Matrix4x4[this._numberOfElements];
    this._colors = new Vector4[this._numberOfElements];
    this._elements = new List<BlocksBlade.Element>(this._numberOfElements);
    Color color = this.color;
    for (int index = 0; index < this._numberOfElements; ++index)
    {
      BlocksBlade.Element element = new BlocksBlade.Element();
      element.idx = index;
      this.SetUpElement(element, Random.Range(this._minVelocity, this._maxVelocity), color);
      this._elements.Add(element);
    }
    this._materialPropertyBlock = new MaterialPropertyBlock();
  }

  public virtual void Update()
  {
    Matrix4x4 localToWorldMatrix = this.transform.localToWorldMatrix;
    Color color = this.color;
    foreach (BlocksBlade.Element element in this._elements)
    {
      this._positions[element.idx].z += element.velocity * Time.deltaTime;
      Vector4 position = this._positions[element.idx];
      Vector4 siz = this._sizes[element.idx];
      if ((double) element.velocity > 0.0 && (double) position.z - (double) siz.z * 0.5 > (double) this._length)
        this.SetUpElement(element, Random.Range(this._minVelocity, this._maxVelocity), color);
      this._matrices[element.idx] = localToWorldMatrix;
    }
    this._materialPropertyBlock.SetVectorArray(BlocksBlade._positionPropertyID, this._positions);
    this._materialPropertyBlock.SetVectorArray(BlocksBlade._sizePropertyID, this._sizes);
    this._materialPropertyBlock.SetVectorArray(BlocksBlade._colorPropertyID, this._colors);
    this._materialPropertyBlock.SetFloat(BlocksBlade._zClipPropertyID, this._length);
    Graphics.DrawMeshInstanced(this._elementMesh, 0, this._material, this._matrices, this._matrices.Length, this._materialPropertyBlock, ShadowCastingMode.Off, false, this._layer);
  }

  public virtual void SetUpElement(BlocksBlade.Element element, float velocity, Color color)
  {
    Vector4 vector4 = new Vector4(this._elementWidth, this._elementWidth, Random.Range(this._minElementLength, this._maxElementLength), 1f);
    this._sizes[element.idx] = vector4;
    this._positions[element.idx] = (Vector4) this.RandomPointOnCircle(this._radius - this._elementWidth * 0.5f);
    this._positions[element.idx].z = (float) (-(double) this._sizes[element.idx].z * 0.5);
    element.velocity = velocity;
    color.a = 1f;
    this._colors[element.idx] = (Vector4) color;
  }

  public virtual Vector2 RandomPointOnCircle(float radius)
  {
    double f = (double) Random.Range(0.0f, 6.28318548f);
    return new Vector2(Mathf.Sin((float) f) * radius, Mathf.Cos((float) f) * radius);
  }

  public class Element
  {
    public int idx;
    public float velocity;
  }
}
