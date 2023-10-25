using System;

// Token: 0x02000002 RID: 2
public class BasicMockPlayerScoreCalculator : IMockPlayerScoreCalculator
{
    // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
    public BasicMockPlayerScoreCalculator(float hitFrequency = 0.95f, int minScore = 66, int maxScore = 110)
    {
        this._random = new Random(Guid.NewGuid().GetHashCode());
        this._hitFrequency = hitFrequency;
        this._minScore = minScore;
        this._maxScore = maxScore;
    }

    // Token: 0x06000002 RID: 2 RVA: 0x00002096 File Offset: 0x00000296
    public int GetScoreForNote(MockNoteData noteData)
    {
        if (this._random.NextDouble() < (double)this._hitFrequency)
        {
            return this._minScore + this._random.Next() % (this._maxScore - this._minScore);
        }
        return 0;
    }

    // Token: 0x04000001 RID: 1
    private readonly float _hitFrequency;

    // Token: 0x04000002 RID: 2
    private readonly int _minScore;

    // Token: 0x04000003 RID: 3
    private readonly int _maxScore;

    // Token: 0x04000004 RID: 4
    private readonly Random _random;
}
