// Decompiled with JetBrains decompiler
// Type: SongTimeToShaderWriter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SongTimeToShaderWriter : MonoBehaviour
{
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _songTimePropertyId = Shader.PropertyToID("_SongTime");

  public virtual void Update() => Shader.SetGlobalVector(SongTimeToShaderWriter._songTimePropertyId, new Vector4(this._audioTimeSource.songTime * 0.05f, this._audioTimeSource.songTime, this._audioTimeSource.songTime * 2f, this._audioTimeSource.songTime * 3f));
}
