using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform icon;
    [SerializeField] string sceneName;

    private void Update()
    {
        icon?.Rotate(0, 0, 1f);
    }
    private void Awake()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        if (op.isDone)
        {
            op.allowSceneActivation = true;
        }
        else
        {
            text.text = $"LOADING {op.progress*100}%";
            yield return null;
        }
    }
}
