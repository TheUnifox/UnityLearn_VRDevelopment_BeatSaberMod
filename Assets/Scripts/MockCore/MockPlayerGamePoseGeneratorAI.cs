using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class MockPlayerGamePoseGeneratorAI : MockPlayerGamePoseGenerator
{
    // Token: 0x0600004B RID: 75 RVA: 0x00002488 File Offset: 0x00000688
    public MockPlayerGamePoseGeneratorAI(IMultiplayerSessionManager multiplayerSessionManager, IGameplayRpcManager gameplayRpcManager, IMockPlayerScoreCalculator scoreCalculator, bool leftHanded) : base(multiplayerSessionManager, gameplayRpcManager, leftHanded)
    {
        gameplayRpcManager.noteWasSpawnedEvent += this.HandleNoteWasSpawned;
        gameplayRpcManager.obstacleWasSpawnedEvent += this.HandleObstacleWasSpawned;
        gameplayRpcManager.sliderWasSpawnedEvent += this.HandleSliderWasSpawned;
        this._scoreCalculator = scoreCalculator;
    }

    // Token: 0x0600004C RID: 76 RVA: 0x000024DC File Offset: 0x000006DC
    public override void Dispose()
    {
        base.Dispose();
        this.gameplayRpcManager.noteWasSpawnedEvent -= this.HandleNoteWasSpawned;
        this.gameplayRpcManager.obstacleWasSpawnedEvent -= this.HandleObstacleWasSpawned;
        this.gameplayRpcManager.sliderWasSpawnedEvent -= this.HandleSliderWasSpawned;
        this._isInited = false;
    }

    // Token: 0x0600004D RID: 77 RVA: 0x0000253C File Offset: 0x0000073C
    public override void Init(float introStartTime, MockBeatmapData beatmapData, GameplayModifiers gameplayModifiers, Action onSongFinished)
    {
        this._onSongFinished = onSongFinished;
        this._gameplayModifiers = gameplayModifiers;
        if (this.leftHanded)
        {
            MockNoteData[] rightNotes = beatmapData.rightNotes;
            MockNoteData[] array = beatmapData.leftNotes;
            beatmapData.leftNotes = rightNotes;
            beatmapData.rightNotes = array;
            array = beatmapData.leftNotes;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Mirror(beatmapData.numberOfLines);
            }
            array = beatmapData.rightNotes;
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Mirror(beatmapData.numberOfLines);
            }
        }
        this._songStartTime = introStartTime + 10f;
        this._timeScale = gameplayModifiers.songSpeedMul;
        this._lastEventTime = beatmapData.songEndTime;
        this._lastHeadPose = new Pose(new Vector3(0f, 1.5f, 0f), Quaternion.identity);
        this._lastLeftHandPose = new Pose(new Vector3(-0.2f, 1f, 0.2f), Quaternion.identity);
        this._lastRightHandPose = new Pose(new Vector3(0.2f, 1f, 0.2f), Quaternion.identity);
        this._lastSongTime = (this.multiplayerSessionManager.syncTime - this._songStartTime) * this._timeScale;
        this._lineCount = beatmapData.numberOfLines;
        this._leftNotes = beatmapData.leftNotes;
        this._rightNotes = beatmapData.rightNotes;
        this._bombNotes = beatmapData.bombNotes;
        this._obstacles = beatmapData.obstacles;
        this._leftNoteIndex = 0;
        this._rightNoteIndex = 0;
        this._bombNoteIndex = 0;
        this._obstacleIndex = 0;
        this._prevLeftScore = 0;
        this._prevRightScore = 0;
        this._nextLeftHitScore = ((this._leftNotes.Length != 0) ? this._scoreCalculator.GetScoreForNote(this._leftNotes[0]) : 0);
        this._nextRightHitScore = ((this._rightNotes.Length != 0) ? this._scoreCalculator.GetScoreForNote(this._rightNotes[0]) : 0);
        this._score = 0;
        this._combo = 0;
        this._multiplier = 1;
        this._fullCombo = true;
        this._hasFinishedLevel = false;
        this._isInited = true;
    }

    // Token: 0x0600004E RID: 78 RVA: 0x0000275C File Offset: 0x0000095C
    public override void Tick()
    {
        if (!this._isInited || !this.multiplayerSessionManager.localPlayer.IsActive())
        {
            return;
        }
        float num = (this.multiplayerSessionManager.syncTime - this._songStartTime) * this._timeScale;
        bool flag;
        Pose pose = this.ProcessNotes(this._leftNotes, Vector3.left, ref this._leftNoteIndex, ref this._prevLeftScore, ref this._nextLeftHitScore, this._bombNotes, ref this._bombNoteIndex, this._lineCount, num, out flag);
        bool flag2;
        Pose pose2 = this.ProcessNotes(this._rightNotes, Vector3.right, ref this._rightNoteIndex, ref this._prevRightScore, ref this._nextRightHitScore, this._bombNotes, ref this._bombNoteIndex, this._lineCount, num, out flag2);
        Pose pose3 = this.ProcessObstacles(this._obstacles, ref this._obstacleIndex, this._lineCount, this._lastHeadPose, pose, pose2, num);
        this.mockNodePoseSyncStateSender.SendPose(pose3, pose, pose2);
        if (flag)
        {
            this._fullCombo = (this._fullCombo && this._prevLeftScore > 0);
            this.UpdateScore(ref this._score, ref this._combo, ref this._multiplier, this._prevLeftScore, this._lineCount, this._lastLeftHandPose, pose, this._lastSongTime, num, this._leftNotes[this._leftNoteIndex - 1], (this._leftNoteIndex < this._leftNotes.Length) ? this._leftNotes[this._leftNoteIndex] : null);
        }
        if (flag2)
        {
            this._fullCombo = (this._fullCombo && this._prevRightScore > 0);
            this.UpdateScore(ref this._score, ref this._combo, ref this._multiplier, this._prevRightScore, this._lineCount, this._lastRightHandPose, pose2, this._lastSongTime, num, this._rightNotes[this._rightNoteIndex - 1], (this._rightNoteIndex < this._rightNotes.Length) ? this._rightNotes[this._rightNoteIndex] : null);
        }
        this._lastHeadPose = pose3;
        this._lastLeftHandPose = pose;
        this._lastRightHandPose = pose2;
        this._lastSongTime = num;
        this._lastKnowScore = this._score;
        if (this._hasFinishedLevel || this.multiplayerSessionManager.syncTime <= this._songStartTime + this._lastEventTime)
        {
            return;
        }
        this._hasFinishedLevel = true;
        LevelCompletionResults levelCompletionResults = new LevelCompletionResults(this._gameplayModifiers, this._score, this._score, RankModel.Rank.A, this._fullCombo, 0f, 0f, 0f, 0f, LevelCompletionResults.LevelEndStateType.Cleared, LevelCompletionResults.LevelEndAction.None, 1f, 1, 0, 0, 0, 1, 100, 1000, 1, 8f, 100f, 0, 0f);
        this.gameplayRpcManager.LevelFinished(new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.SongFinished, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Cleared, levelCompletionResults));
        this.multiplayerSessionManager.SetLocalPlayerState("finished_level", true);
        Action onSongFinished = this._onSongFinished;
        if (onSongFinished == null)
        {
            return;
        }
        onSongFinished();
    }

    // Token: 0x0600004F RID: 79 RVA: 0x00002A30 File Offset: 0x00000C30
    private static Vector3 GetCutDirection(NoteCutDirection cutDirection)
    {
        switch (cutDirection)
        {
            case NoteCutDirection.Up:
                return Vector3.up;
            case NoteCutDirection.Down:
                return Vector3.down;
            case NoteCutDirection.Left:
                return Vector3.left;
            case NoteCutDirection.Right:
                return Vector3.right;
            case NoteCutDirection.UpLeft:
                return (MockPlayerGamePoseGeneratorAI.GetCutDirection(NoteCutDirection.Up) + MockPlayerGamePoseGeneratorAI.GetCutDirection(NoteCutDirection.Left)).normalized;
            case NoteCutDirection.UpRight:
                return (MockPlayerGamePoseGeneratorAI.GetCutDirection(NoteCutDirection.Up) + MockPlayerGamePoseGeneratorAI.GetCutDirection(NoteCutDirection.Right)).normalized;
            case NoteCutDirection.DownLeft:
                return (MockPlayerGamePoseGeneratorAI.GetCutDirection(NoteCutDirection.Down) + MockPlayerGamePoseGeneratorAI.GetCutDirection(NoteCutDirection.Left)).normalized;
            case NoteCutDirection.DownRight:
                return (MockPlayerGamePoseGeneratorAI.GetCutDirection(NoteCutDirection.Down) + MockPlayerGamePoseGeneratorAI.GetCutDirection(NoteCutDirection.Right)).normalized;
            case NoteCutDirection.Any:
                return Vector3.down;
            default:
                return Vector3.zero;
        }
    }

    // Token: 0x06000050 RID: 80 RVA: 0x00002AF8 File Offset: 0x00000CF8
    private static Vector3 GetNotePosition(int lineCount, MockNoteData noteData)
    {
        return new Vector3(((float)(1 - lineCount) * 0.5f + (float)noteData.lineIndex) * 0.6f, 0.4f + ((noteData.noteLineLayer == NoteLineLayer.Base) ? 0.25f : ((noteData.noteLineLayer == NoteLineLayer.Upper) ? 0.85f : 1.45f)), 0.6f);
    }

    // Token: 0x06000051 RID: 81 RVA: 0x00002B54 File Offset: 0x00000D54
    private static Vector3 Blerp(Vector3 prevStart, Vector3 prevEnd, Vector3 currStart, Vector3 currEnd, float t)
    {
        Vector3 a = Vector3.LerpUnclamped(prevStart, prevEnd, t);
        Vector3 vector = Vector3.LerpUnclamped(prevEnd, currStart, t);
        Vector3 b = Vector3.LerpUnclamped(currStart, currEnd, t);
        Vector3 a2 = Vector3.LerpUnclamped(a, vector, t);
        Vector3 b2 = Vector3.LerpUnclamped(vector, b, t);
        return Vector3.LerpUnclamped(a2, b2, t);
    }

    // Token: 0x06000052 RID: 82 RVA: 0x00002B98 File Offset: 0x00000D98
    private Pose ProcessNotes(MockNoteData[] notes, Vector3 handDirection, ref int noteIndex, ref int prevHitScore, ref int nextHitScore, MockNoteData[] bombs, ref int bombIndex, int lineCount, float songTime, out bool wasHitOrMiss)
    {
        wasHitOrMiss = false;
        if (noteIndex < notes.Length)
        {
            MockNoteData mockNoteData = notes[noteIndex];
            if (mockNoteData.time < songTime)
            {
                prevHitScore = nextHitScore;
                nextHitScore = this._scoreCalculator.GetScoreForNote(mockNoteData);
                wasHitOrMiss = true;
                noteIndex++;
            }
        }
        bool flag = prevHitScore > 0;
        bool flag2 = nextHitScore > 0;
        while (bombIndex < bombs.Length && bombs[bombIndex].time < songTime - 0.2f)
        {
            bombIndex++;
        }
        MockNoteData mockNoteData2 = (noteIndex < notes.Length) ? notes[noteIndex] : null;
        MockNoteData mockNoteData3 = (noteIndex > 0) ? notes[noteIndex - 1] : null;
        Vector3 vector = default(Vector3);
        Vector3 vector2 = default(Vector3);
        Vector3 vector3 = default(Vector3);
        Vector3 vector4 = default(Vector3);
        float num = 0f;
        float num2 = 0f;
        if (mockNoteData3 != null)
        {
            Vector3 cutDirection = MockPlayerGamePoseGeneratorAI.GetCutDirection(mockNoteData3.cutDirection);
            Vector3 notePosition = MockPlayerGamePoseGeneratorAI.GetNotePosition(lineCount, mockNoteData3);
            Vector3 vector5 = Vector3.zero;
            if (!flag)
            {
                vector5 += new Vector3(cutDirection.y, cutDirection.x, 0f).normalized * 0.75f;
            }
            vector = notePosition + Vector3.back * 0.2f + vector5;
            vector3 = Vector3.zero;
            vector2 = notePosition + Vector3.back * 0.4f + cutDirection * 0.4f + vector5;
            vector4 = new Vector3(60f * -cutDirection.y, 60f * cutDirection.x, 0f);
            num = mockNoteData3.time;
            num2 = mockNoteData3.time + 0.06f;
        }
        Vector3 vector6 = default(Vector3);
        Vector3 vector7 = default(Vector3);
        Vector3 vector8 = default(Vector3);
        Vector3 vector9 = default(Vector3);
        float num3 = 0f;
        float num4 = 0f;
        if (mockNoteData2 != null)
        {
            Vector3 cutDirection2 = MockPlayerGamePoseGeneratorAI.GetCutDirection(mockNoteData2.cutDirection);
            Vector3 notePosition2 = MockPlayerGamePoseGeneratorAI.GetNotePosition(lineCount, mockNoteData2);
            Vector3 vector10 = Vector3.zero;
            if (!flag2)
            {
                vector10 += new Vector3(cutDirection2.y, cutDirection2.x, 0f).normalized * 0.75f;
            }
            vector7 = notePosition2 + Vector3.back * 0.2f + vector10;
            vector9 = Vector3.zero;
            vector6 = notePosition2 + Vector3.back * 0.4f - cutDirection2 * 0.3f + vector10;
            vector8 = new Vector3(75f * cutDirection2.y, 75f * -cutDirection2.x, 0f);
            num3 = mockNoteData2.time - 0.1f;
            num4 = mockNoteData2.time;
        }
        if (mockNoteData3 == null)
        {
            vector = vector6;
            num = -6f;
            vector += 0.05f * new Vector3(Mathf.Sin(songTime), Mathf.Cos(songTime));
            vector3 = new Vector3(60f + 30f * Mathf.Cos(songTime), -30f * Mathf.Sin(songTime), 0f);
            vector2 = vector6;
            num2 = 0f;
            vector2 += 0.05f * new Vector3(Mathf.Sin(songTime), Mathf.Cos(songTime));
            vector4 = new Vector3(60f + 30f * Mathf.Cos(songTime), -30f * Mathf.Sin(songTime), 0f);
        }
        if (mockNoteData2 == null)
        {
            num3 = num2 + 1f;
            vector6 = 0.05f * new Vector3(Mathf.Sin(songTime), Mathf.Cos(songTime)) + handDirection * 0.45f + Vector3.up;
            vector8 = new Vector3(30f * Mathf.Cos(songTime), -30f * Mathf.Sin(songTime), 0f);
            num4 = num2 + 5f;
            vector7 = 0.05f * new Vector3(Mathf.Sin(songTime), Mathf.Cos(songTime)) + handDirection * 0.45f + Vector3.up;
            vector9 = new Vector3(30f * Mathf.Cos(songTime), -30f * Mathf.Sin(songTime), 0f);
        }
        float num5 = (num2 + num3) * 0.5f;
        float num6 = Mathf.Min(Mathf.Max(num + 0.12f, num2), Mathf.Max(num5 - 0.015f, num));
        float num7 = Mathf.Max(Mathf.Min(num3, num4 - 0.2f), Mathf.Min(num5 + 0.015f, num4));
        if (num2 > num)
        {
            vector2 = Vector3.LerpUnclamped(vector, vector2, (num6 - num) / (num2 - num));
            vector4 = Vector3.LerpUnclamped(vector3, vector4, (num6 - num) / (num2 - num));
        }
        num2 = num6;
        if (num4 > num3)
        {
            vector6 = Vector3.LerpUnclamped(vector7, vector6, (num4 - num7) / (num4 - num3));
            vector8 = Vector3.LerpUnclamped(vector9, vector8, (num4 - num7) / (num4 - num3));
        }
        num3 = num7;
        float num8 = (num4 > num) ? Mathf.Min(0.3f, (num3 - num2) / (num4 - num)) : 0.3f;
        float num9 = Mathf.Min(num2 + 0.015f, num5);
        float num10 = Mathf.Max(num3 - 0.015f, num5);
        float num11 = Mathf.Max(num9, num5 - 0.015f);
        float num12 = Mathf.Min(num10, num5 + 0.015f);
        float num13 = 0.5f * (1f - num8);
        float num14 = num8 * Mathf.Min(0.5f, (num12 - num11) / (num10 - num9));
        float num15 = 0.5f * (num8 - num14);
        float t;
        if (songTime < num)
        {
            t = 0f;
        }
        else if (songTime < num9)
        {
            float num16 = (songTime - num) / (num9 - num);
            t = num13 * num16;
        }
        else if (songTime < num11)
        {
            float num17 = (songTime - num9) / (num11 - num9);
            t = num13 + num17 * num15;
        }
        else if (songTime < num12)
        {
            float num18 = (songTime - num11) / (num12 - num11);
            t = num13 + num15 + num18 * num14;
        }
        else if (songTime < num10)
        {
            float num19 = (songTime - num12) / (num10 - num12);
            t = num13 + num15 + num14 + num19 * num15;
        }
        else if (songTime < num4)
        {
            float num20 = (songTime - num10) / (num4 - num10);
            t = 1f - num13 + num13 * num20;
        }
        else
        {
            t = 1f;
        }
        Pose pose = new Pose(MockPlayerGamePoseGeneratorAI.Blerp(vector, vector2, vector6, vector7, t), Quaternion.Euler(MockPlayerGamePoseGeneratorAI.Blerp(vector3, vector4, vector8, vector9, t)));
        int num21 = bombIndex;
        while (num21 < bombs.Length && bombs[num21].time < songTime + 0.2f)
        {
            MockNoteData noteData = bombs[num21];
            Vector3 notePosition3 = MockPlayerGamePoseGeneratorAI.GetNotePosition(lineCount, noteData);
            Vector3 b = pose.position + pose.rotation * Vector3.forward * 0.2f;
            Vector3 vector11 = notePosition3 - b;
            vector11.z = 0f;
            if (vector11.magnitude < 0.5f)
            {
                pose.position += 0.5f * vector11.normalized;
            }
            num21++;
        }
        return pose;
    }

    // Token: 0x06000053 RID: 83 RVA: 0x00003300 File Offset: 0x00001500
    private Pose ProcessObstacles(MockObstacleData[] obstacles, ref int obstacleIndex, int lineCount, Pose prevHeadPose, Pose leftHandPose, Pose rightHandPose, float songTime)
    {
        float num = Mathf.Sin(songTime * 2f);
        float num2 = Mathf.Cos(songTime);
        Pose pose = new Pose(new Vector3((leftHandPose.position.x + rightHandPose.position.x) * 0.5f + 0.1f * num2, 1.5f + 0.1f * num, 0.05f * num), Quaternion.identity);
        int num3 = obstacleIndex;
        while (num3 < obstacles.Length && obstacles[num3].time - 0.5f < songTime)
        {
            MockObstacleData mockObstacleData = obstacles[num3];
            if (mockObstacleData.time + mockObstacleData.duration < songTime)
            {
                obstacleIndex++;
            }
            else
            {
                float num4 = ((float)(1 - lineCount) * 0.5f + (float)mockObstacleData.lineIndex) * 0.6f;
                float num5 = num4 - (float)mockObstacleData.width * 0.5f;
                float num6 = num4 + (float)mockObstacleData.width * 0.5f;
                if (mockObstacleData.lineLayer == NoteLineLayer.Top)
                {
                    pose.position.y = 0.5f + 0.1f * num;
                }
                else if (num4 > 0f)
                {
                    pose.position.x = Mathf.Min(pose.position.x, num5 - 0.3f + 0.1f * num2);
                }
                else
                {
                    pose.position.x = Mathf.Max(pose.position.x, num6 + 0.3f + 0.1f * num2);
                }
            }
            num3++;
        }
        pose.rotation = Quaternion.Euler(new Vector3
        {
            x = -30f * (pose.position.y - 1.5f) + 10f * num,
            y = 30f * pose.position.x + num2 * 15f,
            z = num * 2f
        });
        return PosePrediction.InterpolatePose(prevHeadPose, pose, 0.02f);
    }

    // Token: 0x06000054 RID: 84 RVA: 0x00003500 File Offset: 0x00001700
    private void UpdateScore(ref int currentScore, ref int currentCombo, ref int currentMultiplier, int hitScore, int lineCount, Pose lastPose, Pose currentPose, float lastSongTime, float songTime, MockNoteData noteData, MockNoteData nextNoteData)
    {
        if (hitScore > 0)
        {
            currentScore += hitScore * currentMultiplier;
            currentCombo++;
            int num = currentCombo;
            if (num != 2)
            {
                if (num != 6)
                {
                    if (num == 14)
                    {
                        currentMultiplier = 8;
                    }
                }
                else
                {
                    currentMultiplier = 4;
                }
            }
            else
            {
                currentMultiplier = 2;
            }
        }
        else
        {
            currentCombo = 0;
            currentMultiplier = 1;
        }
        this.mockScoreSyncStateSender.SendScore(currentScore, currentScore, 1000000, currentCombo, currentMultiplier);
        Vector3 lhs = (currentPose.position - lastPose.position) / (songTime - lastSongTime);
        Vector3 cutPoint = currentPose.position + currentPose.forward * 0.2f;
        Vector3 vector = Vector3.Cross(lhs, currentPose.forward);
        if (hitScore > 0)
        {
            this.gameplayRpcManager.NoteCut(songTime, PoolableSerializable.Obtain<NoteCutInfoNetSerializable>().Init(lhs.magnitude, true, lhs.normalized, cutPoint, vector.normalized, noteData.gameplayType, noteData.colorType, noteData.noteLineLayer, noteData.lineIndex, noteData.time, (nextNoteData == null) ? float.MaxValue : (nextNoteData.time - noteData.time), MockPlayerGamePoseGeneratorAI.GetNotePosition(lineCount, noteData), Quaternion.identity, Vector3.one, Vector3.back));
            return;
        }
        this.gameplayRpcManager.NoteMissed(songTime, PoolableSerializable.Obtain<NoteMissInfoNetSerializable>().Init(noteData.colorType, noteData.noteLineLayer, noteData.lineIndex, noteData.time));
    }

    // Token: 0x06000055 RID: 85 RVA: 0x0000366C File Offset: 0x0000186C
    public override void SimulateFail()
    {
        LevelCompletionResults levelCompletionResults = new LevelCompletionResults(GameplayModifiers.noModifiers, this._lastKnowScore, this._lastKnowScore, RankModel.Rank.A, false, 0f, 0f, 0f, 0f, LevelCompletionResults.LevelEndStateType.Failed, LevelCompletionResults.LevelEndAction.None, 1f, 1, 0, 0, 0, 1, 100, 1000, 1, 8f, 100f, 0, 0f);
        this.gameplayRpcManager.LevelFinished(new MultiplayerLevelCompletionResults(MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndState.NotFinished, MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Failed, levelCompletionResults));
    }

    // Token: 0x06000056 RID: 86 RVA: 0x000036E0 File Offset: 0x000018E0
    private void HandleNoteWasSpawned(string userId, float syncTime, float songTime, NoteSpawnInfoNetSerializable noteSpawnInfoNetSerializable)
    {
        IConnectedPlayer playerByUserId = this.multiplayerSessionManager.GetPlayerByUserId(userId);
        if (playerByUserId != null && playerByUserId.isMe)
        {
            float songTime2 = (this.multiplayerSessionManager.syncTime - this._songStartTime) * this._timeScale;
            this.gameplayRpcManager.NoteSpawned(songTime2, noteSpawnInfoNetSerializable);
        }
    }

    // Token: 0x06000057 RID: 87 RVA: 0x00003730 File Offset: 0x00001930
    private void HandleObstacleWasSpawned(string userId, float syncTime, float songTime, ObstacleSpawnInfoNetSerializable obstacleSpawnInfoNetSerializable)
    {
        IConnectedPlayer playerByUserId = this.multiplayerSessionManager.GetPlayerByUserId(userId);
        if (playerByUserId != null && playerByUserId.isMe)
        {
            float songTime2 = (this.multiplayerSessionManager.syncTime - this._songStartTime) * this._timeScale;
            this.gameplayRpcManager.ObstacleSpawned(songTime2, obstacleSpawnInfoNetSerializable);
        }
    }

    // Token: 0x06000058 RID: 88 RVA: 0x00003780 File Offset: 0x00001980
    private void HandleSliderWasSpawned(string userId, float syncTime, float songTime, SliderSpawnInfoNetSerializable sliderSpawnInfoNetSerializable)
    {
        IConnectedPlayer playerByUserId = this.multiplayerSessionManager.GetPlayerByUserId(userId);
        if (playerByUserId != null && playerByUserId.isMe)
        {
            float songTime2 = (this.multiplayerSessionManager.syncTime - this._songStartTime) * this._timeScale;
            this.gameplayRpcManager.SliderSpawned(songTime2, sliderSpawnInfoNetSerializable);
        }
    }

    // Token: 0x04000021 RID: 33
    private readonly IMockPlayerScoreCalculator _scoreCalculator;

    // Token: 0x04000022 RID: 34
    private int _lastKnowScore;

    // Token: 0x04000023 RID: 35
    private float _songStartTime;

    // Token: 0x04000024 RID: 36
    private float _timeScale;

    // Token: 0x04000025 RID: 37
    private Action _onSongFinished;

    // Token: 0x04000026 RID: 38
    private GameplayModifiers _gameplayModifiers;

    // Token: 0x04000027 RID: 39
    private float _lastEventTime;

    // Token: 0x04000028 RID: 40
    private Pose _lastHeadPose;

    // Token: 0x04000029 RID: 41
    private Pose _lastLeftHandPose;

    // Token: 0x0400002A RID: 42
    private Pose _lastRightHandPose;

    // Token: 0x0400002B RID: 43
    private float _lastSongTime;

    // Token: 0x0400002C RID: 44
    private int _lineCount;

    // Token: 0x0400002D RID: 45
    private MockNoteData[] _leftNotes;

    // Token: 0x0400002E RID: 46
    private MockNoteData[] _rightNotes;

    // Token: 0x0400002F RID: 47
    private MockNoteData[] _bombNotes;

    // Token: 0x04000030 RID: 48
    private MockObstacleData[] _obstacles;

    // Token: 0x04000031 RID: 49
    private int _leftNoteIndex;

    // Token: 0x04000032 RID: 50
    private int _rightNoteIndex;

    // Token: 0x04000033 RID: 51
    private int _bombNoteIndex;

    // Token: 0x04000034 RID: 52
    private int _obstacleIndex;

    // Token: 0x04000035 RID: 53
    private int _prevLeftScore;

    // Token: 0x04000036 RID: 54
    private int _prevRightScore;

    // Token: 0x04000037 RID: 55
    private int _nextLeftHitScore;

    // Token: 0x04000038 RID: 56
    private int _nextRightHitScore;

    // Token: 0x04000039 RID: 57
    private int _score;

    // Token: 0x0400003A RID: 58
    private int _combo;

    // Token: 0x0400003B RID: 59
    private int _multiplier;

    // Token: 0x0400003C RID: 60
    private bool _fullCombo;

    // Token: 0x0400003D RID: 61
    private bool _hasFinishedLevel;

    // Token: 0x0400003E RID: 62
    private bool _isInited;
}
