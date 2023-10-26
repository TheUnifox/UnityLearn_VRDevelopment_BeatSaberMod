// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.FlashMessagesView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class FlashMessagesView : BeatmapEditorView
  {
    [SerializeField]
    private RectTransform _flashMessagesRectTransform;
    [SerializeField]
    private RectTransform _contentRectTransform;
    [Space]
    [SerializeField]
    private Button _expandMessagesButton;
    [SerializeField]
    private Button _shrinkMessagesButton;
    [SerializeField]
    private float _expandedSize;
    [SerializeField]
    private float _shrunkSize;
    [Space]
    [SerializeField]
    private float _displayTime = 3f;
    [Space]
    [SerializeField]
    private float _itemHeight;
    [SerializeField]
    private float _itemSpacing;
    [SerializeField]
    private int _maxItemsCount;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly FlashMessageView.Pool _flashMessageViewPool;
    private readonly ButtonBinder _buttonBinder = new ButtonBinder();
    private readonly List<(FlashMessageView view, float endTime)> _currentMessages = new List<(FlashMessageView, float)>();

    [field: SerializeField]
    public int field { get; }

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<CommandMessageSignal>(new Action<CommandMessageSignal>(this.HandleCommandMessage));
      this._buttonBinder.AddBinding(this._expandMessagesButton, new Action(this.HandleExpandMessagesButtonPressed));
      this._buttonBinder.AddBinding(this._shrinkMessagesButton, new Action(this.HandleShrinkMessagesButtonPressed));
      this._expandMessagesButton.gameObject.SetActive(true);
      this._shrinkMessagesButton.gameObject.SetActive(false);
      this.SetSize(this._shrunkSize);
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<CommandMessageSignal>(new Action<CommandMessageSignal>(this.HandleCommandMessage));
      this._buttonBinder.ClearBindings();
    }

    protected override void ViewUpdate()
    {
      while (this._currentMessages.Count > 0 && (double) Time.time >= (double) this._currentMessages.Last<(FlashMessageView, float)>().Item2 || this._currentMessages.Count > this._maxItemsCount)
      {
        (FlashMessageView view, float endTime) tuple = this._currentMessages.Last<(FlashMessageView, float)>();
        this._currentMessages.Remove(tuple);
        this._flashMessageViewPool.Despawn(tuple.view);
      }
    }

    private void PushFlashMessage(CommandMessageType messageType, string message)
    {
      FlashMessageView flashMessageView = this._flashMessageViewPool.Spawn(messageType, message);
      flashMessageView.rectTransform.SetParent((Transform) this._contentRectTransform, false);
      float num = Time.time + this._displayTime;
      this._currentMessages.Insert(0, (flashMessageView, num));
      this.LayoutMessages();
    }

    private void LayoutMessages()
    {
      for (int index = 0; index < this._currentMessages.Count; ++index)
        this._currentMessages[index].view.rectTransform.anchoredPosition = new Vector2(0.0f, (float) -((double) this._itemHeight + (double) this._itemSpacing) * (float) index);
    }

    private void HandleCommandMessage(CommandMessageSignal signal) => this.PushFlashMessage(signal.type, signal.message);

    private void HandleShrinkMessagesButtonPressed()
    {
      this._expandMessagesButton.gameObject.SetActive(true);
      this._shrinkMessagesButton.gameObject.SetActive(false);
      this.SetSize(this._shrunkSize);
    }

    private void HandleExpandMessagesButtonPressed()
    {
      this._expandMessagesButton.gameObject.SetActive(false);
      this._shrinkMessagesButton.gameObject.SetActive(true);
      this.SetSize(this._expandedSize);
    }

        private void SetSize(float size)
        {
            Vector2 sizeDelta = this._flashMessagesRectTransform.sizeDelta;
            sizeDelta.x = size;
            this._flashMessagesRectTransform.sizeDelta = sizeDelta;
        }
    }
}
