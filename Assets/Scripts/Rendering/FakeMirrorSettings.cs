// Decompiled with JetBrains decompiler
// Type: FakeMirrorSettings
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class FakeMirrorSettings : MonoBehaviour
{
  [SerializeField]
  private float _fakeMirrorTransparency = 0.1f;
  [Space(12f)]
  [SerializeField]
  private bool _useVertexDistortion;
  [SerializeField]
  [DrawIf("_useVertexDistortion", true, DrawIfAttribute.DisablingType.DontDraw)]
  [Min(0.0f)]
  private float _vertexDistortionNoiseScale = 1.5f;
  [SerializeField]
  [DrawIf("_useVertexDistortion", true, DrawIfAttribute.DisablingType.DontDraw)]
  private float _vertexDistortionStrength = 0.08f;
  [SerializeField]
  [DrawIf("_useVertexDistortion", true, DrawIfAttribute.DisablingType.DontDraw)]
  private Vector3 _vertexDistortionDirectionality = new Vector3(1f, 0.0f, 0.5f);
  [SerializeField]
  [DrawIf("_useVertexDistortion", true, DrawIfAttribute.DisablingType.DontDraw)]
  [Min(0.0f)]
  private float _vertexDistortionZposMultiplier = 0.1f;
  [DoesNotRequireDomainReloadInit]
  private static readonly int _fakeMirrorTransparencyId = Shader.PropertyToID("_FakeMirrorTransparency");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _vertexDistortionNoiseScaleId = Shader.PropertyToID("_FakeMirrorDistortionNoiseScale");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _vertexDistortionStrengthId = Shader.PropertyToID("_FakeMirrorDistortionStrength");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _vertexDistortionDirectionalityId = Shader.PropertyToID("_FakeMirrorDistortionDirectionality");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _vertexDistortionZposMultiplierId = Shader.PropertyToID("_FakeMirrorDistortionZposMultiplier");

  public float fakeMirrorTransparency
  {
    get => this._fakeMirrorTransparency;
    set => this._fakeMirrorTransparency = value;
  }

  protected void Start() => this.SetGlobalParameters();

  protected void OnEnable() => this.SetGlobalParameters();

  protected void OnValidate() => this.SetGlobalParameters();

  private void SetGlobalParameters()
  {
    Shader.SetGlobalFloat(FakeMirrorSettings._fakeMirrorTransparencyId, this._fakeMirrorTransparency);
    if (this._useVertexDistortion)
    {
      Shader.EnableKeyword("NOTE_VERTEX_DISTORTION");
      Shader.SetGlobalFloat(FakeMirrorSettings._vertexDistortionNoiseScaleId, this._vertexDistortionNoiseScale);
      Shader.SetGlobalFloat(FakeMirrorSettings._vertexDistortionStrengthId, this._vertexDistortionStrength);
      Shader.SetGlobalVector(FakeMirrorSettings._vertexDistortionDirectionalityId, (Vector4) this._vertexDistortionDirectionality);
      Shader.SetGlobalFloat(FakeMirrorSettings._vertexDistortionZposMultiplierId, this._vertexDistortionZposMultiplier);
    }
    else
      Shader.DisableKeyword("NOTE_VERTEX_DISTORTION");
  }
}
