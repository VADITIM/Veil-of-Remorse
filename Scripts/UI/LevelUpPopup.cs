using System.Collections;
using UnityEngine;

public class LevelUpPopup : MonoBehaviour
{
    LevelSystem LevelSystem;
    
    public GameObject LevelUpPopupObject;

    void Start()
    {
        LevelUpPopupObject = FindObjectOfType<LevelUpPopup>().gameObject;
        LevelSystem = FindObjectOfType<LevelSystem>();
    }


    public void ShowLevelUpPopup()
    {
        LevelUpPopupObject.SetActive(true);
        StartCoroutine(HideLevelUpPopup());
    }
    
    IEnumerator HideLevelUpPopup()
    {
        yield return new WaitForSeconds(2f);
        LevelUpPopupObject.SetActive(false);
    }
}
