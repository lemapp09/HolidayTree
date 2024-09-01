using UnityEngine;
using UnityEngine.UIElements;

public class GridSettings
{
    private Box gridSettingsBox;
    private int rows = 5;
    private int columns = 5;
    private string prefabPath;
    private bool verbose;

    public GridSettings(VisualElement root, string prefabPath, bool verbose)
    {
        this.prefabPath = prefabPath;
        this.verbose = verbose;

        gridSettingsBox = new Box { style = { paddingTop = 5, paddingBottom = 5 } };
        gridSettingsBox.Add(new Label("Grid Settings:"));

        var rowsField = new IntegerField("Rows") { value = rows };
        rowsField.RegisterValueChangedCallback(evt => { rows = evt.newValue; });
        gridSettingsBox.Add(rowsField);

        var columnsField = new IntegerField("Columns") { value = columns };
        columnsField.RegisterValueChangedCallback(evt => { columns = evt.newValue; });
        gridSettingsBox.Add(columnsField);

        root.Add(gridSettingsBox);
    }

    public void SetVisible(bool visible)
    {
        gridSettingsBox.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void GenerateGrid(Vector3 cubeCenter, Vector3 cubeSize)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = cubeCenter + new Vector3(
                    (col - (columns - 1) / 2.0f) * (cubeSize.x / columns),
                    cubeSize.y / 2,
                    (row - (rows - 1) / 2.0f) * (cubeSize.z / rows)
                );

                TreeInstantiator.InstantiateTree(prefabPath, position, verbose);
            }
        }
    }
}