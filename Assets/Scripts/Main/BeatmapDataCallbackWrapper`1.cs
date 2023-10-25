// Decompiled with JetBrains decompiler
// Type: BeatmapDataCallbackWrapper`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class BeatmapDataCallbackWrapper<T> : BeatmapDataCallbackWrapper where T : BeatmapDataItem
{
  protected readonly BeatmapDataCallback<T> _callback;

  public BeatmapDataCallbackWrapper(
    BeatmapDataCallback<T> callback,
    float aheadTime,
    params int[] beatmapEventSubtypeIdentifiers)
    : base(aheadTime, typeof (T), beatmapEventSubtypeIdentifiers)
  {
    this._callback = callback;
  }

  public override void CallCallback(BeatmapDataItem beatmapData) => this._callback((T) beatmapData);
}
