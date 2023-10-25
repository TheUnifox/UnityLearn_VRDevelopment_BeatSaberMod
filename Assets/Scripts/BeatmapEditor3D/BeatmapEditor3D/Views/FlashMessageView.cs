// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.FlashMessageView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class FlashMessageView : BeatmapEditorView
  {
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private TextMeshProUGUI _text;
    [Space]
    [SerializeField]
    [NullAllowed]
    private Sprite _normalIcon;
    [SerializeField]
    [NullAllowed]
    private Sprite _warningIcon;
    [SerializeField]
    [NullAllowed]
    private Sprite _errorIcon;

    public RectTransform rectTransform => this._rectTransform;

    private void Reinitialize(CommandMessageType messageType, string message)
    {
      this._iconImage.enabled = messageType != 0;
      switch (messageType)
      {
        case CommandMessageType.Normal:
          this._iconImage.sprite = this._normalIcon;
          break;
        case CommandMessageType.Warning:
          this._iconImage.sprite = this._warningIcon;
          break;
        case CommandMessageType.Error:
          this._iconImage.sprite = this._errorIcon;
          break;
      }
      this._text.text = message;
    }

    public class Pool : MonoMemoryPool<CommandMessageType, string, FlashMessageView>
    {
      protected override void Reinitialize(
        CommandMessageType messageType,
        string text,
        FlashMessageView item)
      {
        item.Reinitialize(messageType, text);
      }
    }
  }
}
