using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDissovle : MonoBehaviour
{
    Material matt;
    float t;
    public float limit = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        matt = gameObject.GetComponent<Renderer>().material;
        matt.SetFloat("Vector1_1264ABDA", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / limit;
        matt.SetFloat("Vector1_1264ABDA", Mathf.Lerp(-0.8f, 0.5f, t));

        if (t > limit) {
            Destroy(gameObject);
        }

    }
}
