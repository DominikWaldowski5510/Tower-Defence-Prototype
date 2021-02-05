using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//script that loads our game in second window
public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Text loadingText = null;              //text that displays how much we loaded % wise
    [SerializeField] private Image loadingImage = null;             //image that displays how much has loaded so far

    //button which initiates loading sequence
    public void LoadButton()
    {
        StartCoroutine(LoadScene());
    }

    //corutine that handles loading of a scene
    private IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {

            loadingText.text = "Loading: " + (asyncOperation.progress * 100) + "%";
            loadingImage.fillAmount = asyncOperation.progress / 1;
            if (asyncOperation.progress >= 0.9f)
            {
                loadingText.text = "Press Space to Continue";
                loadingImage.fillAmount = 1;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }
            yield return null;
        }

    }
}
