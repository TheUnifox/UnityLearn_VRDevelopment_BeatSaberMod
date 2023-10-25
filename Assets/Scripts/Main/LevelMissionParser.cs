// Decompiled with JetBrains decompiler
// Type: LevelMissionParser
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class LevelMissionParser
{
  protected Dictionary<string, LevelMissionParser.ParserFunction> _functions;

  public LevelMissionParser() => this._functions = new Dictionary<string, LevelMissionParser.ParserFunction>(10);

  public virtual void AddFunction(string name, LevelMissionParser.ParserFunction function) => this._functions[name] = function;

  public virtual bool Parse(string s) => this.Parse(s, 0, s.Length);

  public virtual bool Parse(string s, int start, int length)
  {
    if (length == 0 || start + length > s.Length)
    {
      Debug.LogError((object) "Bad mission string format.");
      return false;
    }
    if (s[start] == '(')
    {
      if (s[start + length - 1] == ')' && length >= 2)
        return this.Parse(s, start + 1, length - 2);
      Debug.LogError((object) "Bad mission string format.");
      return false;
    }
    int num = 0;
    for (int index = start; index < start + length; ++index)
    {
      if (s[index] == ')')
        --num;
      else if (s[index] == '(')
        ++num;
      if (num < 0)
      {
        Debug.LogError((object) "Bad mission string format.");
        return false;
      }
      if (num == 0)
      {
        if (s[index] == '&')
          return this.Parse(s, start, index - start) & this.Parse(s, index + 1, length - index + start - 1);
        if (s[index] == '|')
          return this.Parse(s, start, index - start) | this.Parse(s, index + 1, length - index + start - 1);
      }
    }
    return s[start] == '!' ? !this.Parse(s, start + 1, length - 1) : this.ParseFunction(s, start, length);
  }

  public virtual bool ParseFunction(string s, int start, int length)
  {
    if (length < 3 || start + length > s.Length)
    {
      Debug.LogError((object) "Bad mission string format.");
      return false;
    }
    string key = "";
    int num = 0;
    for (int index = start; index < start + length; ++index)
    {
      if (s[index] == '(')
      {
        key = s.Substring(start, index - start);
        num = index + 1;
      }
    }
    if (num < start + 2 || num + 1 > start + length || s[start + length - 1] != ')')
    {
      Debug.LogError((object) "Bad mission string format.");
      return false;
    }
    float[] functionParams = new float[5];
    int paramCount = 0;
    int startIndex = num;
    for (int index = num; index < start + length - 1; ++index)
    {
      if (s[index] == ',')
      {
        if (paramCount + 1 > 5)
        {
          Debug.LogError((object) "Bad mission string format.");
          return false;
        }
        functionParams[paramCount] = float.Parse(s.Substring(startIndex, index - startIndex + 1));
        startIndex = index + 1;
        ++paramCount;
      }
    }
    if (paramCount < 5 && start + length - startIndex > 1)
    {
      functionParams[paramCount] = float.Parse(s.Substring(startIndex, start + length - startIndex - 1));
      ++paramCount;
    }
    LevelMissionParser.ParserFunction function = this._functions[key];
    return function == null || function(functionParams, paramCount);
  }

  public delegate bool ParserFunction(float[] functionParams, int paramCount);
}
