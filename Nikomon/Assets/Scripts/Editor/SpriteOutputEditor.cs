using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class SaveSprites
{
    [MenuItem("Tools/Generate Sprites")]
    static void SaveSprite()
    {
        string resourcesPath = "Assets/Resources/Sprites";
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/BagUIIcon");
        Debug.Log("Start to generate sprites");
        if (sprites.Length > 0)
        {
            string outPath = resourcesPath+"/BagIcons";
            
            // System.IO.Directory.CreateDirectory(outPath);
            foreach (Sprite sprite in sprites)
            {
                // if (!Regex.IsMatch(sprite.name, @"^\d")) continue; //判断是否是以数字开头
                Texture2D tex = new Texture2D((int) sprite.rect.width, (int) sprite.rect.height,
                    sprite.texture.format, false);
                tex.SetPixels(sprite.texture.GetPixels((int) sprite.rect.xMin, (int) sprite.rect.yMin,
                    (int) sprite.rect.width, (int) sprite.rect.height));
                tex.Apply();
                System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
            }

            Debug.Log("SaveSprite to " + outPath);
        }

        Debug.Log("SaveSprite Finished");
    }
}