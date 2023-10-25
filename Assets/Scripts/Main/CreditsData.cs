// Decompiled with JetBrains decompiler
// Type: CreditsData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class CreditsData
{
  public CreditsData.RootCreditsItem[] creditsItems;

  public static CreditsData Deserialize(string text)
  {
    try
    {
      return JsonUtility.FromJson<CreditsData>(text);
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) string.Format("Exception in CreditsData json loading:\n{0}", (object) ex));
      return (CreditsData) null;
    }
  }

  public enum TextStyle
  {
    Normal,
    Title,
    Header,
  }

  [Serializable]
  public class Text
  {
    public string text;
    public bool localized;
    public CreditsData.TextStyle style;

    public virtual bool IsEmpty() => this.text == null || this.text.Length <= 0;

    public override string ToString() => string.Format("{0}[localized = {1}, style = {2}]", (object) this.text, (object) this.localized, (object) this.style);
  }

  [Serializable]
  public class RootCreditsItem
  {
    public CreditsData.Text title;
    public CreditsData.Text text;
    public int rowCountOverride;
    public CreditsData.ChildCreditsItem[] creditsItems;

    public virtual bool HasTitle() => this.title != null;

    public virtual bool HasText() => this.text != null;

    public virtual bool HasRowItems() => this.creditsItems != null && this.creditsItems.Length != 0;
  }

  [Serializable]
  public class ChildCreditsItem
  {
    public CreditsData.Text title;
    public CreditsData.Text text;

    public virtual bool HasTitle() => this.title != null;

    public virtual bool HasText() => this.text != null;
  }
}
