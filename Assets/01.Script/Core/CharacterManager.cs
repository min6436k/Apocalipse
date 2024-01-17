using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//�÷��̾�� ���õ� ��ũ��Ʈ���� �����ϰ� �� ManagerŬ����
//�� Ŭ������ ���� ĳ���� ������Ʈ�� ��ġ ���� ������ ��� ����
public class CharacterManager : BaseManager
{
    [SerializeField]
    private BaseCharacter _player;
    public BaseCharacter Player => _player;
    //BaseCharacterŬ���� ������ ������� ������, �� ������ set���� get�� �Ҽ� �ֵ��� �ϴ� ���ٽ� (���� ������ ���� ����)

    //BaseManager�� �ʱ�ȭ �Լ��� ���� _player������ �ڽ����� �ʱ�ȭ
    public override void Init(GameManager gameManager)
    {
        base.Init(gameManager);
        _player.Init(this);
    }
}