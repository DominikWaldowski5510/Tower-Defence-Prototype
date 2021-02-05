using UnityEngine;

//makes the enemy death angel 3d visual fly upward to showcase his passing(being killed)
public class EnemyDeath : MonoBehaviour
{
    private Transform startPos = null;              //starting position of the living enemy
    private float speed = 3;                        //how fast the enemy flies to the sky

    //triggers the object being destroyed after half a second
    private void OnEnable()
    {
        startPos = this.transform;
        Invoke("DestroyObj", 0.5f);
    }

    //disables the trigger when this object becomes disabled
    private void OnDisable()
    {
        CancelInvoke("DestroyObj");
    }

    //sets the object to inactive so we can re use it later using the pooling script
    private void DestroyObj()
    {
        gameObject.SetActive(false);
    }

    //makes the object move upwards based on the speed value
    private void Update()
    {
        startPos.transform.position = transform.position + speed * Time.deltaTime * transform.up;
    }
}
