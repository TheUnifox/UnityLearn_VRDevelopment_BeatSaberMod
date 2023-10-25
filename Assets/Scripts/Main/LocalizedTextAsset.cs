// Decompiled with JetBrains decompiler
// Type: LocalizedTextAsset
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalizedTextAsset : ScriptableObject
{
  [SerializeField]
  protected LocalizedTextAsset.TextInfo[] _textInfos;
  protected LocalizedTextAsset.TextInfo _lastTextInfo;

  public LocalizedTextAsset.TextInfo[] textInfos => this._textInfos;

  public string localizedText
  {
    get
    {
      Language language = Localization.Instance.SelectedLanguage;
      if (language == Language.Debug_Keys)
        return this.name;
      if (language == Language.Debug_English_Reverted)
      {
        LocalizedTextAsset.TextInfo textInfo = ((IEnumerable<LocalizedTextAsset.TextInfo>) this.textInfos).FirstOrDefault<LocalizedTextAsset.TextInfo>((Func<LocalizedTextAsset.TextInfo, bool>) (t => t.language == Language.English));
        return new string(textInfo != null ? ((IEnumerable<char>) textInfo.localizedText.text.ToCharArray()).Reverse<char>().ToArray<char>() : (char[]) null);
      }
      if (language == Language.Debug_Word_With_Max_Lenght)
      {
        string localizedText = "";
        foreach (LocalizedTextAsset.TextInfo textInfo in this._textInfos)
        {
          if (localizedText.Length < textInfo.localizedText.text.Length)
            localizedText = textInfo.localizedText.text;
        }
        return localizedText;
      }
      this._lastTextInfo = ((IEnumerable<LocalizedTextAsset.TextInfo>) this._textInfos).FirstOrDefault<LocalizedTextAsset.TextInfo>((Func<LocalizedTextAsset.TextInfo, bool>) (t => t.language == language));
      return this._lastTextInfo?.localizedText.text;
    }
  }

  [Serializable]
  public class TextInfo
  {
    public Language language;
    public TextAsset localizedText;
  }
}
