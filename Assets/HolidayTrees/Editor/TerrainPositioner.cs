using UnityEngine;

public static class TerrainPositioner
{
    // Method to position the tree on the terrain by raycasting downwards
    public static bool PositionTreeOnTerrain(GameObject tree, Vector3 initialPosition, bool verbose)
    {
        // Set the initial position at the top of the bounding box (or a high Y value)
        tree.transform.position = initialPosition;

        // Get the BoxCollider component to determine the top position
        BoxCollider collider = tree.GetComponent<BoxCollider>();
        if (collider != null)
        {
            // Determine the starting position of the raycast (slightly above the top of the collider)
            Vector3 rayStartPosition = tree.transform.position + Vector3.up * (collider.bounds.extents.y + 1f);

            // Cast a ray downward to find the terrain
            RaycastHit hit;
            if (Physics.Raycast(rayStartPosition, Vector3.down, out hit, Mathf.Infinity))
            {
                // Check if the hit object is the terrain
                if (hit.collider.gameObject.GetComponent<Terrain>() != null)
                {
                    // Calculate the slope of the terrain using the dot product with Vector3.up
                    float slope = Vector3.Dot(hit.normal, Vector3.up);

                    // Check if the slope is within the acceptable range (1 to 0.9)
                    if (slope >= 0.9f)
                    {
                        // Adjust the position so that the base of the BoxCollider is on the terrain
                        Vector3 adjustedPosition = hit.point + Vector3.up * collider.bounds.extents.y;
                        tree.transform.position = adjustedPosition;
                        if (verbose)
                            Debug.Log($"Placed tree at {tree.transform.position} with slope {slope}");
                        return true;
                    }
                    else
                    {
                        if (verbose)
                            Debug.LogWarning("Slope too steep to place the tree.");
                        return false;
                    }
                }
                else
                {
                    if (verbose)
                        Debug.LogWarning("The object hit by the raycast is not the terrain.");
                    return false;
                }
            }
            else
            {
                if (verbose)
                    Debug.LogError("Raycast did not hit any objects. The tree might not be placed correctly.");
                return false;
            }
        }
        else
        {
            if (verbose)
                Debug.LogError("The tree prefab does not have a BoxCollider component.");
            return false;
        }
    }
}
