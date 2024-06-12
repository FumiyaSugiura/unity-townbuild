using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//プレイヤーによるマウス操作での選択などに関するプログラム
public class RayScript : MonoBehaviour
{   
    //RaycastHitは他スクリプトと共有する
    public static RaycastHit hitObject; //Object用
    public static RaycastHit hitGround; //Ground用
    float maxDistance = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //Rayの設定
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //カメラからRayを常に投射
        bool isHitObject = Physics.Raycast(ray, out hitObject, maxDistance, 1<<LayerMask.NameToLayer("Object")); 
        bool isHitGround = Physics.Raycast(ray, out hitGround, maxDistance, 1<<LayerMask.NameToLayer("Ground"));

        //デバッグ情報用の情報抽出
        if (Input.GetMouseButtonDown(0))
        {                   
            if (isHitObject)
            {
                //デバッグ情報
                string name = hitObject.collider.name;
                string tag = hitObject.collider.tag;
                Debug.Log("name:" + name + ",tag:" + tag);
                
            }
            else
            {
                Debug.Log("No hit.");                
            }
            

            //Rayを表示
            float distance = 100f;
            float duration = 5.0f;
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.yellow, duration, true);
        }      
    }
}
