using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//CharacterManager�� ���� ���� Manager���� �����ϴ� Ŭ������, ������ ���� �ڵ� ���� �����
//���������� ������Ʈ ���� Manager��ũ��Ʈ�� ����� ��� Ŭ������ GameManager�� ���� �ٸ� ��κ��� ��ü���� ������ �� �ְ� ��
public class GameManager : MonoBehaviour
{
    static public GameManager Instance; //�̱����� ���� ���� ����

    public CharacterManager CharacterManager;
    public MapManager MapManager;
    public EnemySpawnManager EnemySpawnManager;
    public ItemManager ItemManager;

    public Canvas StageResultCanvas;
    public TMP_Text CurrentScoreText;
    public TMP_Text TimeText;

    [HideInInspector] public bool bStageCleared = false;
    //����Manager���� ���� ����, ���������� Ŭ���� ���θ� ���� bool����

    private void Awake()  // ��ü ������ ���� ���� (�׷��� �̱����� ���⼭ ����)
    {
        if (Instance == null)  // �� �ϳ��� �����ϰԲ�
        {
            Instance = this;  // ��ü ������ instance�� �ڱ� �ڽ��� �־���
        }
        else Destroy(this.gameObject);
    }

    //�ٸ� Ŭ�������� CharacterManager�� �����ϱ� ���� �Լ�
    public PlayerCharacter GetPlayerCharacter() { return CharacterManager.Player.GetComponent<PlayerCharacter>(); }

    //ó�� ȣ��� �� CharacterManager�� ������ �� CharacterManager�� gameManager�� �ڽ����� �ʱ�ȭ
    void Start()
    {
        if (CharacterManager == null) { return; }
        CharacterManager.Init(this);

        MapManager.Init(this);
        EnemySpawnManager.Init(this);
    }

    //������ ���������� 1�������� ���� �ε�
    public void GameStart()
    {
        SceneManager.LoadScene("Stage1");
        //���� ��ư�� ������ �� ù��° �������� �ҷ�����
    }

    public void EnemyDies()
    {
        AddScore(10);
        //���� ���� ������ ���ھ� ������ ����
    }

    public void StageClear()
    {
        AddScore(500); //���ھ� ������ ����

        float gameStartTime = GameInstance.instance.GameStartTime; //������ ������������ �ð�
        int score = GameInstance.instance.Score; //���� ����

        // �ɸ� �ð�
        int elapsedTime = Mathf.FloorToInt(Time.time - gameStartTime); //����ð����� ������ �ð��� ���� ���

        // �������� Ŭ���� ���â : ����, �ð�
        StageResultCanvas.gameObject.SetActive(true); //StageResultCanvas Ȱ��ȭ
        CurrentScoreText.text = "CurrentScore : " + score; //���� �ؽ�Ʈ ���
        TimeText.text = "ElapsedTime : " + elapsedTime; //����ð� �ؽ�Ʈ ���

        bStageCleared = true; //�������� Ŭ���� ���� true

        // 5�� �ڿ� ���� ��������
        StartCoroutine(LoadNextStageAfterDelay(5f));
    }

    IEnumerator LoadNextStageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); //delay �Ű����� �ʸ�ŭ ���

        switch (GameInstance.instance.CurrentStageLevel)
        {
            case 1: //���� ���������� 1�̶��
                SceneManager.LoadScene("Stage2"); //2�������� �� �ҷ�����
                GameInstance.instance.CurrentStageLevel = 2; //���� �������� 2�� ����
                break; //switch�� ����

            case 2:
                SceneManager.LoadScene("Result");//��� �� �ҷ�����
                break;//switch�� ����
        }
    }

    public void AddScore(int score)
    {
        GameInstance.instance.Score += score; //���� �ν��Ͻ��� ���ھ �Ű�������ŭ ���ϱ�
    }

    private void Update()
    {
        // �� ���� ��� �� ���� ����.
        if (Input.GetKeyUp(KeyCode.F1))
        {
            // ��� Enemy ã��
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //���� ���� ��� Enemy �±׸� ���� ������Ʈ�� ������ ����
            foreach (GameObject obj in enemies) //enemies ���� ��� ���ҿ� ���� �ݺ�
            {
                Enemy enemy = obj?.GetComponent<Enemy>(); //obj�� null �� �ƴ϶�� Enemy ������Ʈ�� ������ ����
                enemy?.Dead(); //ã�� ���� enemy������Ʈ�� null�� �ƴ϶�� ���� ó��
            }
        }

        // ���� ���׷��̵带 �ְ� �ܰ�� ���
        if (Input.GetKeyUp(KeyCode.F2))
        {
            GetPlayerCharacter().CurrentWeaponLevel = 3; //ĳ������ ���� ���� ������ 3���� ���� (GetPlayerCharacter().MaxWeaponLevel �� ���� �ִ뷹�� ����)
            GameInstance.instance.CurrentPlayerWeaponLevel = GetPlayerCharacter().CurrentWeaponLevel; //���� �ν��Ͻ��� ������Ʈ
        }

        // ��ų�� ��Ÿ�� �� Ƚ���� �ʱ�ȭ ��Ų��
        if (Input.GetKeyUp(KeyCode.F3))
        {
            GetPlayerCharacter().InitSkillCoolDown(); //ĳ���Ϳ� ����� Base ��ų�� �ʱ�ȭ �Լ� ȣ��
        }

        // ������ �ʱ�ȭ
        if (Input.GetKeyUp(KeyCode.F4))
        {
            GetPlayerCharacter().GetComponent<PlayerHPSystem>().InitHealth(); //�÷��̾��� ü�� �ʱ�ȭ �Լ� ȣ��
        }

        // ���� �ʱ�ȭ
        if (Input.GetKeyUp(KeyCode.F5))
        {
            GetPlayerCharacter().GetComponent<PlayerFuelSystem>().InitFuel(); //�÷��̾��� ���� �ʱ�ȭ �Լ� ȣ��
        }

        // �������� Ŭ����
        if (Input.GetKeyUp(KeyCode.F6))
        {
            StageClear(); //�������� Ŭ���� �Լ� ȣ��
        }
    }
}