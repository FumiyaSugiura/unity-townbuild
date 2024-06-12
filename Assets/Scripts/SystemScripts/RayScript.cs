using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�v���C���[�ɂ��}�E�X����ł̑I���ȂǂɊւ���v���O����
public class RayScript : MonoBehaviour
{   
    //RaycastHit�͑��X�N���v�g�Ƌ��L����
    public static RaycastHit hitObject; //Object�p
    public static RaycastHit hitGround; //Ground�p
    float maxDistance = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //Ray�̐ݒ�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //�J��������Ray����ɓ���
        bool isHitObject = Physics.Raycast(ray, out hitObject, maxDistance, 1<<LayerMask.NameToLayer("Object")); 
        bool isHitGround = Physics.Raycast(ray, out hitGround, maxDistance, 1<<LayerMask.NameToLayer("Ground"));

        //�f�o�b�O���p�̏�񒊏o
        if (Input.GetMouseButtonDown(0))
        {                   
            if (isHitObject)
            {
                //�f�o�b�O���
                string name = hitObject.collider.name;
                string tag = hitObject.collider.tag;
                Debug.Log("name:" + name + ",tag:" + tag);
                
            }
            else
            {
                Debug.Log("No hit.");                
            }
            

            //Ray��\��
            float distance = 100f;
            float duration = 5.0f;
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow, duration, true);
        }      
    }
}
