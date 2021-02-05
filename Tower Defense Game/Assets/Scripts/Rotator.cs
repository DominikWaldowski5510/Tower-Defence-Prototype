using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float  rotationValueX = 45, rotationValueY = 0, rotationValueZ = 0;
    private void Update()
    {
        this.transform.Rotate(rotationValueX , rotationValueY * Time.deltaTime, rotationValueZ * Time.deltaTime);
    }
}
