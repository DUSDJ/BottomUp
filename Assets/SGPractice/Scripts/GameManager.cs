using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BottomUp;

public class GameManager : MonoBehaviour
{
    /* SingleTone Instance */
    public static GameManager gm;

    /* External Prefabs */
    public GameObject canvasPrefab;
    public Material mate;

    /* VRmode */
    public bool setVRMode = true;
    public static bool VRMode = true;

    /* Trap Trigger */
    public bool isTriggered = false;
    

    /* Target Objects */
    public GameObject[] target_cores;
    public GameObject[] target_doors;
    public GameObject[] target_enemies;
    public Text[] target_text;

    /* ItemTextBox */
    public GameObject itemCube;

    /* sound effects */
    public AudioClip sound_welldone;

    AudioSource audioSource;

    public char item = '\0';

    public Stage setStage;

    public enum Stage
    {
        Stage_1,
        Stage_2,
        Stage_3,
        Stage_4,
        CubeSpace,
    }
    
    /* For Init */
    GameObject[] objects;
    FillCube[] cubes;
    GameObject[] cores;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
        audioSource = GetComponent<AudioSource>();

        if (gm == null)
        {
            gm = this;
            VRMode = setVRMode;
        }
        else
        {
            Destroy(this.gameObject);
        }


        objects = GameObject.FindGameObjectsWithTag("Cube");

        for (int i = 0; i < objects.Length; i++)
        {
            /* ClearBox 일 경우 FillCube 스킵한다. */
            if (objects[i].GetComponent<ClearTrigger>() != null)
            {
                continue;
            }
                
            if (objects[i].GetComponent<FillCube>() == null)
            {
                objects[i].AddComponent<FillCube>();
            }
            
            Transform tf = Instantiate(canvasPrefab).transform;
            tf.SetParent(objects[i].transform);
            tf.localPosition = new Vector3(0, 0, 0);

            TextControl tc = objects[i].GetComponent<TextControl>();
            if (tc == null)
            {
                tc = objects[i].AddComponent<TextControl>();
                tc.coreType = CoreType.justCube;
            }

            tc.Init();
            /*
            tc.SetAddress("#" + i);
            */
        }

        cores = GameObject.FindGameObjectsWithTag("Core");
        for (int i = 0; i < cores.Length; i++)
        {
            if (cores[i].GetComponent<FillCube>() == null)
            {
                cores[i].AddComponent<FillCube>();
            }

            Transform tf = Instantiate(canvasPrefab).transform;
            tf.SetParent(cores[i].transform);
            tf.localPosition = new Vector3(0, 0, 0);

            if (cores[i].GetComponent<TextControl>() != null)
            {
                TextControl tc = cores[i].GetComponent<TextControl>();
                tc.Init();
            }
        }
        
        SetDoors();
        SetAddressValues();

        cubes = FindObjectsOfType<FillCube>();
        
        itemCube.GetComponentInChildren<Text>().text = "";
        // SetAllCubeTriggerFalse();
    }

    // Update is called once per frame
    void Update()
    {
        /* 타이틀 화면으로 이동 VR*/
        if (GameManager.VRMode && Input.GetButtonUp("Fire2"))
        {
            SceneManager.LoadScene(0);
        }


        /* 타이틀 화면으로 이동 일반*/
        if (!GameManager.VRMode  && Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0);
        }

        /* 임시 VR 전환 버튼 : VR 장착 시 삭제 */
        if (Input.GetButtonUp("Jump"))
        {
            GameManager.VRMode = !GameManager.VRMode;
        }
    }
    

    /* 텍스트를 replace하여 코드 onOff를 표시. */
    public void SetTextCube(Text target_text, string replaceTarget, bool isGreen)
    {
        string temp = target_text.text;
        if (isGreen)
        {
            string temp_prev = temp;
            temp = temp.Replace("<color=#ff0000>" + replaceTarget + "</color>", "<color=#00ff00>" + replaceTarget + "</color>");
            if (!temp.Equals(temp_prev))
            {
                audioSource.PlayOneShot(sound_welldone);
            }
        }
        else
        {
            temp = temp.Replace("<color=#00ff00>" + replaceTarget + "</color>", "<color=#ff0000>" + replaceTarget + "</color>");
        }
        
        target_text.text = temp;
        
    }


    /* 게임 클리어를 체크하는 함수. 각 스테이지마다 다른 코드. */
    public void ClearCheck() {
        itemCube.GetComponentInChildren<Text>().text = "";
        if(item != '\0')
            itemCube.GetComponentInChildren<Text>().text = item + "";
        
        switch (setStage)
        {
            case Stage.Stage_1:
                if (target_cores[0].GetComponent<TextControl>().intValue == target_cores[1].GetComponent<TextControl>().intValue
                    && target_cores[1].GetComponent<TextControl>().intValue == target_cores[2].GetComponent<TextControl>().intValue)
                {
                    target_doors[0].GetComponent<FillCube>().SetCollider(true);
                    SetTextCube(target_text[0], "gate1Open();", true);

                }
                else
                {
                    target_doors[0].GetComponent<FillCube>().SetCollider(false);
                    SetTextCube(target_text[0], "gate1Open();", false);
                }

                break;
            case Stage.Stage_2:
                if (target_enemies[0].GetComponent<RobotScript>().IsOpen())
                {
                    SetTextCube(target_text[0], "EnemyMove();", true);
                }

                if (target_cores[1].GetComponent<TextControl>().boolValue) {
                    target_doors[0].GetComponent<FillCube>().SetCollider(true);
                    SetTextCube(target_text[0], "gate1Open();", true);
                }
                else
                {
                    target_doors[0].GetComponent<FillCube>().SetCollider(false);
                    SetTextCube(target_text[0], "gate1Open();", false);
                }

                if (target_cores[0].GetComponent<TextControl>().boolValue == true && target_cores[2].GetComponent<TextControl>().boolValue == true)
                {
                    target_doors[1].GetComponent<FillCube>().SetCollider(true);
                    SetTextCube(target_text[0], "gate2Open();", true);
                }
                else
                {
                    target_doors[1].GetComponent<FillCube>().SetCollider(false);
                    SetTextCube(target_text[0], "gate2Open();", false);
                }

                break;
            case Stage.Stage_3:
                target_cores[8].GetComponent<TextControl>().intValue = 0;

                //b0
                if (target_cores[0].GetComponent<TextControl>().boolValue)
                {
                    target_doors[0].GetComponent<FillCube>().SetCollider(true);
                    SetTextCube(target_text[0], "gate1Open();", true);
                }
                else
                {
                    target_doors[0].GetComponent<FillCube>().SetCollider(false);
                    SetTextCube(target_text[0], "gate1Open();", false);
                }

                //b1
                if (target_cores[1].GetComponent<TextControl>().boolValue)
                {
                    target_doors[1].GetComponent<FillCube>().SetCollider(true);
                    SetTextCube(target_text[0], "gate2OpenForever();", true);
                    SetTextCube(target_text[0], "EnemyMove();", true);
                    
                    if (!target_enemies[0].GetComponent<RobotScript>().IsOpen())
                        target_enemies[0].GetComponent<RobotScript>().Open();

                    if (!target_enemies[1].GetComponent<RobotScript>().IsOpen())
                        target_enemies[1].GetComponent<RobotScript>().Open();
                }

                //c1 c2 c3 YES
                if (target_cores[3].GetComponent<TextControl>().charValue == 'Y'
                    && target_cores[4].GetComponent<TextControl>().charValue == 'E'
                    && target_cores[5].GetComponent<TextControl>().charValue == 'S')
                {
                    target_doors[2].GetComponent<FillCube>().SetCollider(true);
                    SetTextCube(target_text[1], "gate3Open();", true);
                }
                else
                {
                    target_doors[2].GetComponent<FillCube>().SetCollider(false);
                    SetTextCube(target_text[1], "gate3Open();", false);
                }

                //i0 i1 i2
                for (int i=0; i<target_cores[6].GetComponent<TextControl>().intValue; ++i)
                {
                    for (int j = 0; j < target_cores[7].GetComponent<TextControl>().intValue; ++j)
                    {
                        target_cores[8].GetComponent<TextControl>().intValue += 1;
                        target_cores[8].GetComponent<TextControl>().SetIntValueText();
                    }
                }
                
                // c0 c1 c2 i2
                if (target_cores[2].GetComponent<TextControl>().charValue == 'K'
                    && target_cores[3].GetComponent<TextControl>().charValue == 'E'
                    && target_cores[4].GetComponent<TextControl>().charValue == 'Y'
                    && target_cores[8].GetComponent<TextControl>().intValue == 20)
                {
                    target_doors[3].GetComponent<FillCube>().SetCollider(true);
                    SetTextCube(target_text[2], "gate4Open();", true);
                }
                else
                {
                    target_doors[3].GetComponent<FillCube>().SetCollider(false);
                    SetTextCube(target_text[2], "gate4Open();", false);
                }

                break;
            case Stage.Stage_4:

                //c1
                if (target_cores[1].GetComponent<TextControl>().charValue == 'P')
                {
                    target_doors[1].GetComponent<FillCube>().SetCollider(true);
                    SetTextCube(target_text[0], "gate2Open();", true);
                }
                else
                {
                    target_doors[1].GetComponent<FillCube>().SetCollider(false);
                    SetTextCube(target_text[0], "gate2Open();", false);
                }

                //c2
                if (target_cores[2].GetComponent<TextControl>().charValue == 'E')
                {
                    if (target_enemies[0].GetComponent<RobotScript>().IsOpen())
                        target_enemies[0].GetComponent<RobotScript>().Stop();
                    SetTextCube(target_text[0], "EnemyStop();", true);
                }
                else
                {
                    if (!target_enemies[0].GetComponent<RobotScript>().IsOpen())
                        target_enemies[0].GetComponent<RobotScript>().Open();
                    SetTextCube(target_text[0], "EnemyStop();", false);
                }

                //c0123
                if (target_cores[0].GetComponent<TextControl>().charValue == 'O'
                    && target_cores[1].GetComponent<TextControl>().charValue == 'P'
                    && target_cores[2].GetComponent<TextControl>().charValue == 'E'
                    && target_cores[3].GetComponent<TextControl>().charValue == 'N')
                {
                    target_doors[2].GetComponent<FillCube>().SetCollider(true);
                    SetTextCube(target_text[1], "gate3OpenForever();", true);
                }

                //c4 c5
                target_doors[0].GetComponent<FillCube>().SetCollider(false);
                SetTextCube(target_text[2], "gate1Open();", false);
                for (int i = 'N' - target_cores[4].GetComponent<TextControl>().charValue;
                    i < (target_cores[5].GetComponent<TextControl>().charValue - 'E');
                    ++i)
                {
                    if (i == 5)
                    {
                        Debug.Log("i : " + i);
                        target_doors[0].GetComponent<FillCube>().SetCollider(true);
                        SetTextCube(target_text[2], "gate1Open();", true);
                        break;
                    }
                }


                break;
            default:
                break;
        }

        
    }


    public void SetDoors()
    {
        for (int i = 0; i < target_doors.Length; i++)
        {
            target_doors[i].GetComponent<TextControl>().SetAddressSize(30);
            target_doors[i].GetComponent<TextControl>().SetAddress("gate" + (i + 1) + "\nOpen()");
        }
    }

    public void SetAddressValues()
    {
        int boolCount = 0;
        int intCount = 0;
        int charCount = 0;
        for (int i = 0; i < target_cores.Length; i++)
        {
            switch (target_cores[i].GetComponent<TextControl>().coreType)
            {
                case CoreType.boolCore:
                    target_cores[i].GetComponent<TextControl>().SetAddress("#b" + (boolCount));
                    boolCount++;
                    break;
                case CoreType.intCore:
                    target_cores[i].GetComponent<TextControl>().SetAddress("#i" + (intCount));
                    intCount++;
                    break;
                case CoreType.charCore:
                    target_cores[i].GetComponent<TextControl>().SetAddress("#c" + (charCount));
                    charCount++;
                    break;
                case CoreType.justCube:
                    break;
                default:
                    break;
            }
        }
    }

    public void SetAllCubeTriggerFalse()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].SetTrigger(false);
        }
    }

    public void Alert()
    {
        GameManager.gm.isTriggered = true;
        Debug.Log("Alert!");

        SetAllCubeTriggerFalse();

        StartCoroutine("AlertFadeRed");

        FindObjectOfType<CameraController>().cinematic = true;

        Invoke("SceneReStart", 2.0f);
    }

    public void SceneReStart()
    {
        SceneManager.LoadScene((GameManager.gm.setStage).ToString());
    }

    IEnumerator AlertFadeRed()
    {

        //빠르게 Dissolve Out
        float t = 0;
        float limit = 2.0f;
        float temp = 0;
        
        //빨간색이 되면서 Dissolve In
        Color tempColor = Color.black;


        while (t < limit)
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                Material m = cubes[i].GetComponent<Renderer>().material;
                temp = m.GetFloat("Vector1_1264ABDA");
                tempColor = m.GetColor("Color_2F8BD401");

                m.SetFloat("Vector1_1264ABDA", Mathf.Lerp(temp, -1f, t));
                m.SetColor("Color_2F8BD401", Color.Lerp(tempColor, Color.red, t));
            }
            t += Time.deltaTime / limit;
            yield return null;
        }

        
        yield return new WaitForSeconds(3.0f);
        
    }
}
