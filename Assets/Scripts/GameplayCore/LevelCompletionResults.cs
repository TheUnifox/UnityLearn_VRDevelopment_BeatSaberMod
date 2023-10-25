using System;
using LiteNetLib.Utils;
using UnityEngine.Scripting;

// Token: 0x0200001F RID: 31
public class LevelCompletionResults : INetImmutableSerializable<LevelCompletionResults>, IComparable
{
    // Token: 0x0600009D RID: 157 RVA: 0x00002083 File Offset: 0x00000283
    [Preserve]
    public LevelCompletionResults()
    {
    }

    // Token: 0x0600009E RID: 158 RVA: 0x00003AF0 File Offset: 0x00001CF0
    public LevelCompletionResults(GameplayModifiers gameplayModifiers, int modifiedScore, int multipliedScore, RankModel.Rank rank, bool fullCombo, float leftSaberMovementDistance, float rightSaberMovementDistance, float leftHandMovementDistance, float rightHandMovementDistance, LevelCompletionResults.LevelEndStateType levelEndStateType, LevelCompletionResults.LevelEndAction levelEndAction, float energy, int goodCutsCount, int badCutsCount, int missedCount, int notGoodCount, int okCount, int maxCutScore, int totalCutScore, int goodCutsCountForNotesWithFullScoreScoringType, float averageCenterDistanceCutScoreForNotesWithFullScoreScoringType, float averageCutScoreForNotesWithFullScoreScoringType, int maxCombo, float endSongTime)
    {
        this.gameplayModifiers = gameplayModifiers;
        this.modifiedScore = modifiedScore;
        this.multipliedScore = multipliedScore;
        this.rank = rank;
        this.fullCombo = fullCombo;
        this.leftSaberMovementDistance = leftSaberMovementDistance;
        this.rightSaberMovementDistance = rightSaberMovementDistance;
        this.leftHandMovementDistance = leftHandMovementDistance;
        this.rightHandMovementDistance = rightHandMovementDistance;
        this.levelEndStateType = levelEndStateType;
        this.levelEndAction = levelEndAction;
        this.energy = energy;
        this.goodCutsCount = goodCutsCount;
        this.badCutsCount = badCutsCount;
        this.missedCount = missedCount;
        this.notGoodCount = notGoodCount;
        this.okCount = okCount;
        this.maxCutScore = maxCutScore;
        this.totalCutScore = totalCutScore;
        this.goodCutsCountForNotesWithFullScoreScoringType = goodCutsCountForNotesWithFullScoreScoringType;
        this.averageCenterDistanceCutScoreForNotesWithFullScoreScoringType = averageCenterDistanceCutScoreForNotesWithFullScoreScoringType;
        this.averageCutScoreForNotesWithFullScoreScoringType = averageCutScoreForNotesWithFullScoreScoringType;
        this.maxCombo = maxCombo;
        this.endSongTime = endSongTime;
    }

    // Token: 0x0600009F RID: 159 RVA: 0x00003BC0 File Offset: 0x00001DC0
    public virtual int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }
        LevelCompletionResults levelCompletionResults;
        if ((levelCompletionResults = (obj as LevelCompletionResults)) == null)
        {
            throw new ArgumentException("Comparing not comparable data.");
        }
        if (this.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared && levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed)
        {
            return -1;
        }
        if (this.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed && levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared)
        {
            return 1;
        }
        return this.modifiedScore.CompareTo(levelCompletionResults.modifiedScore) * -1;
    }

    // Token: 0x060000A0 RID: 160 RVA: 0x00003C28 File Offset: 0x00001E28
    public virtual void Serialize(NetDataWriter writer)
    {
        this.gameplayModifiers.Serialize(writer);
        writer.PutVarInt(this.modifiedScore);
        writer.PutVarInt(this.multipliedScore);
        writer.PutVarInt((int)this.rank);
        writer.Put(this.fullCombo);
        writer.Put(this.leftSaberMovementDistance);
        writer.Put(this.rightSaberMovementDistance);
        writer.Put(this.leftHandMovementDistance);
        writer.Put(this.rightHandMovementDistance);
        writer.PutVarInt((int)this.levelEndStateType);
        writer.PutVarInt((int)this.levelEndAction);
        writer.Put(this.energy);
        writer.PutVarInt(this.goodCutsCount);
        writer.PutVarInt(this.badCutsCount);
        writer.PutVarInt(this.missedCount);
        writer.PutVarInt(this.notGoodCount);
        writer.PutVarInt(this.okCount);
        writer.PutVarInt(this.maxCutScore);
        writer.PutVarInt(this.totalCutScore);
        writer.PutVarInt(this.goodCutsCountForNotesWithFullScoreScoringType);
        writer.Put(this.averageCenterDistanceCutScoreForNotesWithFullScoreScoringType);
        writer.Put(this.averageCutScoreForNotesWithFullScoreScoringType);
        writer.PutVarInt(this.maxCombo);
        writer.Put(this.endSongTime);
    }

    // Token: 0x060000A1 RID: 161 RVA: 0x00003D55 File Offset: 0x00001F55
    LevelCompletionResults INetImmutableSerializable<LevelCompletionResults>.CreateFromSerializedData(NetDataReader reader)
    {
        return LevelCompletionResults.CreateFromSerializedData(reader);
    }

    // Token: 0x060000A2 RID: 162 RVA: 0x00003D60 File Offset: 0x00001F60
    public static LevelCompletionResults CreateFromSerializedData(NetDataReader reader)
    {
        return new LevelCompletionResults(GameplayModifiers.CreateFromSerializedData(reader), reader.GetVarInt(), reader.GetVarInt(), (RankModel.Rank)reader.GetVarInt(), reader.GetBool(), reader.GetFloat(), reader.GetFloat(), reader.GetFloat(), reader.GetFloat(), (LevelCompletionResults.LevelEndStateType)reader.GetVarInt(), (LevelCompletionResults.LevelEndAction)reader.GetVarInt(), reader.GetFloat(), reader.GetVarInt(), reader.GetVarInt(), reader.GetVarInt(), reader.GetVarInt(), reader.GetVarInt(), reader.GetVarInt(), reader.GetVarInt(), reader.GetVarInt(), reader.GetFloat(), reader.GetFloat(), reader.GetVarInt(), reader.GetFloat());
    }

    // Token: 0x04000054 RID: 84
    public readonly GameplayModifiers gameplayModifiers;

    // Token: 0x04000055 RID: 85
    public readonly int modifiedScore;

    // Token: 0x04000056 RID: 86
    public readonly int multipliedScore;

    // Token: 0x04000057 RID: 87
    public readonly RankModel.Rank rank;

    // Token: 0x04000058 RID: 88
    public readonly bool fullCombo;

    // Token: 0x04000059 RID: 89
    public readonly float leftSaberMovementDistance;

    // Token: 0x0400005A RID: 90
    public readonly float rightSaberMovementDistance;

    // Token: 0x0400005B RID: 91
    public readonly float leftHandMovementDistance;

    // Token: 0x0400005C RID: 92
    public readonly float rightHandMovementDistance;

    // Token: 0x0400005D RID: 93
    public readonly LevelCompletionResults.LevelEndStateType levelEndStateType;

    // Token: 0x0400005E RID: 94
    public readonly LevelCompletionResults.LevelEndAction levelEndAction;

    // Token: 0x0400005F RID: 95
    public readonly float energy;

    // Token: 0x04000060 RID: 96
    public readonly int goodCutsCount;

    // Token: 0x04000061 RID: 97
    public readonly int badCutsCount;

    // Token: 0x04000062 RID: 98
    public readonly int missedCount;

    // Token: 0x04000063 RID: 99
    public readonly int notGoodCount;

    // Token: 0x04000064 RID: 100
    public readonly int okCount;

    // Token: 0x04000065 RID: 101
    public readonly int maxCutScore;

    // Token: 0x04000066 RID: 102
    public readonly int totalCutScore;

    // Token: 0x04000067 RID: 103
    public readonly int goodCutsCountForNotesWithFullScoreScoringType;

    // Token: 0x04000068 RID: 104
    public readonly float averageCenterDistanceCutScoreForNotesWithFullScoreScoringType;

    // Token: 0x04000069 RID: 105
    public readonly float averageCutScoreForNotesWithFullScoreScoringType;

    // Token: 0x0400006A RID: 106
    public readonly int maxCombo;

    // Token: 0x0400006B RID: 107
    public readonly float endSongTime;

    // Token: 0x02000020 RID: 32
    public enum LevelEndStateType
    {
        // Token: 0x0400006D RID: 109
        Incomplete,
        // Token: 0x0400006E RID: 110
        Cleared,
        // Token: 0x0400006F RID: 111
        Failed
    }

    // Token: 0x02000021 RID: 33
    public enum LevelEndAction
    {
        // Token: 0x04000071 RID: 113
        None,
        // Token: 0x04000072 RID: 114
        Quit,
        // Token: 0x04000073 RID: 115
        Restart
    }
}
