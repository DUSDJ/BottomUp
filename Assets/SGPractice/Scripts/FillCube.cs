using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillCube : MonoBehaviour
{
    Material matt;
    public float t;
    public float limit = 0.25f;
    
    public bool seeing;
    Collider col;
    float temp;

    private TextControl tc;

    // Start is called before the first frame update
    private void Awake()
    {
        matt = gameObject.GetComponent<Renderer>().material;
        matt.SetFloat("Vector1_1264ABDA", -1.0f);
        temp = matt.GetFloat("Vector1_1264ABDA");
        
    }

    void Start()
    {
        col = GetComponent<Collider>();
        seeing = false;
        t = 0;
        if (gameObject.GetComponent<TextControl>() != null)
        {
            tc = gameObject.GetComponent<TextControl>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.isTriggered) {
            return;
        }

        if (seeing || col.isTrigger)
        {
            t += Time.deltaTime / limit;
            matt.SetFloat("Vector1_1264ABDA", Mathf.Lerp(temp, limit, t));
            
            if(matt.GetFloat("Vector1_1264ABDA") >= 0.2f && tc != null)
            {
                tc.SetFade(true);
            }
        }
        else
        {
            t += Time.deltaTime / limit;
            matt.SetFloat("Vector1_1264ABDA", Mathf.Lerp(temp, -1, t));
            if (tc != null && tc.GetFade())
            {
                gameObject.GetComponent<TextControl>().SetFade(false);
            }
        }


    }
    
    public void SetTrigger(bool see)
    {
        if (seeing != see)
        {
            t = 0f;
            temp = matt.GetFloat("Vector1_1264ABDA");
        }
        seeing = see;
    }

    public void SetCollider()
    {
        t = 0f;
        temp = matt.GetFloat("Vector1_1264ABDA");
        col.isTrigger = !col.isTrigger;        
    }

    public void SetCollider(bool onOff)
    {
        t = 0f;
        temp = matt.GetFloat("Vector1_1264ABDA");
        col.isTrigger = onOff;
    }
}
