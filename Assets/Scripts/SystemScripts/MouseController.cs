using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    Vector3 mousePos;
    Vector3 prevMousePos;
    public static Vector3 cursorPos;

    //デバッグ用変数
    float debugTime = 0;
    float dx = 0;
    float dy = 0;
    float ix = 0;
    float iy = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Screen.currentResolution);
        Debug.Log(Screen.width);

        mousePos = Input.mousePosition;
        cursorPos = mousePos;
        prevMousePos = mousePos;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
       
        Vector3 mouseDelta = mousePos - prevMousePos;
        cursorPos += mouseDelta;
        prevMousePos = mousePos;
        if (mousePos == cursorPos)
        {
            //Debug.Log("same");
        }
        else
        {
            float dx = cursorPos.x - mousePos.x;
            float dy = cursorPos.y - mousePos.y;
            float dz = cursorPos.z - mousePos.z;
            //Debug.Log($"difference: ({dx},{dy},{dz})");
        }


        //======================================== デバッグ用 ========================================
        //マウスのxyに関する検証
        float interval = 2f;
        debugTime += Time.deltaTime;

        dx += Input.GetAxis("Mouse X");
        dy += Input.GetAxis("Mouse Y");
        if (debugTime > interval)
        {
            //Debug.Log("x: " + dx + ", y: " + dy);
            debugTime = 0;
            dx = 0;
            dy = 0;
            
            
        }
    }
}
