using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCeilScript : MonoBehaviour
{
    Material matt;
    public float t;
    public float limit = 0.5f;
    float temp;
    public bool ceilTrigger = false;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        matt = gameObject.GetComponent<Renderer>().material;
        matt.SetFloat("Vector1_1264ABDA", 1.0f);
        temp = matt.GetFloat("Vector1_1264ABDA");
        t = 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ceilTrigger)
        {
            t += Time.deltaTime / limit;
            matt.SetFloat("Vector1_1264ABDA", Mathf.Lerp(temp, -1, t));

            if (matt.GetFloat("Vector1_1264ABDA") <= -0.95f)
            {
                SetTrigger(false);
            }
        }
        else
        {
            t += Time.deltaTime / limit;
            matt.SetFloat("Vector1_1264ABDA", Mathf.Lerp(temp, 1, t));
           
        }
    }

    public void SetTrigger(bool boolean)
    {
        ceilTrigger = boolean;
        t = 0;
        temp = matt.GetFloat("Vector1_1264ABDA");
    }
}
