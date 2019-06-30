using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SwitchHealthAndShield : HealthAndShield {
    public void death(){
        this.GetComponent<Switch>().trigger();
    }
}