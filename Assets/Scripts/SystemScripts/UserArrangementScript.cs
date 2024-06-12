using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.TMP_ExampleScript_01;

//User's Arrange Objectsにアタッチ
public class UserArrangementScript : MonoBehaviour
{
    public static Dictionary<string, GameObject> objectPrefabDictionary = new Dictionary<string, GameObject>(); //Prefabの辞書
    [SerializeField] string[] objectPrefabKeyArray; //Prefabの辞書のkeyを保管する配列
    [SerializeField] GameObject[] objectPrefabValueArray; //Prefabの辞書のvalueを保管する配列

    List<GameObject> usersArrangedObjectList = new List<GameObject>(); //生成したオブジェクトのリスト
    List<GameObject> usersArrangedObjectPositionList; //生成したオブジェクトの位置リスト

    public static GameObject arrangingObject; //配置中のオブジェクト
    public static Vector3 arrangingPosition; //配置中（場所決定前のObjectの場所）
    public static float ajustingY; //オブジェクトの配置高さをオブジェクトの種類ごとに調整する値
    public static Quaternion arrangingRotation; //配置中の回転
    [SerializeField] float rotateMouseSensitivity = 5f; //回転時限定のマウス感度

    public static GameObject usersArrangeObjects; //parent（つまりwrapper）

    // Start is called before the first frame update
    void Start()
    {
        //objectPrefabDictionaryの設定
        for (int i = 0; i < objectPrefabKeyArray.Length; i++)
        {
            string key = objectPrefabKeyArray[i];
            GameObject value = objectPrefabValueArray[i];

            if (key == value.gameObject.tag)
            {
                Debug.Log($"index:{i}, key:{key}, value:{value}");
                objectPrefabDictionary.Add(key, value);
            }
            else
            {
                Debug.Log($"CAUTON: キー'{key}'にオブジェクト'{value}'が設定されているため実行できません");
                break;
            }

            if (i == objectPrefabKeyArray.Length - 1)
            {
                Debug.Log("オブジェクトを正常に設定しました");
            }
        }

        //User's Arrange Objectsを親に指定
        usersArrangeObjects = this.gameObject;

        //初期化
        arrangingRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = RayScript.hitGround;
        float x = hit.point.x;
        float y = hit.point.y;
        float z = hit.point.z;
        float r = Input.GetAxis("Mouse X");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            arrangingRotation = Quaternion.AngleAxis(-1 * rotateMouseSensitivity * r, Vector3.up) * arrangingRotation;
        }
        else
        {
            arrangingPosition = new Vector3(x, y + ajustingY, z);
        }        

        if (arrangingObject != null)
        {
            arrangingObject.transform.position = arrangingPosition;
            arrangingObject.transform.rotation = arrangingRotation;

            if (Input.GetMouseButtonDown(0))
            {
                usersArrangedObjectList.Add(Instantiate(arrangingObject, arrangingPosition, arrangingRotation, usersArrangeObjects.transform));
                Destroy(arrangingObject);
                Cursor.visible = true;
                ajustingY = 0;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Destroy(arrangingObject);
                Cursor.visible = true;
                ajustingY = 0;
            }
        }
        
        
    }
    
    public static void ArrangeObject(string objectType)
    {
        //親を設定
        Transform parent = usersArrangeObjects.transform;
        if (objectPrefabDictionary.ContainsKey(objectType))
        {
            ajustingY = objectPrefabDictionary[objectType].gameObject.transform.localPosition.y - 1.5f; //1.5はGroundの厚さ
            arrangingObject = Instantiate(objectPrefabDictionary[objectType], arrangingPosition, Quaternion.identity, parent);
            arrangingRotation = Quaternion.identity;
            Cursor.visible = false;
        }
        else
        {
            Debug.Log("エラー");
        }
        
    }
}
