using System;

namespace ModestTree.Util
{
    // Token: 0x0200001C RID: 28
    public static class ValuePair
    {
        // Token: 0x060000BF RID: 191 RVA: 0x00003BEC File Offset: 0x00001DEC
        public static ValuePair<T1, T2> New<T1, T2>(T1 first, T2 second)
        {
            return new ValuePair<T1, T2>(first, second);
        }

        // Token: 0x060000C0 RID: 192 RVA: 0x00003BF8 File Offset: 0x00001DF8
        public static ValuePair<T1, T2, T3> New<T1, T2, T3>(T1 first, T2 second, T3 third)
        {
            return new ValuePair<T1, T2, T3>(first, second, third);
        }

        // Token: 0x060000C1 RID: 193 RVA: 0x00003C04 File Offset: 0x00001E04
        public static ValuePair<T1, T2, T3, T4> New<T1, T2, T3, T4>(T1 first, T2 second, T3 third, T4 fourth)
        {
            return new ValuePair<T1, T2, T3, T4>(first, second, third, fourth);
        }
    }

    public class ValuePair<T1, T2>
    {
        // Token: 0x060000B0 RID: 176 RVA: 0x000037D0 File Offset: 0x000019D0
        public ValuePair()
        {
            this.First = default(T1);
            this.Second = default(T2);
        }

        // Token: 0x060000B1 RID: 177 RVA: 0x000037F0 File Offset: 0x000019F0
        public ValuePair(T1 first, T2 second)
        {
            this.First = first;
            this.Second = second;
        }

        // Token: 0x060000B2 RID: 178 RVA: 0x00003808 File Offset: 0x00001A08
        public override bool Equals(object obj)
        {
            ValuePair<T1, T2> valuePair = obj as ValuePair<T1, T2>;
            return valuePair != null && this.Equals(valuePair);
        }

        // Token: 0x060000B3 RID: 179 RVA: 0x00003828 File Offset: 0x00001A28
        public bool Equals(ValuePair<T1, T2> that)
        {
            return that != null && object.Equals(this.First, that.First) && object.Equals(this.Second, that.Second);
        }

        // Token: 0x060000B4 RID: 180 RVA: 0x00003874 File Offset: 0x00001A74
        public override int GetHashCode()
        {
            int num = 17 * 29;
            int num2;
            if (this.First != null)
            {
                T1 first = this.First;
                num2 = first.GetHashCode();
            }
            else
            {
                num2 = 0;
            }
            int num3 = (num + num2) * 29;
            int num4;
            if (this.Second != null)
            {
                T2 second = this.Second;
                num4 = second.GetHashCode();
            }
            else
            {
                num4 = 0;
            }
            return num3 + num4;
        }

        // Token: 0x04000025 RID: 37
        public readonly T1 First;

        // Token: 0x04000026 RID: 38
        public readonly T2 Second;
    }

    public class ValuePair<T1, T2, T3>
    {
        // Token: 0x060000B5 RID: 181 RVA: 0x000038D4 File Offset: 0x00001AD4
        public ValuePair()
        {
            this.First = default(T1);
            this.Second = default(T2);
            this.Third = default(T3);
        }

        // Token: 0x060000B6 RID: 182 RVA: 0x00003900 File Offset: 0x00001B00
        public ValuePair(T1 first, T2 second, T3 third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }

        // Token: 0x060000B7 RID: 183 RVA: 0x00003920 File Offset: 0x00001B20
        public override bool Equals(object obj)
        {
            ValuePair<T1, T2, T3> valuePair = obj as ValuePair<T1, T2, T3>;
            return valuePair != null && this.Equals(valuePair);
        }

        // Token: 0x060000B8 RID: 184 RVA: 0x00003940 File Offset: 0x00001B40
        public bool Equals(ValuePair<T1, T2, T3> that)
        {
            return that != null && (object.Equals(this.First, that.First) && object.Equals(this.Second, that.Second)) && object.Equals(this.Third, that.Third);
        }

        // Token: 0x060000B9 RID: 185 RVA: 0x000039AC File Offset: 0x00001BAC
        public override int GetHashCode()
        {
            int num = 17 * 29;
            int num2;
            if (this.First != null)
            {
                T1 first = this.First;
                num2 = first.GetHashCode();
            }
            else
            {
                num2 = 0;
            }
            int num3 = (num + num2) * 29;
            int num4;
            if (this.Second != null)
            {
                T2 second = this.Second;
                num4 = second.GetHashCode();
            }
            else
            {
                num4 = 0;
            }
            int num5 = (num3 + num4) * 29;
            int num6;
            if (this.Third != null)
            {
                T3 third = this.Third;
                num6 = third.GetHashCode();
            }
            else
            {
                num6 = 0;
            }
            return num5 + num6;
        }

        // Token: 0x04000027 RID: 39
        public readonly T1 First;

        // Token: 0x04000028 RID: 40
        public readonly T2 Second;

        // Token: 0x04000029 RID: 41
        public readonly T3 Third;
    }

    public class ValuePair<T1, T2, T3, T4>
    {
        // Token: 0x060000BA RID: 186 RVA: 0x00003A34 File Offset: 0x00001C34
        public ValuePair()
        {
            this.First = default(T1);
            this.Second = default(T2);
            this.Third = default(T3);
            this.Fourth = default(T4);
        }

        // Token: 0x060000BB RID: 187 RVA: 0x00003A6C File Offset: 0x00001C6C
        public ValuePair(T1 first, T2 second, T3 third, T4 fourth)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Fourth = fourth;
        }

        // Token: 0x060000BC RID: 188 RVA: 0x00003A94 File Offset: 0x00001C94
        public override bool Equals(object obj)
        {
            ValuePair<T1, T2, T3, T4> valuePair = obj as ValuePair<T1, T2, T3, T4>;
            return valuePair != null && this.Equals(valuePair);
        }

        // Token: 0x060000BD RID: 189 RVA: 0x00003AB4 File Offset: 0x00001CB4
        public bool Equals(ValuePair<T1, T2, T3, T4> that)
        {
            return that != null && (object.Equals(this.First, that.First) && object.Equals(this.Second, that.Second) && object.Equals(this.Third, that.Third)) && object.Equals(this.Fourth, that.Fourth);
        }

        // Token: 0x060000BE RID: 190 RVA: 0x00003B3C File Offset: 0x00001D3C
        public override int GetHashCode()
        {
            int num = 17 * 29;
            int num2;
            if (this.First != null)
            {
                T1 first = this.First;
                num2 = first.GetHashCode();
            }
            else
            {
                num2 = 0;
            }
            int num3 = (num + num2) * 29;
            int num4;
            if (this.Second != null)
            {
                T2 second = this.Second;
                num4 = second.GetHashCode();
            }
            else
            {
                num4 = 0;
            }
            int num5 = (num3 + num4) * 29;
            int num6;
            if (this.Third != null)
            {
                T3 third = this.Third;
                num6 = third.GetHashCode();
            }
            else
            {
                num6 = 0;
            }
            int num7 = (num5 + num6) * 29;
            int num8;
            if (this.Fourth != null)
            {
                T4 fourth = this.Fourth;
                num8 = fourth.GetHashCode();
            }
            else
            {
                num8 = 0;
            }
            return num7 + num8;
        }

        // Token: 0x0400002A RID: 42
        public readonly T1 First;

        // Token: 0x0400002B RID: 43
        public readonly T2 Second;

        // Token: 0x0400002C RID: 44
        public readonly T3 Third;

        // Token: 0x0400002D RID: 45
        public readonly T4 Fourth;
    }
}
