using UnityEngine;
using System.IO;
using System.Collections;
using SFB;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;

public class MusicImporter : MonoBehaviour, IMusicImporter
{
    public String MusicPath { get; set; }   
    public void ImportMusic()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Audio Files", "mp3", "wav", "ogg"),
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Виберіть музику", "", extensions, false);

        if (paths.Length == 0)
            return;

        string sourcePath = paths[0];
        string fileName = Path.GetFileName(sourcePath);

        string musicFolder = Path.Combine(Application.persistentDataPath, "Music");

        if (!Directory.Exists(musicFolder))
            Directory.CreateDirectory(musicFolder);

        string destPath = Path.Combine(musicFolder, fileName);

        File.Copy(sourcePath, destPath, true);

        Debug.Log("Музика скопійована: " + destPath);

        MusicPath = destPath;
    }

    public async Task<AudioClip> LoadAsync(string path)
    {
        string url = "file://" + path;

        using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            var op = req.SendWebRequest();

            while (!op.isDone)
                await Task.Yield();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Audio load error: " + req.error);
                return null;
            }

            return DownloadHandlerAudioClip.GetContent(req);
        }
    }
}
