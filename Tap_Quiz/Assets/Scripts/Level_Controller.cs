using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Level_Controller : MonoBehaviour
{
    [SerializeField] private Grid_Generator gridGenerator;
    [SerializeField] private Level_Config[] firstLevel;
    [SerializeField] private Level_Config[] secondLevel;
    [SerializeField] private Level_Config[] thirdLevel;
    [SerializeField] private TextMeshProUGUI taskText;

    [SerializeField] private GameObject restartButton;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image loadingScreenImage;
    [SerializeField] private ParticleSpawner particleSpawner;

    [SerializeField] private bool isFirstLevel = true;

    [SerializeField] private int currentLevel = 0;
    private Cell_Data correctCellData;

    private Level_Config selectedConfig;

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 3)
        {
            ShowEndGameScreen();
            return;
        }
        
        gridGenerator.ClearGrid();
        
        selectedConfig = SelectRandomConfig(levelIndex);
        
        gridGenerator.GenerateGrid(selectedConfig.rows, selectedConfig.columns);
        Transform gridParent = gridGenerator.transform;

        // Создаём копию данных и перемешиваем их
        List<Cell_Data> originalData = new List<Cell_Data>(selectedConfig.cellData);
        ShuffleList(originalData);

        List<Cell_Data> displayedData = new List<Cell_Data>();

        // Инициализируем ячейки и сохраняем отображаемые данные
        for (int i = 0; i < gridParent.childCount; i++)
        {
            Cell_Handler cell = gridParent.GetChild(i).GetComponent<Cell_Handler>();
            if (cell != null)
            {

                Cell_Data data = originalData[i % originalData.Count];
                displayedData.Add(data);

                // Инициализация ячейки
                cell.Initialize(data, OnCellClicked);


                if (isFirstLevel)
                {
                    AnimateCellAppearance(cell.transform, i * 0.1f);
                }
            }
        }

        // Обновляем правильный ответ
        correctCellData = displayedData[Random.Range(0, displayedData.Count)];
        taskText.text = $"Find: {correctCellData.answer}";

        isFirstLevel = false;
        currentLevel = levelIndex;
    }


    private Level_Config SelectRandomConfig(int levelIndex)
    {
        Level_Config[] selectedConfigs;

        // В зависимости от уровня сложности выбираем соответствующий список конфигов
        switch (levelIndex)
        {
            case 0:
                selectedConfigs = firstLevel;
                break;
            case 1:
                selectedConfigs = secondLevel;
                break;
            case 2:
                selectedConfigs = thirdLevel;
                break;
            default:
                selectedConfigs = firstLevel;
                break;
        }

        // Возвращаем случайный конфиг из выбранного списка
        return selectedConfigs[Random.Range(0, selectedConfigs.Length)];
    }

    public void NextLevel()
    {
        LoadLevel(currentLevel + 1);
    }

    private void OnCellClicked(string selectedAnswer, Transform cellTransform)
    {
        if (selectedAnswer == correctCellData.answer)
        {
            Debug.Log("Правильно! Переход на следующий уровень.");
            cellTransform.DOScale(Vector3.one * 0.8f, 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuad).OnComplete(NextLevel);
            particleSpawner.SpawnCorrectAnswerParticle(cellTransform.position);
        }
        else
        {
            Debug.Log("Ошибка! Попробуйте снова.");
            HandleError(cellTransform);
        }
    }
    private void HandleError(Transform cellTransform)
    {
        cellTransform.DOShakePosition(0.5f, new Vector3(10f, 0, 0), vibrato: 10, randomness: 90, snapping: false)
            .SetEase(Ease.InBounce);
    }

    private void ShowEndGameScreen()
    {
        fadeImage.gameObject.SetActive(true);
        restartButton.SetActive(true);
        fadeImage.DOFade(0.5f, 1f);
        restartButton.transform.SetAsLastSibling();
    }

    private void AnimateCellAppearance(Transform cellTransform, float delay)
    {
        // Устанавливаем начальные параметры для эффекта
        taskText.DOFade(1f, 1f);
        cellTransform.localScale = Vector3.zero;
        CanvasGroup canvasGroup = cellTransform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = cellTransform.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;

        // Анимация появления (bounce и fade-in)
        cellTransform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBounce).SetDelay(delay);
        canvasGroup.DOFade(1f, 0.4f).SetDelay(delay);
    }

    public void RestartGame()
    {
        // Перезапускаем игру с первого уровня
        currentLevel = 0;
        isFirstLevel = true;
        restartButton.SetActive(false);
        loadingScreenImage.gameObject.SetActive(true);

        fadeImage.DOFade(0f, 1f).OnComplete(() =>
        {
            // Выключаем объект fadeImage после окончания анимации
            fadeImage.gameObject.SetActive(false);
        });

        loadingScreenImage.DOFade(1f, 1f).OnComplete(() =>
        {
            LoadLevel(currentLevel);

            loadingScreenImage.DOFade(0f, 1f).OnComplete(() =>
            {
                loadingScreenImage.gameObject.SetActive(false); // Отключаем объект после анимации
            });
        });
    }

    // Метод для перемешивания списка
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}