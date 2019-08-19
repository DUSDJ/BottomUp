using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject goSpawn;
    public float fDifficulty;

    float fSpawn = 0;

    // Start is called before the first frame update
    void Start()
    {
        fDifficulty = SceneTransParam.instance.fDifficulty;
    }

    // Update is called once per frame
    void Update()
    {
        fSpawn += fDifficulty * Time.deltaTime;
        if (fDifficulty < 40)
        {
            fDifficulty += Time.deltaTime * 4f;
        }
        else
        {
            fDifficulty = 40;
        }

        while (fSpawn > 0)
        {
            fSpawn -= 1;
            Vector3 vPos = new Vector3(Random.value * 40f - 20f, 0, Random.value * 40f - 20f) + transform.position;
            Quaternion qRot = Quaternion.Euler(0, Random.value * 360f, Random.value * 30f);
            Vector3 v3Scale = new Vector3(Random.value + 0.1f, 10f, Random.value + 0.1f);

            GameObject goCreate = Instantiate(goSpawn, vPos, qRot);
            goCreate.transform.localScale = v3Scale;


            // -0.8 ~ 0.5
            Destroy(goCreate, 5.0f);
        }
    }
}
