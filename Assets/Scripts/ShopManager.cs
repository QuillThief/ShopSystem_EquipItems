using UnityEngine;
using UnityEngine.UI;

public enum BuyOrSell { Buy, Sell }

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }
    [SerializeField] GameObject options;
    [SerializeField] GameObject shopUI;
    [SerializeField] Dialogue dialogue;
    [SerializeField] ShopItemScriptableObject[] shopItemSO;
    [SerializeField] ShopTemplate[] shopPanels;
    [SerializeField] GameObject txtSellInfo;
    [SerializeField] Button btnIncrementCoins;
    [SerializeField] Button btnDecrementCoins;
    [SerializeField] Text txtCoin;
    [SerializeField] int coins = 100;

    void Start()
    {
        Instance = this;
        txtCoin.text = $"{coins}G";
        options.SetActive(false);
        shopUI.SetActive(false);
    }

    public EquipMethod GetItemEquipMethod(string itemName)
    {
        EquipMethod equipMethod = EquipMethod.Add;

        foreach (var item in shopItemSO)
        {
            if (item.name == itemName)
            {
                equipMethod = item.equipMethod;
                break;
            }
        }

        return equipMethod;
    }

    public void IncrementCoins()
    {
        if (coins <= 5) btnDecrementCoins.interactable = true;

        coins += 10;
        txtCoin.text = $"{coins}G";

        if (coins >= 99990) btnIncrementCoins.interactable = false; //max 100000
    }

    public void DecrementCoins()
    {
        if (coins >= 99990) btnIncrementCoins.interactable = true;

        coins -= 10;
        txtCoin.text = $"{coins}G";

        if (coins <= 5) btnDecrementCoins.interactable = false; //min 0
    }

    public void EnablePlusMinusButtons()
    {
        btnIncrementCoins.gameObject.SetActive(true);
        btnDecrementCoins.gameObject.SetActive(true);
    }

    public void DisablePlusMinusButtons()
    {
        btnIncrementCoins.gameObject.SetActive(false);
        btnDecrementCoins.gameObject.SetActive(false);
    }

    public void ShowOptions()
    {
        options.SetActive(true);
    }

    public void OptionsBuyClicked()
    {
        dialogue.CloseDialogBox();
        LoadBuyPanels();
        shopUI.SetActive(true);
    }

    public void OptionsSellClicked()
    {
        dialogue.CloseDialogBox();
        LoadSellPanels();
        shopUI.SetActive(true);
    }

    public void OptionsCancelClicked()
    {
        dialogue.CloseDialogBox();
        options.SetActive(false);

        txtSellInfo.SetActive(false);
        DeactivateShopPanels();
        shopUI.SetActive(false);

        dialogue.StartDialogue(MerchantTalk.Instance.endTalk, false);
    }

    public void AddCoins(int coinCount) //supply negative argument to subtract
    {
        coins += coinCount;
        txtCoin.text = $"{coins}G";
    }

    private void LoadBuyPanels()
    {
        txtSellInfo.SetActive(false);

        for (int i = 0, length = shopItemSO.Length; i < length; i++)
        {
            ShopItemScriptableObject shopItemScriptableObject = shopItemSO[i];
            ShopTemplate shopPanel = shopPanels[i];
            
            string name = shopItemScriptableObject.name;
            int cost = shopItemScriptableObject.cost;

            shopPanel.txtName.text = name;
            shopPanel.imgSprite.sprite = shopItemScriptableObject.sprite;
            shopPanel.txtCost.text = $"{cost}G";

            if (Inventory.Instance.CheckItemIfExist(name))
            {
                shopPanel.ShowEquipped();
            }
            else
            {
                if (coins < cost) shopPanel.DeactivateButton(BuyOrSell.Buy);
                else shopPanel.ActivateButton(BuyOrSell.Buy);
            }

            shopPanel.gameObject.SetActive(true);
        }
    }

    public void LoadSellPanels()
    {
        var inventory = Inventory.Instance;
        DeactivateShopPanels();

        if (inventory.IsInventoryEmpty())
        {
            foreach (var item in shopPanels)
            {
                item.gameObject.SetActive(false);
            }

            txtSellInfo.SetActive(true);
        }
        else
        {
            for (int i = 0, length = shopItemSO.Length; i < length; i++)
            {
                ShopItemScriptableObject shopItemScriptableObject = shopItemSO[i];
                string name = shopItemScriptableObject.name;

                if (inventory.CheckItemIfExist(name))
                {
                    ShopTemplate shopPanel = shopPanels[i];
                    int cost = shopItemScriptableObject.cost;

                    shopPanel.txtName.text = name;
                    shopPanel.imgSprite.sprite = shopItemScriptableObject.sprite;
                    shopPanel.txtCost.text = $"{cost}G";

                    shopPanel.ActivateButton(BuyOrSell.Sell);
                    shopPanel.gameObject.SetActive(true);
                }
            }
        }
    }

    public void DisableOtherBuyButtonsIfApplicable()
    {
        for (int i = 0, length = shopItemSO.Length; i < length; i++)
        {
            ShopItemScriptableObject shopItemScriptableObject = shopItemSO[i];

            if (Inventory.Instance.CheckItemIfExist(shopItemScriptableObject.name)) continue;
            if (shopItemScriptableObject.cost > coins) shopPanels[i].DeactivateButton(BuyOrSell.Buy);
        }
    }

    public void DeactivateShopPanels()
    {
        foreach (var item in shopPanels)
        {
            item.gameObject.SetActive(false);
        }
    }

}
