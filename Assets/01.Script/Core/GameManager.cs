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
    }

    //게임을 시작했을때 1스테이지 씬을 로드
    public void GameStart()
    {
        SceneManager.LoadScene("Stage1");
    }
}