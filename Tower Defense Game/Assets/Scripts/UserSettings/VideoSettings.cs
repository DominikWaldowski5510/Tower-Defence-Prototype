using System;
using UnityEngine;
using UnityEngine.UI;

//Users video settings for resolution and screen size
public class VideoSettings : MonoBehaviour
{
    //preset resolutions available for the user to choose from in height and width
    private int[] height = new int[8] { 1080, 1024, 900, 1024, 768, 768, 600, 480 };
    private int[] width = new int[8] { 1920, 1600, 1600, 1280, 1280, 1024, 800, 640 };
    [SerializeField] private Text resolutionText = null;                //The text that displays what resolution user can choose from        
    private int resolutionIteration;                                    //iteration of the resolution based on height and width array
    [SerializeField] private Toggle fullScreenToggle = null;            //toggle button that is responsible for switching fullscreen on and off
    private bool isFullScreen;                                          //a value that is attached to the toggle selection used for saving the state of fullscreen

    //sets up the video settings default states
    private void Start()
    {
        isFullScreen =
        Convert.ToBoolean(SaveDataManager.instance.IsFullScreen);
        resolutionIteration = (int)SaveDataManager.instance.ResolutionIndex;
        fullScreenToggle.isOn = isFullScreen;
        SetResolutionText();

    }

    //Sets the next resolution from the array of available resolutions
    public void NextResolution()
    {
        resolutionIteration++;
        if (resolutionIteration > width.Length - 1)
        {
            resolutionIteration = 0;
        }
        SetResolutionText();
    }

    //sets the previous resolution fro mthe array of available resolutions
    public void PreviousResolution()
    {
        resolutionIteration--;
        if (resolutionIteration <= -1)
        {
            resolutionIteration = height.Length - 1;
        }
        SetResolutionText();
    }

    //sets the text that shows what the current resolution is
    private void SetResolutionText()
    {
        resolutionText.text = width[resolutionIteration] + "x" + height[resolutionIteration];
    }

    //sets the fullscreen bool to match the toggle when the toggle gets interacted by  the user
    public void OnToggleChange()
    {
        isFullScreen = fullScreenToggle.isOn;
    }

    //confirms the changes to the screen resolution and screen size
    public void ConfirmVideoChanges()
    {
        if (isFullScreen)
        {
            Screen.SetResolution(width[resolutionIteration], height[resolutionIteration], FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(width[resolutionIteration], height[resolutionIteration], FullScreenMode.Windowed);
        }
        SaveDataManager.instance.SetScreenOptions(Convert.ToInt32(isFullScreen), resolutionIteration);
        SaveDataManager.instance.Save();
    }
}
