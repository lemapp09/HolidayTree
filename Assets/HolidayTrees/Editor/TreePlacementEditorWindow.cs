using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TreePlacementEditorWindow : EditorWindow
{
    private Vector3 cubeSize = new Vector3(200, 10, 200); // Default size for the cube
    private Vector3 cubeCenter = new Vector3(0, 5, 0); // Default center of the cube
    private const string prefabName = "Prefab/TreeGreen"; // Path within the Resources folder
    private static bool verbose = false; // Controls verbosity of debug messages
    private PlacementModeSettings placementModeSettings;
    private TreeColorSettings treeColorSettings;
    private bool randomSizes = false; // Controls whether to apply random scaling to trees
    private bool placeOrnaments = false; // Controls whether to place ornaments on the tree

    [MenuItem("Tools/Tree Placement Tool")]
    public static void ShowWindow()
    {
        var window = GetWindow<TreePlacementEditorWindow>();
        window.titleContent = new GUIContent("Tree Placement Tool");
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void CreateGUI()
    {
        var root = rootVisualElement;

        // Space above Verbose checkbox
        root.Add(new VisualElement { style = { height = 10 } });

        // Verbose Checkbox
        var verboseToggle = new Toggle("Verbose") { value = verbose };
        verboseToggle.RegisterValueChangedCallback(evt =>
        {
            verbose = evt.newValue;
        });
        root.Add(verboseToggle);

        // Initialize Placement Mode Settings
        placementModeSettings = new PlacementModeSettings(root);

        // Initialize Tree Color Settings
        treeColorSettings = new TreeColorSettings(root);

        // Random Sizes Checkbox
        var sizeToggle = new Toggle("Random Sizes") { value = randomSizes };
        sizeToggle.RegisterValueChangedCallback(evt =>
        {
            randomSizes = evt.newValue;
            TreeInstantiator.SetRandomSizes(randomSizes);
        });
        root.Add(sizeToggle);

        // Ornament Placement Checkbox
        var ornamentToggle = new Toggle("Place Ornaments") { value = placeOrnaments };
        ornamentToggle.RegisterValueChangedCallback(evt =>
        {
            placeOrnaments = evt.newValue;
            TreeInstantiator.SetPlaceOrnaments(placeOrnaments);
        });
        root.Add(ornamentToggle);

        // Box for Cube Size
        var cubeSizeBox = new Box { style = { paddingTop = 5, paddingBottom = 5 } };
        cubeSizeBox.Add(new Label("Cube Size:"));
        var sizeField = new Vector3Field("Size") { value = cubeSize };
        sizeField.RegisterValueChangedCallback(evt =>
        {
            cubeSize = evt.newValue;
            SceneView.RepaintAll(); // Refresh the Scene view when values change
        });
        cubeSizeBox.Add(sizeField);
        root.Add(cubeSizeBox);

        // Box for Cube Center
        var cubeCenterBox = new Box { style = { paddingTop = 5, paddingBottom = 5 } };
        cubeCenterBox.Add(new Label("Cube Center:"));
        var centerField = new Vector3Field("Center") { value = cubeCenter };
        centerField.RegisterValueChangedCallback(evt =>
        {
            cubeCenter = evt.newValue;
            SceneView.RepaintAll(); // Refresh the Scene view when values change
        });
        cubeCenterBox.Add(centerField);
        root.Add(cubeCenterBox);

        // Space before Generate Trees button
        root.Add(new VisualElement { style = { height = 10 } });

        // Generate Trees Button
        var generateButton = new Button(() => GenerateTrees())
        {
            text = "Generate Trees"
        };
        root.Add(generateButton);
    }

    // Generate Trees based on the selected settings
    private void GenerateTrees()
    {
        placementModeSettings.GenerateTrees(cubeCenter, cubeSize, verbose);
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        Handles.EndGUI();

        HandleSceneInteraction(sceneView);

        Handles.color = Color.green;
        var window = GetWindow<TreePlacementEditorWindow>();

        if (window != null)
        {
            Handles.DrawWireCube(window.cubeCenter, window.cubeSize);
        }
    }

    private static void HandleSceneInteraction(SceneView sceneView)
    {
        Event e = Event.current;
        if (e != null && e.isMouse)
        {
            if (e.button == 1 || e.button == 2)
            {
                e.Use();
            }
        }
        sceneView.Repaint();
    }
}
