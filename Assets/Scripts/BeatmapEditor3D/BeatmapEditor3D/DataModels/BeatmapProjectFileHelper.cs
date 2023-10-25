// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapProjectFileHelper
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.SerializedData;
using BeatmapEditor3D.SerializedData.Bpm;
using BeatmapSaveDataVersion3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BeatmapEditor3D.DataModels
{
  public static class BeatmapProjectFileHelper
  {
    public const int kMaxBackupCount = 10;

    public static string[] GetProjectDirectories(string parentDirectory) => BeatmapProjectFileHelper.GetProjectDirectoriesRecursively(parentDirectory).ToArray<string>();

    public static IEnumerable<string> GetProjectDirectoriesRecursively(string parentDirectory)
    {
      IEnumerable<string> strings = Enumerable.Empty<string>();
      return File.Exists(Path.Combine(parentDirectory, "Info.dat")) ? strings.Append<string>(parentDirectory) : ((IEnumerable<string>) Directory.GetDirectories(parentDirectory)).Where<string>((Func<string, bool>) (path => !BeatmapFileUtils.GetDirectoryName(path).StartsWith("~"))).Aggregate<string, IEnumerable<string>>(strings, (Func<IEnumerable<string>, string, IEnumerable<string>>) ((current, validFolder) => current.Concat<string>(BeatmapProjectFileHelper.GetProjectDirectoriesRecursively(validFolder))));
    }

    public static bool ProjectTempExists(string projectPath) => Directory.Exists(BeatmapFileUtils.GetTempBeatmapDirectoryPath(projectPath));

    public static void CopyProjectToTemp(string originalProjectPath, out string tempProjectPath)
    {
      string beatmapDirectoryPath = BeatmapFileUtils.GetTempBeatmapDirectoryPath(originalProjectPath);
      if (Directory.Exists(beatmapDirectoryPath))
        Directory.Delete(beatmapDirectoryPath, true);
      StandardLevelInfoSaveData info = BeatmapProjectFileHelper.LoadProjectInfo(originalProjectPath);
      BeatmapFileUtils.CopyBeatmapProject(originalProjectPath, beatmapDirectoryPath, info.songFilename, info.coverImageFilename, BeatmapProjectFileHelper.GetDifficultyBeatmapNames(info));
      BeatmapFileUtils.MakeDirectoryHidden(beatmapDirectoryPath);
      tempProjectPath = beatmapDirectoryPath;
    }

    public static Version GetVersionedJSONVersion(string projectPath, string filePath)
    {
      VersionSerializedData versionSerializedData = BeatmapProjectFileHelper.LoadBeatmapJsonObject<VersionSerializedData>(projectPath, filePath);
      return versionSerializedData != null ? new Version(versionSerializedData.v) : (Version) null;
    }

    public static void LoadProjectBackups(
      string projectPath,
      ref Queue<string> backupQueue,
      ref string lastBackup)
    {
      string beatmapDirectory = BeatmapFileUtils.GetParentBackupBeatmapDirectory(projectPath);
      if (!Directory.Exists(beatmapDirectory))
        return;
      string[] array = ((IEnumerable<string>) Directory.GetDirectories(beatmapDirectory)).ToArray<string>();
      Array.Sort<string>(array);
      foreach (string str in array)
        backupQueue.Enqueue(str);
      lastBackup = array[array.Length - 1];
    }

    public static T LoadBeatmapJsonObject<T>(string projectPath, string filename) where T : class
    {
      string str = Path.Combine(projectPath, filename);
      return !File.Exists(str) ? default (T) : FileHelpers.LoadFromJSONFile<T>(str);
    }

    public static StandardLevelInfoSaveData LoadProjectInfo(string projectPath) => BeatmapProjectFileHelper.LoadBeatmapJsonObject<StandardLevelInfoSaveData>(projectPath, "Info.dat");

    public static string[] GetDifficultyBeatmapNames(StandardLevelInfoSaveData info) => ((IEnumerable<StandardLevelInfoSaveData.DifficultyBeatmapSet>) info.difficultyBeatmapSets).Aggregate<StandardLevelInfoSaveData.DifficultyBeatmapSet, List<string>>(new List<string>(), (Func<List<string>, StandardLevelInfoSaveData.DifficultyBeatmapSet, List<string>>) ((acc, difficultyBeatmapSet) =>
    {
      acc.AddRange(((IEnumerable<StandardLevelInfoSaveData.DifficultyBeatmap>) difficultyBeatmapSet.difficultyBeatmaps).Select<StandardLevelInfoSaveData.DifficultyBeatmap, string>((Func<StandardLevelInfoSaveData.DifficultyBeatmap, string>) (difficultyBeatmap => difficultyBeatmap.beatmapFilename)));
      return acc;
    })).ToArray();

    public static T LoadBeatmapLevel<T>(string projectPath, string beatmapLevelFilename) where T : class => BeatmapProjectFileHelper.LoadBeatmapJsonObject<T>(projectPath, beatmapLevelFilename);

    public static void SaveSettings(BeatmapEditorSettingsSerializedData settings)
    {
      string settingsFilePath = BeatmapFileUtils.beatmapEditorSettingsFilePath;
      FileHelpers.SaveToJSONFile((object) settings, settingsFilePath, true);
    }

    public static void SaveProjectInfo(string projectPath, StandardLevelInfoSaveData info)
    {
      string filePath = Path.Combine(projectPath, "Info.dat");
      FileHelpers.SaveToJSONFile((object) info, filePath, true);
      Directory.SetLastWriteTime(projectPath, DateTime.Now);
    }

    public static void SaveBpmInfo(string projectPath, BpmInfoSerializedDataV2 bpmInfo)
    {
      string filePath = Path.Combine(projectPath, "BPMInfo.dat");
      FileHelpers.SaveToJSONFile((object) bpmInfo, filePath, true);
      Directory.SetLastWriteTime(projectPath, DateTime.Now);
    }

    public static (string, string) SaveBeatmapFile(
      string currentFilePath,
      string projectPath,
      string newFilePath)
    {
      if (string.IsNullOrEmpty(newFilePath) || !File.Exists(newFilePath))
        return ((string) null, (string) null);
      if (!string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath))
        File.Delete(currentFilePath);
      string fileName = Path.GetFileName(newFilePath);
      string destFileName = Path.Combine(projectPath, fileName);
      if (newFilePath != destFileName)
        File.Copy(newFilePath, destFileName);
      return (fileName, destFileName);
    }

    public static void DeleteBeatmapFile(string projectPath, string beatmapFilename) => File.Delete(Path.Combine(projectPath, beatmapFilename));

    public static void CopyBeatmapFile(
      string sourceProjectPath,
      string destinationProjectPath,
      string beatmapFileFilename)
    {
      File.Copy(Path.Combine(sourceProjectPath, beatmapFileFilename), Path.Combine(destinationProjectPath, beatmapFileFilename));
    }

    public static void SaveBeatmapLevel(
      string projectPath,
      string beatmapLevelFilename,
      BeatmapSaveData beatmapSaveData)
    {
      string filePath = Path.Combine(projectPath, beatmapLevelFilename);
      FileHelpers.SaveToJSONFile((object) beatmapSaveData, filePath);
      Directory.SetLastWriteTime(projectPath, DateTime.Now);
    }

    public static void DeleteBeatmapLevel(string projectPath, string beatmapLevelFilename) => File.Delete(Path.Combine(projectPath, beatmapLevelFilename));

    public static void CopyBeatmapLevel(
      string fromProjectPath,
      string fromBeatmapLevelFilename,
      string toProjectPath,
      string toBeatmapLevelFilename)
    {
      File.Copy(Path.Combine(fromProjectPath, fromBeatmapLevelFilename), Path.Combine(toProjectPath, toBeatmapLevelFilename), true);
    }

    public static void CopyProjectFile(string fromProjectPath, string toProjectPath) => File.Copy(Path.Combine(fromProjectPath, "Info.dat"), Path.Combine(toProjectPath, "Info.dat"), true);

    public static void DeleteBackup(string backupPath) => Directory.Delete(backupPath, true);

    public static (string path, string filename) GetCorrectedPathAndFilename(
      string projectPath,
      string filename)
    {
      string path = Path.Combine(projectPath, filename);
      return !File.Exists(path) ? ((string) null, (string) null) : (path, filename);
    }

    public static void BackupProject(
      string projectPath,
      ref Queue<string> projectBackups,
      out string lastBackup)
    {
      string beatmapDirectory1 = BeatmapFileUtils.GetParentBackupBeatmapDirectory(projectPath);
      if (!Directory.Exists(beatmapDirectory1))
        BeatmapFileUtils.CreateDirectoryHidden(beatmapDirectory1);
      string beatmapDirectory2 = BeatmapFileUtils.GetBackupBeatmapDirectory(projectPath, beatmapDirectory1);
      Directory.Move(projectPath, beatmapDirectory2);
      projectBackups.Enqueue(beatmapDirectory2);
      lastBackup = beatmapDirectory2;
    }

    public static void CopyTempToProject(string tempProjectPath, string originalProjectPath)
    {
      StandardLevelInfoSaveData info = BeatmapProjectFileHelper.LoadProjectInfo(tempProjectPath);
      BeatmapFileUtils.CopyBeatmapProject(tempProjectPath, originalProjectPath, info.songFilename, info.coverImageFilename, BeatmapProjectFileHelper.GetDifficultyBeatmapNames(info));
    }

    public static void DeleteTempDirectory(string tempProjectPath)
    {
      if (!Directory.Exists(tempProjectPath))
        return;
      Directory.Delete(tempProjectPath, true);
    }

    public static bool FileExists(string projectPath, string filename) => !string.IsNullOrEmpty(filename) && File.Exists(Path.Combine(projectPath, filename));

    public static string GetInvalidDirectoryNameCharacters() => " " + new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

    public static Regex GetInvalidDirectoryNameRegex() => new Regex("[" + Regex.Escape(BeatmapProjectFileHelper.GetInvalidDirectoryNameCharacters()) + "]");

    public static string CreateSafeDirectoryPath(string projectsRoot, string name)
    {
      string str = BeatmapProjectFileHelper.GetInvalidDirectoryNameRegex().Replace(name, "");
      string path = Path.Combine(projectsRoot, str);
      int count = 0;
      while (Directory.Exists(path))
      {
        path = Path.Combine(projectsRoot, BeatmapFileUtils.GetBeatmapDirectoryDuplicateName(str, count));
        ++count;
      }
      return path;
    }
  }
}
