using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//플레이어의 체력을 관리하는 함수
//그 외에도 전반적인 충돌이나 무적 처리 등을 담당함
public class PlayerHPSystem : MonoBehaviour
{
    public int Health;
    public int MaxHealth;
    //현제 체력과 최대 체력을 담는 변수

    //시작 시 체력을 GameInstance의 현체 체력으로 설정
    void Start()
    {
        Health = GameInstance.instance.CurrentPlayerHP;
    }

    //GameInstance와 현재 체력을 최대 체력으로 초기화
    public void InitHealth()
    {
        Health = MaxHealth;
        GameInstance.instance.CurrentPlayerHP = Health;
    }

    //무적시간 때 시작적인 깜박임을 처리
    IEnumerator HitFlick()
    {
        int flickCount = 0; // 깜박인 횟수를 기록하는 변수

        while (flickCount < 5) // 5번 깜박일 때까지 반복
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f); // 스프라이트를 투명도 0.5로 설정

            yield return new WaitForSeconds(0.1f); // 0.1초 대기

            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // 스프라이트를 원래 투명도로 설정

            yield return new WaitForSeconds(0.1f); // 0.1초 대기

            flickCount++; // 깜박인 횟수 증가
        }
    }

    //적이나 아이템과의 충돌을 처리함
    //그 과정에서 사망 처리나 무적 함수 호출 등도 같이 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")
            && !GameManager.Instance.GetPlayerCharacter().Invincibility
            && !GameManager.Instance.bStageCleared)
        {
            Health -= 1;
            StartCoroutine(HitFlick());

            //GameManager.Instance.SoundManager.PlaySFX("Hit");

            Destroy(collision.gameObject);

            if (Health <= 0)
            {
                GameManager.Instance.GetPlayerCharacter().DeadProcess();
            }
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }

            //GameManager.Instance.SoundManager.PlaySFX("GetItem");
            Destroy(collision.gameObject);
        }

        GameInstance.instance.CurrentPlayerHP = Health;
    }
}