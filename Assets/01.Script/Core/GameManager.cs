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
    }

    //������ ���������� 1�������� ���� �ε�
    public void GameStart()
    {
        SceneManager.LoadScene("Stage1");
    }
}