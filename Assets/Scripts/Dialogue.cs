using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] Text txtDialogue;
    [SerializeField] string[] lines;
    [SerializeField] float textSpeed = 0.3f;

    void Start()
    {
        txtDialogue.text = string.Empty;
        gameObject.SetActive(false);
    }

    public void StartDialogue(string message, bool showOptions)
    {
        gameObject.SetActive(true);
        PlayerController.Instance.SetFreezePlayer(true);
        StartCoroutine(TypeLine(message, showOptions));
    }

    IEnumerator TypeLine(string message, bool showOptions)
    {
        foreach (var c in message.ToCharArray())
        {
            txtDialogue.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        if (showOptions)
        {
            ShopManager.Instance.ShowOptions();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            PlayerController.Instance.SetFreezePlayer(false);
            CloseDialogBox();
            ShopManager.Instance.EnablePlusMinusButtons();
        }
    }

    public void CloseDialogBox()
    {
        txtDialogue.text = string.Empty;
        gameObject.SetActive(false);
    }

}
