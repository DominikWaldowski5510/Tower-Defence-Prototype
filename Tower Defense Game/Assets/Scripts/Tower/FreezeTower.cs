﻿using UnityEngine;

//handles freezing tower functionalities
public class FreezeTower : Tower
{
    [SerializeField] private LineRenderer lineRenderer = null;              //line that connects between tower and enemy
    private ApplyingDebuff debuff;                                          //debuff generated by the tower
    [SerializeField] private Transform endRayParticle = null;
    public LineRenderer LineRenderer { get => lineRenderer; }
    public ApplyingDebuff Debuff { get => debuff; }
    public Transform EndRayParticle { get => endRayParticle; }

    //sets up the debuff for this tower
    public virtual void Start()
    {
        endRayParticle.gameObject.SetActive(false);
        lineRenderer.enabled = false;
        debuff = new ApplyingDebuff();
        debuff.debuffDamage = 0;
        debuff.debuffDuration = 0.01f;
        debuff.typeOfDebuff = ApplyingDebuff.DebuffType.Freeze;
    }

    //handles how the freeze tower shoots, by drawing a line between enemy and itself and doing constant damage over time while applying debuff
    public override void Shoot()
    {
        if (Target != null)
        {
            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, Muzzle[0].transform.position);
            lineRenderer.SetPosition(1, Target.transform.position);
            endRayParticle.transform.position = Target.transform.position;
            endRayParticle.gameObject.SetActive(true);
            EnemyHealth health = Target.GetComponent<EnemyHealth>();
            health.TakeDamage(Damage * Time.deltaTime);
            health.DealDebuff(debuff);

        }
        else
        {
            lineRenderer.enabled = false;
            endRayParticle.gameObject.SetActive(false);
        }
    }

}