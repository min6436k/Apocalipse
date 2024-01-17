using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//스킬들의 부모 클래스가 될 클래스
//스킬들이 공통으로 가진 쿨타임과 관련된 처리
public class BaseSkill : MonoBehaviour
{
    protected CharacterManager _characterManager;

    public float CooldownTime;
    public float CurrentTime;
    public bool bIsCoolDown = false;
    //쿨타임을 재거나 쿨타임 중인지를 확인하는 변수들

    //characterManager를 초기화하는 함수
    public void Init(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    //매 업데이트 마다 현재 쿨다운 중일 때 쿨타임 시간을 줄이고, 쿨다운 상태를 해제하기 위해 사용됨
    public void Update()
    {
        if (bIsCoolDown)
        {
            CurrentTime -= Time.deltaTime;
            if (CurrentTime <= 0)
            {
                bIsCoolDown = false;
            }
        }
    }

    public bool IsAvailable()
    {
        // 스킬이 쿨다운 중인지 확인
        return !bIsCoolDown;
    }

    //하위 스킬들의 활성화의 기반이 될 가상 함수
    //쿨다운 상태를 설정하고 쿨타임 시간을 최대로 정하는 기능
    public virtual void Activate()
    {
        bIsCoolDown = true;
        CurrentTime = CooldownTime;
    }

    //쿨다운 상태를 초기화시키는 함수
    public void InitCoolDown()
    {
        bIsCoolDown = false;
        CurrentTime = 0;
    }

}