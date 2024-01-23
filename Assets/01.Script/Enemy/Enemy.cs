using System.Collections;
using UnityEngine;

public class Enemy : BaseCharacter
{
    public float Health = 3f;
    public float AttackDamage = 1f;
    bool bIsDead = false;
    public bool bMustSpawnItem = false;

    public GameObject ExplodeFX;

    private Color originColor;

    void Start()
    {
        originColor = gameObject.GetComponent<SpriteRenderer>().color;
        if (gameObject.name.Contains("Boss")) bMustSpawnItem = true;
    }

    public void Dead()
    {
        if (!bIsDead) //죽지 않은 상태라면
        {
            GameManager.Instance.EnemyDies(); //완성 프로젝트를 보면 스코어를 증가시키는 함수지만, 현재는 의미없다

            if (!bMustSpawnItem) //확정적인 아이템 드랍이 필요없다면
                GameManager.Instance.ItemManager.SpawnRandomItem(0, 3, transform.position); //0~2, 즉 1/3확률로 아이템 랜덤 스폰
            else
                GameManager.Instance.ItemManager.SpawnRandomItem(transform.position); //확정으로 아이템 랜덤 스폰

            bIsDead = true; //사망 처리

            Instantiate(ExplodeFX, transform.position, Quaternion.identity); //사망 이펙트 생성
            Destroy(gameObject); //오브젝트 제거
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Health -= 1f;
            //GameManager.Instance.SoundManager.PlaySFX("EnemyHit");

            if (Health <= 0f)
            {
                Dead();
            }

            StartCoroutine(HitFlick());
            Destroy(collision.gameObject);
        }
    }
    IEnumerator HitFlick()
    {
        int flickCount = 0; // 깜박인 횟수를 기록하는 변수

        while (flickCount < 1) // 1번 깜박일 때까지 반복
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);

            yield return new WaitForSeconds(0.1f); // 0.1초 대기

            GetComponentInChildren<SpriteRenderer>().color = originColor;

            yield return new WaitForSeconds(0.1f); // 0.1초 대기

            flickCount++; // 깜박인 횟수 증가
        }
    }
    public IEnumerator FreezeTime(int stopTime)
    {
        GetComponentInChildren<SpriteRenderer>().color -= new Color(0.5f, 0, 0, 0);
        yield return new WaitForSeconds(stopTime);
        GetComponentInChildren<SpriteRenderer>().color = originColor;

    }
}

public interface Freeze
{
    IEnumerator FreezeTime(int stopTime);
}