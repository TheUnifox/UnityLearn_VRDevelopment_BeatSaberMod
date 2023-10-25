// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InvalidFolderNameCharactersValidator
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Text.RegularExpressions;
using TMPro;

namespace BeatmapEditor3D
{
  public class InvalidFolderNameCharactersValidator : TMP_InputValidator
  {
    [DoesNotRequireDomainReloadInit]
    private static readonly Regex replaceRegex = BeatmapProjectFileHelper.GetInvalidDirectoryNameRegex();

    public override char Validate(ref string text, ref int pos, char ch)
    {
      if (InvalidFolderNameCharactersValidator.replaceRegex.Match(string.Format("{0}", (object) ch)) != Match.Empty)
        return char.MinValue;
      text = text.Insert(pos, ch.ToString());
      ++pos;
      return ch;
    }
  }
}
