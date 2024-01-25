using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CoolDownText //��ų�� ������ ��� ���� Ŭ����
{
    public EnumTypes.PlayerSkill Skill; //��ų enum ����
    public TextMeshProUGUI Text; //��ų�� ��Ÿ�� �ؽ�Ʈ
}

public class PlayerUI : MonoBehaviour
{
    public Image[] HealthImages = new Image[3]; //ü�� �ܷ� �̹��� �迭
    public Slider FuelSlider; //���� �����̴�

    public TextMeshProUGUI SkillCooldownNoticeText; //CooldownNotice �ؽ�Ʈ
    public List<CoolDownText> SkillCooldownTexts; //��ų�� ������ ��� ���� Ŭ������ �迭

    private void Update()
    {
        UpdateHealth();
        UpdateSkills();
        UpdateFuel();
    }

    private void UpdateHealth()
    {
        int health = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerHPSystem>().Health; //�ν��Ͻ��� ���� ü�� �ҷ�����

        for (int i = 0; i < HealthImages.Length; i++)  //���� ü�±��� �ݺ�
        {
            HealthImages[i].gameObject.SetActive(i < health); //���� ü�¿� ���� ü�� �ܷ� �̹��� ǥ�� - �̹��� �迭�� �ε����� ����ϱ� ���� < ������ ���(-1)
        }
    }

    private void UpdateSkills()
    {
        foreach(var item in SkillCooldownTexts) //��ų Ŭ���� �迭 ����ŭ �ݺ�
        {
            bool isCoolDown = GameManager.Instance.GetPlayerCharacter().Skills[item.Skill].bIsCoolDown; //�ν��Ͻ��� ���� ��Ÿ�� �ҷ�����
            float currentTime = GameManager.Instance.GetPlayerCharacter().Skills[item.Skill].CurrentTime;//�ν��Ͻ��� ���� ���� ��Ÿ�� �ð� �ҷ�����

            item.Text.gameObject.SetActive(isCoolDown); //��ٿ� ���ο� ���� Active ����
            item.Text.text = $"{Mathf.RoundToInt(currentTime)}"; //���� ��Ÿ�� �ð��� �ݿø��Ͽ� �ؽ�Ʈ�� ������Ʈ
        }
    }

    private void UpdateFuel()
    {
        FuelSlider.GetComponent<Slider>().value = GameManager.Instance.GetPlayerCharacter().GetComponent<PlayerFuelSystem>().Fuel / 100f; //������ ���� ���� �����̴� ������Ʈ
    }

    public void NoticeSkillCooldown(EnumTypes.PlayerSkill playerSkill) //�ܺο��� ������ �ڷ�ƾ�� �۵���Ű�� ���� �Լ�
    {
        StartCoroutine(NoticeText(playerSkill)); 
    }

    IEnumerator NoticeText(EnumTypes.PlayerSkill playerSkill)
    {
        SkillCooldownNoticeText.gameObject.SetActive(true); //CooldownNotice ������Ʈ Ȱ��ȭ 
        SkillCooldownNoticeText.text = $"{playerSkill.ToString()} Skill is Cooldown"; //�ؽ�Ʈ�� ��ų enum�� �̸��� ���� ��Ÿ�ӿ��� ǥ��

        yield return new WaitForSeconds(3);//3�� ���

        SkillCooldownNoticeText.gameObject.SetActive(false);//CooldownNotice ������Ʈ ��Ȱ��ȭ 
    }
}
