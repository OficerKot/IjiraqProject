using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float speed = 2f;
    private void Update()
    {
        if(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * speed;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


}
