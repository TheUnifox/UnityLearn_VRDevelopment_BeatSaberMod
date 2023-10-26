using System;

public static class EaseTypeExtensions
{
    public static EaseType ToEaseType(this BeatmapSaveDataVersion3.BeatmapSaveData.EaseType e)
    {
        EaseType toType;
        switch (e)
        {
            case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.None: 
                toType = EaseType.None; 
                break;
            case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.Linear: 
                toType = EaseType.Linear; 
                break;
            case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InQuad:
                toType = EaseType.InQuad;
                break;
            case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.OutQuad:
                toType = EaseType.OutQuad;
                break;
            case BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InOutQuad:
                toType = EaseType.InOutQuad;
                break;
            default:
                throw new ArgumentNullException(nameof(e));
        }
        return toType;
    }

    public static BeatmapSaveDataVersion3.BeatmapSaveData.EaseType FromEaseType(this EaseType e)
    {
        BeatmapSaveDataVersion3.BeatmapSaveData.EaseType toType;
        switch (e)
        {
            case EaseType.None:
                toType = BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.None;
                break;
            case EaseType.Linear:
                toType = BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.Linear;
                break;
            case EaseType.InQuad:
                toType = BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InQuad;
                break;
            case EaseType.OutQuad:
                toType = BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.OutQuad;
                break;
            case EaseType.InOutQuad:
                toType = BeatmapSaveDataVersion3.BeatmapSaveData.EaseType.InOutQuad;
                break;
            default:
                throw new ArgumentNullException(nameof(e));
        }
        return toType;
    }
}