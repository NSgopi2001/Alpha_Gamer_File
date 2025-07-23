using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GridEnum;

public class CardsGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gridPanel;
    [SerializeField] private GameObject cardPrefab;

    [Header("Layout Settings")]
    [SerializeField] private Vector2 spacing = new Vector2(10, 10);
    [SerializeField] private Vector2 padding = new Vector2(0, 10);

    [Header("Default Grid Size")]
    [SerializeField] private GridSize defaultGridSize = GridSize.Grid_2x3;

    private GridLayoutGroup gridLayout;
    private RectTransform panelRect;

    private void Awake()
    {
        gridLayout = gridPanel.GetComponent<GridLayoutGroup>();
        panelRect = gridPanel.GetComponent<RectTransform>();

        if(GameSettings.Instance != null)
        SetGrid(GameSettings.Instance.SelectedGridSize);
        else
        SetGrid(defaultGridSize);
    }

    public void SetGrid(GridSize gridSize)
    {
        int rows = 0, cols = 0;

        switch (gridSize)
        {
            case GridSize.Grid_2x2: rows = 2; cols = 2; break;
            case GridSize.Grid_2x3: rows = 2; cols = 3; break;
            case GridSize.Grid_5x6: rows = 5; cols = 6; break;
        }

        if (gridLayout == null || panelRect == null)
            return;

        // Remove old cards
        foreach (Transform child in gridPanel)
            Destroy(child.gameObject);

        // Calculate dynamic cell size
        float width = panelRect.rect.width - padding.x * 2 - spacing.x * (cols - 1);
        float height = panelRect.rect.height - padding.y * 2 - spacing.y * (rows - 1);
        Vector2 cellSize = new Vector2(200, 200);

        // Apply layout settings
        gridLayout.cellSize = cellSize;
        gridLayout.spacing = spacing;
        gridLayout.padding = new RectOffset((int)padding.x, (int)padding.x, (int)padding.y, (int)padding.y);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = cols;

        // Create cards
        int total = rows * cols;
        for (int i = 0; i < total; i++)
        {
            GameObject card = Instantiate(cardPrefab, gridPanel);
            card.name = $"Card_{i}";
        }
    }
}
