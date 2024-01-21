using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawnManager : BaseManager
{
    public GameObject[] Enemys; //�� ������ �迭
    public GameObject[] Boss; //���� ������ �迭
    public GameObject Meteor; //���׿� ������
    public float SpawnXSize = 10;
    private float SpawnY = 6.6f;
    public float CoolDownTime; //��Ÿ��
    private float CoolTimeAmplitude;
    public float MeteorCoolDownTime;
    private float MeteorCoolTimeAmp;

    private int _spawnCount = 0; //���ݱ��� ���� �� ��
    public int BossSpawnCount = 10; //������ ��������� �� ���� ��

    private bool _bSpawnBoss = false; //���� ������ ����
    public override void Init(GameManager gameManager) //�ʱ�ȭ �Լ�
    {
        base.Init(gameManager); //���ӸŴ��� �ʱ�ȭ
        StartCoroutine(SpawnEnemy()); //���� �ڷ�ƾ ȣ��
        StartCoroutine(SpawnMeteor());
    }

    IEnumerator SpawnEnemy()
    {
        CoolTimeAmplitude = CoolDownTime * 0.2f;

        while (!_bSpawnBoss) //������ �������� �ƴ϶��
        {
            float randomCoolTime = CoolDownTime + Random.Range(-CoolTimeAmplitude, CoolTimeAmplitude);
            yield return new WaitForSeconds(Mathf.Max(randomCoolTime, 0.1f)); //��Ÿ�Ӹ�ŭ ���

            int randomEnemy = Random.Range(0, Enemys.Length); //��� ���� ������ų������

            Instantiate(Enemys[randomEnemy], new Vector3(Random.Range(-SpawnXSize / 2, SpawnXSize / 2), SpawnY), Quaternion.identity); //randomPosition��° ������ġ�� randomEnemy�� �� ����

            _spawnCount += 1; //_spawnCount ����


            if (_spawnCount >= BossSpawnCount) //_spawnCount��BossSpawnCount���� ũ�ų� ���ٸ�
            {
                _bSpawnBoss = true; //_bSpawnBoss ��Ȱ��ȭ
                int bossIndex = GameInstance.instance.CurrentStageLevel - 1;
                Instantiate(Boss[bossIndex], new Vector3(0, SpawnY + 1, 0f), Quaternion.identity);
                //ù��° ���� ��ġ�� 1 ���� ���� ����
            }
        }

    }

    IEnumerator SpawnMeteor()
    {
        MeteorCoolTimeAmp = MeteorCoolDownTime * 0.2f;

        while (!_bSpawnBoss)
        {
            yield return new WaitForSeconds(MeteorCoolDownTime + Random.Range(-MeteorCoolTimeAmp, MeteorCoolTimeAmp));
            Instantiate(Meteor, new Vector3(Random.Range(-SpawnXSize / 2, SpawnXSize / 2), SpawnY), Quaternion.identity);
        }
    }

}