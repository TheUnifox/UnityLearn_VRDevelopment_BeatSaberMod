// Decompiled with JetBrains decompiler
// Type: BakedLightsNormalizer
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using UnityEngine;

public class BakedLightsNormalizer : MonoBehaviour
{
  [SerializeField]
  protected float _maxTotalIntensity = 1f;
  protected readonly Dictionary<LightConstants.BakeId, LightmapLightWithIds> _lightmapLightDict = new Dictionary<LightConstants.BakeId, LightmapLightWithIds>();
  protected LightWithIdManager _lightManager;
  protected bool _lightmapDictInitialized;
  protected float _prevMaxTotalIntensity = -1f;
  protected float _grayscaleTotal;
  protected int _lastCalculatedOnFrame;
  protected bool _grayscaleCalculatedOnce;
  protected bool _newUpdates = true;
  protected const int kMaxFramesWithoutUpdate = 5;

  public Dictionary<LightConstants.BakeId, LightmapLightWithIds> lightmapLightDict => this._lightmapLightDict;

  public float maxTotalIntensity => this._maxTotalIntensity;

  public virtual void LateUpdate()
  {
    if (!this._newUpdates || Time.frameCount - this._lastCalculatedOnFrame <= 5)
      return;
    this.UpdateGrayscaleTotal();
    this._newUpdates = false;
  }

  public virtual void GetLightmapLights()
  {
    this._lightmapLightDict.Clear();
    foreach (LightmapLightWithIds lightmapLightWithIds in Object.FindObjectsOfType<LightmapLightWithIds>())
      this._lightmapLightDict[lightmapLightWithIds.bakeId] = lightmapLightWithIds;
    this._lightmapDictInitialized = true;
  }

  public virtual void UpdateGrayscaleTotal()
  {
    if (this._lightmapLightDict.Count == 0 && !this._lightmapDictInitialized)
      this.GetLightmapLights();
    if (Time.frameCount == this._lastCalculatedOnFrame && this._grayscaleCalculatedOnce)
      return;
    this._grayscaleTotal = 0.0f;
    foreach (LightmapLightWithIds lightmapLightWithIds in this._lightmapLightDict.Values)
      this._grayscaleTotal += lightmapLightWithIds.calculatedColorPreNormalization.grayscale * lightmapLightWithIds.normalizerWeight;
    this._lastCalculatedOnFrame = Time.frameCount;
    this._grayscaleCalculatedOnce = true;
  }

  public virtual float GetNormalizationMultiplier()
  {
    this.UpdateGrayscaleTotal();
    this._newUpdates = true;
    return !this._lightmapDictInitialized || (double) this._grayscaleTotal <= (double) this._maxTotalIntensity ? 1f : Mathf.LinearToGammaSpace(this._maxTotalIntensity / this._grayscaleTotal);
  }
}
