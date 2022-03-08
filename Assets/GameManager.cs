using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ImageTimer HarvestTimer; // Таймер сбора пшеницы
    public ImageTimer EatingTimer; // Таймер потребления пшеницы
    public Image RaidTimerImg; // Таймер набега врагов
    public Image PeasantTimerImg; // Таймер создания крестьян
    public Image WarriorTimerImg; // Таймер создания воинов
    
    public AudioSource HarvestTimerAudio; // Звук цикла сбора пшеницы
    public AudioSource EatingTimerAudio; // Звук цикла потребления пшеницы
    public AudioSource RaidTimerAudio; // Звук нападения врагов
    public AudioSource PeasantTimerAudio; // Звук создания крестьян
    public AudioSource WarriorTimerAudio; // Звук создания воинов

    public Text resourcesText; // Текстовое поле ресурсов
    public Text nextRaidText; // Текстовое поле количества врагов в следующем набеге
    public Text resultText; // Текстовое поле результатов игры после выигрыша/проигрыша
    public Text purposeOfWheatText; // Текстовое поле выполнения первой цели (накопление пшеницы)
    public Text purposeOfSurvivalText; // Текстовое поле выполнения второй цели (выживание)
    public Text purposeOfImprovementText; // Текстовое поле выполнения третьей цели (улучшение)

    public Button peasantButton; // Кнопка найма крестьян
    public Button warriorButton; // Кнопка найма воинов

    public Button improveCastle; // Кнопка улучшения замка
    public Button improveBarrack; // Кнопка улучшения казармы
    public Button improveWheatField; // Кнопка улучшения пшеничной фермы

    public GameObject GameOverScreen; // Сцена проигрыша
    public GameObject GameWinScreen; // Сцена победы
    public GameObject ResultPanel; // Панель результатов игры
    public GameObject Background; // Задний фон (картинка)
    public GameObject improveMark01; // Обозначение выполнения первого задания
    public GameObject improveMark02; // Обозначение выполнения второго задания
    public GameObject improveMark03; // Обозначение выполнения третьего задания

    public int peasantCount; // Текущее количество крестьян
    public int warriorCount; // Текущее количество воинов
    private int totalWarrior; // Общее количество воинов
    public int wheatCount; // Текущее количество пшеницы
    public int totalWheat; // Общее количество произведенной пшеницы

    public int peasantLimit; // Лимит количества крестьян
    public int warriorLimit; // Лимит количества воинов
    public int wheatLimit; // Лимит количества пшеницы

    public int peasantCost; // Стоимость найма крестьянина
    public int warriorCost; // Стоимость найма воина
    public int improveCost; // Стоимость улучшения

    public int eatingOfPeasant; // Потребление пшеницы крестьянином за цикл
    public int eatingOfWarrior; // Потребление пшеницы воином за цикл

    public int productionOfWheat; // производство пшеницы крестьянином за цикл
    public int powerOfWarrior; // Мощь воина (соотношение к врагам)

    public float peasantCreateTime; // Время создания крестьянина
    public float warriorCreateTime; // Время создания воина
    public float raidMaxTime; // Время до следующего набега
    public int nextRaid; // Значение количества врагов в следующем набеге
    public int raidIncrease; // Значение, на которое увеличивается количество врагов в следующем забеге

    public int raidCount; // Количество пережитых набегов врагов
    public int improvementCount; // Количество текущих улучшений деревни

    public int wheatPurpose; // Количество требуемой пшеницы для победы
    public int raidPurpose; // Количество пережитых набегов врагов для победы
    public int improvementPurpose; // Количество улучшений деревни для победы

    private float peasantTimer = -1; // Значение таймера создания крестьян в неактивном режиме
    private float warriorTimer = -1; // Значение таймера создания воинов в неактивном режиме
    private float raidTimer; // Таймер набега врагов в неактивном режиме

    void Start()
    {
        UpdateResourcesText();
        UpdatePurposeTexts();
        UpdatenextRaidText();
    }

    void Update()
    {
    // Таймер набега врагов
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

    // Таймер сбора пшеницы
        if (HarvestTimer.Tick)
        {
            wheatCount += peasantCount * productionOfWheat;
            totalWheat += peasantCount * productionOfWheat;
            HarvestTimerAudio.Play();
        }

    // Таймер потребления пшеницы
        if (EatingTimer.Tick)
        {
            wheatCount -= ((peasantCount * eatingOfPeasant) + (warriorCount * eatingOfWarrior));
            EatingTimerAudio.Play();
        }

    // Кнопка найма крестьян
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

    // Кнопка найма воинов
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

    // Проверка на наличие ресурсов
        if (wheatCount < peasantCost || PeasantTimerImg.fillAmount != 0 || peasantCount >= peasantLimit) peasantButton.interactable = false; // Найм крестьян
        else peasantButton.interactable = true;

        if (wheatCount < warriorCost || WarriorTimerImg.fillAmount != 0 || warriorCount >= warriorLimit) warriorButton.interactable = false; // Найм воинов
        else warriorButton.interactable = true;

        if (wheatCount < improveCost) // Улучшение деревни
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

        if (wheatCount >= wheatLimit) wheatCount = wheatLimit; // Лимит пшеницы
        if (wheatCount <= 0) wheatCount = 0; // Отсутствие пшеницы

    // Сцена проигрыша
        if (warriorCount < 0)
        {
            Time.timeScale = 0;
            GameOverScreen.SetActive(true);
            Background.SetActive(false);
            ResultPanel.SetActive(true);
            UpdateResultText();
        }

    // Сцена победы
        if (improveMark01.activeSelf && improveMark02.activeSelf && improveMark03.activeSelf)
        {
            Time.timeScale = 0;
            GameWinScreen.SetActive(true);
            Background.SetActive(false);
            ResultPanel.SetActive(true);
            UpdateResultText();
        }

    // Обновление текстовых полей
        UpdateResourcesText();
        UpdatePurposeTexts();
        UpdatenextRaidText();
    }
    /// <summary>
    /// Найм крестьян по нажатию кнопки
    /// </summary>
    public void CreatePeasant()
    {
        wheatCount -= peasantCost;
        peasantTimer = 0;
        peasantButton.interactable = false;
    }

    /// <summary>
    /// Найм воинов по нажатию кнопки
    /// </summary>
    public void CreateWarrior()
    {
        wheatCount -= warriorCost;
        warriorTimer = 0;
        warriorButton.interactable = false;
    }

    /// <summary>
    /// Улучение главного здания в деревне по нажатию кнопки
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
    /// Улучение казармы в деревне по нажатию кнопки
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
    /// Улучение пшеничной фермы в деревне по нажатию кнопки
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
    /// Отображение текущих ресурсов в тектовом поле
    /// </summary>
    public void UpdateResourcesText()
    {
        resourcesText.text = peasantCount + "/" + peasantLimit + "\n\n\n" + warriorCount + "/" + warriorLimit + "\n\n\n" + wheatCount + "/" + wheatLimit;
    }

    /// <summary>
    /// Отображение прогресса достижения целей игры
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
    /// Отображение количества врагов в следующем набеге
    /// </summary>
    public void UpdatenextRaidText()
    {
        nextRaidText.text = nextRaid.ToString();
    }

    /// <summary>
    /// Отображение результата игры после выигрыша/проигрыша
    /// </summary>
    public void UpdateResultText()
    {
        resultText.text = "\n" + peasantCount + "\n\n\n" + totalWarrior + "\n\n\n" + totalWheat + "\n\n\n" + raidCount + "\n\n\n" + improvementCount;
    }

    /// <summary>
    /// Перезапуск игры (сброс игровых параметров)
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

