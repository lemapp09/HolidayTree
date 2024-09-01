using UnityEditor;
using UnityEngine;

public static class TreeInstantiator
{
    private static GameObject treeParent; // To hold the parent GameObject
    private static int treeCount = 0; // Counter for naming the trees
    private static ColorMode colorMode = ColorMode.RandomColor; // Controls the current color mode
    private static bool randomSizes = false; // Controls whether to apply random scaling to trees
    private static bool placeOrnaments = false; // Controls whether to place ornaments on the tree

    // Paths to prefabs
    private const string ornamentPrefabPath = "Prefab/TreeOrnament"; // Ornament prefab path

    // Method to load and instantiate the tree prefab at a given position
    public static void InstantiateTree(string prefabPath, Vector3 initialPosition, bool verbose)
    {
        // Create the parent GameObject if it does not exist
        CreateTreeParent(initialPosition);

        // Load the tree prefab from the Resources folder
        GameObject treePrefab = Resources.Load<GameObject>(prefabPath);
        GameObject ornamentPrefab = Resources.Load<GameObject>(ornamentPrefabPath); // Load the ornament prefab

        if (treePrefab != null)
        {
            // Perform a collision check before instantiating
            if (!IsPlacementBlocked(treePrefab, initialPosition, verbose))
            {
                // Instantiate the tree prefab at the initial position
                GameObject treeInstance = PrefabUtility.InstantiatePrefab(treePrefab) as GameObject;
                if (treeInstance != null)
                {
                    // Name the instantiated prefab sequentially
                    treeCount++;
                    treeInstance.name = $"{treePrefab.name}_{treeCount}";

                    // Set the instantiated prefab as a child of the Tree Landscape parent
                    treeInstance.transform.SetParent(treeParent.transform);

                    // Apply random scaling if the option is enabled
                    if (randomSizes)
                    {
                        initialPosition = ApplyRandomScale(treeInstance, initialPosition); // Adjust initialPosition based on scaling
                    }

                    // Place the prefab on the terrain using the TerrainPositioner script
                    if (!TerrainPositioner.PositionTreeOnTerrain(treeInstance, initialPosition, verbose))
                    {
                        // If placement on terrain failed, destroy the prefab
                        GameObject.DestroyImmediate(treeInstance);
                    }
                    else
                    {
                        // Handle color assignment based on the selected color mode
                        AssignColor(treeInstance);

                        // If ornament placement is enabled, instantiate and parent the TreeOrnament prefab
                        if (placeOrnaments && ornamentPrefab != null)
                        {
                            GameObject ornamentInstance = PrefabUtility.InstantiatePrefab(ornamentPrefab) as GameObject;
                            if (ornamentInstance != null)
                            {
                                ornamentInstance.name = $"{ornamentPrefab.name}_{treeCount}";
                                ornamentInstance.transform.SetParent(treeInstance.transform); // Parent to the tree
                                ornamentInstance.transform.localPosition = Vector3.zero; // Center it on the tree
                                ornamentInstance.transform.localScale = Vector3.one; // Scale to parent
                            }
                            else
                            {
                                Debug.LogError("Failed to instantiate the TreeOrnament prefab.");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"Failed to load the tree prefab at path: {prefabPath}");
        }
    }

    // Method to create the parent GameObject for trees
    private static void CreateTreeParent(Vector3 position)
    {
        // Check if the parent GameObject already exists, create if it does not
        if (treeParent == null)
        {
            treeParent = new GameObject("Tree Landscape");
            treeParent.transform.position = position;
        }
    }

    // Method to check if placement is blocked by other objects
    private static bool IsPlacementBlocked(GameObject prefab, Vector3 initialPosition, bool verbose)
    {
        // Create a temporary GameObject to get the bounds of the BoxCollider
        GameObject tempObject = GameObject.Instantiate(prefab, initialPosition, Quaternion.identity);
        BoxCollider collider = tempObject.GetComponent<BoxCollider>();

        if (collider != null)
        {
            // Store the necessary information before destroying the temporary object
            Vector3 boxCenter = collider.bounds.center;
            Vector3 boxSize = collider.size;

            // Destroy the temporary object after gathering necessary data
            GameObject.DestroyImmediate(tempObject);

            // Check for overlapping colliders in the intended placement area
            Collider[] hits = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity);

            foreach (Collider hit in hits)
            {
                // Ignore self-collision and the terrain
                if (hit.gameObject != prefab && hit.gameObject.GetComponent<Terrain>() == null)
                {
                    if (verbose)
                        Debug.LogError($"Cannot place TreeGreen prefab because it collides with {hit.gameObject.name}.");
                    return true; // Placement is blocked
                }
            }
        }
        else
        {
            GameObject.DestroyImmediate(tempObject);
            return true; // The prefab does not have a BoxCollider
        }

        return false; // No collision detected, placement is clear
    }

    // Method to assign color to a tree instance based on the selected mode
    private static void AssignColor(GameObject tree)
    {
        // Get the Renderer component of the tree
        Renderer renderer = tree.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Create a new temporary material instance based on the shared material
            Material tempMaterial = new Material(renderer.sharedMaterial);

            // Assign a color based on the selected mode
            if (colorMode == ColorMode.RandomColor)
            {
                // Fully random color
                tempMaterial.color = Random.ColorHSV();
            }
            else if (colorMode == ColorMode.RandomGreen)
            {
                // Random green color with specific HSV range
                tempMaterial.color = Random.ColorHSV(100f / 360f, 140f / 360f, 0.25f, 0.75f, 0.25f, 0.75f);
            }
            else if (colorMode == ColorMode.UniformGreen)
            {
                // Uniform green color (no randomness)
                tempMaterial.color = new Color(0.2f, 0.6f, 0.2f); // Consistent green color
            }

            // Assign the new material instance back to the renderer's shared material
            renderer.sharedMaterial = tempMaterial;
        }
    }

    // Method to apply random scaling as a percentage multiplier to the existing scale of the tree
    // and adjust the initialPosition based on the new height
    private static Vector3 ApplyRandomScale(GameObject tree, Vector3 initialPosition)
    {
        // Get the current local scale of the tree
        Vector3 currentScale = tree.transform.localScale;

        // Generate random scale multipliers between 50% and 150%
        float scaleXMultiplier = Random.Range(0.5f, 1.5f);
        float scaleYMultiplier = Random.Range(0.5f, 1.5f);
        float scaleZMultiplier = Random.Range(0.5f, 1.5f);

        // Apply the random scale multipliers to the existing scale
        tree.transform.localScale = new Vector3(
            currentScale.x * scaleXMultiplier,
            currentScale.y * scaleYMultiplier,
            currentScale.z * scaleZMultiplier
        );

        // Adjust the initial Y position to account for the new height of the tree
        float adjustedY = initialPosition.y + (tree.transform.localScale.y - currentScale.y) * 0.5f;
        initialPosition = new Vector3(initialPosition.x, adjustedY, initialPosition.z);

        return initialPosition;
    }

    // Method to set the color mode
    public static void SetColorMode(ColorMode mode)
    {
        colorMode = mode;
    }

    // Method to enable or disable random sizes
    public static void SetRandomSizes(bool enable)
    {
        randomSizes = enable;
    }

    // Method to enable or disable ornament placement
    public static void SetPlaceOrnaments(bool enable)
    {
        placeOrnaments = enable;
    }
}
