using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerCharacter�� �θ� Ŭ����
//PlayerCharacter�� ��ũ��Ʈ�� CharacterManager�� ������ �� �ְ� ��
public class BaseCharacter : MonoBehaviour
{
    private CharacterManager _characterManager;
    public CharacterManager CharacterManager => _characterManager;
    //CharacterManagerŬ���� ������ ������� ������, �� ������ set���� get�� �Ҽ� �ֵ��� �ϴ� ���ٽ� (���� ������ ���� ����)
    /*
      public CharacterManager CharacterManager
      {
        get { return _characterManager; }
      }
     */

    //CharacterManager�� �ʱ�ȭ�ϱ� ���� ���� �Լ�
    public virtual void Init(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }
}