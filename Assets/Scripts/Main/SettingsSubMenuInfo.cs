// Decompiled with JetBrains decompiler
// Type: SettingsSubMenuInfo
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using UnityEngine;

[Serializable]
public class SettingsSubMenuInfo
{
  [SerializeField]
  protected ViewController _viewController;
  [SerializeField]
  [LocalizationKey]
  protected string _menuName;

  public ViewController viewController => this._viewController;

  public string localizedMenuName => Localization.Get(this._menuName);
}
