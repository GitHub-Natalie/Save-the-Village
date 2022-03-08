using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ImageTimer HarvestTimer; // ������ ����� �������
    public ImageTimer EatingTimer; // ������ ����������� �������
    public Image RaidTimerImg; // ������ ������ ������
    public Image PeasantTimerImg; // ������ �������� ��������
    public Image WarriorTimerImg; // ������ �������� ������
    
    public AudioSource HarvestTimerAudio; // ���� ����� ����� �������
    public AudioSource EatingTimerAudio; // ���� ����� ����������� �������
    public AudioSource RaidTimerAudio; // ���� ��������� ������
    public AudioSource PeasantTimerAudio; // ���� �������� ��������
    public AudioSource WarriorTimerAudio; // ���� �������� ������

    public Text resourcesText; // ��������� ���� ��������
    public Text nextRaidText; // ��������� ���� ���������� ������ � ��������� ������
    public Text resultText; // ��������� ���� ����������� ���� ����� ��������/���������
    public Text purposeOfWheatText; // ��������� ���� ���������� ������ ���� (���������� �������)
    public Text purposeOfSurvivalText; // ��������� ���� ���������� ������ ���� (���������)
    public Text purposeOfImprovementText; // ��������� ���� ���������� ������� ���� (���������)

    public Button peasantButton; // ������ ����� ��������
    public Button warriorButton; // ������ ����� ������

    public Button improveCastle; // ������ ��������� �����
    public Button improveBarrack; // ������ ��������� �������
    public Button improveWheatField; // ������ ��������� ��������� �����

    public GameObject GameOverScreen; // ����� ���������
    public GameObject GameWinScreen; // ����� ������
    public GameObject ResultPanel; // ������ ����������� ����
    public GameObject Background; // ������ ��� (��������)
    public GameObject improveMark01; // ����������� ���������� ������� �������
    public GameObject improveMark02; // ����������� ���������� ������� �������
    public GameObject improveMark03; // ����������� ���������� �������� �������

    public int peasantCount; // ������� ���������� ��������
    public int warriorCount; // ������� ���������� ������
    private int totalWarrior; // ����� ���������� ������
    public int wheatCount; // ������� ���������� �������
    public int totalWheat; // ����� ���������� ������������� �������

    public int peasantLimit; // ����� ���������� ��������
    public int warriorLimit; // ����� ���������� ������
    public int wheatLimit; // ����� ���������� �������

    public int peasantCost; // ��������� ����� �����������
    public int warriorCost; // ��������� ����� �����
    public int improveCost; // ��������� ���������

    public int eatingOfPeasant; // ����������� ������� ������������ �� ����
    public int eatingOfWarrior; // ����������� ������� ������ �� ����

    public int productionOfWheat; // ������������ ������� ������������ �� ����
    public int powerOfWarrior; // ���� ����� (����������� � ������)

    public float peasantCreateTime; // ����� �������� �����������
    public float warriorCreateTime; // ����� �������� �����
    public float raidMaxTime; // ����� �� ���������� ������
    public int nextRaid; // �������� ���������� ������ � ��������� ������
    public int raidIncrease; // ��������, �� ������� ������������� ���������� ������ � ��������� ������

    public int raidCount; // ���������� ��������� ������� ������
    public int improvementCount; // ���������� ������� ��������� �������

    public int wheatPurpose; // ���������� ��������� ������� ��� ������
    public int raidPurpose; // ���������� ��������� ������� ������ ��� ������
    public int improvementPurpose; // ���������� ��������� ������� ��� ������

    private float peasantTimer = -1; // �������� ������� �������� �������� � ���������� ������
    private float warriorTimer = -1; // �������� ������� �������� ������ � ���������� ������
    private float raidTimer; // ������ ������ ������ � ���������� ������

    void Start()
    {
        UpdateResourcesText();
        UpdatePurposeTexts();
        UpdatenextRaidText();
    }

    void Update()
    {
    // ������ ������ ������
        raidTimer += Time.deltaTime;
        RaidTimerImg.fillAmount = raidTimer / raidMaxTime;

        if (RaidTimerImg.fillAmount == 1)
        {
            raidTimer = 0;
            warriorCount -= (nextRaid / powerOfWarrior);
            nextRaid += raidIncrease;
            raidCount += 1;
            RaidTimerAudio.Play();
        }

    // ������ ����� �������
        if (HarvestTimer.Tick)
        {
            wheatCount += peasantCount * productionOfWheat;
            totalWheat += peasantCount * productionOfWheat;
            HarvestTimerAudio.Play();
        }

    // ������ ����������� �������
        if (EatingTimer.Tick)
        {
            wheatCount -= ((peasantCount * eatingOfPeasant) + (warriorCount * eatingOfWarrior));
            EatingTimerAudio.Play();
        }

    // ������ ����� ��������
        if (peasantTimer >= 0)
        {
            peasantTimer += Time.deltaTime;
            PeasantTimerImg.fillAmount = peasantTimer / peasantCreateTime;
        }
        if (PeasantTimerImg.fillAmount == 1)
        {
            PeasantTimerImg.fillAmount = 0;
            peasantButton.interactable = true;
            peasantCount += 1;
            peasantTimer = -1;
            PeasantTimerAudio.Play();
        }

    // ������ ����� ������
        if (warriorTimer >= 0)
        {
            warriorTimer += Time.deltaTime;
            WarriorTimerImg.fillAmount = warriorTimer / warriorCreateTime;
        }
        if (WarriorTimerImg.fillAmount == 1)
        {
            WarriorTimerImg.fillAmount = 0;
            warriorButton.interactable = true;
            warriorCount += 1;
            totalWarrior += 1;
            warriorTimer = -1;
            WarriorTimerAudio.Play();
        }

    // �������� �� ������� ��������
        if (wheatCount < peasantCost || PeasantTimerImg.fillAmount != 0 || peasantCount >= peasantLimit) peasantButton.interactable = false; // ���� ��������
        else peasantButton.interactable = true;

        if (wheatCount < warriorCost || WarriorTimerImg.fillAmount != 0 || warriorCount >= warriorLimit) warriorButton.interactable = false; // ���� ������
        else warriorButton.interactable = true;

        if (wheatCount < improveCost) // ��������� �������
        {
            improveCastle.interactable = false;
            improveBarrack.interactable = false;
            improveWheatField.interactable = false;
        }
        else
        {
            improveCastle.interactable = true;
            improveBarrack.interactable = true;
            improveWheatField.interactable = true;
        }

        if (wheatCount >= wheatLimit) wheatCount = wheatLimit; // ����� �������
        if (wheatCount <= 0) wheatCount = 0; // ���������� �������

    // ����� ���������
        if (warriorCount < 0)
        {
            Time.timeScale = 0;
            GameOverScreen.SetActive(true);
            Background.SetActive(false);
            ResultPanel.SetActive(true);
            UpdateResultText();
        }

    // ����� ������
        if (improveMark01.activeSelf && improveMark02.activeSelf && improveMark03.activeSelf)
        {
            Time.timeScale = 0;
            GameWinScreen.SetActive(true);
            Background.SetActive(false);
            ResultPanel.SetActive(true);
            UpdateResultText();
        }

    // ���������� ��������� �����
        UpdateResourcesText();
        UpdatePurposeTexts();
        UpdatenextRaidText();
    }
    /// <summary>
    /// ���� �������� �� ������� ������
    /// </summary>
    public void CreatePeasant()
    {
        wheatCount -= peasantCost;
        peasantTimer = 0;
        peasantButton.interactable = false;
    }

    /// <summary>
    /// ���� ������ �� ������� ������
    /// </summary>
    public void CreateWarrior()
    {
        wheatCount -= warriorCost;
        warriorTimer = 0;
        warriorButton.interactable = false;
    }

    /// <summary>
    /// �������� �������� ������ � ������� �� ������� ������
    /// </summary>
    public void ImproveCastle()
    {
        improvementCount += 1;
        wheatCount -= improveCost;
        peasantLimit *= 2;
        warriorLimit *= 2;
        wheatLimit *= 2;
    }

    /// <summary>
    /// �������� ������� � ������� �� ������� ������
    /// </summary>
    public void ImproveBarrack()
    {
        improvementCount += 1;
        wheatCount -= improveCost;
        warriorCost += (warriorCost / 2);
        eatingOfWarrior += (eatingOfWarrior / 2);
        powerOfWarrior += 1;
        warriorCreateTime -= 2;
    }

    /// <summary>
    /// �������� ��������� ����� � ������� �� ������� ������
    /// </summary>
    public void ImproveWheatField()
    {
        improvementCount += 1;
        wheatCount -= improveCost;
        peasantCost += (peasantCost / 2);       
        eatingOfPeasant += eatingOfPeasant;
        productionOfWheat += productionOfWheat;
        peasantCreateTime -= 1;
    }

    /// <summary>
    /// ����������� ������� �������� � �������� ����
    /// </summary>
    public void UpdateResourcesText()
    {
        resourcesText.text = peasantCount + "/" + peasantLimit + "\n\n\n" + warriorCount + "/" + warriorLimit + "\n\n\n" + wheatCount + "/" + wheatLimit;
    }

    /// <summary>
    /// ����������� ��������� ���������� ����� ����
    /// </summary>
    public void UpdatePurposeTexts()
    {
        purposeOfWheatText.text = wheatCount + "/" + wheatPurpose;
        if (wheatCount >= wheatPurpose)
        {
            purposeOfWheatText.enabled = false;
            improveMark01.SetActive(true);
        }

        purposeOfSurvivalText.text = raidCount + "/" + raidPurpose;
        if (raidCount >= raidPurpose)
        {
            purposeOfSurvivalText.enabled = false;
            improveMark02.SetActive(true);
        }

        purposeOfImprovementText.text = improvementCount + "/" + improvementPurpose;
        if (improvementCount >= improvementPurpose)
        {
            purposeOfImprovementText.enabled = false;
            improveMark03.SetActive(true);
        }
    }

    /// <summary>
    /// ����������� ���������� ������ � ��������� ������
    /// </summary>
    public void UpdatenextRaidText()
    {
        nextRaidText.text = nextRaid.ToString();
    }

    /// <summary>
    /// ����������� ���������� ���� ����� ��������/���������
    /// </summary>
    public void UpdateResultText()
    {
        resultText.text = "\n" + peasantCount + "\n\n\n" + totalWarrior + "\n\n\n" + totalWheat + "\n\n\n" + raidCount + "\n\n\n" + improvementCount;
    }

    /// <summary>
    /// ���������� ���� (����� ������� ����������)
    /// </summary>
    public void RestartGameParameters()
    {
        Time.timeScale = 1;
        peasantCount = 2;
        warriorCount = 0;
        totalWarrior = 0;
        totalWheat = 0;
        wheatCount = 10;
        peasantLimit = 30;
        warriorLimit = 10;
        wheatLimit = 2500;
        peasantCost = 4;
        warriorCost = 10;
        eatingOfPeasant = 1;
        eatingOfWarrior = 10;
        productionOfWheat = 3;
        powerOfWarrior = 1;
        peasantCreateTime = 5;
        warriorCreateTime = 8;
        nextRaid = 0;
        raidCount = 0;
        improvementCount = 0;
        peasantTimer = -1;
        PeasantTimerImg.fillAmount = 0;
        warriorTimer = -1;
        WarriorTimerImg.fillAmount = 0;
        raidTimer = 0;
        HarvestTimer.currentTime = 0;
        EatingTimer.currentTime = 0;
        purposeOfWheatText.text = wheatCount + "/" + wheatPurpose;
        purposeOfWheatText.enabled = true;
        improveMark01.SetActive(false);
        purposeOfSurvivalText.enabled = true;
        improveMark02.SetActive(false);
        purposeOfImprovementText.enabled = true;
        improveMark03.SetActive(false);
        Background.SetActive(true);
    }
}

