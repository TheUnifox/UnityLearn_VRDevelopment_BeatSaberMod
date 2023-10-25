// Decompiled with JetBrains decompiler
// Type: NoteTrailParticleSystem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class NoteTrailParticleSystem : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _particleSystem;
  protected ParticleSystem.EmitParams _emitParams;

  public virtual void Awake() => this._emitParams = new ParticleSystem.EmitParams();

  public virtual void Emit(Vector3 startPos, Vector3 endPos, int count)
  {
    for (int index = 0; index < count; ++index)
    {
      this._emitParams.position = Vector3.Lerp(startPos, endPos, ((float) index + 1f) / (float) count);
      this._particleSystem.Emit(this._emitParams, 1);
    }
  }
}
