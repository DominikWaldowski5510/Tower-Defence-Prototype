using System.Collections.Generic;
using UnityEngine;

//Stores default levels existing within the project
[CreateAssetMenu(fileName = "New Level", menuName = "Level/New Level", order = 0)]
public class Level : ScriptableObject
{
    public int height, width;               //height and width of the level
    public List<int> blockIds = new List<int>();            //stores the blocks of the level
    public List<int> path = new List<int>();                //stores the path for the enemies

}
