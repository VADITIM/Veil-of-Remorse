using UnityEngine;
using UnityEngine.EventSystems;

public class SkillHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string skillID;
    public string skillName;
    public string skillDescription;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillTreeManager.Instance.HoverAbility(skillID, skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillTreeManager.Instance.ExitHover();
    }
}