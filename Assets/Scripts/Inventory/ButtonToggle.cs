using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string type;
    private TextMeshProUGUI typeText;

    private void Awake() => typeText = this.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>();
    public void OnPointerEnter(PointerEventData eventData) => typeText.text = type;
    public void OnPointerExit(PointerEventData eventData) => typeText.text = "";
}