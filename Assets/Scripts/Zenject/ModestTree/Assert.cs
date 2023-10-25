using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace ModestTree
{
    // Token: 0x02000008 RID: 8
    public static class Assert
    {
        // Token: 0x06000014 RID: 20 RVA: 0x00002274 File Offset: 0x00000474
        public static void That(bool condition)
        {
            if (!condition)
            {
                throw Assert.CreateException("Assert hit!");
            }
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002284 File Offset: 0x00000484
        public static void IsNotEmpty(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw Assert.CreateException("Unexpected null or empty string");
            }
        }

        // Token: 0x06000016 RID: 22 RVA: 0x0000229C File Offset: 0x0000049C
        public static void IsEmpty<T>(IList<T> list)
        {
            if (list.Count != 0)
            {
                throw Assert.CreateException("Expected collection to be empty but instead found '{0}' elements", new object[]
                {
                    list.Count
                });
            }
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000022C8 File Offset: 0x000004C8
        public static void IsEmpty<T>(IEnumerable<T> sequence)
        {
            if (!sequence.IsEmpty<T>())
            {
                throw Assert.CreateException("Expected collection to be empty but instead found '{0}' elements", new object[]
                {
                    sequence.Count<T>()
                });
            }
        }

        // Token: 0x06000018 RID: 24 RVA: 0x000022F4 File Offset: 0x000004F4
        public static void IsType<T>(object obj)
        {
            Assert.IsType<T>(obj, "");
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00002304 File Offset: 0x00000504
        public static void IsType<T>(object obj, string message)
        {
            if (!(obj is T))
            {
                throw Assert.CreateException("Assert Hit! {0}\nWrong type found. Expected '{1}' (left) but found '{2}' (right). ", new object[]
                {
                    message,
                    typeof(T).PrettyName(),
                    obj.GetType().PrettyName()
                });
            }
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00002344 File Offset: 0x00000544
        public static void DerivesFrom<T>(Type type)
        {
            if (!type.DerivesFrom<T>())
            {
                throw Assert.CreateException("Expected type '{0}' to derive from '{1}'", new object[]
                {
                    type.Name,
                    typeof(T).Name
                });
            }
        }

        // Token: 0x0600001B RID: 27 RVA: 0x0000237C File Offset: 0x0000057C
        public static void DerivesFromOrEqual<T>(Type type)
        {
            if (!type.DerivesFromOrEqual<T>())
            {
                throw Assert.CreateException("Expected type '{0}' to derive from or be equal to '{1}'", new object[]
                {
                    type.Name,
                    typeof(T).Name
                });
            }
        }

        // Token: 0x0600001C RID: 28 RVA: 0x000023B4 File Offset: 0x000005B4
        public static void DerivesFrom(Type childType, Type parentType)
        {
            if (!childType.DerivesFrom(parentType))
            {
                throw Assert.CreateException("Expected type '{0}' to derive from '{1}'", new object[]
                {
                    childType.Name,
                    parentType.Name
                });
            }
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000023E4 File Offset: 0x000005E4
        public static void DerivesFromOrEqual(Type childType, Type parentType)
        {
            if (!childType.DerivesFromOrEqual(parentType))
            {
                throw Assert.CreateException("Expected type '{0}' to derive from or be equal to '{1}'", new object[]
                {
                    childType.Name,
                    parentType.Name
                });
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00002414 File Offset: 0x00000614
        public static void IsEqual(object left, object right)
        {
            Assert.IsEqual(left, right, "");
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002424 File Offset: 0x00000624
        public static void IsEqual(object left, object right, Func<string> messageGenerator)
        {
            if (!object.Equals(left, right))
            {
                left = (left ?? "<NULL>");
                right = (right ?? "<NULL>");
                throw Assert.CreateException("Assert Hit! {0}.  Expected '{1}' (left) but found '{2}' (right). ", new object[]
                {
                    messageGenerator(),
                    left,
                    right
                });
            }
        }

        // Token: 0x06000020 RID: 32 RVA: 0x00002474 File Offset: 0x00000674
        public static void IsApproximately(float left, float right, float epsilon = 1E-05f)
        {
            if (Math.Abs(left - right) >= epsilon)
            {
                throw Assert.CreateException("Assert Hit! Expected '{0}' (left) but found '{1}' (right). ", new object[]
                {
                    left,
                    right
                });
            }
        }

        // Token: 0x06000021 RID: 33 RVA: 0x000024A8 File Offset: 0x000006A8
        public static void IsEqual(object left, object right, string message)
        {
            if (!object.Equals(left, right))
            {
                left = (left ?? "<NULL>");
                right = (right ?? "<NULL>");
                throw Assert.CreateException("Assert Hit! {0}\nExpected '{1}' (left) but found '{2}' (right). ", new object[]
                {
                    message,
                    left,
                    right
                });
            }
        }

        // Token: 0x06000022 RID: 34 RVA: 0x000024E8 File Offset: 0x000006E8
        public static void IsNotEqual(object left, object right)
        {
            Assert.IsNotEqual(left, right, "");
        }

        // Token: 0x06000023 RID: 35 RVA: 0x000024F8 File Offset: 0x000006F8
        public static void IsNotEqual(object left, object right, Func<string> messageGenerator)
        {
            if (object.Equals(left, right))
            {
                left = (left ?? "<NULL>");
                right = (right ?? "<NULL>");
                throw Assert.CreateException("Assert Hit! {0}.  Expected '{1}' (left) to differ from '{2}' (right). ", new object[]
                {
                    messageGenerator(),
                    left,
                    right
                });
            }
        }

        // Token: 0x06000024 RID: 36 RVA: 0x00002548 File Offset: 0x00000748
        public static void IsNull(object val)
        {
            if (val != null)
            {
                throw Assert.CreateException("Assert Hit! Expected null pointer but instead found '{0}'", new object[]
                {
                    val
                });
            }
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00002564 File Offset: 0x00000764
        public static void IsNull(object val, string message)
        {
            if (val != null)
            {
                throw Assert.CreateException("Assert Hit! {0}", new object[]
                {
                    message
                });
            }
        }

        // Token: 0x06000026 RID: 38 RVA: 0x00002580 File Offset: 0x00000780
        public static void IsNull(object val, string message, object p1)
        {
            if (val != null)
            {
                throw Assert.CreateException("Assert Hit! {0}", new object[]
                {
                    message.Fmt(new object[]
                    {
                        p1
                    })
                });
            }
        }

        // Token: 0x06000027 RID: 39 RVA: 0x000025B4 File Offset: 0x000007B4
        public static void IsNotNull(object val)
        {
            if (val == null)
            {
                throw Assert.CreateException("Assert Hit! Found null pointer when value was expected");
            }
        }

        // Token: 0x06000028 RID: 40 RVA: 0x000025C4 File Offset: 0x000007C4
        public static void IsNotNull(object val, string message)
        {
            if (val == null)
            {
                throw Assert.CreateException("Assert Hit! {0}", new object[]
                {
                    message
                });
            }
        }

        // Token: 0x06000029 RID: 41 RVA: 0x000025E0 File Offset: 0x000007E0
        public static void IsNotNull(object val, string message, object p1)
        {
            if (val == null)
            {
                throw Assert.CreateException("Assert Hit! {0}", new object[]
                {
                    message.Fmt(new object[]
                    {
                        p1
                    })
                });
            }
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002614 File Offset: 0x00000814
        public static void IsNotNull(object val, string message, object p1, object p2)
        {
            if (val == null)
            {
                throw Assert.CreateException("Assert Hit! {0}", new object[]
                {
                    message.Fmt(new object[]
                    {
                        p1,
                        p2
                    })
                });
            }
        }

        // Token: 0x0600002B RID: 43 RVA: 0x0000264C File Offset: 0x0000084C
        public static void IsNotEmpty<T>(IEnumerable<T> val, string message = "")
        {
            if (!val.Any<T>())
            {
                throw Assert.CreateException("Assert Hit! Expected empty collection but found {0} values. {1}", new object[]
                {
                    val.Count<T>(),
                    message
                });
            }
        }

        // Token: 0x0600002C RID: 44 RVA: 0x0000267C File Offset: 0x0000087C
        public static void IsNotEqual(object left, object right, string message)
        {
            if (object.Equals(left, right))
            {
                left = (left ?? "<NULL>");
                right = (right ?? "<NULL>");
                throw Assert.CreateException("Assert Hit! {0}. Unexpected value found '{1}'. ", new object[]
                {
                    message,
                    left
                });
            }
        }

        // Token: 0x0600002D RID: 45 RVA: 0x000026B8 File Offset: 0x000008B8
        public static void Warn(bool condition)
        {
            if (!condition)
            {
                Log.Warn("Warning!  See call stack", Array.Empty<object>());
            }
        }

        // Token: 0x0600002E RID: 46 RVA: 0x000026CC File Offset: 0x000008CC
        public static void Warn(bool condition, Func<string> messageGenerator)
        {
            if (!condition)
            {
                Log.Warn("Warning Assert hit! " + messageGenerator(), Array.Empty<object>());
            }
        }

        // Token: 0x0600002F RID: 47 RVA: 0x000026EC File Offset: 0x000008EC
        public static void That(bool condition, string message)
        {
            if (!condition)
            {
                throw Assert.CreateException("Assert hit! " + message);
            }
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00002704 File Offset: 0x00000904
        public static void That(bool condition, string message, object p1)
        {
            if (!condition)
            {
                throw Assert.CreateException("Assert hit! " + message.Fmt(new object[]
                {
                    p1
                }));
            }
        }

        // Token: 0x06000031 RID: 49 RVA: 0x0000272C File Offset: 0x0000092C
        public static void That(bool condition, string message, object p1, object p2)
        {
            if (!condition)
            {
                throw Assert.CreateException("Assert hit! " + message.Fmt(new object[]
                {
                    p1,
                    p2
                }));
            }
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002758 File Offset: 0x00000958
        public static void That(bool condition, string message, object p1, object p2, object p3)
        {
            if (!condition)
            {
                throw Assert.CreateException("Assert hit! " + message.Fmt(new object[]
                {
                    p1,
                    p2,
                    p3
                }));
            }
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00002788 File Offset: 0x00000988
        public static void Warn(bool condition, string message)
        {
            if (!condition)
            {
                Log.Warn("Warning Assert hit! " + message, Array.Empty<object>());
            }
        }

        // Token: 0x06000034 RID: 52 RVA: 0x000027A4 File Offset: 0x000009A4
        public static void Throws(Action action)
        {
            Assert.Throws<Exception>(action);
        }

        // Token: 0x06000035 RID: 53 RVA: 0x000027AC File Offset: 0x000009AC
        public static void Throws<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }
            throw Assert.CreateException("Expected to receive exception of type '{0}' but nothing was thrown", new object[]
            {
                typeof(TException).Name
            });
        }

        // Token: 0x06000036 RID: 54 RVA: 0x000027F8 File Offset: 0x000009F8
        public static Zenject.ZenjectException CreateException()
        {
            return new Zenject.ZenjectException("Assert hit!");
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00002804 File Offset: 0x00000A04
        public static Zenject.ZenjectException CreateException(string message)
        {
            return new Zenject.ZenjectException(message);
        }

        // Token: 0x06000038 RID: 56 RVA: 0x0000280C File Offset: 0x00000A0C
        public static Zenject.ZenjectException CreateException(string message, params object[] parameters)
        {
            return new Zenject.ZenjectException(message.Fmt(parameters));
        }

        // Token: 0x06000039 RID: 57 RVA: 0x0000281C File Offset: 0x00000A1C
        public static Zenject.ZenjectException CreateException(Exception innerException, string message, params object[] parameters)
        {
            return new Zenject.ZenjectException(message.Fmt(parameters), innerException);
        }
    }
}
