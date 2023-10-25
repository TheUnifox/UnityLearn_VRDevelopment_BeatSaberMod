// Decompiled with JetBrains decompiler
// Type: ResetLocalLeaderboardOnKey
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ResetLocalLeaderboardOnKey : MonoBehaviour
{
  [SerializeField]
  protected LocalLeaderboardsModel _localLeaderboardsModel;
  [SerializeField]
  protected KeyCode _keyCode = KeyCode.F9;

  public virtual void Update()
  {
    if (!Input.GetKeyDown(this._keyCode))
      return;
    this._localLeaderboardsModel.ClearAllLeaderboards(true);
  }
}
