using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject cursor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cursor.gameObject.transform.position = MouseController.cursorPos;
    }

    //�I�u�W�F�N�g�z�u�{�^��������
    public void ArrangeObjectButton(string objectType)
    {
        Debug.Log(objectType + " button is clicked");
        UserArrangementScript.ArrangeObject(objectType);
    }
}
