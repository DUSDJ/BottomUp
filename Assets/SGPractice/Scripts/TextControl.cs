using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CoreType
{
    boolCore,
    intCore,
    charCore,
    justCube,
    clearBox,
    enemy,

}

public class TextControl : MonoBehaviour
{
    
    public CoreType coreType = CoreType.justCube;
    
    private Canvas textCanvas;

    private Text[] textBoxes;
    private Text valueBox;
    private Text addressBox;
    

    /* core values */
    public int intValue;
    public bool boolValue = false;
    public char charValue;

    /* triggers */
    bool fade = false;
    bool onAction = false;
    

    float limit = 2.0f;
    float t = 0;
    Color originColor;
    Color tempColor;

    Color addressOriginColor;
    Color addressTempColor;

    /* sound effects */
    public AudioClip sound_bool;
    public AudioClip sound_int;
    public AudioClip sound_char;
    
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Init()
    {
        if (gameObject.GetComponentInChildren<Canvas>() != null)
        {
            textCanvas = GetComponentInChildren<Canvas>();
        }

        if (gameObject.GetComponentInChildren<Text>() != null)
        {
            textBoxes = GetComponentsInChildren<Text>();

            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (textBoxes[i].name == "Address")
                {
                    addressBox = textBoxes[i];
                    addressOriginColor = addressBox.color;
                }

                if (textBoxes[i].name == "Value")
                {
                    valueBox = textBoxes[i];
                    originColor = valueBox.color;

                }
            }

            switch (coreType)
            {
                case CoreType.boolCore:
                    if (boolValue == false) valueBox.text = "F";
                    if (boolValue == true) valueBox.text = "T";
                    break;
                case CoreType.intCore:
                    SetIntValueText();
                    break;
                case CoreType.charCore:
                    if (charValue.Equals('\0'))
                        valueBox.text = "[ ]";
                    else
                        valueBox.text = "[" + charValue.ToString() + "]";
                    break;
                case CoreType.justCube:
                    Destroy(valueBox);
                    break;
                case CoreType.clearBox:
                    break;
                case CoreType.enemy:
                    SetAddress("#??");
                    valueBox.fontSize = 30;
                    valueBox.text = "Enemy";
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        textCanvas.transform.LookAt(Camera.main.transform);
        ValueFade();
        AddressFade();
    }

    public void SetAddress(string address)
    {
        if (addressBox != null)
        {
            addressBox.text = address;
        }
    }
    public void SetAddressSize(int size)
    {
        if (addressBox != null)
        {
            addressBox.fontSize = size;
        }
    }

    public void SetIntValueText()
    {
        valueBox.text = intValue.ToString();
    }


    public void ValueFade()
    {
        if (valueBox != null)
        {
            tempColor = valueBox.color;
            if (fade)
            {
                t += Time.deltaTime / limit;
                valueBox.color = Color.Lerp(tempColor, originColor, t);
                if (t >= limit)
                {
                    onAction = true;
                    return;
                }
            }
            else
            {
                t += Time.deltaTime / limit;
                valueBox.color = Color.Lerp(tempColor, new Color(originColor.r, originColor.g, originColor.b, 0), t);
            }

            onAction = false;
        }
    }

    public void AddressFade()
    {
        if (addressBox != null)
        {
            addressTempColor = addressBox.color;
            if (fade)
            {
                t += Time.deltaTime / limit;
                addressBox.color = Color.Lerp(addressTempColor, addressOriginColor, t);
            }
            else
            {
                t += Time.deltaTime / limit;
                addressBox.color = Color.Lerp(addressTempColor, new Color(addressOriginColor.r, addressOriginColor.g, addressOriginColor.b, 0), t);
            }
        }
    }

    public void Action()
    {
        if (!onAction)
        {
            return;
        }

        switch (coreType)
        {
            case CoreType.boolCore:
                boolValue = !boolValue;
                if(boolValue == false) valueBox.text = "F";
                if(boolValue == true) valueBox.text = "T";
                GameManager.gm.ClearCheck();
                audioSource.PlayOneShot(sound_bool);
                break;

            case CoreType.intCore:
                intValue += 1;
                if (intValue > 9) intValue = 0;
                SetIntValueText();
                GameManager.gm.ClearCheck();
                audioSource.PlayOneShot(sound_int);
                break;

            case CoreType.charCore:
                /* 인벤토리에 저장된 Char을 하나 넣거나 빼는 방식 */
                if(charValue == '\0')
                {
                    if (GameManager.gm.item == '\0')
                    {
                        return;
                    }
                    audioSource.PlayOneShot(sound_char);
                    // 인벤토리에서 값 가져오기
                    charValue = GameManager.gm.item;
                    // 인벤토리에서 값 제거
                    GameManager.gm.item = '\0';                  
                }
                else
                {
                    // 값이 있는 큐브일 경우 인벤토리가 공백이 아니면 아무 액션 없음.
                    if(GameManager.gm.item != '\0')
                    {
                        return;
                    }
                    audioSource.PlayOneShot(sound_char);
                    // 값을 인벤토리에 추가.
                    GameManager.gm.item = charValue;
                    // 값 제거
                    charValue = '\0';
                }

                if (charValue.Equals('\0'))
                    valueBox.text = "[ ]";
                else
                    valueBox.text = "[" + charValue.ToString() + "]";

                GameManager.gm.ClearCheck();
                break;
        }
        
    }
    
    public bool GetFade()
    {
        return fade;
    }
    public void SetFade(bool fade)
    {
        this.fade = fade;
    }

    public int GetValue()
    {
        return intValue;
    }
}
