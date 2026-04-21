using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using Unity.VisualScripting;
public class TexturePostProcessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if (assetPath.Contains("Assets/Media/HeroesImg/"))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            textureImporter.mipmapEnabled = false;
        }
    }
}
