using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawnManager : BaseManager
{
    public GameObject[] Enemys; //적 프리팹 배열
    public GameObject[] Boss; //보스 프리팹 배열
    public GameObject Meteor; //메테오 프리팹
    public float SpawnXSize = 10;
    private float SpawnY = 6.6f;
    public float CoolDownTime; //쿨타임
    private float CoolTimeAmplitude;
    public float MeteorCoolDownTime;
    private float MeteorCoolTimeAmp;

    private int _spawnCount = 0; //지금까지 나온 몹 수
    public int BossSpawnCount = 10; //보스가 나오기까지 몹 출현 수

    private bool _bSpawnBoss = false; //보스 스폰중 여부
    public override void Init(GameManager gameManager) //초기화 함수
    {
        base.Init(gameManager); //게임매니저 초기화
        StartCoroutine(SpawnEnemy()); //스폰 코루틴 호출
        StartCoroutine(SpawnMeteor());
    }

    IEnumerator SpawnEnemy()
    {
        CoolTimeAmplitude = CoolDownTime * 0.2f;

        while (!_bSpawnBoss) //보스가 출현중이 아니라면
        {
            float randomCoolTime = CoolDownTime + Random.Range(-CoolTimeAmplitude, CoolTimeAmplitude);
            yield return new WaitForSeconds(Mathf.Max(randomCoolTime, 0.1f)); //쿨타임만큼 대기

            int randomEnemy = Random.Range(0, Enemys.Length); //어느 적을 스폰시킬것인지

            Instantiate(Enemys[randomEnemy], new Vector3(Random.Range(-SpawnXSize / 2, SpawnXSize / 2), SpawnY), Quaternion.identity); //randomPosition번째 스폰위치에 randomEnemy번 적 생성

            _spawnCount += 1; //_spawnCount 증가


            if (_spawnCount >= BossSpawnCount) //_spawnCount가BossSpawnCount보다 크거나 같다면
            {
                _bSpawnBoss = true; //_bSpawnBoss 비활성화
                int bossIndex = GameInstance.instance.CurrentStageLevel - 1;
                Instantiate(Boss[bossIndex], new Vector3(0, SpawnY + 1, 0f), Quaternion.identity);
                //첫번째 스폰 위치의 1 위로 보스 생성
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