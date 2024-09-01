/** using UnityEngine;
using UnityEngine.UIElements;

public class PlacementSettings
{
    private EnumField modeField;
    private PlacementMode placementMode = PlacementMode.Grid;
    private GridSettings gridSettings;
    private RandomSettings randomSettings;
    private VisualElement root;

    public PlacementSettings(VisualElement rootElement, string prefabPath, bool verbose)
    {
        root = rootElement;

        // Placement Mode Dropdown
        modeField = new EnumField("Placement Mode", placementMode);
        modeField.RegisterValueChangedCallback(evt =>
        {
            placementMode = (PlacementMode)evt.newValue;
            UpdateVisibility();
        });
        root.Add(modeField);

        // Initialize Grid and Random settings
        gridSettings = new GridSettings(root, prefabPath, verbose);
        randomSettings = new RandomSettings(root, prefabPath, verbose);

        UpdateVisibility();
    }

    public void GenerateTrees(Vector3 cubeCenter, Vector3 cubeSize)
    {
        if (placementMode == PlacementMode.Grid)
        {
            gridSettings.GenerateGrid(cubeCenter, cubeSize);
        }
        else if (placementMode == PlacementMode.Random)
        {
            randomSettings.GenerateRandom(cubeCenter, cubeSize);
        }
    }

    private void UpdateVisibility()
    {
        gridSettings.SetVisible(placementMode == PlacementMode.Grid);
        randomSettings.SetVisible(placementMode == PlacementMode.Random);
    }
}

public enum PlacementMode
{
    Grid,
    Random
}
**/