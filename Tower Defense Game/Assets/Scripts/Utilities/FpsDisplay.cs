using UnityEngine;
using UnityEngine.UI;

//Displays current fps in a text 
public class FpsDisplay : MonoBehaviour
{
    public Text fpsText;                    //text field that displays the fps
    private float deltaTime;                //float used for calculating the fps

    //calculates the fps of the project and displays it in a text
    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }
}
