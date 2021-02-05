using UnityEngine;
using UnityEngine.SceneManagement;

//Allows for movement with camera using WASD keys
public class CameraController : MonoBehaviour
{
    private float panSpeed = 4;                                //speed of moving the camera
    private Vector2 panLimit = new Vector2(10, 10);            //limits of the map terrain

    //sets the camera position based on active scene and height and width settings of the map
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            MapGenerator mapGen = GameObject.Find("Map").GetComponent<MapGenerator>();
            Camera.main.transform.position = new Vector3(mapGen.Width / 2
                , Camera.main.transform.position.y,
                mapGen.Height / 4);
        }
        else
        {
            Camera.main.transform.position = new Vector3(MapEditorGenerator.instance.Width / 2
                , Camera.main.transform.position.y,
                MapEditorGenerator.instance.Height / 2);
        }
    }

    //updates camera movement based on vertical and horizontal input using unitys input system
    private void Update()
    {
        Vector3 pos = Camera.main.transform.position;

        if (Input.GetAxis("Vertical") > 0)
        {
            pos.z += panSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            pos.z -= panSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            pos.x += panSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            pos.x -= panSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pos.x = MapEditorGenerator.instance.Width / 2;
            pos.z = MapEditorGenerator.instance.Height / 2;
        }

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        Camera.main.transform.position = pos;
    }
}
