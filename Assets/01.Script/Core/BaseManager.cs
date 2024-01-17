using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CharacterManager�� ���� ���� �Ŵ����� �����ϱ� ���� Ŭ����, GameManager�� ������ �� �ֵ��� ��
//�� ��ü���� �������̵��Ͽ� ����� �ʱ�ȭ �Լ��� �����ϴ� ��
public class BaseManager : MonoBehaviour
{
    protected GameManager _gameManager;  //�ڽŰ� �θ� Ŭ���������� ���� _gameManager�� ���� �����ϵ��� �ϴ� ����


    public GameManager GameManager { get { return _gameManager; } } //�ܺο��� GameManager�� get�ϱ� ���� �Լ�

    //���� �Ŵ������� �ʱ�ȭ �Լ��� ����� �Ǵ� �����Լ�
    //��ü�鿡�� ������ GameManager�� �ʱ�ȭ �ϱ� ����
    public virtual void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}