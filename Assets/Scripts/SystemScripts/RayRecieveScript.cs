using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�N���b�N���ꂽ�I�u�W�F�N�g���̏����Ɋւ���X�N���v�g

public class RayRecieveScript : MonoBehaviour
{
    public Material[] materialArray;
    Material setMaterial;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = materialArray[0];
        setMaterial = materialArray[0];
    }
    
    // Update is called once per frame
    void Update()
    {       
        //�N���b�N���ɏ��𒊏o
        if (Input.GetMouseButtonDown(0))
        {
            //Operator.cs����̋��L�����g���₷�����H
            RaycastHit hit = RayScript.hitObject;

            if(hit.collider != null)
            {
                //ray���I�u�W�F�N�g�ɓ������Ă���ꍇ
                if (hit.collider.gameObject == this.gameObject)
                {
                    //ray�����������I�u�W�F�N�g�������̏ꍇ��ɌĂ΂��
                    Clicked();
                }
                else
                {
                    //�ʂ̃I�u�W�F�N�g�������ꍇ�ꍇ��ɌĂ΂��
                    UnClicked();
                }
            }
            else
            {
                //ray�����̃I�u�W�F�N�g�ɂ��������Ă��Ȃ��ꍇ
                UnClicked();
            }
            
        }
        
        
    }

    void Clicked()
    {
        setMaterial = materialArray[1];
        //�}�e���A����K�p
        this.gameObject.GetComponent<MeshRenderer>().material = setMaterial;
    }

    void UnClicked()
    {
        setMaterial = materialArray[0];
        //�}�e���A����K�p
        this.gameObject.GetComponent<MeshRenderer>().material = setMaterial;
    }

}
