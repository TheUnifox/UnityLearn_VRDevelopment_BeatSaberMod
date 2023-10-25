using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public static class NoteCutDirectionExtensions
{
    // Token: 0x0600007C RID: 124 RVA: 0x00003044 File Offset: 0x00001244
    public static Vector2 Direction(this NoteCutDirection cutDirection)
    {
        switch (cutDirection)
        {
            case NoteCutDirection.Up:
                return new Vector2(0f, 1f);
            case NoteCutDirection.Down:
                return new Vector2(0f, -1f);
            case NoteCutDirection.Left:
                return new Vector2(-1f, 0f);
            case NoteCutDirection.Right:
                return new Vector2(1f, 0f);
            case NoteCutDirection.UpLeft:
                return new Vector2(-0.7071f, 0.7071f);
            case NoteCutDirection.UpRight:
                return new Vector2(0.7071f, 0.7071f);
            case NoteCutDirection.DownLeft:
                return new Vector2(-0.7071f, -0.7071f);
            case NoteCutDirection.DownRight:
                return new Vector2(0.7071f, -0.7071f);
            default:
                return new Vector2(0f, 0f);
        }
    }

    // Token: 0x0600007D RID: 125 RVA: 0x0000310C File Offset: 0x0000130C
    public static float RotationAngle(this NoteCutDirection cutDirection)
    {
        switch (cutDirection)
        {
            case NoteCutDirection.Up:
                return -180f;
            case NoteCutDirection.Down:
                return 0f;
            case NoteCutDirection.Left:
                return -90f;
            case NoteCutDirection.Right:
                return 90f;
            case NoteCutDirection.UpLeft:
                return -135f;
            case NoteCutDirection.UpRight:
                return 135f;
            case NoteCutDirection.DownLeft:
                return -45f;
            case NoteCutDirection.DownRight:
                return 45f;
            default:
                return 0f;
        }
    }

    // Token: 0x0600007E RID: 126 RVA: 0x00003176 File Offset: 0x00001376
    public static Quaternion Rotation(this NoteCutDirection cutDirection, float offset = 0f)
    {
        return Quaternion.Euler(0f, 0f, cutDirection.RotationAngle() + offset);
    }

    // Token: 0x0600007F RID: 127 RVA: 0x0000318F File Offset: 0x0000138F
    public static bool IsMainDirection(this NoteCutDirection cutDirection)
    {
        switch (cutDirection)
        {
            case NoteCutDirection.Up:
                return true;
            case NoteCutDirection.Down:
                return true;
            case NoteCutDirection.Left:
                return true;
            case NoteCutDirection.Right:
                return true;
            default:
                return false;
        }
    }

    // Token: 0x06000080 RID: 128 RVA: 0x000031B4 File Offset: 0x000013B4
    public static NoteCutDirection MainNoteCutDirectionFromCutDirAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f)
        {
            angle += 360f;
        }
        if (angle < 45f || angle > 315f)
        {
            return NoteCutDirection.Right;
        }
        if (angle < 135f)
        {
            return NoteCutDirection.Up;
        }
        if (angle < 225f)
        {
            return NoteCutDirection.Left;
        }
        return NoteCutDirection.Down;
    }

    // Token: 0x06000081 RID: 129 RVA: 0x00003202 File Offset: 0x00001402
    public static NoteCutDirection Mirrored(this NoteCutDirection cutDirection)
    {
        switch (cutDirection)
        {
            case NoteCutDirection.Left:
                return NoteCutDirection.Right;
            case NoteCutDirection.Right:
                return NoteCutDirection.Left;
            case NoteCutDirection.UpLeft:
                return NoteCutDirection.UpRight;
            case NoteCutDirection.UpRight:
                return NoteCutDirection.UpLeft;
            case NoteCutDirection.DownLeft:
                return NoteCutDirection.DownRight;
            case NoteCutDirection.DownRight:
                return NoteCutDirection.DownLeft;
            default:
                return cutDirection;
        }
    }

    // Token: 0x06000082 RID: 130 RVA: 0x00003233 File Offset: 0x00001433
    public static NoteCutDirection Opposite(this NoteCutDirection cutDirection)
    {
        switch (cutDirection)
        {
            case NoteCutDirection.Up:
                return NoteCutDirection.Down;
            case NoteCutDirection.Down:
                return NoteCutDirection.Up;
            case NoteCutDirection.Left:
                return NoteCutDirection.Right;
            case NoteCutDirection.Right:
                return NoteCutDirection.Left;
            case NoteCutDirection.UpLeft:
                return NoteCutDirection.DownRight;
            case NoteCutDirection.UpRight:
                return NoteCutDirection.DownLeft;
            case NoteCutDirection.DownLeft:
                return NoteCutDirection.UpRight;
            case NoteCutDirection.DownRight:
                return NoteCutDirection.UpLeft;
            default:
                return cutDirection;
        }
    }

    // Token: 0x06000083 RID: 131 RVA: 0x00003270 File Offset: 0x00001470
    public static bool IsOnSamePlane(this NoteCutDirection noteCutDirection1, NoteCutDirection noteCutDirection2)
    {
        float a = Mathf.Abs(noteCutDirection1.RotationAngle() - noteCutDirection2.RotationAngle());
        return Mathf.Approximately(a, 0f) || Mathf.Approximately(a, 180f);
    }

    // Token: 0x06000084 RID: 132 RVA: 0x000032AC File Offset: 0x000014AC
    public static NoteCutDirection NoteCutDirectionFromDirection(Vector3 direction)
    {
        float num = Vector2.Angle(direction, Vector2.up);
        if (direction.x < 0f)
        {
            if (num <= 22.5f)
            {
                return NoteCutDirection.Up;
            }
            if (num > 22.5f && num <= 67.5f)
            {
                return NoteCutDirection.UpLeft;
            }
            if (num > 67.5f && num <= 112.5f)
            {
                return NoteCutDirection.Left;
            }
            if (num > 112.5f && num <= 157.5f)
            {
                return NoteCutDirection.DownLeft;
            }
            return NoteCutDirection.Down;
        }
        else
        {
            if (num <= 22.5f)
            {
                return NoteCutDirection.Up;
            }
            if (num > 22.5f && num <= 67.5f)
            {
                return NoteCutDirection.UpRight;
            }
            if (num > 67.5f && num <= 112.5f)
            {
                return NoteCutDirection.Right;
            }
            if (num > 112.5f && num <= 157.5f)
            {
                return NoteCutDirection.DownRight;
            }
            return NoteCutDirection.Down;
        }
    }
}
