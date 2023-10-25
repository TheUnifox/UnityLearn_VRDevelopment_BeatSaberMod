// Decompiled with JetBrains decompiler
// Type: MenuEnvironmentManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class MenuEnvironmentManager : MonoBehaviour
{
  [SerializeField]
  protected MenuEnvironmentManager.MenuEnvironmentObjects[] _data;
  protected MenuEnvironmentManager.MenuEnvironmentType _prevMenuEnvironmentType;

  public virtual void Start() => this.ShowEnvironmentType(MenuEnvironmentManager.MenuEnvironmentType.Default);

  public virtual void ShowEnvironmentType(
    MenuEnvironmentManager.MenuEnvironmentType menuEnvironmentType)
  {
    if (menuEnvironmentType == this._prevMenuEnvironmentType)
      return;
    foreach (MenuEnvironmentManager.MenuEnvironmentObjects environmentObjects in this._data)
    {
      if (environmentObjects.menuEnvironmentType == this._prevMenuEnvironmentType && (UnityEngine.Object) environmentObjects.wrapper != (UnityEngine.Object) null)
      {
        environmentObjects.wrapper.SetActive(false);
        break;
      }
    }
    foreach (MenuEnvironmentManager.MenuEnvironmentObjects environmentObjects in this._data)
    {
      if (environmentObjects.menuEnvironmentType == menuEnvironmentType && (UnityEngine.Object) environmentObjects.wrapper != (UnityEngine.Object) null)
      {
        environmentObjects.wrapper.SetActive(true);
        break;
      }
    }
    this._prevMenuEnvironmentType = menuEnvironmentType;
  }

  public enum MenuEnvironmentType
  {
    None,
    Default,
    Lobby,
  }

  [Serializable]
  public class MenuEnvironmentObjects
  {
    [SerializeField]
    protected MenuEnvironmentManager.MenuEnvironmentType _menuEnvironmentType;
    [SerializeField]
    protected GameObject _wrapper;

    public MenuEnvironmentManager.MenuEnvironmentType menuEnvironmentType => this._menuEnvironmentType;

    public GameObject wrapper => this._wrapper;
  }
}
