using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadOfflineLevel : MonoBehaviour
{
    public Slider progressBar; // Прогресс-бар для отображения загрузки

    public GameObject objToVisible;

    public void LoadLevel(string levelName)
    {
        objToVisible.SetActive(true);

        StartCoroutine(LoadLevelAsync(levelName));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f; // Прогресс загрузки от 0 до 1
            progressBar.value = progress;

            Debug.Log($"progress: {progress}");

            yield return null;
        }
    }
}
