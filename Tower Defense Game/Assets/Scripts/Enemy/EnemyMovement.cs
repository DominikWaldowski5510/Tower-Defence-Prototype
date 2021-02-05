using UnityEngine;

//Handles enemy movement as well as any debuffs that slow the movement of the enemy
public class EnemyMovement : MonoBehaviour
{
    private Transform[] pathToTravel;               //path that the enemy has to travel through which is taken from the map
    private int positionId;                     //current position id based on pathToTravel array
    private float speed = 1f;                    //the speed at which the enemy moves
    private Vector3 endPos;                   //The node that the player wants to move towards                  
    private float slowSpeed;                  //the value of the slow
    private float currentSpeed;                //how fast is the enemy moving at currently
    [SerializeField] private GameObject frozenDisplay = null;               //the 3d visual of the enemy being slowed down
    private EnemyHealth health;                                     //reference to the health class that takes in debuffs and damage

    //sets up the enemy when its enabled, by setting up the path and all start up values such as speed
    private void OnEnable()
    {
        if (health == null)
            health = this.gameObject.GetComponent<EnemyHealth>();
        positionId = 0;
        pathToTravel = WaveManager.instance.GetPath();
        ChangeNode();
        speed = health.Enemy.speed;
        currentSpeed = speed;
        slowSpeed = speed / 2;
        frozenDisplay.SetActive(false);
    }

    //controls the movement of our character by moving it from node to node
    private void Update()
    {
        //Checks if the debuff freeze is currently applied to the enemy if it is changes the speed to slow speed
        bool hasFreeze = false;
        for (int i = 0; i < health.AppliedDebuffs.Count; i++)
        {
            if (health.AppliedDebuffs[i].typeOfDebuff == ApplyingDebuff.DebuffType.Freeze)
            {
                hasFreeze = true;
            }
        }
        if (hasFreeze == false)
        {
            currentSpeed = speed;
            frozenDisplay.SetActive(false);
        }
        else if (hasFreeze == true)
        {
            currentSpeed = slowSpeed;
            frozenDisplay.SetActive(true);
        }


        //Checks if the enemy has reached the end of the path if so it destroys the enemy and player loses a health
        if (positionId + 1 >= pathToTravel.Length)
        {
            GameManager.instance.UpdatePlayerHealth();
            Destroy(this.gameObject);
            return;
        }

        //Moves the enemy by checking how far the enemy is from the current node point 
        if (Vector3.Distance(this.transform.position, pathToTravel[positionId + 1].transform.position) < 0.1f)
        {
            positionId++;           //add to travel to next node
            if (positionId < pathToTravel.Length - 1)
            {
                ChangeNode();
            }
        }
        else
        {
            Vector3 direction = endPos - this.transform.position;
            transform.Translate(direction.normalized * currentSpeed * Time.deltaTime, Space.World);
        }
    }

    //Sets new node for movement when the player gets close enough to the node
    private void ChangeNode()
    {
        endPos = pathToTravel[positionId + 1].transform.position;
    }
}
