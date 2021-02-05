using UnityEngine;
using UnityEngine.UI;

//displays useful information about things user hovers over on
public class Tooltips : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPanel = null;            //stores the tooltip panel
    [SerializeField] private Text tooltipText = null;                   //stores the text that displays tool tips

    //initialises the boxes
    private void Start()
    {
        HideToolTip();
    }

    //displays the tool tip for the user
    public void DisplayTooltip(string tip)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = tip;
    }

    //hides the tooltip from the user
    public void HideToolTip()
    {
        tooltipText.text = "";
        tooltipPanel.SetActive(false);
    }
}
