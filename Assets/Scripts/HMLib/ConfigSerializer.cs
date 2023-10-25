// Decompiled with JetBrains decompiler
// Type: ConfigSerializer
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class ConfigSerializer
{
  public static void SaveConfig(object config, string filePath)
  {
    List<string> stringList = new List<string>();
    FieldInfo[] fields = config.GetType().GetFields();
    for (int index = 0; index < fields.Length; ++index)
    {
      Type fieldType = fields[index].FieldType;
      if (fieldType == typeof (float) || fieldType == typeof (int) || fieldType == typeof (bool))
        stringList.Add(fields[index].Name + "=" + fields[index].GetValue(config).ToString());
      else if (fieldType == typeof (string))
        stringList.Add(fields[index].Name + "=\"" + fields[index].GetValue(config) + "\"");
    }
    File.WriteAllLines(filePath, stringList.ToArray());
  }

  public static bool LoadConfig(object config, string filePath)
  {
    try
    {
      foreach (string readAllLine in File.ReadAllLines(filePath))
      {
        char[] chArray = new char[1]{ '=' };
        string[] strArray = readAllLine.Split(chArray);
        if (strArray.Length == 2)
        {
          string name = strArray[0];
          FieldInfo field = config.GetType().GetField(name);
          if (field != (FieldInfo) null)
          {
            Type fieldType = field.FieldType;
            if (fieldType == typeof (float))
              field.SetValue(config, (object) float.Parse(strArray[1]));
            else if (fieldType == typeof (int))
              field.SetValue(config, (object) int.Parse(strArray[1]));
            else if (fieldType == typeof (bool))
            {
              if (strArray[1].Length == 1)
                field.SetValue(config, (object) (strArray[1] == "1"));
              else
                field.SetValue(config, (object) Convert.ToBoolean(strArray[1]));
            }
            else if (fieldType == typeof (string))
              field.SetValue(config, (object) strArray[1].Trim('"'));
          }
        }
      }
    }
    catch
    {
      return false;
    }
    return true;
  }
}
