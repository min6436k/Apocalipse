using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//�÷��̾��� ü���� �����ϴ� �Լ�
//�� �ܿ��� �������� �浹�̳� ���� ó�� ���� �����
public class PlayerHPSystem : MonoBehaviour
{
    public int Health;
    public int MaxHealth;
    //���� ü�°� �ִ� ü���� ��� ����

    //���� �� ü���� GameInstance�� ��ü ü������ ����
    void Start()
    {
        Health = GameInstance.instance.CurrentPlayerHP;
    }

    //GameInstance�� ���� ü���� �ִ� ü������ �ʱ�ȭ
    public void InitHealth()
    {
        Health = MaxHealth;
        GameInstance.instance.CurrentPlayerHP = Health;
    }

    //�����ð� �� �������� �������� ó��
    IEnumerator HitFlick()
    {
        int flickCount = 0; // ������ Ƚ���� ����ϴ� ����

        while (flickCount < 5) // 5�� ������ ������ �ݺ�
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f); // ��������Ʈ�� ���� 0.5�� ����

            yield return new WaitForSeconds(0.1f); // 0.1�� ���

            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // ��������Ʈ�� ���� ������ ����

            yield return new WaitForSeconds(0.1f); // 0.1�� ���

            flickCount++; // ������ Ƚ�� ����
        }
    }

    //���̳� �����۰��� �浹�� ó����
    //�� �������� ��� ó���� ���� �Լ� ȣ�� � ���� ó��
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