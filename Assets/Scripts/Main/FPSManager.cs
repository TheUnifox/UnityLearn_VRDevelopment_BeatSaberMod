// Decompiled with JetBrains decompiler
// Type: FPSManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class FPSManager : MonoBehaviour
{
  [SerializeField]
  protected BoolSO _enableFPSCounter;
  [SerializeField]
  [NullAllowed]
  protected Canvas _fpsCounterCanvasPrefab;
  [Inject]
  protected readonly MainCamera _mainCamera;

  public virtual void Start()
  {
    if (!(bool) (ObservableVariableSO<bool>) this._enableFPSCounter && (Object) this._fpsCounterCanvasPrefab != (Object) null)
      return;
    Object.Instantiate<Canvas>(this._fpsCounterCanvasPrefab).worldCamera = this._mainCamera.camera;
  }
}
