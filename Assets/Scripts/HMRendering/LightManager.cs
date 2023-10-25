// Decompiled with JetBrains decompiler
// Type: LightManager
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightManager : MonoBehaviour
{
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _directionalLightDirectionsID = Shader.PropertyToID("_DirectionalLightDirections");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _directionalLightPositionID = Shader.PropertyToID("_DirectionalLightPositions");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _directionalLightRadiiID = Shader.PropertyToID("_DirectionalLightRadii");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _directionalLightColorsID = Shader.PropertyToID("_DirectionalLightColors");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _pointLightPositionsID = Shader.PropertyToID("_PointLightPositions");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _pointLightColorsID = Shader.PropertyToID("_PointLightColors");
  protected readonly Vector4[] _directionalLightDirections = new Vector4[5];
  protected readonly Vector4[] _directionalLightColors = new Vector4[5];
  protected readonly Vector4[] _directionalLightPositions = new Vector4[5];
  protected readonly float[] _directionalLightRadii = new float[5];
  protected readonly Vector4[] _pointLightPositions = new Vector4[1];
  protected readonly Vector4[] _pointLightColors = new Vector4[1];
  protected int lastRefreshFrameNum = -1;

  public virtual void OnEnable() => Camera.onPreRender += new Camera.CameraCallback(this.OnCameraPreRender);

  public virtual void OnDisable() => Camera.onPreRender -= new Camera.CameraCallback(this.OnCameraPreRender);

  public virtual void OnCameraPreRender(Camera camera)
  {
    if (camera.cullingMask != (camera.cullingMask | 1 << this.gameObject.layer) || this.lastRefreshFrameNum == Time.frameCount)
      return;
    this.lastRefreshFrameNum = Time.frameCount;
    List<DirectionalLight> lights1 = DirectionalLight.lights;
    for (int index = 0; index < 5; ++index)
    {
      if (index < lights1.Count && lights1[index].isActiveAndEnabled)
      {
        DirectionalLight directionalLight = lights1[index];
        Transform transform = directionalLight.transform;
        this._directionalLightPositions[index] = (Vector4) transform.position;
        this._directionalLightDirections[index] = (Vector4) -transform.forward;
        this._directionalLightColors[index] = (Vector4) (directionalLight.color * directionalLight.intensity).linear;
        this._directionalLightRadii[index] = directionalLight.radius;
      }
      else
      {
        this._directionalLightColors[index] = (Vector4) new Color(0.0f, 0.0f, 0.0f, 0.0f);
        this._directionalLightRadii[index] = 100f;
      }
    }
    Shader.SetGlobalVectorArray(LightManager._directionalLightDirectionsID, this._directionalLightDirections);
    Shader.SetGlobalVectorArray(LightManager._directionalLightPositionID, this._directionalLightPositions);
    Shader.SetGlobalFloatArray(LightManager._directionalLightRadiiID, this._directionalLightRadii);
    Shader.SetGlobalVectorArray(LightManager._directionalLightColorsID, this._directionalLightColors);
    List<PointLight> lights2 = PointLight.lights;
    for (int index = 0; index < 1; ++index)
    {
      if (index < lights2.Count && lights2[index].isActiveAndEnabled)
      {
        PointLight pointLight = lights2[index];
        this._pointLightPositions[index] = (Vector4) pointLight.transform.position;
        this._pointLightColors[index] = (Vector4) (pointLight.color * pointLight.intensity).linear;
      }
      else
        this._pointLightColors[index] = (Vector4) new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
    Shader.SetGlobalVectorArray(LightManager._pointLightPositionsID, this._pointLightPositions);
    Shader.SetGlobalVectorArray(LightManager._pointLightColorsID, this._pointLightColors);
  }

  public virtual void OnDestroy() => this.ResetColors();

  public virtual void ResetColors()
  {
    for (int index = 0; index < 5; ++index)
      this._directionalLightColors[index] = (Vector4) new Color(0.0f, 0.0f, 0.0f, 0.0f);
    for (int index = 0; index < 1; ++index)
      this._pointLightColors[index] = (Vector4) new Color(0.0f, 0.0f, 0.0f, 0.0f);
    Shader.SetGlobalVectorArray(LightManager._directionalLightDirectionsID, this._directionalLightDirections);
    Shader.SetGlobalVectorArray(LightManager._directionalLightColorsID, this._directionalLightColors);
    Shader.SetGlobalVectorArray(LightManager._pointLightColorsID, this._pointLightColors);
  }
}
