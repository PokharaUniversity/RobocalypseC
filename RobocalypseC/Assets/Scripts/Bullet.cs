//Author @ ANUJ SHRESTHA


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float bulletSpeed;
	private Rigidbody bulletRB;
	public float damage;
    public float BulletLife;
    public bool FromPlayer;
    public float healthDamageRatio;
    public float ShieldDamageRatio;




	void Start () {
		bulletRB = gameObject.GetComponent<Rigidbody> ();
		Vector3 bulletDirection = new Vector3 (0,0,bulletSpeed);
		bulletDirection = gameObject.transform.rotation*bulletDirection;
		bulletRB.AddForce (bulletDirection, ForceMode.Force);
        Destroy(gameObject, BulletLife);
	}
	void OnCollisionEnter(Collision col){
        if ((col.gameObject.tag == "Enemy" && FromPlayer)||(col.gameObject.tag=="Player"&&!FromPlayer)) {
            col.gameObject.GetComponent<HealthAndShield>().damage(damage, healthDamageRatio, ShieldDamageRatio);
        }
        destroy();
	}

    public void destroy()
    {
        Destroy(gameObject);
    }


}
