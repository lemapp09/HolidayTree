using UnityEngine;

public class SetTreeColor : MonoBehaviour
{
    private Renderer treeRenderer; // Renderer component of the tree
    private Material materialInstance; // Material instance to ensure unique colors per tree

    private void Awake()
    {
        // Get the Renderer component attached to this GameObject
        treeRenderer = GetComponent<Renderer>();

        // Ensure the tree has a renderer and a material
        if (treeRenderer != null && treeRenderer.sharedMaterial != null)
        {
            // Create a new material instance to avoid shared materials between objects
            materialInstance = new Material(treeRenderer.sharedMaterial);
        }
    }

    // Public method to set the color of the tree and assign the material instance
    public void SetColor(Color color)
    {
        if (materialInstance != null)
        {
            // Set the color on the newly created material instance
            materialInstance.SetColor("_Color", color); // "_Color" should match the exposed parameter name in the shader

            // Assign the new material instance to the renderer to apply the color
            treeRenderer.material = materialInstance;
        }
    }
}