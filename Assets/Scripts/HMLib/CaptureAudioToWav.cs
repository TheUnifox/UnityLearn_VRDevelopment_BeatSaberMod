// Decompiled with JetBrains decompiler
// Type: CaptureAudioToWav
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.IO;
using System.Text;
using UnityEngine;

public class CaptureAudioToWav : MonoBehaviour
{
  [SerializeField]
  protected string _fileName = "recTest.wav";
  protected int _sampleRate;
  protected int _headerSize = 44;
  protected bool _recording;
  protected FileStream _fileStream;

  public virtual void Awake() => this._sampleRate = AudioSettings.GetConfiguration().sampleRate;

  public virtual void Update()
  {
    if (!Input.GetKeyDown("r"))
      return;
    MonoBehaviour.print((object) "rec");
    if (!this._recording)
    {
      this.StartWriting(this._fileName);
      this._recording = true;
    }
    else
    {
      this._recording = false;
      this.WriteHeader();
      MonoBehaviour.print((object) "rec stop");
    }
  }

  public virtual void StartWriting(string name)
  {
    this._fileStream = new FileStream(name, FileMode.Create);
    byte num = 0;
    for (int index = 0; index < this._headerSize; ++index)
      this._fileStream.WriteByte(num);
  }

  public virtual void OnAudioFilterRead(float[] data, int channels)
  {
    if (!this._recording)
      return;
    this.ConvertAndWrite(data);
  }

  public virtual void ConvertAndWrite(float[] dataSource)
  {
    short[] numArray1 = new short[dataSource.Length];
    byte[] buffer = new byte[dataSource.Length * 2];
    int maxValue = (int) short.MaxValue;
    for (int index = 0; index < dataSource.Length; ++index)
    {
      numArray1[index] = (short) ((double) dataSource[index] * (double) maxValue);
      byte[] numArray2 = new byte[2];
      BitConverter.GetBytes(numArray1[index]).CopyTo((Array) buffer, index * 2);
    }
    this._fileStream.Write(buffer, 0, buffer.Length);
  }

  public virtual void WriteHeader()
  {
    this._fileStream.Seek(0L, SeekOrigin.Begin);
    this._fileStream.Write(Encoding.UTF8.GetBytes("RIFF"), 0, 4);
    this._fileStream.Write(BitConverter.GetBytes(this._fileStream.Length - 8L), 0, 4);
    this._fileStream.Write(Encoding.UTF8.GetBytes("WAVE"), 0, 4);
    this._fileStream.Write(Encoding.UTF8.GetBytes("fmt "), 0, 4);
    this._fileStream.Write(BitConverter.GetBytes(16), 0, 4);
    this._fileStream.Write(BitConverter.GetBytes((ushort) 1), 0, 2);
    this._fileStream.Write(BitConverter.GetBytes((ushort) 2), 0, 2);
    this._fileStream.Write(BitConverter.GetBytes(this._sampleRate), 0, 4);
    this._fileStream.Write(BitConverter.GetBytes(this._sampleRate * 4), 0, 4);
    this._fileStream.Write(BitConverter.GetBytes((ushort) 4), 0, 2);
    this._fileStream.Write(BitConverter.GetBytes((ushort) 16), 0, 2);
    this._fileStream.Write(Encoding.UTF8.GetBytes("data"), 0, 4);
    this._fileStream.Write(BitConverter.GetBytes(this._fileStream.Length - (long) this._headerSize), 0, 4);
    this._fileStream.Close();
  }
}
