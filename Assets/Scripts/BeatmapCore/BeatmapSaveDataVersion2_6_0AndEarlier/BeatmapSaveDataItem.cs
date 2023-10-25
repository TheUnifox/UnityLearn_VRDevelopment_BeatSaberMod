using System;

namespace BeatmapSaveDataVersion2_6_0AndEarlier
{
    public abstract class BeatmapSaveDataItem : IComparable<BeatmapSaveDataItem>
    {
        public abstract float time { get; }

        public int CompareTo(BeatmapSaveDataItem other)
        {
            return this.time.CompareTo(other.time);
        }
    }
}
