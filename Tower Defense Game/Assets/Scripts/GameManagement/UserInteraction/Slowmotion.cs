using UnityEngine;
using UnityEngine.UI;

//controls the speed at which the enemies move and towers fire
public class Slowmotion : MonoBehaviour
{
    [SerializeField]
    private Slider slowSlider = null;                                 //reference to slider on the UI
    private float slowFactor = 1f;                  //value that handles speed of the game

    //initializes the slow factor value to default
    private void Start()
    {
        slowFactor = 1f;
        UpdateSlider();
    }

    //runs every frame
    private void Update()
    {
        Time.timeScale = slowFactor;
        UpdateTime();
    }

    //sets the time to our slow factor
    private void TimeManager()
    {
        Time.timeScale = slowFactor;
    }

    //sets our slow factor based on the slider vlaue
    public void OnSliderModified()
    {
        slowFactor = slowSlider.value;
    }

    //handles right and left arrow inputs as well as updates the time 
    private void UpdateTime()
    {
        if (PauseMenu.instance.IsPaused == false)
        {
            Time.timeScale = slowFactor;
        }
        else if (PauseMenu.instance.IsPaused == true)
        {
            Time.timeScale = 0;
        }
    }


    //updates the value of a slider
    private void UpdateSlider()
    {
        slowSlider.value = slowFactor;
    }
}
