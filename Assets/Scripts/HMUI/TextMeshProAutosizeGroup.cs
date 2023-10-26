// Decompiled with JetBrains decompiler
// Type: TextMeshProAutosizeGroup
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using TMPro;
using UnityEngine;

public class TextMeshProAutosizeGroup : MonoBehaviour
{
  [SerializeField]
  protected TMP_Text[] _texts;

  public virtual void Start()
  {
    if (this._texts == null || this._texts.Length == 0)
      return;
    float a = float.MaxValue;
    foreach (TMP_Text text in this._texts)
    {
      text.ForceMeshUpdate(true);
      a = Mathf.Min(a, text.fontSize);
    }
    foreach (TMP_Text text in this._texts)
    {
      text.enableAutoSizing = false;
      text.fontSize = a;
    }
  }
}
