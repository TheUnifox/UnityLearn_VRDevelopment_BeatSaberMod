// Decompiled with JetBrains decompiler
// Type: WindowResolutionSettingsController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class WindowResolutionSettingsController : ListSettingsController
{
  [SerializeField]
  protected Vector2IntSO _windowResolution;
  protected Vector2Int[] _windowResolutions;

  protected override bool GetInitValues(out int idx, out int numberOfElements)
  {
    this._windowResolutions = new Vector2Int[Screen.resolutions.Length + 1];
    idx = -1;
    numberOfElements = 0;
    for (int index = 0; index < Screen.resolutions.Length; ++index)
    {
      int width = Screen.resolutions[index].width;
      int height = Screen.resolutions[index].height;
      if (numberOfElements == 0 || this._windowResolutions[numberOfElements - 1].x != width || this._windowResolutions[numberOfElements - 1].y != height)
      {
        this._windowResolutions[numberOfElements] = new Vector2Int(width, height);
        Vector2Int windowResolution = (Vector2Int) (ObservableVariableSO<Vector2Int>) this._windowResolution;
        if (width == windowResolution.x && height == windowResolution.y)
          idx = numberOfElements;
        ++numberOfElements;
      }
    }
    if (idx == -1)
    {
      idx = numberOfElements;
      this._windowResolutions[idx] = (Vector2Int) (ObservableVariableSO<Vector2Int>) this._windowResolution;
      ++numberOfElements;
    }
    return true;
  }

  protected override void ApplyValue(int idx) => this._windowResolution.value = this._windowResolutions[idx];

  protected override string TextForValue(int idx) => this._windowResolutions[idx].x.ToString() + " x " + (object) this._windowResolutions[idx].y;
}
