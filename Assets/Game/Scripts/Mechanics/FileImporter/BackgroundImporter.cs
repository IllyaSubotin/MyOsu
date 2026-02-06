using UnityEngine;
using System.IO;
using System.Collections;
using SFB; // StandaloneFileBrowser
using UnityEngine.UI;

public class BackgroundImporter : MonoBehaviour, IBackgroundImporter
{
    public string BackgroundPath { get; set; }
    public void ImportBackground()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            "Виберіть зображення",
            "",
            extensions,
            false
        );

        if (paths.Length == 0)
            return;

        string sourcePath = paths[0];
        string fileName = Path.GetFileName(sourcePath);

        string folder = Path.Combine(Application.persistentDataPath, "Backgrounds");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string destPath = Path.Combine(folder, fileName);

        File.Copy(sourcePath, destPath, true);

        PlayerPrefs.SetString("bg_path", destPath);
        PlayerPrefs.Save();

        Debug.Log("Background скопійовано: " + destPath);
        BackgroundPath = destPath;
    }

    public Sprite LoadBackground(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }
}
