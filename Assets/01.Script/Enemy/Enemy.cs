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
    }

    public void Dead()
    {
        if (!bIsDead)
        {
            //GameManager.Instance.EnemyDies();

            //if (!bMustSpawnItem)
            //    GameManager.Instance.ItemManager.SpawnRandomItem(0, 3, transform.position);
            //else
            //    GameManager.Instance.ItemManager.SpawnRandomItem(transform.position);

            bIsDead = true;

            //Instantiate(ExplodeFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
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
        int flickCount = 0; // ������ Ƚ���� ����ϴ� ����
        
        while (flickCount < 1) // 1�� ������ ������ �ݺ�
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);

            yield return new WaitForSeconds(0.1f); // 0.1�� ���

            GetComponentInChildren<SpriteRenderer>().color = originColor;

            yield return new WaitForSeconds(0.1f); // 0.1�� ���

            flickCount++; // ������ Ƚ�� ����
        }
    }
    public IEnumerator FreezeTime()
    {
        GetComponentInChildren<SpriteRenderer>().color -= new Color(0.5f,0,0,0);
        yield return new WaitForSeconds(3);
        GetComponentInChildren<SpriteRenderer>().color = originColor;

    }
}

public interface Freeze
{

    IEnumerator FreezeTime();
}