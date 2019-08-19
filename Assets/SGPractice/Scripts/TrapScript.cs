using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 나중에 쿨다운 필요할 수 있음.

        if (other.CompareTag("Player") && !GameManager.gm.isTriggered)
        {
            GameManager.gm.Alert();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
