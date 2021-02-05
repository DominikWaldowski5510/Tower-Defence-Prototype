using UnityEngine;


//displays a circle around a selected tower or an enemy
public class SelectableObject : MonoBehaviour
{
    [SerializeField] private GameObject selectableObject = null;            //the gameobjec that stores a UI element of a circle selection


    //sets the object to be disabled by default
    private void Start()
    {
        OnDeselectObject();
    }

    //on mouse click enables the button
    public void OnSelectObject()
    {
        selectableObject.SetActive(true);
    }

    //on right mouse click or any other interaction disables the object
    public void OnDeselectObject()
    {
        selectableObject.SetActive(false);
    }
}
