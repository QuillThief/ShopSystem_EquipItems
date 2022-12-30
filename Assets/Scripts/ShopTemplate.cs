using Convert = System.Convert;
using UnityEngine;
using UnityEngine.UI;

public class ShopTemplate : MonoBehaviour
{
    public Text txtName;
    public Image imgSprite;
    public Text txtCost;
    public Button btnBuySell;
    public Text btnText;

    public void BuySell()
    {
        var shopManager = ShopManager.Instance;

        string itemName = txtName.text;
        string strCost = txtCost.text;
        strCost = strCost.Remove(strCost.Length - 1, 1); //remove G from strCost

        if (btnText.text == BuyOrSell.Buy.ToString())
        {
            shopManager.AddCoins(-1 * Convert.ToInt32(strCost));
            Inventory.Instance.AddToInventory(itemName);
            ShowEquipped();
            shopManager.DisableOtherBuyButtonsIfApplicable();

            Equip.Instance.EquipPurchasedItem(itemName);
        }
        else if (btnText.text == BuyOrSell.Sell.ToString())
        {
            shopManager.AddCoins(Convert.ToInt32(strCost));
            Inventory.Instance.RemoveFromInventory(itemName);
            shopManager.LoadSellPanels();

            Equip.Instance.UnequipSoldItem(itemName);
        }
    }

    public void ActivateButton(BuyOrSell bs)
    {
        btnBuySell.interactable = true;
        btnText.text = bs.ToString();
    }

    public void DeactivateButton(BuyOrSell bs)
    {
        btnBuySell.interactable = false;
        btnText.text = bs.ToString();
    }

    public void ShowEquipped()
    {
        btnBuySell.interactable = false;
        btnText.text = "Equipped";
    }

}
