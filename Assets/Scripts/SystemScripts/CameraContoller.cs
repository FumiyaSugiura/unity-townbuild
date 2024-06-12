using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�v���C���[�ɂ��J��������ɂ��Ẵv���O����
//CameraSystem�ɃA�^�b�` this = "CameraSystem" �iCameraSystem�͊�{�ړ��A��]�����Ȃ��j
public class CameraController : MonoBehaviour
{
    [SerializeField] float camPositionSpeed;
    [SerializeField] float camRotateSpeed;
    [SerializeField] float camZoomSpeed;

    [SerializeField] GameObject cameraCoordinateRotater; //�J�����̉�]�p�x���L�����AWASD�����𒲐�����i���W����]������j�J�����p���W
    [SerializeField] GameObject cameraCenterSphere; //�J�������S�A�J�����ړ���]���
    [SerializeField] GameObject cameraWrapper; //���Y�[����Ԃ̃J�����ʒu
    [SerializeField] GameObject mainCam;//���C���J����

    //�}�E�X���x
    [SerializeField] float moveMouseSensitivity = 2f; //�ړ�������̃}�E�X���x
    [SerializeField] float rotateMouseSensitivity = 5f; //��]������̃}�E�X���x
    [SerializeField] float zoomMouseSensitivity = 10f; //�Y�[��������̃}�E�X���x
    
    

    // Start is called before the first frame update
    void Start()
    {
        //������
        cameraCenterSphere.transform.localRotation = Quaternion.Euler(40f, 0, 0); 
        cameraWrapper.transform.localPosition = new Vector3(0, 0, -12f);

        camPositionSpeed = 30f;
        camRotateSpeed = 80f;
        camZoomSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        //�J�����̈ړ���́iCameraCcenterSphere�j�̍��W�Ɖ�]
        float x = cameraCenterSphere.transform.localPosition.x;
        float y = cameraCenterSphere.transform.localPosition.y;
        float z = cameraCenterSphere.transform.localPosition.z;
       

        //======================================== �L�[�{�[�h���� ========================================
        //------ WASD�ړ� ------�@�ΏہFCameraCenterSphere�@�J�����ɉ��̊p�x�����Ă��Ă�localPosition�Ȃ̂ŃJ�����̌����ɉ����Đi�s����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical"); 
        cameraCenterSphere.transform.localPosition = new Vector3(x + h * camPositionSpeed * Time.deltaTime, y, z + v * camPositionSpeed * Time.deltaTime);

        //------ RF�J������] ------�@�ΏہFCameraCenterSphere�@WASD�ړ��̎�̂̐e�ŉ�]���邱�Ƃɂ���]����WASD����a���Ȃ����s�ł���
        //�c��]:localRotation
        float ajustedAngleX = Mathf.Repeat(cameraCenterSphere.transform.localEulerAngles.x + 180, 360) - 180; //CameraCenterSphere��x�����S�i�c��]�j�̊p�x��-180~180�Ő��K��        
        if (Input.GetKey(KeyCode.R))
        {
            if (ajustedAngleX < 55) //��]����
            {
                cameraCenterSphere.transform.localRotation = Quaternion.AngleAxis(camRotateSpeed * Time.deltaTime, Vector3.right) * cameraCenterSphere.transform.localRotation;
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (ajustedAngleX > 25) //��]���
            {
                cameraCenterSphere.transform.localRotation = Quaternion.AngleAxis(-1 * camRotateSpeed * Time.deltaTime, Vector3.right) * cameraCenterSphere.transform.localRotation;
            }            
        }       
        //����]:rotation
        if (Input.GetKey(KeyCode.E))
        {
            cameraCenterSphere.transform.rotation = Quaternion.AngleAxis(camRotateSpeed * Time.deltaTime, Vector3.up) * cameraCenterSphere.transform.rotation;
            CoodinateRotate(); //���W���J�����ɍ��킹�ĉ�]
        }
        if (Input.GetKey(KeyCode.Q))
        {
            cameraCenterSphere.transform.rotation = Quaternion.AngleAxis(-1 * camRotateSpeed * Time.deltaTime, Vector3.up) * cameraCenterSphere.transform.rotation;
            CoodinateRotate(); //���W���J�����ɍ��킹�ĉ�]
        }

        //------ XZ�Y�[���A�E�g ------ �ΏہFMain Camera
        if (mainCam.transform.localPosition.z < 10) //�Y�[�����
        {
            if (Input.GetKey(KeyCode.X))
            {
                mainCam.transform.localPosition += new Vector3(0, 0, camZoomSpeed * Time.deltaTime);
            }            
        }
        if (mainCam.transform.localPosition.z > -40) //�Y�[������
        {
            if (Input.GetKey(KeyCode.Z))
            {
                mainCam.transform.localPosition += new Vector3(0, 0, -1 * camZoomSpeed * Time.deltaTime);
            }
        }
        //�Y�[���i�K�ɂ���ăJ�����X�s�[�h��ω�������
        camPositionSpeed = -1.6f * mainCam.transform.localPosition.z + 30;
        camZoomSpeed = -1 * mainCam.transform.localPosition.z + 20;


        //======================================== �}�E�X���� ========================================
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");
        float mouseZoom = Input.GetAxis("Mouse ScrollWheel");

        //------ �}�E�X�ړ� ------�@�����{�^���N���b�N
        if (Input.GetMouseButton(2))
        {
            cameraCenterSphere.transform.localPosition = new Vector3(x - moveMouseSensitivity * moveX, y, z - moveMouseSensitivity * moveY);
        }
        //------ �}�E�X��] ------�@�E�N���b�N
        if (Input.GetMouseButton(1))
        {
            //����]
            cameraCenterSphere.transform.rotation = Quaternion.AngleAxis(rotateMouseSensitivity * moveX, Vector3.up) * cameraCenterSphere.transform.rotation;
            CoodinateRotate(); //���W��]
            //�c��]�@���W��]��ɍs��
            //ajustAngleX�́ACameraCenterSphere��x�����S�i�c��]�j�̊p�x��-180~180�Ő��K����������
            if (moveY > 0) 
            {
                //moveY�����̂Ƃ�ajustAngleX�����炷
                if (ajustedAngleX > 25) // 25������
                {
                    MouseRotateY();
                }
            }
            else
            {
                //moveY�����̂Ƃ�ajustAngleX�𑝂₷
                if (ajustedAngleX < 55) //55�����
                {
                    MouseRotateY();
                }
            }
            void MouseRotateY()
            {
                cameraCenterSphere.transform.localRotation = Quaternion.AngleAxis(-1 * rotateMouseSensitivity * moveY, Vector3.right) * cameraCenterSphere.transform.localRotation;
            }
        }
        //------ �}�E�X�z�C�[���ɂ��Y�[�� ------
        if (mouseZoom > 0)
        {
            //mouseZoom�� = �Y�[���C��, mainCam��localPosition�͑�����
            if(mainCam.transform.localPosition.z < 10) //�Y�[���C�����
            {
                MouseZoom();
            }
        }
        else
        {
            //mouseZoom�� = �Y�[���A�E�g, mainCam��localPosition�͌���
            if (mainCam.transform.localPosition.z > -40) //�Y�[���A�E�g����
            {
                MouseZoom();
            }
        }
        void MouseZoom()
        {
            mainCam.transform.localPosition += new Vector3(0, 0, zoomMouseSensitivity * mouseZoom);
            //Debug.Log("mouseZoom: " + mouseZoom);
        }
        

        //======================================== �f�o�b�O�p ========================================

       


    }
    void CoodinateRotate() //���W��]�֐��@�J������]�����̌�ɌĂԁ@�ΏہFCameraCoodinateRotater
    {
        //�J�����ʒu���L��
        Vector3 spherePosition = cameraCenterSphere.transform.position; //�J�����̏ꏊ�i�O���[�o���j���L��
        Quaternion sphereRotation = cameraCenterSphere.transform.rotation; //�J�����̉�]�i�O���[�o���j���L��

        //���݂̍��W�ɑ΂��Ăǂꂭ�炢��]���������擾
        float camRotationY = cameraCenterSphere.transform.localEulerAngles.y; //�J�����̐����ʂ̉�]�ړ����擾

        //���W����]������
        cameraCoordinateRotater.transform.rotation = Quaternion.AngleAxis(camRotationY, Vector3.up) * cameraCoordinateRotater.transform.rotation;

        //���W��]�ɔ����A�q�v�f�ł���J�������ړ��E��]���邽�߁A�߂�
        cameraCenterSphere.transform.position = spherePosition; //�J�����̏ꏊ��߂�
        cameraCenterSphere.transform.rotation = sphereRotation; //�J�����̉�]���߂�
    }
}