using UnityEditor;
using UnityEngine;

namespace KMTool
{
    public class AssetsImportSetting : AssetPostprocessor
    {

        public void OnPreprocessModel()
        {
            ModelImporter modelImporte = (ModelImporter)assetImporter;
            modelImporte.animationType = ModelImporterAnimationType.Legacy;

            DeubgToConsole("model ", modelImporte);
        }


        public void OnPreprocessTexture()
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;

            Debug.Log(" texture is import " + textureImporter.name, textureImporter);
        }

        void DeubgToConsole(string title, ModelImporter modelImporte)
        {
            Debug.Log(title + " is import " + modelImporte.name, modelImporte);

        }
    }
}