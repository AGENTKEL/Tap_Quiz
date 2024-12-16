using UnityEngine;

[CreateAssetMenu(fileName = "Level_Config", menuName = "Level_Config")]
public class Level_Config : ScriptableObject
{
    public int rows;
    public int columns;
    public Cell_Data[] cellData;
}
    
