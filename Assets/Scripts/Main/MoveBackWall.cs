// Decompiled with JetBrains decompiler
// Type: MoveBackWall
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MoveBackWall : MonoBehaviour
{
  [SerializeField]
  protected float _fadeInRegion = 0.5f;
  [SerializeField]
  protected MeshRenderer _meshRenderer;
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  protected float _thisZ;
  protected bool _isVisible;
  protected Material _material;

  public virtual void Start()
  {
    this._thisZ = this.transform.position.z;
    this._material = this._meshRenderer.sharedMaterial;
    this._meshRenderer.enabled = false;
  }

  public virtual void Update()
  {
    float num = Mathf.Abs(this._playerTransforms.headPseudoLocalPos.z - this._thisZ);
    if ((double) num < (double) this._fadeInRegion && !this._isVisible)
    {
      this._isVisible = true;
      this._meshRenderer.enabled = true;
    }
    else if ((double) num > (double) this._fadeInRegion && this._isVisible)
    {
      this._isVisible = false;
      this._meshRenderer.enabled = false;
    }
    if (!this._isVisible)
      return;
    this._material.color = new Color(1f, 1f, 1f, (float) (1.0 - (double) num / (double) this._fadeInRegion));
  }
}
