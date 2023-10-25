// Decompiled with JetBrains decompiler
// Type: BakedReflectionProbe
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteAlways]
public class BakedReflectionProbe : MonoBehaviour
{
  [SerializeField]
  protected int _resolutionBeforeDownsample = 2048;
  [SerializeField]
  protected int _downsampleByHalfCount = 1;
  [SerializeField]
  protected Vector3 _size;
  [SerializeField]
  protected Vector3 _offset;
  [Space]
  [NullAllowed(NullAllowed.Context.Prefab)]
  [SerializeField]
  protected ReflectionProbeDataSO _reflectionProbeData;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _reflectionProbeBoundsMinPropertyId = Shader.PropertyToID("_ReflectionProbeBoundsMin");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _reflectionProbeBoundsMaxPropertyId = Shader.PropertyToID("_ReflectionProbeBoundsMax");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _reflectionProbePositionPropertyId = Shader.PropertyToID("_ReflectionProbePosition");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _reflectionProbeTexture1PropertyId = Shader.PropertyToID("_ReflectionProbeTexture1");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _reflectionProbeTexture2PropertyId = Shader.PropertyToID("_ReflectionProbeTexture2");
  protected Cubemap _blackCubemap;

  public ReflectionProbeDataSO reflectionProbeData
  {
    get => this._reflectionProbeData;
    set => this._reflectionProbeData = value;
  }

  public Vector3 position => this.transform.position;

  public int resolutionBeforeDownsample => this._resolutionBeforeDownsample;

  public int downsampleByHalfCount => this._downsampleByHalfCount;

  public virtual void Start()
  {
    if ((Object) this._reflectionProbeData != (Object) null)
      this.SendDataToShaders();
    else
      Debug.LogWarning((object) "Reflection Probe Data not set");
  }

  public virtual void SendDataToShaders()
  {
    Vector3 position = this.transform.position;
    Vector3 vector3 = position + this._offset;
    Shader.SetGlobalVector(BakedReflectionProbe._reflectionProbeBoundsMinPropertyId, (Vector4) (-this._size * 0.5f + vector3));
    Shader.SetGlobalVector(BakedReflectionProbe._reflectionProbeBoundsMaxPropertyId, (Vector4) (this._size * 0.5f + vector3));
    Shader.SetGlobalVector(BakedReflectionProbe._reflectionProbePositionPropertyId, (Vector4) position);
    Shader.SetGlobalTexture(BakedReflectionProbe._reflectionProbeTexture1PropertyId, (Object) this._reflectionProbeData.reflectionProbeCubemap1 != (Object) null ? (Texture) this._reflectionProbeData.reflectionProbeCubemap1 : (Texture) this._blackCubemap);
    Shader.SetGlobalTexture(BakedReflectionProbe._reflectionProbeTexture2PropertyId, (Object) this._reflectionProbeData.reflectionProbeCubemap2 != (Object) null ? (Texture) this._reflectionProbeData.reflectionProbeCubemap2 : (Texture) this._blackCubemap);
  }
}
