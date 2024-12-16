using UnityEngine;

public class Grid_Generator : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private Vector2 spacing;

    public void GenerateGrid(int rows, int columns)
    {
        ClearGrid();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Вычисляем позицию каждой ячейки
                Vector3 position = new Vector3(
                    col * (cellSize.x + spacing.x),
                    -row * (cellSize.y + spacing.y),
                    0);
                
                GameObject cell = Instantiate(cellPrefab, transform);
                cell.transform.localPosition = position;
            }
        }
    }

    public void ClearGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
