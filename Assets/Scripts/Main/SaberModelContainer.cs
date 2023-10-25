// Decompiled with JetBrains decompiler
// Type: SaberModelContainer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SaberModelContainer : MonoBehaviour
{
  [SerializeField]
  protected Saber _saber;
  [SerializeField]
  protected SaberModelController _saberModelControllerPrefab;
  [Inject]
  protected readonly DiContainer _container;

  public virtual void Start() => this._container.InstantiatePrefab((Object) this._saberModelControllerPrefab).GetComponent<SaberModelController>().Init(this.transform, this._saber);
}
