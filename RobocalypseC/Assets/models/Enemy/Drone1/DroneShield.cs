using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneShield : MonoBehaviour
{
    public gameManager gameManager;
    public GameObject Player;
    public float AICalculateTime;
    public float speed;
    public float detectRange;
    public float MaxShieldStrength;
    public float shieldStrength;
    public float effectRange;
    public float ShieldRechargeDelay;
    public droneOuterShield outerShield;
    private GameObject Target;
    private Rigidbody Rb;
    public float ShieldRestoreRate;
    public LineRenderer HealLineRenderer;
    public Transform EnergyPoint;
    
    // Start is called before the first frame update
    private void Awake(){
        Player = gameManager.Player;
        shieldStrength = MaxShieldStrength;
        outerShield = GetComponentInChildren<droneOuterShield>();
        outerShield.ShieldOf = this;
        Rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        StartCoroutine(AIBurst());
        HealLineRenderer.SetPosition(1, EnergyPoint.position);
        HealLineRenderer.SetPosition(0, EnergyPoint.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HealLineRenderer.SetPosition(0, EnergyPoint.position);
        if (GetComponent<HealthAndShield>().IsAlive)
        {
            if (Target != null && Target.GetComponent<HealthAndShield>().IsAlive)
            {
                if (Vector3.Distance(Target.transform.position, gameObject.transform.position) > effectRange)
                {
                    moveToTarget();
                }
                else
                {
                    heal();
                }

            }
            else
            {
                seekTarget();
            }
        }
        else {
            HealLineRenderer.SetPosition(1, EnergyPoint.position);
        }
    }
    void heal() {
        Target.GetComponent<HealthAndShield>().recharge(0, ShieldRestoreRate * Time.fixedDeltaTime);
        HealLineRenderer.SetPosition(1, Target.transform.position);
    }
    void moveToTarget() {
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        Rb.AddForce(direction * speed*Time.fixedDeltaTime);
        HealLineRenderer.SetPosition(1, EnergyPoint.position);
    }
  

    IEnumerator AIBurst() {
        while (GetComponent<HealthAndShield>().IsAlive)
        {
            AICalculate();
            yield return new WaitForSeconds(AICalculateTime);
        }
    }

    void AICalculate() {
        if (Target != null)
        {
            HealthAndShield TargetShield = Target.GetComponent<HealthAndShield>();
            if (TargetShield.Shield >= TargetShield.MaxShield)
            {
                seekTarget();
            }
        }
        else {
            seekTarget();
        }
       
    }

    public void DamageOuterShield(float  damage) {
        shieldStrength -= damage;
        if (shieldStrength <= 0) {
            outerShield.gameObject.SetActive(false);
            StartCoroutine(shieldRecharge());
        }
    }

    IEnumerator shieldRecharge()
    {

        yield return new WaitForSeconds(ShieldRechargeDelay);
        if (GetComponent<HealthAndShield>().IsAlive) { 
            shieldStrength = MaxShieldStrength;
            outerShield.gameObject.SetActive(true);
        }
    }

    void seekTarget() {
        HealLineRenderer.SetPosition(1, EnergyPoint.position);
        //Debug.Log("seekTarget");
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRange);
        foreach (Collider collider in colliders) {
            if (collider.tag == "Enemy") {
                HealthAndShield colHealthAndShield = collider.gameObject.GetComponent<HealthAndShield>();
                if (colHealthAndShield.Shield < colHealthAndShield.MaxShield&&colHealthAndShield.IsAlive) {
                    Target = colHealthAndShield.gameObject;
                  
                }
            }

        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position, effectRange);
    }
}
