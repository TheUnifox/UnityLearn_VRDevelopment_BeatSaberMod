// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapFileUtils
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BeatmapEditor3D
{
  public static class BeatmapFileUtils
  {
    public const string kHiddenDirectoryPrefix = "~";
    public const string kTempDirectorySuffix = "_Temp";
    public const string kParentBackupDirectorySuffix = "_Backups";
    public const string kBackupDirectorySuffix = "_Backup";
    public const string kBeatmapLevelExtension = ".dat";
    public const string kBeatmapEditorSettingsFilename = "BeatmapEditorSettings.dat";
    public const string kStandardLevelInfoFilename = "Info.dat";
    public const string kDefaultCustomLevelsDirectoryName = "CustomLevels";
    public const string kBPMInfoFilename = "BPMInfo.dat";

    public static string beatmapEditorSettingsFilePath => Path.Combine(Application.persistentDataPath, "BeatmapEditorSettings.dat");

    public static string GetBeatmapDirectoryDuplicateName(string beatmapDirectory, int count) => string.Format("{0} ({1})", (object) beatmapDirectory, (object) count);

    public static string GetBeatmapLevelName(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      return beatmapCharacteristic.compoundIdPartName + beatmapDifficulty.SerializedName() + ".dat";
    }

    public static string GetParentBackupBeatmapDirectory(string beatmapFolderPath)
    {
      if (!BeatmapFileUtils.IsDirectory(beatmapFolderPath))
        return (string) null;
      (string, string) andDirectoryName = BeatmapFileUtils.GetParentPathAndDirectoryName(beatmapFolderPath);
      return Path.Combine(andDirectoryName.Item1, "~" + andDirectoryName.Item2 + "_Backups");
    }

    public static string GetBackupBeatmapDirectory(
      string beatmapFolderPath,
      string destinationBackupPath)
    {
      if (!BeatmapFileUtils.IsDirectory(beatmapFolderPath) || !BeatmapFileUtils.IsDirectory(destinationBackupPath))
        return (string) null;
      string str1 = string.Format("{0}{1}_{2:yyyyMMddTHHmmssffff}", (object) BeatmapFileUtils.GetParentPathAndDirectoryName(beatmapFolderPath).Item2, (object) "_Backup", (object) DateTime.Now);
      string str2 = str1;
      int num = 0;
      while (Directory.Exists(str1))
      {
        str1 = string.Format("{0} ({1})", (object) str2, (object) num);
        ++num;
      }
      return Path.Combine(destinationBackupPath, str1);
    }

    public static string GetTempBeatmapDirectoryPath(string beatmapFolderPath)
    {
      if (!BeatmapFileUtils.IsDirectory(beatmapFolderPath))
        return (string) null;
      (string, string) andDirectoryName = BeatmapFileUtils.GetParentPathAndDirectoryName(beatmapFolderPath);
      return Path.Combine(andDirectoryName.Item1, "~" + andDirectoryName.Item2 + "_Temp");
    }

    public static void CopyBeatmapProject(
      string sourceDirectoryPath,
      string destinationDirectoryPath,
      string songFilename,
      string coverImageFilename,
      string[] difficultyFilenames)
    {
      if (!new DirectoryInfo(sourceDirectoryPath).Exists)
        Debug.LogError((object) ("Source directory does not exist or could not be found: " + sourceDirectoryPath));
      else if (Directory.Exists(destinationDirectoryPath))
      {
        Debug.LogError((object) ("Destination directory already exists: " + destinationDirectoryPath));
      }
      else
      {
        Directory.CreateDirectory(destinationDirectoryPath);
        File.Copy(GetSourcePath("Info.dat"), GetDestinationPath("Info.dat"));
        if (File.Exists(GetSourcePath("BPMInfo.dat")))
          File.Copy(GetSourcePath("BPMInfo.dat"), GetDestinationPath("BPMInfo.dat"));
        if (!string.IsNullOrEmpty(songFilename) && !BeatmapFileUtils.CreateHardLink(GetSourcePath(songFilename), GetDestinationPath(songFilename)))
          File.Copy(GetSourcePath(songFilename), GetDestinationPath(songFilename));
        if (!string.IsNullOrEmpty(coverImageFilename) && File.Exists(GetSourcePath(coverImageFilename)) && !BeatmapFileUtils.CreateHardLink(GetSourcePath(coverImageFilename), GetDestinationPath(coverImageFilename)))
          File.Copy(GetSourcePath(coverImageFilename), GetDestinationPath(coverImageFilename));
        foreach (string difficultyFilename in difficultyFilenames)
        {
          if (File.Exists(GetSourcePath(difficultyFilename)))
            File.Copy(GetSourcePath(difficultyFilename), GetDestinationPath(difficultyFilename));
        }
      }

      string GetSourcePath(string file) => Path.Combine(sourceDirectoryPath, file);

      string GetDestinationPath(string file) => Path.Combine(destinationDirectoryPath, file);
    }

    public static bool CopyDirectory(
      string sourceDirectoryPath,
      string destinationDirectoryPath,
      bool copySubDirectories)
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(sourceDirectoryPath);
      if (!directoryInfo1.Exists)
      {
        Debug.LogError((object) ("Source directory does not exist or could not be found: " + sourceDirectoryPath));
        return false;
      }
      DirectoryInfo[] directories = directoryInfo1.GetDirectories();
      Directory.CreateDirectory(destinationDirectoryPath);
      foreach (FileInfo file in directoryInfo1.GetFiles())
      {
        string destFileName = Path.Combine(destinationDirectoryPath, file.Name);
        file.CopyTo(destFileName, false);
      }
      bool flag = true;
      if (copySubDirectories)
      {
        foreach (DirectoryInfo directoryInfo2 in directories)
        {
          string destinationDirectoryPath1 = Path.Combine(destinationDirectoryPath, directoryInfo2.Name);
          flag &= BeatmapFileUtils.CopyDirectory(directoryInfo2.FullName, destinationDirectoryPath1, true);
        }
      }
      if (!flag && Directory.Exists(destinationDirectoryPath))
        Directory.Delete(destinationDirectoryPath);
      return flag;
    }

    public static void CreateDirectoryHidden(string path) => Directory.CreateDirectory(path).Attributes |= FileAttributes.Hidden;

    public static void MakeDirectoryHidden(string directoryPath)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
      if (directoryInfo.Attributes.HasFlag((Enum) FileAttributes.Hidden))
        return;
      directoryInfo.Attributes |= FileAttributes.Hidden;
    }

    public static string GetDirectoryName(string directoryPath) => (int) directoryPath[directoryPath.Length - 1] == (int) Path.DirectorySeparatorChar ? Path.GetDirectoryName(directoryPath) : Path.GetFileName(directoryPath);

    public static DateTime GetDirectoryModifiedDateTime(string path) => Directory.GetLastWriteTime(path);

    private static bool IsDirectory(string path) => Directory.Exists(path) && (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;

    private static (string, string) GetParentPathAndDirectoryName(string beatmapFolderPath) => (int) beatmapFolderPath[beatmapFolderPath.Length - 1] == (int) Path.DirectorySeparatorChar ? (Directory.GetParent(beatmapFolderPath).FullName, Path.GetDirectoryName(beatmapFolderPath)) : (Path.GetDirectoryName(beatmapFolderPath), Path.GetFileName(beatmapFolderPath));

    private static bool CreateHardLink(string sourceFilePath, string destinationFilePath) => BeatmapFileUtils.CreateHardLink(destinationFilePath, sourceFilePath, IntPtr.Zero);

    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CreateHardLink(
      string lpFileName,
      string lpExistingFileName,
      IntPtr lpSecurityAttributes);
  }
}
