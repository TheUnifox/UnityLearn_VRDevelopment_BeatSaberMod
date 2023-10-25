// Decompiled with JetBrains decompiler
// Type: LocalizedAudioClipSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalizedAudioClipSO : ScriptableObject
{
  [SerializeField]
  protected LocalizedAudioClipSO.LocalizedAudioClipInfo[] _localizedAudioClipInfo;
  protected LocalizedAudioClipSO.LocalizedAudioClipInfo _lastLocalizedAudioClipInfo;

  public AudioClip localizedAudioClip
  {
    get
    {
      Language language = Localization.Instance.SelectedLanguage;
      if (this._lastLocalizedAudioClipInfo == null || language != this._lastLocalizedAudioClipInfo.language)
        this._lastLocalizedAudioClipInfo = ((IEnumerable<LocalizedAudioClipSO.LocalizedAudioClipInfo>) this._localizedAudioClipInfo).FirstOrDefault<LocalizedAudioClipSO.LocalizedAudioClipInfo>((Func<LocalizedAudioClipSO.LocalizedAudioClipInfo, bool>) (t => t.language == language));
      return this._lastLocalizedAudioClipInfo?.localizedAudioClip;
    }
  }

  [Serializable]
  public class LocalizedAudioClipInfo
  {
    public Language language;
    public AudioClip localizedAudioClip;
  }
}
