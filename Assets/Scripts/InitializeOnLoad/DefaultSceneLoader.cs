#if UNITY_EDITOR
using System.Reflection;
using CoinPackage.Debugging;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace InitializeOnLoad {
    [InitializeOnLoad]
    public static class DefaultSceneLoader
    {
        static DefaultSceneLoader(){
            SceneAsset entryScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/Entry.unity");
            if (entryScene != null)
                EditorSceneManager.playModeStartScene = entryScene;
            else
                CDebug.Log("Entry scene could not be found.");
        }
    }   
}
#endif
