// Decompiled with JetBrains decompiler
// Type: TimeExtensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Runtime.CompilerServices;

public static class TimeExtensions
{
  public static string MinSecDurationText(this float duration) => float.IsNaN(duration) ? "" : string.Format("{0}:{1}", (object) duration.Minutes(), (object) string.Format("{0:00}", (object) duration.Seconds()));

  public static string MinSecMillisecDurationText(this float duration) => duration.MinSecDurationText() + ":" + string.Format("{0:000}", (object) duration.Milliseconds());

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float OneBeatDuration(this float bpm) => (double) bpm <= 0.0 ? 0.0f : 60f / bpm;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float TimeToBeat(this float time, float bpm) => time / 60f * bpm;

  public static float SecondsToMinutes(this float seconds) => seconds / 60f;

  public static int SecondsToDays(this int time) => time / 86400;

  public static int SecondsToHours(this int time) => time / 3600;

  public static int SecondsToMinutes(this int time) => time / 60;

  public static int DaysToSeconds(this int days) => days * 3600 * 24;

  public static int HoursToSeconds(this int hours) => hours * 3600;

  public static int MinutesToSeconds(this int minutes) => minutes * 60;

  public static int Hours(this float time) => (int) ((double) time - (double) time.TotalDays().DaysToSeconds()) / 3600;

  public static int Minutes(this float time) => (int) ((double) time - (double) time.TotalHours().HoursToSeconds()) / 60;

  public static int Seconds(this float time) => (int) ((double) time % 60.0);

  public static int Milliseconds(this float time) => (int) ((double) time % 1.0 * 1000.0);

  public static int TotalDays(this float time) => ((int) time).SecondsToDays();

  public static int TotalHours(this float time) => ((int) time).SecondsToHours();

  public static int TotalMinutes(this float time) => ((int) time).SecondsToMinutes();

  public static int TotalSeconds(this float time) => (int) time;

  public static long ToUnixTime(this DateTime dateTime) => (long) (dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

  public static DateTime AsUnixTime(this long unixTime) => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds((double) unixTime);
}
