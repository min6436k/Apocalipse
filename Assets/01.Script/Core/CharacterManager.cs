using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//플레이어와 관련된 스크립트들이 참조하게 될 Manager클래스
//이 클래스를 통해 캐릭터 오브젝트의 위치 등의 정보를 사용 가능
public class CharacterManager : BaseManager
{
    [SerializeField]
    private BaseCharacter _player;
    public BaseCharacter Player => _player;
    //BaseCharacter클래스 정보를 담기위한 변수와, 그 변수를 set없이 get만 할수 있도록 하는 람다식 (서로 데이터 참조 가능)

    //BaseManager의 초기화 함수에 더해 _player변수를 자신으로 초기화
    public override void Init(GameManager gameManager)
    {
        base.Init(gameManager);
        _player.Init(this);
    }
}