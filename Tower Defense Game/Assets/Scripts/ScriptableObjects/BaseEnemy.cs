using UnityEngine;

//scriptable object of the enemy within the project
[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/New Enemy", order = 1)]
public class BaseEnemy : ScriptableObject
{
    public string enemyName;        //the name assigned to the enemy
    public float health;            //how much health the enemy has left
    public float speed;             //how fast the enemy moves
    public Sprite enemyIcon;        //image of the enemy
    public int value;               //how much the unit is worth after death
}
