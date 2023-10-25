// Decompiled with JetBrains decompiler
// Type: LocalizedHoverHint
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;

public class LocalizedHoverHint : LocalizedTextComponent<HoverHint>
{
  protected override void SetText(HoverHint hoverHint, string value) => hoverHint.text = value;

  protected override void UpdateAlignment(HoverHint hoverHint, LanguageDirection direction)
  {
  }
}
