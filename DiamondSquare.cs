using UnityEngine;
using System.Collections;

public class DiamondSquare: MonoBehaviour
{

    /* Public constants */
    public Shader shader;
	public int iterations;
	//private const int numVertices = 2*iterations -1;

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
		Vector3[] finalMesh = new Vector3[(int)Mathf.Pow(iterations, 2)*6];
		Debug.Log("finalMesh created with size: "+finalMesh.Length);

		for (int i=0; i<iterations; i++) {
			for (int j=0; j<iterations; j++) {
				Debug.Log("creating square: ["+i+", "+j+"], using index: "+index);
				unitSquare(finalMesh, index, i, j);
				index+=6;
			}
		}

		float blop=0f;
		for(int i=0; i<iterations; i++) {
			finalMesh[i].y += blop;
			blop+=0.03f;

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

	void squarex4(Vector3[] v, int index, int posx, int posz) {
		unitSquare(v, index+=6, posx, posz);
		unitSquare(v, index+=6, posx+1, posz);
		unitSquare(v, index+=6, posx, posz+1);
		unitSquare(v, index+=6, posx+1, posz+1);

	}
	
}
