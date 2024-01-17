using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CharacterManager와 같은 하위 매니저를 관리하기 위한 클래스, GameManager로 접근할 수 있도록 함
//각 객체들이 오버라이드하여 사용할 초기화 함수를 선언하는 곳
public class BaseManager : MonoBehaviour
{
    protected GameManager _gameManager;  //자신과 부모 클래스에서만 접근 _gameManager에 접근 가능하도록 하는 변수


    public GameManager GameManager { get { return _gameManager; } } //외부에서 GameManager를 get하기 위한 함수

    //하위 매니저들의 초기화 함수의 기반이 되는 가상함수
    //객체들에게 설정된 GameManager를 초기화 하기 위함
    public virtual void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}