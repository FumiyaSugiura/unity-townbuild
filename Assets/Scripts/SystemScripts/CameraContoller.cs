using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//プレイヤーによるカメラ制御についてのプログラム
//CameraSystemにアタッチ this = "CameraSystem" （CameraSystemは基本移動、回転させない）
public class CameraController : MonoBehaviour
{
    [SerializeField] float camPositionSpeed;
    [SerializeField] float camRotateSpeed;
    [SerializeField] float camZoomSpeed;

    [SerializeField] GameObject cameraCoordinateRotater; //カメラの回転角度を記憶し、WASD方向を調整する（座標を回転させる）カメラ用座標
    [SerializeField] GameObject cameraCenterSphere; //カメラ中心、カメラ移動回転主体
    [SerializeField] GameObject cameraWrapper; //未ズーム状態のカメラ位置
    [SerializeField] GameObject mainCam;//メインカメラ

    //マウス感度
    [SerializeField] float moveMouseSensitivity = 2f; //移動時限定のマウス感度
    [SerializeField] float rotateMouseSensitivity = 5f; //回転時限定のマウス感度
    [SerializeField] float zoomMouseSensitivity = 10f; //ズーム時限定のマウス感度
    
    

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        cameraCenterSphere.transform.localRotation = Quaternion.Euler(40f, 0, 0); 
        cameraWrapper.transform.localPosition = new Vector3(0, 0, -12f);

        camPositionSpeed = 30f;
        camRotateSpeed = 80f;
        camZoomSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        //カメラの移動主体（CameraCcenterSphere）の座標と回転
        float x = cameraCenterSphere.transform.localPosition.x;
        float y = cameraCenterSphere.transform.localPosition.y;
        float z = cameraCenterSphere.transform.localPosition.z;
       

        //======================================== キーボード操作 ========================================
        //------ WASD移動 ------　対象：CameraCenterSphere　カメラに横の角度がついていてもlocalPositionなのでカメラの向きに沿って進行する
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical"); 
        cameraCenterSphere.transform.localPosition = new Vector3(x + h * camPositionSpeed * Time.deltaTime, y, z + v * camPositionSpeed * Time.deltaTime);

        //------ RFカメラ回転 ------　対象：CameraCenterSphere　WASD移動の主体の親で回転することにより回転時のWASDを違和感なく実行できる
        //縦回転:localRotation
        float ajustedAngleX = Mathf.Repeat(cameraCenterSphere.transform.localEulerAngles.x + 180, 360) - 180; //CameraCenterSphereのx軸中心（縦回転）の角度を-180~180で正規化        
        if (Input.GetKey(KeyCode.R))
        {
            if (ajustedAngleX < 55) //回転下限
            {
                cameraCenterSphere.transform.localRotation = Quaternion.AngleAxis(camRotateSpeed * Time.deltaTime, Vector3.right) * cameraCenterSphere.transform.localRotation;
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (ajustedAngleX > 25) //回転上限
            {
                cameraCenterSphere.transform.localRotation = Quaternion.AngleAxis(-1 * camRotateSpeed * Time.deltaTime, Vector3.right) * cameraCenterSphere.transform.localRotation;
            }            
        }       
        //横回転:rotation
        if (Input.GetKey(KeyCode.E))
        {
            cameraCenterSphere.transform.rotation = Quaternion.AngleAxis(camRotateSpeed * Time.deltaTime, Vector3.up) * cameraCenterSphere.transform.rotation;
            CoodinateRotate(); //座標をカメラに合わせて回転
        }
        if (Input.GetKey(KeyCode.Q))
        {
            cameraCenterSphere.transform.rotation = Quaternion.AngleAxis(-1 * camRotateSpeed * Time.deltaTime, Vector3.up) * cameraCenterSphere.transform.rotation;
            CoodinateRotate(); //座標をカメラに合わせて回転
        }

        //------ XZズームアウト ------ 対象：Main Camera
        if (mainCam.transform.localPosition.z < 10) //ズーム上限
        {
            if (Input.GetKey(KeyCode.X))
            {
                mainCam.transform.localPosition += new Vector3(0, 0, camZoomSpeed * Time.deltaTime);
            }            
        }
        if (mainCam.transform.localPosition.z > -40) //ズーム下限
        {
            if (Input.GetKey(KeyCode.Z))
            {
                mainCam.transform.localPosition += new Vector3(0, 0, -1 * camZoomSpeed * Time.deltaTime);
            }
        }
        //ズーム段階によってカメラスピードを変化させる
        camPositionSpeed = -1.6f * mainCam.transform.localPosition.z + 30;
        camZoomSpeed = -1 * mainCam.transform.localPosition.z + 20;


        //======================================== マウス操作 ========================================
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");
        float mouseZoom = Input.GetAxis("Mouse ScrollWheel");

        //------ マウス移動 ------　中央ボタンクリック
        if (Input.GetMouseButton(2))
        {
            cameraCenterSphere.transform.localPosition = new Vector3(x - moveMouseSensitivity * moveX, y, z - moveMouseSensitivity * moveY);
        }
        //------ マウス回転 ------　右クリック
        if (Input.GetMouseButton(1))
        {
            //横回転
            cameraCenterSphere.transform.rotation = Quaternion.AngleAxis(rotateMouseSensitivity * moveX, Vector3.up) * cameraCenterSphere.transform.rotation;
            CoodinateRotate(); //座標回転
            //縦回転　座標回転後に行う
            //ajustAngleXは、CameraCenterSphereのx軸中心（縦回転）の角度を-180~180で正規化したもの
            if (moveY > 0) 
            {
                //moveYが正のときajustAngleXを減らす
                if (ajustedAngleX > 25) // 25が下限
                {
                    MouseRotateY();
                }
            }
            else
            {
                //moveYが負のときajustAngleXを増やす
                if (ajustedAngleX < 55) //55が上限
                {
                    MouseRotateY();
                }
            }
            void MouseRotateY()
            {
                cameraCenterSphere.transform.localRotation = Quaternion.AngleAxis(-1 * rotateMouseSensitivity * moveY, Vector3.right) * cameraCenterSphere.transform.localRotation;
            }
        }
        //------ マウスホイールによるズーム ------
        if (mouseZoom > 0)
        {
            //mouseZoom正 = ズームイン, mainCamのlocalPositionは増える
            if(mainCam.transform.localPosition.z < 10) //ズームイン上限
            {
                MouseZoom();
            }
        }
        else
        {
            //mouseZoom負 = ズームアウト, mainCamのlocalPositionは減る
            if (mainCam.transform.localPosition.z > -40) //ズームアウト下限
            {
                MouseZoom();
            }
        }
        void MouseZoom()
        {
            mainCam.transform.localPosition += new Vector3(0, 0, zoomMouseSensitivity * mouseZoom);
            //Debug.Log("mouseZoom: " + mouseZoom);
        }
        

        //======================================== デバッグ用 ========================================

       


    }
    void CoodinateRotate() //座標回転関数　カメラ回転処理の後に呼ぶ　対象：CameraCoodinateRotater
    {
        //カメラ位置を記憶
        Vector3 spherePosition = cameraCenterSphere.transform.position; //カメラの場所（グローバル）を記憶
        Quaternion sphereRotation = cameraCenterSphere.transform.rotation; //カメラの回転（グローバル）を記憶

        //現在の座標に対してどれくらい回転したかを取得
        float camRotationY = cameraCenterSphere.transform.localEulerAngles.y; //カメラの水平面の回転移動を取得

        //座標を回転させる
        cameraCoordinateRotater.transform.rotation = Quaternion.AngleAxis(camRotationY, Vector3.up) * cameraCoordinateRotater.transform.rotation;

        //座標回転に伴い、子要素であるカメラが移動・回転するため、戻す
        cameraCenterSphere.transform.position = spherePosition; //カメラの場所を戻す
        cameraCenterSphere.transform.rotation = sphereRotation; //カメラの回転も戻す
    }
}