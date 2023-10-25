// Decompiled with JetBrains decompiler
// Type: BloomFogEnvironment
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class BloomFogEnvironment : MonoBehaviour
{
  [SerializeField]
  protected BloomFogSO _bloomFog;
  [Space]
  [FormerlySerializedAs("_fog0Params")]
  [SerializeField]
  protected BloomFogEnvironmentParams _fogParams;

  public virtual void OnEnable()
  {
    this._bloomFog.transition = 0.0f;
    this._bloomFog.Setup(this._fogParams);
  }

  public virtual void OnValidate()
  {
    if (Application.isPlaying)
      return;
    this._bloomFog.Setup(this._fogParams);
    this._bloomFog.bloomFogEnabled = true;
    this._bloomFog.UpdateShaderParams();
  }
}
