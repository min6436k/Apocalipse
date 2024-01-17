using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerCharacter의 부모 클래스
//PlayerCharacter의 스크립트가 CharacterManager를 참조할 수 있게 함
public class BaseCharacter : MonoBehaviour
{
    private CharacterManager _characterManager;
    public CharacterManager CharacterManager => _characterManager;
    //CharacterManager클래스 정보를 담기위한 변수와, 그 변수를 set없이 get만 할수 있도록 하는 람다식 (서로 데이터 참조 가능)
    /*
      public CharacterManager CharacterManager
      {
        get { return _characterManager; }
      }
     */

    //CharacterManager를 초기화하기 위한 가상 함수
    public virtual void Init(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }
}