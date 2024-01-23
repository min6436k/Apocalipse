using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item //�������� ������ ���� Ŭ����, ����ü�� �ص� �������?
{
    public EnumTypes.ItemName Name; 
    public GameObject Prefab; //�������� �̸��� �������� �� ����
}

//public struct _Item 
//{
//    public EnumTypes.ItemName Name;
//    public GameObject Prefab;
//}
//�� �ǵ鿩���� �����ҵ���. Ŭ������ Find�� ���� �� null�� �ݸ� ����ü�� ����Ʈ���� �׻� �ֱ� ����.

public class BaseItem : MonoBehaviour
{
    protected void Update()
    {
        transform.Translate(new Vector3(0, -0.005f, 0f)); //�������� ������Ʈ���� ����Ʈ��. ���� �������� �������� �ʱ� ������ �޸𸮴����� ����
    }

    public virtual void OnGetItem(CharacterManager characterManager) { } //������ ȹ�� ó�� �Լ��� �⺻��
}

public class ItemManager : MonoBehaviour
{
    public List<Item> Items = new List<Item>(); //������ Ŭ������ ���� �迭
    public void SpawnItem(EnumTypes.ItemName name, Vector3 position) //�������� ������ ������Ű�� �Լ�
    {
        Item foundItem = Items.Find(item => item.Name == name); //enum�̸��� ���� Ŭ���� �迭���� ���� ã����

        if (foundItem != null) //���� �����Ѵٸ�
        {
            GameObject itemPrefab = foundItem.Prefab; //������ ���� ������Ʈ ������ ã�� ���� �Ҵ�, �����ѹ� ����?
            GameObject instance = Instantiate(itemPrefab, position, Quaternion.identity); //�ִϸ��̼��� ���� �ν��Ͻ� ���� �Ҵ�
            if (instance.GetComponent<Animator>()) instance.GetComponent<Animator>().SetInteger("State", ((int)name)); //������ ������ ���� �ִϸ��̼� ����

        }
    }

    public void SpawnRandomItem(int min, int max, Vector3 position) //�������� �������� �����ϱ� ���� �Լ�
    {
        if (Random.Range(0, 3) == 0) //0~2�Ƿ��� ������ 0�� ���´ٸ�
        {
            SpawnItem(EnumTypes.ItemName.AddOn, position); //����ȸ�� ������ ����
            return; //�������� ��������Ƿ� �Լ� ����
        }

        //�Ű����� ������ ���� �ּڰ��̶�� - �� �������� �����ϱ� ���� Ȯ�� ���.
        if (Random.Range(min, max) == min) 
        {
            int randomInt = Random.Range(0, (int)EnumTypes.ItemName.Last-1); //���� �ε��� ���� ����
            EnumTypes.ItemName itemName = (EnumTypes.ItemName)randomInt; //enum���� ��ȯ
            SpawnItem(itemName, position); //�� ���� ���� ������ ����
            //������ ��ġ�� ������, �����ε带 ����ϴ°� �������� ����.
            //SpawnRandomItem(position);
        }
    }

    public void SpawnRandomItem(Vector3 position) //���������Լ��� �����ε�
    {
        int randomInt = Random.Range(0, (int)EnumTypes.ItemName.Last-1); //==
        EnumTypes.ItemName itemName = (EnumTypes.ItemName)randomInt;
        SpawnItem(itemName, position);
    }
}