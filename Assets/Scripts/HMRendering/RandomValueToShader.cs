// Decompiled with JetBrains decompiler
// Type: RandomValueToShader
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class RandomValueToShader : PersistentScriptableObject
{
  protected int _lastFrameNum = -1;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _randomValueID = Shader.PropertyToID("_GlobalRandomValue");

  public virtual void SetRandomValueToShaders()
  {
    int frameCount = Time.frameCount;
    if (this._lastFrameNum == frameCount)
      return;
    Shader.SetGlobalFloat(RandomValueToShader._randomValueID, Random.value);
    this._lastFrameNum = frameCount;
  }
}
