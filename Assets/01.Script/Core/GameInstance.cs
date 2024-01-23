using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

//컴포넌트가 생성된 이후 스테이지 전환 등으로 인해 씬을 이동하더라도 유지되어야 하는 변수들을 담는 역할
//싱글톤으로 설계되어, 프로젝트 내의 모든 객체들이 클래스 내의 변수에 접근할수 있게 함
public class GameInstance : MonoBehaviour
{
    public static GameInstance instance; //싱글톤을 위한 정적 변수

    public float GameStartTime = 0f;
    public int Score = 0;
    public int CurrentStageLevel = 1;

    public int CurrentPlayerWeaponLevel = 0;
    public int CurrentPlayerHP = 3;
    public float CurrentPlayerFuel = 100f;
    public int CurrentAddOnCount = 0;
    //현재 체력이나 시작 시간 등 씬 이동 시에도 사라지지 않아야 하는 변수들


    //클래스가 생성되면서 싱글톤 패턴을 구현
    //instance가 null이라면 자신을 넣고, 그렇지 않다면 자신을 파괴함
    //GameStartTime에 현재 시간을 입력함
    private void Awake()
    {
        if (instance == null)  // 단 하나만 존재하게끔
        {
            instance = this;  // 객체 생성시 instance에 자기 자신을 넣어줌
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        GameStartTime = Time.time;
    }
}