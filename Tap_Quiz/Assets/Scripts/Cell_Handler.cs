using UnityEngine;
using UnityEngine.UI;

public class Cell_Handler : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image icon;

    private string cellAnswer;
    private System.Action<string, Transform> onClickCallback;

    public void Initialize(Cell_Data data, System.Action<string, Transform> callback)
    {
        cellAnswer = data.answer;
        icon.sprite = data.sprite;
        backgroundImage.color = data.backgroundColor;
        onClickCallback = callback;
    }
    
    public string GetAnswer()
    {
        return cellAnswer;
    }

    private void OnCellClicked()
    {
        Debug.Log($"Clicked on cell with answer: {cellAnswer}");
        onClickCallback?.Invoke(cellAnswer, icon.transform);
    }
}
