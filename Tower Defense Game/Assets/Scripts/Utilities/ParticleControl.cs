using UnityEngine;

//controls particle effects
public class ParticleControl : MonoBehaviour
{
    [SerializeField]
    private bool isMain = true;             //checks if this is the parent object of particle system

    //set the time to 0 so we start at beginning of particle each time it is enabled, we invoke disabling of particle if its parent particle
    private void OnEnable()
    {
        this.gameObject.GetComponent<ParticleSystem>().time = 0;
        if (isMain == true)
        {
            Invoke("ParticleDisable", 2);
        }
    }

    //cancels invoke not to trigger it again when particle gets enabled
    private void OnDisable()
    {
        CancelInvoke("ParticleDisable");
    }

    //disables the particle
    private void ParticleDisable()
    {
        this.gameObject.SetActive(false);
    }
}
