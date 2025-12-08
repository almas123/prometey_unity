using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class DefaultSceneLoader
{
    private const string DEFAULT_SCENE_PATH = "Assets/Scenes/SampleScene.unity";

    static DefaultSceneLoader()
    {
        EditorApplication.delayCall += LoadDefaultScene;
    }

    private static void LoadDefaultScene()
    {
        if (EditorSceneManager.GetActiveScene().path == "")
        {
            EditorSceneManager.OpenScene(DEFAULT_SCENE_PATH);
        }
    }
}
