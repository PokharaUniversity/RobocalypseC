using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAction : MonoBehaviour
{
    private bool isOff;
    public bool isChanged;
    // Start is called before the first frame update
    void Start()
    {
        isOff = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isChanged){
            if(isOff){
                Debug.Log("Close");
            } else {
                Debug.Log("Open");
            }
            isChanged = false;
        }
    }

    public void change(){
        isOff = !isOff;
    }
}
