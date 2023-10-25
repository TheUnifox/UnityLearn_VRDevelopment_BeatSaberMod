// Decompiled with JetBrains decompiler
// Type: FileCompressionHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.IO.Compression;

public class FileCompressionHelper
{
  public static void CreateZipFromDirectoryAsync(
    string sourceDirectoryName,
    string destinationArchiveFileName,
    System.Action<bool> finishCallback)
  {
    bool success = false;
    new HMTask((System.Action) (() => success = FileCompressionHelper.CreateZipFromDirectory(sourceDirectoryName, destinationArchiveFileName)), (System.Action) (() =>
    {
      if (finishCallback == null)
        return;
      finishCallback(success);
    })).Run();
  }

  public static void ExtractZipToDirectoryAsync(
    string sourceArchiveFileName,
    string destinationDirectoryName,
    System.Action<bool> finishCallback)
  {
    bool success = false;
    new HMTask((System.Action) (() => success = FileCompressionHelper.ExtractZipToDirectory(sourceArchiveFileName, destinationDirectoryName)), (System.Action) (() =>
    {
      if (finishCallback == null)
        return;
      finishCallback(success);
    })).Run();
  }

  public static bool CreateZipFromDirectory(
    string sourceDirectoryName,
    string destinationArchiveFileName)
  {
    try
    {
      ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, CompressionLevel.Fastest, false);
    }
    catch
    {
      return false;
    }
    return true;
  }

  public static bool ExtractZipToDirectory(
    string sourceArchiveFileName,
    string destinationDirectoryName)
  {
    try
    {
      ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
    }
    catch
    {
      return false;
    }
    return true;
  }
}
