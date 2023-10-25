// Decompiled with JetBrains decompiler
// Type: BloomFogSO
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class BloomFogSO : PersistentScriptableObject
{
  protected bool _bloomFogEnabled = true;
  protected float _transition;
  protected float _autoExposureLimit;
  protected float _noteSpawnIntensity;
  protected const string kBloomFogEnabledKeyword = "ENABLE_BLOOM_FOG";
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _customFogAttenuationID = Shader.PropertyToID("_CustomFogAttenuation");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _customFogOffsetID = Shader.PropertyToID("_CustomFogOffset");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _customFogHeightFogStartYID = Shader.PropertyToID("_CustomFogHeightFogStartY");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _customFogHeightFogHeightID = Shader.PropertyToID("_CustomFogHeightFogHeight");
  protected BloomFogEnvironmentParams _defaultFogParams;
  protected BloomFogEnvironmentParams _transitionFogParams;

  public float transition
  {
    set
    {
      if ((double) value == (double) this._transition)
        return;
      this._transition = value;
      this.UpdateShaderParams();
    }
    get => this._transition;
  }

  public BloomFogEnvironmentParams defaultForParams
  {
    get => this._defaultFogParams;
    set
    {
      if ((Object) value == (Object) this._defaultFogParams)
        return;
      this._defaultFogParams = value;
      if ((double) this._transition >= 1.0)
        return;
      this.UpdateShaderParams();
    }
  }

  public BloomFogEnvironmentParams transitionFogParams
  {
    get => this._transitionFogParams;
    set
    {
      if ((Object) value == (Object) this._transitionFogParams)
        return;
      this._transitionFogParams = value;
      if ((double) this._transition <= 0.0)
        return;
      this.UpdateShaderParams();
    }
  }

  public bool bloomFogEnabled
  {
    set
    {
      if (value == this._bloomFogEnabled)
        return;
      if (value)
        Shader.EnableKeyword("ENABLE_BLOOM_FOG");
      else
        Shader.DisableKeyword("ENABLE_BLOOM_FOG");
      this._bloomFogEnabled = value;
    }
    get => this._bloomFogEnabled;
  }

  public float autoExposureLimit => this._autoExposureLimit;

  public float noteSpawnIntensity => this._noteSpawnIntensity;

  public virtual void Setup(BloomFogEnvironmentParams defaultFogParams) => this._defaultFogParams = this._transitionFogParams = defaultFogParams;

  protected override void OnEnable()
  {
    base.OnEnable();
    this.UpdateShaderParams();
  }

  public virtual void UpdateShaderParams()
  {
    if ((Object) this._defaultFogParams == (Object) null)
      return;
    if ((Object) this._transitionFogParams == (Object) null || (double) this._transition <= (double) Mathf.Epsilon)
      this.SetParams(this._defaultFogParams.attenuation, this._defaultFogParams.offset, this._defaultFogParams.heightFogStartY, this._defaultFogParams.heightFogHeight, this._defaultFogParams.autoExposureLimit, this._defaultFogParams.noteSpawnIntensity);
    else if ((double) this._transition == 1.0)
      this.SetParams(this._transitionFogParams.attenuation, this._transitionFogParams.offset, this._transitionFogParams.heightFogStartY, this._transitionFogParams.heightFogHeight, this._transitionFogParams.autoExposureLimit, this._defaultFogParams.noteSpawnIntensity);
    else
      this.SetParams(Mathf.Lerp(this._defaultFogParams.attenuation, this._transitionFogParams.attenuation, this._transition), Mathf.Lerp(this._defaultFogParams.offset, this._transitionFogParams.offset, this._transition), Mathf.Lerp(this._defaultFogParams.heightFogStartY, this._transitionFogParams.heightFogStartY, this._transition), Mathf.Lerp(this._defaultFogParams.heightFogHeight, this._transitionFogParams.heightFogHeight, this._transition), Mathf.Lerp(this._defaultFogParams.autoExposureLimit, this._transitionFogParams.autoExposureLimit, this._transition), Mathf.Lerp(this._defaultFogParams.noteSpawnIntensity, this._transitionFogParams.noteSpawnIntensity, this._transition));
  }

  public virtual void SetParams(
    float attenuation,
    float offset,
    float heightFogStartY,
    float heightFogHeight,
    float autoExposureLimit,
    float noteSpawnIntensity)
  {
    Shader.SetGlobalFloat(BloomFogSO._customFogAttenuationID, attenuation);
    Shader.SetGlobalFloat(BloomFogSO._customFogOffsetID, offset);
    Shader.SetGlobalFloat(BloomFogSO._customFogHeightFogStartYID, heightFogStartY);
    Shader.SetGlobalFloat(BloomFogSO._customFogHeightFogHeightID, heightFogHeight);
    this._autoExposureLimit = autoExposureLimit;
    this._noteSpawnIntensity = noteSpawnIntensity;
  }
}
