using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public Canvas RankingCanvas; //��ŷ�� ǥ���ϴ� ĵ����
    public Canvas SetRankCanvas; //��ŷ�� �����ϴ� ĵ����

    private List<RankingEntry> rankingEntries = new List<RankingEntry>(); //RankingEntryŬ���� ����Ʈ
    public TextMeshProUGUI[] Rankings = new TextMeshProUGUI[5]; //��ŷ ������ ���� �ؽ�Ʈ ������Ʈ �迭
    public TextMeshProUGUI CurrentPlayerScore; //CurrentPlayerScore�� �ؽ�Ʈ ������Ʈ
    public TextMeshProUGUI InitialInputFieldText; //InitialInputFieldText�� �ؽ�Ʈ ������Ʈ

    private string CurrentPlayerInitial;

    public void SetInitial()
    {
        SetRankCanvas.gameObject.SetActive(false); //��ŷ ���� ĵ���� ��Ȱ��ȭ
        RankingCanvas.gameObject.SetActive(true); //��ŷ ǥ�� ĵ���� Ȱ��ȭ

        CurrentPlayerInitial = InitialInputFieldText.text; //InitialInputFieldText�� �Էµ� ���� CurrentPlayerInitial�� ����

        SetCurrentScore();
        SortRanking();
        UpdateRankingUI();
    }
    public void MainMenu()
    {
        if( SceneManager.GetActiveScene().name == "Result") GameInstance.instance.Initlnstance();
        SceneManager.LoadScene("MainMenu"); //���θ޴� �� �ҷ�����
    }

    public void DataReset()
    {
        PlayerPrefs.DeleteAll();
    }

    public void MainMenuRanking()
    {
        RankingCanvas.gameObject.SetActive(true); //��ŷ ǥ�� ĵ���� Ȱ��ȭ

        for (int i = 0; i < 5; i++) //��ũ�� ������ �ִ� ������ 5�� �ݺ�
        {
            int currentScore = PlayerPrefs.GetInt(i + "BestScore"); //PlayerPrefs�� i + "BestScore"Ű�� ����� int�����͸� �ҷ��� ������ ����
            string currentName = PlayerPrefs.GetString(i + "BestName"); //PlayerPrefs�� i + "BestName"Ű�� ����� string�����͸� �ҷ��� ������ ����
            if (currentName == "")
                currentName = "None"; //�÷��̾ ��ũ�� ������ �� �̸��� �Է����� �ʾ��� �� None���� �̸� ǥ�� 

            rankingEntries.Add(new RankingEntry(currentScore, currentName)); //�� ����Ʈ�� �ش� �ε����� ������ �̸��� �ν��Ͻ������� �Ͽ� �� �߰�
        }

        SortRanking(); //��ŷ ����Ʈ ����, ���� �Լ�

        for (int i = 0; i < Rankings.Length; i++) //Rankings ���̸�ŭ �ݺ�
        {
            if (i < rankingEntries.Count) //����� RankingEntryŬ������ ������ �۴ٸ� (��ϵ� ��ŷ�� �������� �ش�)
            {
                Rankings[i].text = $"{i + 1} {rankingEntries[i].Name} : {rankingEntries[i].Score}"; //�ش� �ε��� ��ŷ �ؽ�Ʈ�� ���+�̸�+������ ����
            }
            else
            {
                Rankings[i].text = $"{i + 1} -"; //�ش� �ε��� ��ŷ �ؽ�Ʈ�� ���+ -�� �Ͽ� ������ ������ ǥ��
            }
        }
    }

    void SetCurrentScore()
    {
        rankingEntries.Clear(); //rankingEntries ����Ʈ �ʱ�ȭ

        for (int i = 0; i < 5; i++) //=
        {
            int currentScore = PlayerPrefs.GetInt(i + "BestScore");
            string currentName = PlayerPrefs.GetString(i + "BestName");
            if (currentName == "")
                currentName = "None";

            rankingEntries.Add(new RankingEntry(currentScore, currentName));
        }

        // ���� �÷��̾��� ������ �̸��� ������ ��ŷ�� ���
        int currentPlayerScore = GameInstance.instance.Score; //���� ĳ������ ���� �ҷ�����
        string currentPlayerName = CurrentPlayerInitial; //������ �Է��� �̸� �ҷ�����

        // ���� �÷��̾��� ������ ��ŷ�� ��� �������� Ȯ��
        if (IsScoreEligibleForRanking(currentPlayerScore))
        {
            rankingEntries.Add(new RankingEntry(currentPlayerScore, currentPlayerName));
        }
    }

    bool IsScoreEligibleForRanking(int currentPlayerScore)
    {
        // ��ŷ�� ��� �������� Ȯ�� (��: ���� 5�������� ��� �����ϵ��� ����)
        return rankingEntries.Count < 5 || currentPlayerScore > rankingEntries.Min(entry => entry.Score);
        //rankingEntries�� ���� 5���� �۰ų�, rankingEntries�� ���ٽ����� �˻��� ���� ���� ������ �Է¹��� ���� ũ�ٸ� true ��ȯ
    }

    void SortRanking()
    {
        // ������������ ����
        rankingEntries = rankingEntries.OrderByDescending(entry => entry.Score).ToList();
        //rankingEntries�� �� ����� Score�� �������� �������� ����
        //���� �ڼ��� �𸣰�����, ��ȯ ������ IEnumerable<T>�̱� ������ ToList()�� ����� �ٲ������

        // ��ŷ�� 5���� �ʰ��ϸ� ���� ���� ������ ���� �׸��� ����
        if (rankingEntries.Count > 5)
        {
            rankingEntries.RemoveAt(rankingEntries.Count - 1);
            //�̹� �������� ������ �Ǿ����Ƿ�, rankingEntries.Count - 1�� ���� ���� �ε����� ū ��Ҹ� ������ ���� ���� ���� ����
        }
    }

    void UpdateRankingUI()
    {
        CurrentPlayerScore.text = $"{CurrentPlayerInitial} : {GameInstance.instance.Score}"; //�÷��̾��� ������ ǥ���ϴ� �ؽ�Ʈ�� �÷��̾��̸��� ������ ������Ʈ

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

        // PlayerPrefs ������Ʈ
        for (int i = 0; i < rankingEntries.Count; i++) //rankingEntries�� ����Ʈ ������ŭ �ݺ�
        {
            PlayerPrefs.SetInt(i + "BestScore", rankingEntries[i].Score); //PlayerPrefs�� int�� i + "BestScore"Ű�� �ش� �ε��� RankingEntry�� ���� ����
            PlayerPrefs.SetString(i + "BestName", rankingEntries[i].Name); //PlayerPrefs�� string���� i + "BestName"Ű�� �ش� �ε��� RankingEntry�� �̸� ����
        }
    }
}

public class RankingEntry //��ŷ �����͸� ���� Ŭ����
{
    public int Score { get; set; }
    public string Name { get; set; } //������ �̸��� ���� getter�� setter ����

    public RankingEntry(int score, string name) //RankingEntry Ŭ������ ������
    {
        Score = score;
        Name = name;
    }
}