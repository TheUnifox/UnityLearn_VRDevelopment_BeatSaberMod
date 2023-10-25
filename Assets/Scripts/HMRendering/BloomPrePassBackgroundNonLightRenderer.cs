// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundNonLightRenderer
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteAlways]
public class BloomPrePassBackgroundNonLightRenderer : BloomPrePassBackgroundNonLightRendererCore
{
  [SerializeField]
  protected Renderer _renderer;
  [SerializeField]
  protected MeshFilter _meshFilter;
  protected bool _isPartOfInstancedRendering;
  protected Transform _cachedTransform;

  public override Renderer renderer => this._renderer;

  public MeshFilter meshFilter => this._meshFilter;

  public Transform cachedTransform => this._cachedTransform;

  public bool isPartOfInstancedRendering
  {
    set
    {
      if (value)
        this.Unregister();
      else
        this.Register();
      this._isPartOfInstancedRendering = value;
    }
  }

  protected override void Awake()
  {
    base.Awake();
    this._cachedTransform = this.transform;
  }

  protected override void OnEnable()
  {
    if (this._isPartOfInstancedRendering)
      return;
    base.OnEnable();
  }

  protected override void OnValidate()
  {
    if (this.isActiveAndEnabled && !this._isPartOfInstancedRendering)
      this.Register();
    else
      this.Unregister();
  }

  protected override void InitIfNeeded()
  {
    if (!this._isPartOfInstancedRendering)
    {
      base.InitIfNeeded();
    }
    else
    {
      if ((Object) this.renderer == (Object) null || this._keepDefaultRendering)
        return;
      this.renderer.enabled = false;
    }
  }
}
