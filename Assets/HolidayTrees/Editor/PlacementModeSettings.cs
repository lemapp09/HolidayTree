using UnityEngine;
using UnityEngine.UIElements;

public class PlacementModeSettings
{
    private EnumField modeField;
    private PlacementMode placementMode = PlacementMode.Grid;
    private Box gridSettingsBox;
    private Box randomSettingsBox;
    private int rows = 5;
    private int columns = 5;
    private int numberOfTrees = 10;

    public PlacementModeSettings(VisualElement root)
    {
        // Placement Mode Dropdown
        modeField = new EnumField("Placement Mode", placementMode);
        modeField.RegisterValueChangedCallback(evt =>
        {
            placementMode = (PlacementMode)evt.newValue;
            UpdateVisibility();
        });
        root.Add(modeField);

        // Grid Settings Box
        gridSettingsBox = new Box { style = { paddingTop = 5, paddingBottom = 5 } };
        gridSettingsBox.Add(new Label("Grid Settings:"));

        var rowsField = new IntegerField("Rows") { value = rows };
        rowsField.RegisterValueChangedCallback(evt => { rows = evt.newValue; });
        gridSettingsBox.Add(rowsField);

        var columnsField = new IntegerField("Columns") { value = columns };
        columnsField.RegisterValueChangedCallback(evt => { columns = evt.newValue; });
        gridSettingsBox.Add(columnsField);

        root.Add(gridSettingsBox);

        // Random Settings Box
        randomSettingsBox = new Box { style = { paddingTop = 5, paddingBottom = 5 } };
        randomSettingsBox.Add(new Label("Random Settings:"));

        var numberField = new IntegerField("Number of Trees") { value = numberOfTrees };
        numberField.RegisterValueChangedCallback(evt => { numberOfTrees = evt.newValue; });
        randomSettingsBox.Add(numberField);

        root.Add(randomSettingsBox);

        UpdateVisibility();
    }

    // Generate Trees based on the selected placement mode
    public void GenerateTrees(Vector3 cubeCenter, Vector3 cubeSize, bool verbose)
    {
        if (placementMode == PlacementMode.Grid)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Vector3 position = cubeCenter + new Vector3(
                        (col - (columns - 1) * 0.5f) * (cubeSize.x / columns),
                        cubeSize.y * 0.5f,
                        (row - (rows - 1) * 0.5f) * (cubeSize.z / rows)
                    );

                    TreeInstantiator.InstantiateTree("Prefab/TreeGreen", position, verbose);
                }
            }
        }
        else if (placementMode == PlacementMode.Random)
        {
            for (int i = 0; i < numberOfTrees; i++)
            {
                Vector3 position = cubeCenter + new Vector3(
                    Random.Range(-cubeSize.x * 0.5f, cubeSize.x * 0.5f),
                    cubeSize.y * 0.5f,
                    Random.Range(-cubeSize.z * 0.5f, cubeSize.z * 0.5f)
                );

                TreeInstantiator.InstantiateTree("Prefab/TreeGreen", position, verbose);
            }
        }
    }

    // Update the visibility of the Grid and Random settings
    private void UpdateVisibility()
    {
        gridSettingsBox.style.display = (placementMode == PlacementMode.Grid) ? DisplayStyle.Flex : DisplayStyle.None;
        randomSettingsBox.style.display = (placementMode == PlacementMode.Random) ? DisplayStyle.Flex : DisplayStyle.None;
    }
}

public enum PlacementMode
{
    Grid,
    Random
}
