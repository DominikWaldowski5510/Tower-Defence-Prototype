using UnityEngine;

//Checks if we can place a tower 
public static class AreaCheck
{
    private static float radius = 0.2f;                 //the radius of placement(how big of a gap between the 2 objects there has to be
    private static bool canPlace = true;                //value that allows or doesnt allow new tower placement


    //Checks for any colliders, if there are any in our radius returns false
    public static bool CheckRadius(Transform passObject, string ground)
    {
        canPlace = true;
        Collider[] hitColliders = Physics.OverlapSphere(passObject.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag != ground)//Tile
            {
                canPlace = false;
            }
            i++;
        }
        return canPlace;
    }
}
