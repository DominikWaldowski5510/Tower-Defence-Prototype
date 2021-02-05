using UnityEngine;

//placed spikes on road damage enemy who steps son it but get destroyed in the process
public class RoadSpike : MonoBehaviour
{
    [SerializeField] private BaseTower spike = null;                //reference to object that stores damage of the spike

    public BaseTower Spike { get => spike; }

    //called on collision enter between enemy and spike object
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyHealth>().TakeDamage(spike.towerTier[0].damage);
            this.gameObject.SetActive(false);
        }
    }
}
