   This is an Editor Tool to place 'Holiday Trees' across a lanscape. There needs to be a Terrain object for the tool to find.

The tool is only usable in the Unity Editor. Under the Tools menu, select "Tree Placement Tool".

Options:
  - Verbose - Messages are sent to the console log.
  - Placement Mode - Grid or Random
    provide either count for rows and columns or a total count for random
  - Color Mode - Random Color, or Random Green (Shades of Green) or Uniform Green (single shade of green)
  - Random Size - Randomly scales the trees
  - Place Ornaments - Places ornaments on each tree
  - Cube Size - These are the real world units for the cube where trees are placed.
  - Cub Center - The real world units for the center of the rendering cube.

Click "Generate Trees" to generate an game object in the hierarchy window named "Landscape". Each tree is a child of this object.

Its important for the top of the cube be well above the terrain and the bottom of the cube be well below the terrain. 
The tool selects a position in X and Z, then projects a line down in Y to determine where to place tree on the terrain.
The tool will reportan error if it can't find the terrain, if there a collision with another tree, or that the sloop of the 
terrain is too steep. 

There is a sample scene in the Assets/HolidayTrees/Scene folder named Holiday. There is a human scale Capsule and a sample tree on a km2 landscape. 

Enjoy!
