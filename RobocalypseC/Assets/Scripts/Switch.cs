using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private bool isOff;
    public GameObject switchAction;
    // Start is called before the first frame update
    void Start()
    {
        isOff = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void trigger(){
        isOff = !isOff;
        switchAction.GetComponent<SwitchAction>().change();
        switchAction.GetComponent<SwitchAction>().isChanged = true;
    }
}
