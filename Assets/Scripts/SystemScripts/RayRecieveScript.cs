using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//クリックされたオブジェクト側の処理に関するスクリプト

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
        //クリック時に情報を抽出
        if (Input.GetMouseButtonDown(0))
        {
            //Operator.csからの共有情報を使いやすく加工
            RaycastHit hit = RayScript.hitObject;

            if(hit.collider != null)
            {
                //rayがオブジェクトに当たっている場合
                if (hit.collider.gameObject == this.gameObject)
                {
                    //rayが当たったオブジェクトが自分の場合常に呼ばれる
                    Clicked();
                }
                else
                {
                    //別のオブジェクトだった場合場合常に呼ばれる
                    UnClicked();
                }
            }
            else
            {
                //rayが何のオブジェクトにも当たっていない場合
                UnClicked();
            }
            
        }
        
        
    }

    void Clicked()
    {
        setMaterial = materialArray[1];
        //マテリアルを適用
        this.gameObject.GetComponent<MeshRenderer>().material = setMaterial;
    }

    void UnClicked()
    {
        setMaterial = materialArray[0];
        //マテリアルを適用
        this.gameObject.GetComponent<MeshRenderer>().material = setMaterial;
    }

}
