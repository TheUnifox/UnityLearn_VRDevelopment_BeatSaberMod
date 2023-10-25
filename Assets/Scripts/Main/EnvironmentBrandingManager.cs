// Decompiled with JetBrains decompiler
// Type: EnvironmentBrandingManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class EnvironmentBrandingManager : MonoBehaviour
{
  [SerializeField]
  protected GameObject[] _brandingObjects;
  [SerializeField]
  protected GameObject[] _replacementBrandingObjects;
  [InjectOptional]
  protected readonly EnvironmentBrandingManager.InitData _initData;

  public virtual void Start()
  {
    if (this._initData == null)
      return;
    for (int index = 0; index < this._brandingObjects.Length; ++index)
      this._brandingObjects[index].SetActive(!this._initData.hideBranding);
    for (int index = 0; index < this._replacementBrandingObjects.Length; ++index)
      this._replacementBrandingObjects[index].SetActive(this._initData.hideBranding);
  }

  public class InitData
  {
    public readonly bool hideBranding;

    public InitData(bool hideBranding) => this.hideBranding = hideBranding;
  }
}
