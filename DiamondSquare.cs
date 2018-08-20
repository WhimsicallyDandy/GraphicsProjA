using UnityEngine;
using System.Collections;


/*
	* Idea for increasing the randomness through iterations and a few other things found at:
	* https://github.com/jmecom/procedural-terrain/blob/master/js/terrain_generator.js
 */
public class DiamondSquare: MonoBehaviour
{

    /* Public constants */
    public Shader shader;
	public int iterations;
	//for now, only use iterations in even numbers

	/* Private Constants */
	private const int MAX_HEIGHT = 5;
	private const float INIT_LOWER = -0.1f;
	private const float INIT_UPPER = 0.1f;
	private const float INCREMENT_LOWER = -0.005F;
	private const float INCREMENT_UPPER = 0.005f;

	/* Private Variables */
	private float lower = INIT_LOWER;
	private float upper = INIT_UPPER;
	private Vector3[] test = new Vector3[2*3*4];
	private float[,] height = new float[3, 3];


    // Use this for initialization
    void Start()
    {
		
        // Add a MeshFilter component to this entity. This essentially comprises of a
        // mesh definition, which in this example is a collection of vertices, colours 
        // and triangles (groups of three vertices). 
        MeshFilter cubeMesh = this.gameObject.AddComponent<MeshFilter>();
        cubeMesh.mesh = this.CreateCubeMesh();

        // Add a MeshRenderer component. This component actually renders the mesh that
        // is defined by the MeshFilter component. You will need to modify this 
        // component for task 1 (and again for task 5), in order to set the material
        // to be used in rendering the cube.
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        //renderer.material ...
        renderer.material.shader = shader;
		renderer.material.SetFloat("size", iterations);
        
    }

    // Method to create a cube mesh with coloured vertices
    Mesh CreateCubeMesh()
    {
        Mesh m = new Mesh();
        m.name = "Cube";

		test = new[] {
			new Vector3(0, 0, 0),
			new Vector3(0, 0, 1),
			new Vector3(1, 0, 1),

			new Vector3(0, 0, 0),
			new Vector3(1, 0, 1),
			new Vector3(1, 0, 0)
		};
        // Define the vertices. These are the "points" in 3D space that allow us to
        // construct 3D geometry (by connecting groups of 3 points into triangles).
        m.vertices = genVertices();

        // Automatically define the triangles based on the number of vertices
        int[] triangles = new int[m.vertices.Length];
        for (int i = 0; i < m.vertices.Length; i++)
            triangles[i] = i;

        m.triangles = triangles;

        return m;
    }


	/*
	2 2
	4 3
	8 4
	16 5 (11)

	2(n-1)

	0 0
	2 1
	6 2
	14 3
	 */
	/* Helper Methods */

	// generates heights for mesh, initialises corners, performs Diamond-Square steps
	
	float[,] genHeights() {

		int size = (int)Mathf.Pow(iterations, 2)+1;
		// if iterations = 4, then 8 squares per edge, one extra row of vertices for edge
		float[,] output = new float[size, size];
		// initialise corners, perhaps random? big random?
		output[0, 0] = 1;
		output[0, size-1] = -0.3f;
		output[size-1, 0] = -1.2f;
		output[size-1, size-1] = 2;

		int len = size-1;
		// Main loop, iterate through the steps, increasing each time. size
		for (int steps = 0; steps<size-1; steps++) {
			int halfLen = len/2;
			
			// perform diamond step, iterating through the 2darray according to length
			// iter x iter steps
			for (int i=0; i<steps; i++) {
				for (int j = 0; j<steps; j++) {
					
					diamondStep(output, i, j, len);


				}
			}

			
					

			// perform square step, iterating through the 2darray according to length
			// iter x iter x 4 steps
			bool even = true;
			
			for (int i=0; i<steps; i++) {
				int xStart = (even) ? len : 0;
				
				for (int j = 0; j<steps; j++) {
					// top
					squareStep(output, i+len, j, len);
					// bottom
					//squareStep();
					// left 

					// right
					
				}
				even = !even;
			}

			len /= 2;

		}

		

		

		
		return output;
	} 

	// takes a map of Vector3 and returns the vertex map
	Vector3[] genVertices() {
		//Debug.Log(map);
		int index = 0;
		int iterLength = iterations*2-1;
		//int iterLength = (int)Mathf.Pow(iterations, 2)-1; // need to fix!


		float[,] heights = genHeights();


		// 3 verts per tri, 2 tris per unitsquare, 4 unitsquares per gridsquare
		//Vector3[] finalMesh = new Vector3[(int)Mathf.Pow(iterations, 2)*6*4+6];
		Vector3[] finalMesh = new Vector3[calcVecArraySize()];
		//Debug.Log("finalMesh created with size: "+finalMesh.Length);

		//for (int i=0; i<iterations*2-1; i+=2) {
			//for (int j=0; j<iterations*2-1; j+=2) {
		for (int i=0; i<iterLength; i+=2) {
			for (int j=0; j<iterLength; j+=2) {
				//Debug.Log("Creating a 4x4 unit square at: "+i+", "+j+", accessing index: "+index+" to array size: "+finalMesh.Length);
				// create a 4 squares, to perform operations on
				unitSquare(finalMesh, index+=6, i, j);
				unitSquare(finalMesh, index+=6, i, j+1);
				unitSquare(finalMesh, index+=6, i+1, j);
				unitSquare(finalMesh, index+=6, i+1, j+1);
			}
		}
		
		



		return finalMesh;
	}


	// creates a vertically flat Unitsquare 
	void unitSquare(Vector3[] v, int index, int posx, int posz) {
		v[index] = new Vector3(posx, 0, posz);
		v[index+1] = new Vector3(posx, 0, posz+1);
		v[index+2] = new Vector3(posx+1, 0, posz+1);
		v[index+3] = new Vector3(posx, 0, posz);
		v[index+4] = new Vector3(posx+1, 0, posz+1);
		v[index+5] = new Vector3(posx+1, 0, posz);
	}

	// Diamond steps
	void performDiamondSteps() {

	}

	void performSquareSteps() {

	}
	
	
	// diamond step, takes 2d array of floats, calculates average of four corners and applies to middle
	void diamondStep(float[,] h, int x, int y, int len) {
		h[x+len/2, y+len/2] = (h[x, y]+h[x+len, y]+h[x, y+len]+h[x+len, y+len])/4;

	}

	// square step. note that length is halved relative to the diamond step
	// this method is fucking garbage
	void squareStep(float[,] h, int x, int y, int len) {
		int max = h.GetLength(0);

		// makes sure that if something is out of bounds, it is not accessed
		// top
		float top = (y - len < 0) ? 0 : h[x, y-len];
		// bottom
		float bottom = (y+len >= h.GetLength(0)) ? 0 : h[x, y+len];
		// left
		float left = (x-len < 0) ? 0 : h[x-len, y];
		// right
		float right = (x+len >= h.GetLength(0)) ? 0 : h[x+len, y];

		float avg = (top+bottom+left+right)/4;

		h[x,y] = genPointHeight(avg);
		return;
	}


	// generate appropriate random numbers
	float genPointHeight(float orig) {
		return orig + Random.Range(lower, upper);
	}


	// creates a 4x4 square, made up of unit squares
	void squarex4(Vector3[] v, int index, int posx, int posz) {
		unitSquare(v, index+=6, posx, posz);
		unitSquare(v, index+=6, posx+1, posz);
		unitSquare(v, index+=6, posx, posz+1);
		unitSquare(v, index+=6, posx+1, posz+1);

	}

	int calcVecArraySize() {
		// variables are added for clarity of communication
		int gridDimensions = (int)Mathf.Pow(iterations, 2);
		// 4 unitsquares per gridsquare
		int unitSquares = 4; 
		// 6 vectors per gridsquare
		int vecPerSquare = 6;
		//Mathf.Pow(iterations, 2)*6*4+6
		// something's not quite right here
		return (gridDimensions+1) * unitSquares * vecPerSquare * 2;
	}
	
}
