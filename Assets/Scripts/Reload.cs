using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Reload : MonoBehaviour
{
#if UNITY_EDITOR
    [HideInInspector] [SerializeField] private bool isCompiling;
    [HideInInspector] [SerializeField] private List<string> currentScenes;

    private static bool instanced;

    public void OnEnable()
    {
        if (instanced)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instanced = true;
    }

    public void Update()
    {
        if (isCompiling == false && EditorApplication.isCompiling)
        {
            Debug.Log($"{EditorApplication.timeSinceStartup:0.00} compile start");
            isCompiling = true;

            currentScenes = new List<string>();
            for (var x = 0; x < SceneManager.sceneCount; x++)
            {
                currentScenes.Add(SceneManager.GetSceneAt(x).name);
            }
        }

        if (isCompiling && !EditorApplication.isCompiling)
        {
            SceneManager.LoadScene(currentScenes[0]);
            foreach (var scene in currentScenes.Skip(1))
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            }

            Debug.Log($"{EditorApplication.timeSinceStartup:0.00} compile finish");
            isCompiling = false;
        }
    }
#endif
}
