using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Panel")]
    public GameObject hoverPanel;
    public string text;
    public TextMeshProUGUI tmptext;
    
    [Header("Essence Required Display")]
    public TextMeshProUGUI essenceRequiredText;
    public bool showEssenceRequired = false;
    private int essenceRequired = 1; 
    
    [Header("Custom Text Settings")]
    public string textType = ""; 
    private string[] kindlePowerTexts = new string[] 
    {
        "Offer essence to acquire new power",
        "Strengthen your soul with essence",
        "Upgrade your power with essence"
    };

    public string[] offerEssenceTexts = new string[] 
    {
        "Kindle Power to achieve more health & stamina.",
    };
    
    void Update()
    {
        if (ClassManager.Instance.LevelSystem.GetEssence() >= essenceRequired)
        {
            essenceRequiredText.color = Color.green;
        }
        else
        {
            essenceRequiredText.color = Color.red;
        }

        if (ClassManager.Instance.PlayerBar.MaxLevelReached())
        {
            essenceRequiredText.text = "";
        }
    }
    
    void Start()
    {
        
        if (textType.ToLower() == "offeressence")
        {
            
            int randomIndex = Random.Range(0, offerEssenceTexts.Length);
            text = string.Format(offerEssenceTexts[randomIndex], essenceRequired);
        }

        if (textType.ToLower() == "kindlepower")
        {
            
            int randomIndex = Random.Range(0, kindlePowerTexts.Length);
            text = string.Format(kindlePowerTexts[randomIndex], essenceRequired);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!showEssenceRequired) { essenceRequiredText.gameObject.SetActive(false); }
        else  { essenceRequiredText.gameObject.SetActive(true); }

        if (textType.ToLower() == "offeressence")
        {
            
            int randomIndex = Random.Range(0, offerEssenceTexts.Length);
            text = string.Format(offerEssenceTexts[randomIndex], essenceRequired);
        }

        if (textType.ToLower() == "kindlepower")
        {
            
            int randomIndex = Random.Range(0, kindlePowerTexts.Length);
            text = string.Format(kindlePowerTexts[randomIndex], essenceRequired);
        }
        
        hoverPanel.SetActive(true);
        tmptext.text = text;

        if (!showEssenceRequired)
        {
            if (essenceRequiredText != null)
                essenceRequiredText.gameObject.SetActive(false);
        }
        else
        {
                essenceRequiredText.gameObject.SetActive(true);
                essenceRequiredText.text = $"Essence Required: {essenceRequired}";
                
  
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverPanel.SetActive(false);
    }
    
    
    public void SetEssenceRequired(int amount) { essenceRequired = amount; }

}