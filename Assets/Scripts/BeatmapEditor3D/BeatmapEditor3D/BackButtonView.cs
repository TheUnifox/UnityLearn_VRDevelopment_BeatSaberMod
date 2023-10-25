// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BackButtonView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using UnityEngine.UI;

namespace BeatmapEditor3D
{
  public class BackButtonView : MonoBehaviour
  {
    [SerializeField]
    private Button _button;
    [Space]
    [SerializeField]
    private GameObject _backIconGameObject;
    [SerializeField]
    private GameObject _quitIconGameObject;
    [Space]
    [SerializeField]
    private GameObject _dirtyNotificationGo;

    public Button.ButtonClickedEvent onClick => this._button.onClick;

    public void SetBackButtonType(BackButtonView.BackButtonType type)
    {
      if (type != BackButtonView.BackButtonType.Back)
      {
        if (type != BackButtonView.BackButtonType.Quit)
          return;
        this._backIconGameObject.SetActive(false);
        this._quitIconGameObject.SetActive(true);
      }
      else
      {
        this._backIconGameObject.SetActive(true);
        this._quitIconGameObject.SetActive(false);
      }
    }

    public void SetDirtyNotification(bool isDirty) => this._dirtyNotificationGo.SetActive(isDirty);

    public enum BackButtonType
    {
      Back,
      Quit,
    }
  }
}
