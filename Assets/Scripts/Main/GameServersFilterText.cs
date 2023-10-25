// Decompiled with JetBrains decompiler
// Type: GameServersFilterText
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Text;
using UnityEngine;
using Zenject;

[RequireComponent(typeof (CurvedTextMeshPro))]
public class GameServersFilterText : MonoBehaviour
{
  [SerializeField]
  protected CurvedTextMeshPro _text;
  [Inject]
  protected readonly SongPackMasksModel _songPackMasksModel;
  protected readonly StringBuilder _stringBuilder = new StringBuilder();

  public virtual void Setup(GameServersFilter filter, bool visible)
  {
    this.gameObject.SetActive(visible);
    if (!visible)
      return;
    this._stringBuilder.Clear();
    this._stringBuilder.Append(Localization.Get("LABEL_DIFFICULTY") + ": ");
    this._stringBuilder.Append(filter.filterByDifficulty ? Localization.Get(filter.filteredDifficulty.LocalizedKey()) : Localization.Get("BEATMAP_DIFFICULTY_ALL"));
    this._stringBuilder.Append(", ");
    bool plural = true;
    string str = filter.filterBySongPacks ? this._songPackMasksModel.GetSongPackMaskText(in filter.filteredSongPacks, out plural) : Localization.Get("ALL_LEVEL_PACKS");
    this._stringBuilder.Append((plural ? Localization.Get("MUSIC_PACKS_TABBAR_TITLE") : Localization.Get("MUSIC_PACK")) + ": ");
    this._stringBuilder.Append(str);
    this._text.text = this._stringBuilder.ToString();
  }

  public virtual void Setup(
    BeatmapDifficultyMask beatmapDifficultyMask,
    SongPackMask songPackMask,
    bool visible)
  {
    this.gameObject.SetActive(visible);
    if (!visible)
      return;
    this._stringBuilder.Clear();
    this._stringBuilder.Append(Localization.Get("LABEL_DIFFICULTY") + ": ");
    this._stringBuilder.Append(Localization.Get(beatmapDifficultyMask.LocalizedKey()));
    this._stringBuilder.Append(", ");
    bool plural;
    string songPackMaskText = this._songPackMasksModel.GetSongPackMaskText(in songPackMask, out plural);
    this._stringBuilder.Append((plural ? Localization.Get("MUSIC_PACKS_TABBAR_TITLE") : Localization.Get("MUSIC_PACK")) + ": ");
    this._stringBuilder.Append(songPackMaskText);
    this._text.text = this._stringBuilder.ToString();
  }
}
