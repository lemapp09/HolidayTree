using UnityEngine.UIElements;

public enum ColorMode
{
    RandomColor,
    RandomGreen,
    UniformGreen // New option added for consistent green color
}

public class TreeColorSettings
{
    private ColorMode selectedColorMode = ColorMode.RandomColor;

    public TreeColorSettings(VisualElement root)
    {
        // Dropdown to select color mode
        var colorModeDropdown = new EnumField("Color Mode", selectedColorMode);
        colorModeDropdown.RegisterValueChangedCallback(evt =>
        {
            selectedColorMode = (ColorMode)evt.newValue;
            TreeInstantiator.SetColorMode(selectedColorMode);
        });
        root.Add(colorModeDropdown);
    }
}