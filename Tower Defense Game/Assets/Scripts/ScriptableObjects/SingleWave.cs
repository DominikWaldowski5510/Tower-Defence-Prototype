using UnityEngine;

//stores the enemies within a wave
[CreateAssetMenu(fileName = "New Wave", menuName = "Waves/New Wave", order = 0)]
public class SingleWave : ScriptableObject
{
    public GameObject[] enemiesToSpawn;
    public float spawnBetween = 0.5f;
}
