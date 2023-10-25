// Decompiled with JetBrains decompiler
// Type: OnWillRenderObjectTrigger
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class OnWillRenderObjectTrigger : MonoBehaviour
{
  [SerializeField]
  [NullAllowed]
  protected Shader _overrideShader;
  [SerializeField]
  protected int _renderQueue;
  protected Material _material;
  protected Mesh _mesh;
  protected MeshFilter _meshFilter;
  protected MeshRenderer _meshRenderer;

  public virtual void OnEnable()
  {
    if ((Object) this._material == (Object) null)
    {
      this._material = new Material((Object) this._overrideShader != (Object) null ? this._overrideShader : Shader.Find("Diffuse"));
      this._material.renderQueue = this._renderQueue;
    }
    if ((Object) this._mesh == (Object) null)
    {
      this._mesh = new Mesh();
      this._mesh.name = "Huge Mesh";
      this._mesh.hideFlags = HideFlags.HideAndDontSave;
      this._mesh.bounds = new Bounds(Vector3.zero, new Vector3(9999999f, 9999999f, 9999999f));
    }
    this._meshRenderer = this.GetComponent<MeshRenderer>();
    if ((Object) this._meshRenderer == (Object) null)
      this._meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
    this._meshRenderer.hideFlags = HideFlags.None;
    this._meshRenderer.sharedMaterial = this._material;
    this._meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
    this._meshRenderer.receiveShadows = false;
    this._meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
    this._meshRenderer.allowOcclusionWhenDynamic = false;
    this._meshRenderer.lightProbeUsage = LightProbeUsage.Off;
    this._meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
    this._meshFilter = this.GetComponent<MeshFilter>();
    if ((Object) this._meshFilter == (Object) null)
      this._meshFilter = this.gameObject.AddComponent<MeshFilter>();
    this._meshFilter.hideFlags = HideFlags.None;
    this._meshFilter.sharedMesh = this._mesh;
  }

  public virtual void OnDisable()
  {
    EssentialHelpers.SafeDestroy((Object) this._material);
    this._material = (Material) null;
    EssentialHelpers.SafeDestroy((Object) this._mesh);
    this._mesh = (Mesh) null;
  }
}
