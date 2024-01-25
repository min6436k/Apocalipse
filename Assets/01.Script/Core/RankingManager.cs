using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public Canvas RankingCanvas; //랭킹을 표시하는 캔버스
    public Canvas SetRankCanvas; //랭킹을 저장하는 캔버스

    private List<RankingEntry> rankingEntries = new List<RankingEntry>(); //RankingEntry클래스 리스트
    public TextMeshProUGUI[] Rankings = new TextMeshProUGUI[5]; //랭킹 정보를 담을 텍스트 컴포넌트 배열
    public TextMeshProUGUI CurrentPlayerScore; //CurrentPlayerScore의 텍스트 컴포넌트
    public TextMeshProUGUI InitialInputFieldText; //InitialInputFieldText의 텍스트 컴포넌트

    private string CurrentPlayerInitial;

    public void SetInitial()
    {
        SetRankCanvas.gameObject.SetActive(false); //랭킹 저장 캔버스 비활성화
        RankingCanvas.gameObject.SetActive(true); //랭킹 표시 캔버스 활성화

        CurrentPlayerInitial = InitialInputFieldText.text; //InitialInputFieldText에 입력된 값을 CurrentPlayerInitial에 저장

        SetCurrentScore();
        SortRanking();
        UpdateRankingUI();
    }
    public void MainMenu()
    {
        if( SceneManager.GetActiveScene().name == "Result") GameInstance.instance.Initlnstance();
        SceneManager.LoadScene("MainMenu"); //메인메뉴 씬 불러오기
    }

    public void DataReset()
    {
        PlayerPrefs.DeleteAll();
    }

    public void MainMenuRanking()
    {
        RankingCanvas.gameObject.SetActive(true); //랭킹 표시 캔버스 활성화

        for (int i = 0; i < 5; i++) //랭크에 저장할 최대 갯수인 5번 반복
        {
            int currentScore = PlayerPrefs.GetInt(i + "BestScore"); //PlayerPrefs에 i + "BestScore"키로 저장된 int데이터를 불러와 변수에 저장
            string currentName = PlayerPrefs.GetString(i + "BestName"); //PlayerPrefs에 i + "BestName"키로 저장된 string데이터를 불러와 변수에 저장
            if (currentName == "")
                currentName = "None"; //플레이어가 랭크을 저장할 때 이름을 입력하지 않았을 시 None으로 이름 표기 

            rankingEntries.Add(new RankingEntry(currentScore, currentName)); //의 리스트에 해당 인덱스의 점수와 이름을 인스턴스변수로 하여 값 추가
        }

        SortRanking(); //랭킹 리스트 정렬, 정리 함수

        for (int i = 0; i < Rankings.Length; i++) //Rankings 길이만큼 반복
        {
            if (i < rankingEntries.Count) //저장된 RankingEntry클래스의 수보다 작다면 (등록된 랭킹의 수까지만 해당)
            {
                Rankings[i].text = $"{i + 1} {rankingEntries[i].Name} : {rankingEntries[i].Score}"; //해당 인덱스 랭킹 텍스트를 등수+이름+점수로 변경
            }
            else
            {
                Rankings[i].text = $"{i + 1} -"; //해당 인덱스 랭킹 텍스트를 등수+ -로 하여 데이터 없음을 표기
            }
        }
    }

    void SetCurrentScore()
    {
        rankingEntries.Clear(); //rankingEntries 리스트 초기화

        for (int i = 0; i < 5; i++) //=
        {
            int currentScore = PlayerPrefs.GetInt(i + "BestScore");
            string currentName = PlayerPrefs.GetString(i + "BestName");
            if (currentName == "")
                currentName = "None";

            rankingEntries.Add(new RankingEntry(currentScore, currentName));
        }

        // 현재 플레이어의 점수와 이름을 가져와 랭킹에 등록
        int currentPlayerScore = GameInstance.instance.Score; //현재 캐릭터의 점수 불러오기
        string currentPlayerName = CurrentPlayerInitial; //유저가 입력한 이름 불러오기

        // 현재 플레이어의 점수가 랭킹에 등록 가능한지 확인
        if (IsScoreEligibleForRanking(currentPlayerScore))
        {
            rankingEntries.Add(new RankingEntry(currentPlayerScore, currentPlayerName));
        }
    }

    bool IsScoreEligibleForRanking(int currentPlayerScore)
    {
        // 랭킹에 등록 가능한지 확인 (예: 상위 5위까지만 등록 가능하도록 설정)
        return rankingEntries.Count < 5 || currentPlayerScore > rankingEntries.Min(entry => entry.Score);
        //rankingEntries의 수가 5보다 작거나, rankingEntries을 람다식으로 검사해 가장 작은 값보다 입력받은 값이 크다면 true 반환
    }

    void SortRanking()
    {
        // 내림차순으로 정렬
        rankingEntries = rankingEntries.OrderByDescending(entry => entry.Score).ToList();
        //rankingEntries를 각 요소의 Score를 기준으로 내림차순 정렬
        //아직 자세히 모르겠으나, 반환 형식이 IEnumerable<T>이기 때문에 ToList()를 사용해 바꿔줘야함

        // 랭킹이 5개를 초과하면 가장 낮은 점수를 가진 항목을 제거
        if (rankingEntries.Count > 5)
        {
            rankingEntries.RemoveAt(rankingEntries.Count - 1);
            //이미 내림차순 정렬이 되었으므로, rankingEntries.Count - 1을 통해 가장 인덱스가 큰 요소를 지워서 가장 낮은 점수 제거
        }
    }

    void UpdateRankingUI()
    {
        CurrentPlayerScore.text = $"{CurrentPlayerInitial} : {GameInstance.instance.Score}"; //플레이어의 점수를 표시하는 텍스트를 플레이어이름과 점수로 업데이트

        for (int i = 0; i < Rankings.Length; i++) //=
        {
            if (i < rankingEntries.Count)
            {
                Rankings[i].text = $"{i + 1} {rankingEntries[i].Name} : {rankingEntries[i].Score}";
            }
            else
            {
                Rankings[i].text = $"{i + 1} -";
            }
        }

        // PlayerPrefs 업데이트
        for (int i = 0; i < rankingEntries.Count; i++) //rankingEntries의 리스트 갯수만큼 반복
        {
            PlayerPrefs.SetInt(i + "BestScore", rankingEntries[i].Score); //PlayerPrefs에 int로 i + "BestScore"키와 해당 인덱스 RankingEntry의 점수 저장
            PlayerPrefs.SetString(i + "BestName", rankingEntries[i].Name); //PlayerPrefs에 string으로 i + "BestName"키와 해당 인덱스 RankingEntry의 이름 저장
        }
    }
}

public class RankingEntry //랭킹 데이터를 담을 클래스
{
    public int Score { get; set; }
    public string Name { get; set; } //점수와 이름에 대한 getter와 setter 지정

    public RankingEntry(int score, string name) //RankingEntry 클래스의 생성자
    {
        Score = score;
        Name = name;
    }
}