using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//��ų���� �θ� Ŭ������ �� Ŭ����
//��ų���� �������� ���� ��Ÿ�Ӱ� ���õ� ó��
public class BaseSkill : MonoBehaviour
{
    protected CharacterManager _characterManager;

    public float CooldownTime;
    public float CurrentTime;
    public bool bIsCoolDown = false;
    //��Ÿ���� ��ų� ��Ÿ�� �������� Ȯ���ϴ� ������

    //characterManager�� �ʱ�ȭ�ϴ� �Լ�
    public void Init(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    //�� ������Ʈ ���� ���� ��ٿ� ���� �� ��Ÿ�� �ð��� ���̰�, ��ٿ� ���¸� �����ϱ� ���� ����
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
        // ��ų�� ��ٿ� ������ Ȯ��
        return !bIsCoolDown;
    }

    //���� ��ų���� Ȱ��ȭ�� ����� �� ���� �Լ�
    //��ٿ� ���¸� �����ϰ� ��Ÿ�� �ð��� �ִ�� ���ϴ� ���
    public virtual void Activate()
    {
        bIsCoolDown = true;
        CurrentTime = CooldownTime;
    }

    //��ٿ� ���¸� �ʱ�ȭ��Ű�� �Լ�
    public void InitCoolDown()
    {
        bIsCoolDown = false;
        CurrentTime = 0;
    }

}