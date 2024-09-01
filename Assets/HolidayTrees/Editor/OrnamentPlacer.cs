using UnityEngine;

public static class OrnamentPlacer
{
    // Method to place ornaments on the surface of the tree
    public static void PlaceOrnaments(GameObject tree, GameObject ornamentYellow, GameObject ornamentRed, int numberOfOrnaments, bool verbose)
    {
        // Get the scale and height of the tree for placement calculations
        Vector3 treeScale = tree.transform.localScale;
        float treeHeight = treeScale.y; // Tree height based on scaling
        float minY = treeHeight * 0.1f; // Minimum height (10% of tree height)
        float maxY = treeHeight * 0.9f; // Maximum height (90% of tree height)

        for (int i = 0; i < numberOfOrnaments; i++)
        {
            // Randomly choose an ornament type (Yellow or Red)
            GameObject selectedOrnament = Random.value > 0.5f ? ornamentYellow : ornamentRed;

            // Calculate a random height between minY and maxY
            float randomHeight = Random.Range(minY, maxY);

            // Determine a random angle for placement around the tree
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad; // Convert to radians

            // Calculate the horizontal radius at this height
            float heightRatio = (randomHeight - minY) / (maxY - minY); // Range between 0 at the top and 1 at the bottom
            float radius = 0.5f * (1 - heightRatio); // Radius decreases as you go up the tree

            // Scale down the calculated positions to account for the tree's exaggerated size
            float scaleFactor = 0.001f; // Scale down by 1/1000

            // Calculate the position based on the random angle and adjusted radius, scaled down
            Vector3 ornamentPosition = new Vector3(
                Mathf.Cos(randomAngle) * radius * treeScale.x * scaleFactor,
                (randomHeight - (treeHeight / 2)) * scaleFactor, // Adjusting for the pivot point at halfway and scaling
                Mathf.Sin(randomAngle) * radius * treeScale.z * scaleFactor
            );

            // Instantiate the ornament at the calculated position relative to the tree
            GameObject ornamentInstance = GameObject.Instantiate(selectedOrnament, tree.transform);
            ornamentInstance.transform.localPosition = ornamentPosition;

            // Optionally log the placement if verbose is enabled
            if (verbose)
            {
                Debug.Log($"Placed {ornamentInstance.name} at {ornamentInstance.transform.position} on the tree.");
            }
        }
    }
}
