using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : LvlUpStat
{
    [Header("LvlUpPanel")]
    [SerializeField] private GameObject losePanel;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText;

    private UpgradeCategory[] categories;

    private HashSet<Button> _animatingButtons = new HashSet<Button>();

    private void Start()
    {
        losePanel.SetActive(false);
        Time.timeScale = 1f;
        InitializeCategories();
        SetupUI();
        UpdateMoneyText();
    }

    private void InitializeCategories()
    {
        categories = new UpgradeCategory[5];

        categories[0] = new UpgradeCategory
        {
            Type = UpgradeType.ValueCoin,
            Buttons = new Button[] { myButtonLvlUpValueCoin1, myButtonLvlUpValueCoin2, myButtonLvlUpValueCoin3, myButtonLvlUpValueCoin4 },
            Costs = new int[] { costMBLVLUVC1, costMBLVLUVC2, costMBLVLUVC3, costMBLVLUVC4 },
            Buffs = new float[] { bufMBLVLUVC1, bufMBLVLUVC2, bufMBLVLUVC3, bufMBLVLUVC4 },
            CostTexts = new TextMeshProUGUI[] { ballsMBLVLUVC1, ballsMBLVLUVC2, ballsMBLVLUVC3, ballsMBLVLUVC4 },
            CurrentLevel = 0
        };

        categories[1] = new UpgradeCategory
        {
            Type = UpgradeType.SpavnCoin,
            Buttons = new Button[] { myButtonLvlUpSpavnCoin1, myButtonLvlUpSpavnCoin2, myButtonLvlUpSpavnCoin3, myButtonLvlUpSpavnCoin4 },
            Costs = new int[] { costMBLVLUSC1, costMBLVLUSC2, costMBLVLUSC3, costMBLVLUSC4 },
            Buffs = new float[] { bufMBLVLUSC1, bufMBLVLUSC2, bufMBLVLUSC3, bufMBLVLUSC4 },
            CostTexts = new TextMeshProUGUI[] { ballsMBLVLUSC1, ballsMBLVLUSC2, ballsMBLVLUSC3, ballsMBLVLUSC4 },
            CurrentLevel = 0
        };

        categories[2] = new UpgradeCategory
        {
            Type = UpgradeType.SpawnInterval,
            Buttons = new Button[] { myButtonLvlUpSpawnInterval1, myButtonLvlUpSpawnInterval2, myButtonLvlUpSpawnInterval3, myButtonLvlUpSpawnInterval4 },
            Costs = new int[] { costMBLVLUSI1, costMBLVLUSI2, costMBLVLUSI3, costMBLVLUSI4 },
            Buffs = new float[] { bufMBLVLUSI1, bufMBLVLUSI2, bufMBLVLUSI3, bufMBLVLUSI4 },
            CostTexts = new TextMeshProUGUI[] { ballsMBLVLUSI1, ballsMBLVLUSI2, ballsMBLVLUSI3, ballsMBLVLUSI4 },
            CurrentLevel = 0
        };

        categories[3] = new UpgradeCategory
        {
            Type = UpgradeType.SpeedPlayer,
            Buttons = new Button[] { myButtonLvlUpSpeedPlayer1, myButtonLvlUpSpeedPlayer2, myButtonLvlUpSpeedPlayer3, myButtonLvlUpSpeedPlayer4 },
            Costs = new int[] { costMBLVLUSP1, costMBLVLUSP2, costMBLVLUSP3, costMBLVLUSP4 },
            Buffs = new float[] { bufMBLVLUSP1, bufMBLVLUSP2, bufMBLVLUSP3, bufMBLVLUSP4 },
            CostTexts = new TextMeshProUGUI[] { ballsMBLVLUSP1, ballsMBLVLUSP2, ballsMBLVLUSP3, ballsMBLVLUSP4 },
            CurrentLevel = 0
        };

        categories[4] = new UpgradeCategory
        {
            Type = UpgradeType.MagnetPlayer,
            Buttons = new Button[] { myButtonLvlUpMagnetPlayer1, myButtonLvlUpMagnetPlayer2, myButtonLvlUpMagnetPlayer3, myButtonLvlUpMagnetPlayer4 },
            Costs = new int[] { costMBLVLUMP1, costMBLVLUMP2, costMBLVLUMP3, costMBLVLUMP4 },
            Buffs = new float[] { bufMBLVLUMP1, bufMBLVLUMP2, bufMBLVLUMP3, bufMBLVLUMP4 },
            CostTexts = new TextMeshProUGUI[] { ballsMBLVLUMP1, ballsMBLVLUMP2, ballsMBLVLUMP3, ballsMBLVLUMP4 },
            CurrentLevel = 0
        };
    }

    private void SetupUI()
    {
        for (int c = 0; c < categories.Length; c++)
        {
            var cat = categories[c];

            for (int i = 0; i < 4; i++)
            {
                if (cat.Buttons[i] != null)
                {
                    cat.Buttons[i].gameObject.SetActive(i == cat.CurrentLevel);
                    cat.Buttons[i].interactable = true;

                    int categoryIndex = c;
                    cat.Buttons[i].onClick.RemoveAllListeners();
                    cat.Buttons[i].onClick.AddListener(() => BuyUpgrade(categoryIndex));
                }

                if (cat.CostTexts[i] != null)
                {
                    cat.CostTexts[i].text = cat.Costs[i].ToString();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            losePanel.SetActive(true);
            Time.timeScale = 0f;
            UpdateMoneyText();
        }
    }

    public void CloseShopOnClick()
    {
        losePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BuyUpgrade(int categoryIndex)
    {
        UpgradeCategory cat = categories[categoryIndex];
        int level = cat.CurrentLevel;

        if (level >= 4) return;

        int cost = cat.Costs[level];
        Button clickedButton = cat.Buttons[level];

        if (WalletManager.CurrentNumber >= cost)
        {
            WalletManager.CurrentNumber -= cost;
            ApplyBuff(cat.Type, cat.Buffs[level]);
            UpdateMoneyText();

            clickedButton.interactable = false;
            ColorBlock colors = clickedButton.colors;
            Color boughtColor = new Color(0.4f, 0.4f, 0.4f, 1f);
            colors.normalColor = boughtColor;
            colors.highlightedColor = boughtColor;
            colors.pressedColor = boughtColor;
            colors.disabledColor = boughtColor;
            clickedButton.colors = colors;

            if (cat.CostTexts[level] != null)
            {
                cat.CostTexts[level].text = "КУПЛЕНО";
            }

            cat.CurrentLevel++;

            if (cat.CurrentLevel < 4)
            {
                cat.Buttons[cat.CurrentLevel].gameObject.SetActive(true);
            }
        }
        else
        {
            if (!_animatingButtons.Contains(clickedButton))
            {
                StartCoroutine(AnimateNoMoney(clickedButton));
            }
        }
    }

    private IEnumerator AnimateNoMoney(Button btn)
    {
        _animatingButtons.Add(btn);
        btn.interactable = false;

        float duration = 0.4f; 
        float maxScaleMultiplier = 1.15f; 

        Vector3 originalScale = btn.transform.localScale;
        ColorBlock originalColors = btn.colors;
        Color originalNormalColor = originalColors.normalColor;

        float elapsed = 0f;

        while (elapsed < duration)
        {

            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;

 
            float waveFactor = Mathf.Sin(t * Mathf.PI);

            Vector3 newScale = originalScale * (1f + waveFactor * (maxScaleMultiplier - 1f));
            btn.transform.localScale = newScale;

            ColorBlock currentColors = btn.colors;
            currentColors.normalColor = Color.Lerp(originalNormalColor, Color.red, waveFactor);
            currentColors.highlightedColor = currentColors.normalColor;
            currentColors.pressedColor = currentColors.normalColor;
            btn.colors = currentColors;

            yield return null;
        }

        
        btn.transform.localScale = originalScale;
        btn.colors = originalColors;

        btn.interactable = true;
        _animatingButtons.Remove(btn);
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = WalletManager.CurrentNumber.ToString();
        }
    }

    private void ApplyBuff(UpgradeType type, float buff)
    {
        switch (type)
        {
            case UpgradeType.ValueCoin:
                CoinCherecter.coinValue += (int)buff;
                break;
            case UpgradeType.SpavnCoin:
                CoinManager.maxCoin += (int)buff;
                break;
            case UpgradeType.SpawnInterval:
                CoinManager.spawnInterval = buff;
                break;
            case UpgradeType.SpeedPlayer:
                PlayerController.walkSpeed = buff;
                break;
            case UpgradeType.MagnetPlayer:
                PlayerController.zoneRadius += buff;
                break;
        }
    }
}

public class UpgradeCategory
{
    public UpgradeType Type;
    public Button[] Buttons;
    public int[] Costs;
    public float[] Buffs;
    public TextMeshProUGUI[] CostTexts;
    public int CurrentLevel;
}

public enum UpgradeType
{
    ValueCoin,
    SpavnCoin,
    SpawnInterval,
    SpeedPlayer,
    MagnetPlayer
}