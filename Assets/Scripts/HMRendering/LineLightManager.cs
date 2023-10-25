// Decompiled with JetBrains decompiler
// Type: LineLightManager
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineLightManager : MonoBehaviour
{
  protected const int kMaxNumberOfLights = 4;
  protected readonly Vector4[] _points = new Vector4[4];
  protected readonly Vector4[] _dirs = new Vector4[4];
  protected readonly float[] _dirLengths = new float[4];
  protected readonly Vector4[] _colors = new Vector4[4];
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _activeLineLightsCountID = Shader.PropertyToID("_ActiveLineLightsCount");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _lineLightPointsID = Shader.PropertyToID("_LineLightPoints");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _lineLightDirsID = Shader.PropertyToID("_LineLightDirs");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _lineLightDirLengthsID = Shader.PropertyToID("_LineLightDirLengths");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _lineLightColorsID = Shader.PropertyToID("_LineLightColors");

  public virtual void Update()
  {
    List<LineLight> lineLights = LineLight.lineLights;
    int num = Mathf.Min(4, lineLights.Count);
    for (int index = 0; index < num; ++index)
    {
      LineLight lineLight = lineLights[index];
      Transform transform = lineLight.transform;
      Vector3 vector3_1 = transform.TransformPoint(lineLight.p0);
      Vector3 vector3_2 = transform.TransformPoint(lineLight.p1);
      this._points[index] = (Vector4) vector3_1;
      this._dirs[index] = (Vector4) (vector3_2 - vector3_1);
      this._dirLengths[index] = this._dirs[index].magnitude;
      this._colors[index] = (Vector4) lineLight.color;
    }
    for (int index = num; index < 4; ++index)
      this._colors[index] = (Vector4) Color.clear;
    Shader.SetGlobalInt(LineLightManager._activeLineLightsCountID, num);
    Shader.SetGlobalVectorArray(LineLightManager._lineLightPointsID, this._points);
    Shader.SetGlobalVectorArray(LineLightManager._lineLightDirsID, this._dirs);
    Shader.SetGlobalFloatArray(LineLightManager._lineLightDirLengthsID, this._dirLengths);
    Shader.SetGlobalVectorArray(LineLightManager._lineLightColorsID, this._colors);
  }
}
