using UnityEngine;

//Handles interacting between option windows and panels
public class OptionsSettings : MonoBehaviour
{
    [SerializeField] private Transform[] optionPanels = null;               //stores all panels that change between video, sound and gameplay settings

    //sets video settings to be the default panel to show up
    private void Start()
    {
        ChangeSettings(0);
    }

    //handles the change of settings based on the number of the button that gets pressed (0 to 2 values)
    public void ChangeSettings(int optionNum)
    {
        ResetPanels();
        optionPanels[optionNum].gameObject.SetActive(true);
    }

    //sets all the panels to disabled 
    private void ResetPanels()
    {
        for (int i = 0; i < optionPanels.Length; i++)
        {
            optionPanels[i].gameObject.SetActive(false);
        }
    }
}
