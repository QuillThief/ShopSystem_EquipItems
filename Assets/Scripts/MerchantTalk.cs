using UnityEngine;

public class MerchantTalk : MonoBehaviour
{
    public static MerchantTalk Instance { get; private set; }
    [SerializeField] Dialogue dialogue;
    private string startTalk = "How may I help you?";
    public string endTalk { get; private set; } = "Thank you come again.";

    void Start() => Instance = this;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShopManager.Instance.DisablePlusMinusButtons();
            dialogue.StartDialogue(startTalk, true);
        }
    }

}
