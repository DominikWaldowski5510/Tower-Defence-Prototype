using UnityEngine;

//Class that creates spike bullets that fly in various directions in a straight line only
public class SpikeBullet : Bullet
{
    private Transform startPos;         //start postion of the bullet

    //values when the bullet is enabled also inheriting from base bullet
    public override void OnEnable()
    {
        base.OnEnable();
        startPos = this.transform;
    }

    //disabling the base bullet features
    public override void OnDisable()
    {
        base.OnDisable();
    }

    //The way the bullet moves
    public override void BulletTravel()
    {
        startPos.position = transform.position + BulletSpeed * Time.deltaTime * transform.forward;
    }

}
