using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item //아이템의 정보를 담을 클래스, 구조체로 해도 상관없나?
{
    public EnumTypes.ItemName Name; 
    public GameObject Prefab; //아이템의 이름과 프리팹이 들어갈 변수
}

//public struct _Item 
//{
//    public EnumTypes.ItemName Name;
//    public GameObject Prefab;
//}
//잘 건들여보면 가능할듯함. 클래스는 Find가 없을 시 null인 반면 구조체는 디폴트값이 항상 있기 때문.

public class BaseItem : MonoBehaviour
{
    protected void Update()
    {
        transform.Translate(new Vector3(0, -0.005f, 0f)); //아이템을 업데이트마다 떨어트림. 현재 아이템을 지워주지 않기 때문에 메모리누수가 있음
    }

    public virtual void OnGetItem(CharacterManager characterManager) { } //아이템 획득 처리 함수의 기본형
}

public class ItemManager : MonoBehaviour
{
    public List<Item> Items = new List<Item>(); //아이템 클래스를 담을 배열
    public void SpawnItem(EnumTypes.ItemName name, Vector3 position) //아이템을 실제로 스폰시키는 함수
    {
        Item foundItem = Items.Find(item => item.Name == name); //enum이름을 통해 클래스 배열에서 값을 찾아줌

        if (foundItem != null) //값이 존재한다면
        {
            GameObject itemPrefab = foundItem.Prefab; //생성할 게임 오브젝트 변수에 찾은 값을 할당, 매직넘버 방지?
            GameObject instance = Instantiate(itemPrefab, position, Quaternion.identity); //애니메이션을 위해 인스턴스 변수 할당
            if (instance.GetComponent<Animator>()) instance.GetComponent<Animator>().SetInteger("State", ((int)name)); //아이템 정보에 따른 애니메이션 실행

        }
    }

    public void SpawnRandomItem(int min, int max, Vector3 position) //아이템을 랜덤으로 생성하기 위한 함수
    {
        if (Random.Range(0, 3) == 0) //0~2의랜덤 정수중 0이 나온다면
        {
            SpawnItem(EnumTypes.ItemName.AddOn, position); //연료회복 아이템 생성
            return; //아이템을 드랍했으므로 함수 종료
        }

        //매개변수 사이의 값이 최솟값이라면 - 즉 아이템을 생성하기 위한 확률 계산.
        if (Random.Range(min, max) == min) 
        {
            int randomInt = Random.Range(0, (int)EnumTypes.ItemName.Last-1); //랜덤 인덱스 변수 생성
            EnumTypes.ItemName itemName = (EnumTypes.ItemName)randomInt; //enum으로 변환
            SpawnItem(itemName, position); //위 값에 따라 아이템 생성
            //내용이 겹치기 때문에, 오버로드를 재귀하는게 가독성이 좋다.
            //SpawnRandomItem(position);
        }
    }

    public void SpawnRandomItem(Vector3 position) //랜덤생성함수의 오버로드
    {
        int randomInt = Random.Range(0, (int)EnumTypes.ItemName.Last-1); //==
        EnumTypes.ItemName itemName = (EnumTypes.ItemName)randomInt;
        SpawnItem(itemName, position);
    }
}