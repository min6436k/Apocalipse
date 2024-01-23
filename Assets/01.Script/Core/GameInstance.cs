using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

//������Ʈ�� ������ ���� �������� ��ȯ ������ ���� ���� �̵��ϴ��� �����Ǿ�� �ϴ� �������� ��� ����
//�̱������� ����Ǿ�, ������Ʈ ���� ��� ��ü���� Ŭ���� ���� ������ �����Ҽ� �ְ� ��
public class GameInstance : MonoBehaviour
{
    public static GameInstance instance; //�̱����� ���� ���� ����

    public float GameStartTime = 0f;
    public int Score = 0;
    public int CurrentStageLevel = 1;

    public int CurrentPlayerWeaponLevel = 0;
    public int CurrentPlayerHP = 3;
    public float CurrentPlayerFuel = 100f;
    public int CurrentAddOnCount = 0;
    //���� ü���̳� ���� �ð� �� �� �̵� �ÿ��� ������� �ʾƾ� �ϴ� ������


    //Ŭ������ �����Ǹ鼭 �̱��� ������ ����
    //instance�� null�̶�� �ڽ��� �ְ�, �׷��� �ʴٸ� �ڽ��� �ı���
    //GameStartTime�� ���� �ð��� �Է���
    private void Awake()
    {
        if (instance == null)  // �� �ϳ��� �����ϰԲ�
        {
            instance = this;  // ��ü ������ instance�� �ڱ� �ڽ��� �־���
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        GameStartTime = Time.time;
    }
}