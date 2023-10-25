// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.OpenFolderView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D
{
  public class OpenFolderView : MonoBehaviour
  {
    [SerializeField]
    private Button _openFolderButton;
    [SerializeField]
    private TMP_InputField _customDirectoryInputField;
    [SerializeField]
    [NullAllowed]
    private GameObject _valueModifiedHintGo;
    [Space]
    [SerializeField]
    private string _openDialogTitle;

    public string path => this._customDirectoryInputField.text;

    public void SetFolderPath(string path)
    {
      this._customDirectoryInputField.text = path;
      this.ClearDirtyState();
    }

    public void ClearDirtyState()
    {
      if (!((Object) this._valueModifiedHintGo != (Object) null))
        return;
      this._valueModifiedHintGo.SetActive(false);
    }

    protected void OnEnable() => this._openFolderButton.onClick.AddListener(new UnityAction(this.HandleOpenFolderButtonOnClick));

    protected void OnDisable() => this._openFolderButton.onClick.RemoveListener(new UnityAction(this.HandleOpenFolderButtonOnClick));

    private void HandleOpenFolderButtonOnClick()
    {
      string str = NativeFileDialogs.OpenDirectoryDialog(this._openDialogTitle, this._customDirectoryInputField.text);
      if (string.IsNullOrEmpty(str))
        return;
      if ((Object) this._valueModifiedHintGo != (Object) null)
        this._valueModifiedHintGo.SetActive(true);
      this._customDirectoryInputField.text = str;
    }
  }
}
