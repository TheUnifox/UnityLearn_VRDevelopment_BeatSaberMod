// Decompiled with JetBrains decompiler
// Type: ResizableTextMeshPro
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class ResizableTextMeshPro : MonoBehaviour
{
  [SerializeField]
  protected TMP_Text _textMeshPro;
  [SerializeField]
  protected RectTransform _rectTransform;
  [Space]
  [SerializeField]
  protected float _textExtraSpace = 20f;

  public virtual void Start() => TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new System.Action<UnityEngine.Object>(this.HandleTextDidChange));

  public virtual void Update()
  {
    this._rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._textMeshPro.bounds.size.x + this._textExtraSpace);
    this.enabled = false;
  }

  public virtual void OnDestroy() => TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new System.Action<UnityEngine.Object>(this.HandleTextDidChange));

  public virtual void HandleTextDidChange(UnityEngine.Object textMeshPro)
  {
    if (!(textMeshPro == (UnityEngine.Object) this._textMeshPro))
      return;
    this.enabled = true;
  }
}
