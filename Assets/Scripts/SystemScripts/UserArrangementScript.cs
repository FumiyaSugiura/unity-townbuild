using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.TMP_ExampleScript_01;

//User's Arrange Objects�ɃA�^�b�`
public class UserArrangementScript : MonoBehaviour
{
    public static Dictionary<string, GameObject> objectPrefabDictionary = new Dictionary<string, GameObject>(); //Prefab�̎���
    [SerializeField] string[] objectPrefabKeyArray; //Prefab�̎�����key��ۊǂ���z��
    [SerializeField] GameObject[] objectPrefabValueArray; //Prefab�̎�����value��ۊǂ���z��

    List<GameObject> usersArrangedObjectList = new List<GameObject>(); //���������I�u�W�F�N�g�̃��X�g
    List<GameObject> usersArrangedObjectPositionList; //���������I�u�W�F�N�g�̈ʒu���X�g

    public static GameObject arrangingObject; //�z�u���̃I�u�W�F�N�g
    public static Vector3 arrangingPosition; //�z�u���i�ꏊ����O��Object�̏ꏊ�j
    public static float ajustingY; //�I�u�W�F�N�g�̔z�u�������I�u�W�F�N�g�̎�ނ��Ƃɒ�������l
    public static Quaternion arrangingRotation; //�z�u���̉�]
    [SerializeField] float rotateMouseSensitivity = 5f; //��]������̃}�E�X���x

    public static GameObject usersArrangeObjects; //parent�i�܂�wrapper�j

    // Start is called before the first frame update
    void Start()
    {
        //objectPrefabDictionary�̐ݒ�
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
                Debug.Log($"CAUTON: �L�['{key}'�ɃI�u�W�F�N�g'{value}'���ݒ肳��Ă��邽�ߎ��s�ł��܂���");
                break;
            }

            if (i == objectPrefabKeyArray.Length - 1)
            {
                Debug.Log("�I�u�W�F�N�g�𐳏�ɐݒ肵�܂���");
            }
        }

        //User's Arrange Objects��e�Ɏw��
        usersArrangeObjects = this.gameObject;

        //������
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
        //�e��ݒ�
        Transform parent = usersArrangeObjects.transform;
        if (objectPrefabDictionary.ContainsKey(objectType))
        {
            ajustingY = objectPrefabDictionary[objectType].gameObject.transform.localPosition.y - 1.5f; //1.5��Ground�̌���
            arrangingObject = Instantiate(objectPrefabDictionary[objectType], arrangingPosition, Quaternion.identity, parent);
            arrangingRotation = Quaternion.identity;
            Cursor.visible = false;
        }
        else
        {
            Debug.Log("�G���[");
        }
        
    }
}
