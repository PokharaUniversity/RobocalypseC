using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneOuterShield : MonoBehaviour
{
    public DroneShield ShieldOf;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Bullet"){
            Bullet bulletScript = collider.gameObject.GetComponent<Bullet>();
            if (bulletScript.FromPlayer) {
                ShieldOf.DamageOuterShield(bulletScript.damage * bulletScript.ShieldDamageRatio);
                bulletScript.destroy();
            }
        }
    }
}
