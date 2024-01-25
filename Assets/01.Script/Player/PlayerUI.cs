using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CoolDownText //스킬의 정보를 담기 위한 클래스
{
    public EnumTypes.PlayerSkill Skill; //스킬 enum 정보
    public TextMeshProUGUI Text; //스킬의 쿨타임 텍스트
}

public class PlayerUI : MonoBehaviour
{
    public Image[] HealthImages = new Image[3]; //체력 잔량 이미지 배열
    public Slider FuelSlider; //연료 슬라이더

    public TextMeshProUGUI SkillCooldownNoticeText; //CooldownNotice 텍스트
    public List<CoolDownText> SkillCooldownTexts; //스킬의 정보를 담기 위한 클래스의 배열

    private void Update()
    {
        UpdateHealth();
        UpdateSkills();
        UpdateFuel();
    }

    private void UpdateHealth()
    {
        int health = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerHPSystem>().Health; //인스턴스를 통해 체력 불러오기

        for (int i = 0; i < HealthImages.Length; i++)  //남은 체력까지 반복
        {
            HealthImages[i].gameObject.SetActive(i < health); //현재 체력에 따라 체력 잔량 이미지 표기 - 이미지 배열에 인덱스로 사용하기 위해 < 연산자 사용(-1)
        }
    }

    private void UpdateSkills()
    {
        foreach(var item in SkillCooldownTexts) //스킬 클래스 배열 수만큼 반복
        {
            bool isCoolDown = GameManager.Instance.GetPlayerCharacter().Skills[item.Skill].bIsCoolDown; //인스턴스를 통해 쿨타임 불러오기
            float currentTime = GameManager.Instance.GetPlayerCharacter().Skills[item.Skill].CurrentTime;//인스턴스를 통해 현재 쿨타임 시간 불러오기

            item.Text.gameObject.SetActive(isCoolDown); //쿨다운 여부에 따라 Active 조절
            item.Text.text = $"{Mathf.RoundToInt(currentTime)}"; //현재 쿨타임 시간을 반올림하여 텍스트에 업데이트
        }
    }

    private void UpdateFuel()
    {
        FuelSlider.GetComponent<Slider>().value = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerFuelSystem>().Fuel / 100f; //연료의 값에 따라 슬라이더 업데이트
    }

    public void NoticeSkillCooldown(EnumTypes.PlayerSkill playerSkill) //외부에서 내부의 코루틴을 작동시키기 위한 함수
    {
        StartCoroutine(NoticeText(playerSkill)); 
    }

    IEnumerator NoticeText(EnumTypes.PlayerSkill playerSkill)
    {
        SkillCooldownNoticeText.gameObject.SetActive(true); //CooldownNotice 오브젝트 활성화 
        SkillCooldownNoticeText.text = $"{playerSkill.ToString()} Skill is Cooldown"; //텍스트에 스킬 enum의 이름을 더해 쿨타임여부 표기

        yield return new WaitForSeconds(3);//3초 대기

        SkillCooldownNoticeText.gameObject.SetActive(false);//CooldownNotice 오브젝트 비활성화 
    }
}
