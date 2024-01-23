using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPattern1 : MonoBehaviour,Freeze
{
    public float MoveSpeed;
    public float Amplitude; // ������ ����(���Ʒ� �̵� �Ÿ�)

    [HideInInspector]
    public bool movingUp = true;
    [HideInInspector]
    public Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float verticalMovement = MoveSpeed * Time.deltaTime;

        // ���� �̵� ���̸鼭 ���� ��ġ�� ���� ��ġ���� �������� ���� ���
        if (movingUp && transform.position.x < startPosition.x + Amplitude)
        {
            transform.position += new Vector3(verticalMovement, 0f, 0f);
        }
        // �Ʒ��� �̵� ���̸鼭 ���� ��ġ�� ���� ��ġ���� �������� ū ���
        else if (!movingUp && transform.position.x > startPosition.x - Amplitude)
        {
            transform.position -= new Vector3(verticalMovement, 0f, 0f);
        }
        // ���� ������ ��� ��� �̵� ������ �ݴ�� ����
        else
        {
            movingUp = !movingUp;
        }

        transform.position -= new Vector3(0f, MoveSpeed * Time.deltaTime, 0f);
    }

    public IEnumerator FreezeTime(int stopTime)
    {
        if (MoveSpeed != 0)
        {
            float temp = MoveSpeed;
            MoveSpeed = 0;
            yield return new WaitForSeconds(stopTime);
            MoveSpeed = temp;
        }

        yield return null;
    }
}