using UnityEngine;
using UnityEngine.UIElements;

public class RandomSettings
{
    private Box randomSettingsBox;
    private int numberOfTrees = 10;
    private string prefabPath;
    private bool verbose;

    public RandomSettings(VisualElement root, string prefabPath, bool verbose)
    {
        this.prefabPath = prefabPath;
        this.verbose = verbose;

        randomSettingsBox = new Box { style = { paddingTop = 5, paddingBottom = 5 } };
        randomSettingsBox.Add(new Label("Random Settings:"));

        var numberField = new IntegerField("Number of Trees") { value = numberOfTrees };
        numberField.RegisterValueChangedCallback(evt => { numberOfTrees = evt.newValue; });
        randomSettingsBox.Add(numberField);

        root.Add(randomSettingsBox);
    }

    public void SetVisible(bool visible)
    {
        randomSettingsBox.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void GenerateRandom(Vector3 cubeCenter, Vector3 cubeSize)
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 position = cubeCenter + new Vector3(
                Random.Range(-cubeSize.x / 2, cubeSize.x / 2),
                cubeSize.y / 2,
                Random.Range(-cubeSize.z / 2, cubeSize.z / 2)
            );

            TreeInstantiator.InstantiateTree(prefabPath, position, verbose);
        }
    }
}