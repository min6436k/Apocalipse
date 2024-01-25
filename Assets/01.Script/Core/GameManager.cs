using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//CharacterManager와 같은 하위 Manager들을 관리하는 클래스로, 게임의 시작 코드 또한 담당함
//최종적으로 프로젝트 내의 Manager스크립트와 연결된 모든 클래스는 GameManager를 통해 다른 대부분의 객체들을 참조할 수 있게 됨
public class GameManager : MonoBehaviour
{
    static public GameManager Instance; //싱글톤을 위한 정적 변수

    public CharacterManager CharacterManager;
    public MapManager MapManager;
    public EnemySpawnManager EnemySpawnManager;
    public ItemManager ItemManager;

    public Canvas StageResultCanvas;
    public TMP_Text CurrentScoreText;
    public TMP_Text TimeText;

    [HideInInspector] public bool bStageCleared = false;
    //하위Manager들을 담을 변수, 스테이지의 클리어 여부를 담을 bool변수

    private void Awake()  // 객체 생성시 최초 실행 (그래서 싱글톤을 여기서 생성)
    {
        if (Instance == null)  // 단 하나만 존재하게끔
        {
            Instance = this;  // 객체 생성시 instance에 자기 자신을 넣어줌
        }
        else Destroy(this.gameObject);
    }

    //다른 클래스에서 CharacterManager를 참조하기 위한 함수
    public PlayerCharacter GetPlayerCharacter() { return CharacterManager.Player.GetComponent<PlayerCharacter>(); }

    //처음 호출될 때 CharacterManager가 존재할 시 CharacterManager의 gameManager를 자신으로 초기화
    void Start()
    {
        if (CharacterManager == null) { return; }
        CharacterManager.Init(this);

        MapManager.Init(this);
        EnemySpawnManager.Init(this);
    }

    //게임을 시작했을때 1스테이지 씬을 로드
    public void GameStart()
    {
        SceneManager.LoadScene("Stage1");
        //시작 버튼을 눌렀을 때 첫번째 스테이지 불러오기
    }

    public void EnemyDies()
    {
        AddScore(10);
        //적이 죽을 때마다 스코어 일정량 증가
    }

    public void StageClear()
    {
        AddScore(500); //스코어 일정량 증가

        float gameStartTime = GameInstance.instance.GameStartTime; //게임을 시작했을때의 시간
        int score = GameInstance.instance.Score; //현재 점수

        // 걸린 시간
        int elapsedTime = Mathf.FloorToInt(Time.time - gameStartTime); //현재시간에서 시작한 시간을 빼서 계산

        // 스테이지 클리어 결과창 : 점수, 시간
        StageResultCanvas.gameObject.SetActive(true); //StageResultCanvas 활성화
        CurrentScoreText.text = "CurrentScore : " + score; //점수 텍스트 출력
        TimeText.text = "ElapsedTime : " + elapsedTime; //경과시간 텍스트 출력

        bStageCleared = true; //스테이지 클리어 변수 true

        // 5초 뒤에 다음 스테이지
        StartCoroutine(LoadNextStageAfterDelay(5f));
    }

    IEnumerator LoadNextStageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); //delay 매개변수 초만큼 대기

        switch (GameInstance.instance.CurrentStageLevel)
        {
            case 1: //현재 스테이지가 1이라면
                SceneManager.LoadScene("Stage2"); //2스테이지 씬 불러오기
                GameInstance.instance.CurrentStageLevel = 2; //현재 스테이지 2로 변경
                break; //switch문 종료

            case 2:
                SceneManager.LoadScene("Result");//결과 씬 불러오기
                break;//switch문 종료
        }
    }

    public void AddScore(int score)
    {
        GameInstance.instance.Score += score; //게임 인스턴스의 스코어에 매개변수만큼 더하기
    }

    private void Update()
    {
        // 맵 내에 모든 적 유닛 제거.
        if (Input.GetKeyUp(KeyCode.F1))
        {
            // 모든 Enemy 찾기
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //현재 씬의 모든 Enemy 태그를 가진 오브젝트를 변수로 저장
            foreach (GameObject obj in enemies) //enemies 내의 모든 원소에 대해 반복
            {
                Enemy enemy = obj?.GetComponent<Enemy>(); //obj가 null 이 아니라면 Enemy 컴포넌트를 변수에 저장
                enemy?.Dead(); //찾은 적의 enemy컴포넌트가 null이 아니라면 죽음 처리
            }
        }

        // 공격 업그레이드를 최고 단계로 상승
        if (Input.GetKeyUp(KeyCode.F2))
        {
            GetPlayerCharacter().CurrentWeaponLevel = 3; //캐릭터의 현재 무기 레벨을 3으로 변경 (GetPlayerCharacter().MaxWeaponLevel 로 쓰면 최대레벨 가능)
            GameInstance.instance.CurrentPlayerWeaponLevel = GetPlayerCharacter().CurrentWeaponLevel; //게임 인스턴스에 업데이트
        }

        // 스킬의 쿨타임 및 횟수를 초기화 시킨다
        if (Input.GetKeyUp(KeyCode.F3))
        {
            GetPlayerCharacter().InitSkillCoolDown(); //캐릭터에 연결된 Base 스킬의 초기화 함수 호출
        }

        // 내구도 초기화
        if (Input.GetKeyUp(KeyCode.F4))
        {
            GetPlayerCharacter().GetComponent<PlayerHPSystem>().InitHealth(); //플레이어의 체력 초기화 함수 호출
        }

        // 연료 초기화
        if (Input.GetKeyUp(KeyCode.F5))
        {
            GetPlayerCharacter().GetComponent<PlayerFuelSystem>().InitFuel(); //플레이어의 연료 초기화 함수 호출
        }

        // 스테이지 클리어
        if (Input.GetKeyUp(KeyCode.F6))
        {
            StageClear(); //스테이지 클리어 함수 호출
        }
    }
}