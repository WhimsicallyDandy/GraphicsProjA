using UnityEngine;
using System.Collections;

public class DiamondSquare: MonoBehaviour
{

    /* Public constants */
    public Shader shader;
	public int iterations;
	//private const int numVertices = 2*iterations -1;

	/* Private Constants */
	private const int MAX_HEIGHT = 5;

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

	/* Helper Methods */

	// generates coordinates for the mesh, adding random things, init corners
	/*
	float[,] genHeights(int length) {
		Vector3[,] output = new Vector3[meshLength+1, meshLength+1];
		for (int i=0; i<meshLength+1; i++) {
			for (int j=0; j<meshLength+1; j++) {
				output[i, j] = new Vector3(i, 0, j);
			}

		}
		return output;
	} */

	// takes a map of Vector3 and returns the vertex map
	Vector3[] genVertices() {
		//Debug.Log(map);
		int index = 0;

		// 3 verts per tri, 2 tris per unitsquare, 4 unitsquares per gridsquare
		Vector3[] finalMesh = new Vector3[(int)Mathf.Pow(iterations, 2)*6*4+6];
		Debug.Log("finalMesh created with size: "+finalMesh.Length);

		for (int i=0; i<iterations*2-1; i+=2) {
			for (int j=0; j<iterations*2-1; j+=2) {
				Debug.Log("Creating a 4x4 unit square at: "+i+", "+j+", accessing index: "+index+" to array size: "+finalMesh.Length);
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

	// diamond step, takes 2d array of floats, calculates average of four corners and applies to middle
	void diamondStep(float[,] h, int x, int y, int len) {
		h[x+len/2, y+len/2] = (h[x, y]+h[x+len, y]+h[x, y+len]+h[x+len, y+len])/4;

	}

	// square step. note that length is halved relative to the diamond step
	void squareStep(float[,] h, int x, int y, int len) {
		// need to keep the relative positions in mind once len is added!
		int rightx = x+len*2;
		int leftx = x-len*2;
		int upy = y-len*2;
		int downy = y+len*2;
		// top
		h[x, y+len] = (y==0) ? (h[x+len, y]+h[x-len, y]+h[x, y+len])/3 : (h[x+len, y]+h[x-len, y]+h[x, y+len]+h[x, y-len])/4;
		// bottom
		h[x, y+len] = (y==0) ? (h[x+len, y]+h[x-len, y]+h[x, y+len])/3 : (h[x+len, y]+h[x-len, y]+h[x, y+len]+h[x, y-len])/4;
		// left
		h[x, y+len] = (y==0) ? (h[x+len, y]+h[x-len, y]+h[x, y+len])/3 : (h[x+len, y]+h[x-len, y]+h[x, y+len]+h[x, y-len])/4;
		// right
		h[x, y+len] = (y==0) ? (h[x+len, y]+h[x-len, y]+h[x, y+len])/3 : (h[x+len, y]+h[x-len, y]+h[x, y+len]+h[x, y-len])/4;
	}

	// generate appropriate random numbers
	float genRandHeight() {
		return Random.Range(0, MAX_HEIGHT);
	}



	// creates a 4x4 square, made up of unit squares
	void squarex4(Vector3[] v, int index, int posx, int posz) {
		unitSquare(v, index+=6, posx, posz);
		unitSquare(v, index+=6, posx+1, posz);
		unitSquare(v, index+=6, posx, posz+1);
		unitSquare(v, index+=6, posx+1, posz+1);

	}
	
}
