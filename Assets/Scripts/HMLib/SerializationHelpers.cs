// Decompiled with JetBrains decompiler
// Type: SerializationHelpers
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationHelpers
{
  public static T DeserializeData<T>(byte[] data)
  {
    T obj = default (T);
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    MemoryStream serializationStream = new MemoryStream(data);
    using (serializationStream)
    {
      try
      {
        obj = (T) binaryFormatter.Deserialize((Stream) serializationStream);
      }
      catch
      {
        obj = default (T);
      }
    }
    return obj;
  }

  public static byte[] SerializeObject<T>(T serializableObject)
  {
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    MemoryStream serializationStream = new MemoryStream();
    using (serializationStream)
      binaryFormatter.Serialize((Stream) serializationStream, (object) serializableObject);
    return serializationStream.ToArray();
  }

  public static T DeserializeDataFromPlayerPrefs<T>(string key)
  {
    T obj = default (T);
    string s = PlayerPrefs.GetString(key, (string) null);
    if (s != null)
      obj = SerializationHelpers.DeserializeData<T>(Convert.FromBase64String(s));
    return obj;
  }

  public static void SerializeObjectIntoPlayerPrefs<T>(string key, T serializableObject)
  {
    byte[] inArray = SerializationHelpers.SerializeObject<T>(serializableObject);
    PlayerPrefs.SetString(key, Convert.ToBase64String(inArray));
  }

  public static T DeserializeDataFromFile<T>(string filePath)
  {
    if (!File.Exists(filePath))
      return default (T);
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    T obj2;
    try
    {
      FileStream serializationStream = File.Open(filePath, FileMode.Open);
      obj2 = (T) binaryFormatter.Deserialize((Stream) serializationStream);
      serializationStream.Close();
    }
    catch
    {
      obj2 = default (T);
    }
    return obj2;
  }

    public static void SerializeObjectToFile<T>(string filePath, T serializableObject)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(filePath);
        binaryFormatter.Serialize(fileStream, serializableObject);
        fileStream.Close();
    }
}
