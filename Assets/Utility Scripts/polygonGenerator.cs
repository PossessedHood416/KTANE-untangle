using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class polygonGenerator : MonoBehaviour {

    public GameObject blankButton;
    public Material[] materials;
    List<List<int>> polygons = new List<List<int>>();

    List<Vector3> vertices = new List<Vector3>();
    List<int[]> triangles = new List<int[]>();

    int sides;

    void Start()
    {
        sides = UnityEngine.Random.Range(4,13);

        GenerateVertices();
        splitIntoTriangles();
        CreateShape();
    }

    void GenerateVertices() //A regular polygon is inscribed into a unit circle (each vertex's X and Y coordinate, when squared and summed, is equal to 1)
    {
        float angle = 360 / sides;
        float sidesFloat = sides;

        vertices.Add(new Vector3(0, 1, 1));

        for (int i = 1; i < sides; i++)
        {
            if(i*angle == 180)
            {
                vertices.Add(new Vector3(0,1,-1));
            }
            else if(i*angle > 180)
            {
                int gaming = Mathf.RoundToInt(0 - Mathf.Abs(i - sidesFloat / 2) + sidesFloat / 2);
                vertices.Add(new Vector3(-vertices[gaming].x, vertices[gaming].y, vertices[gaming].z));
            }
            else
            {
                vertices.Add(new Vector3(xpos(90-i * angle), 1, xpos(90 - i * angle) * ypos(90 - i * angle)));
            }
        }

        int swag = vertices.Count;
        for (int i = 0; i < swag; i++)
        {
            vertices.Add(new Vector3(vertices[i].x, vertices[i].y + .02f, vertices[i].z));
        }
    }

    void splitIntoTriangles()
    {
        //Vector3.Cross determines how one vertex rotates to another - a positive Y means it's clockwise, and a negative means it's counter-clockwise
        int fromVertex; //which vertex does the line start?
        int toVertex; //which vertex does the line try to connect to?

        bool allTriangles = false;

        int polygonWithLine = sides;
        bool foundPolygon = false;
        bool alreadyPlaced = false;

        int vertexCount = sides;

        polygons.Add(new List<int>());
        for(int i = 0; i < sides; i++)
        {
            polygons[0].Add(i);
        }

        while(!allTriangles) //while there are polygons that arent triangles
        {
            fromVertex = UnityEngine.Random.Range(0,sides);
            print("From vertex: " + fromVertex);

            determineToVertex:
            toVertex = UnityEngine.Random.Range(0, sides);
            if((fromVertex + 1) % sides == toVertex || (fromVertex - 1 + sides) % sides == toVertex || fromVertex == toVertex)
            {
                goto determineToVertex;
            }
            print("To vertex: " + toVertex);

            foundPolygon = false;
            alreadyPlaced = false;
            for(int i = 0; i < polygons.Count; i++)
            {
                if(polygons[i].Contains(fromVertex) && polygons[i].Contains(toVertex)) //if the polygon we're checking contains both 
                {
                    print("Polygon " + i + " contains both vertices.");
                    if(foundPolygon)
                    {
                        print("Another polygon already contains these, so this means these two points are already connected.");
                        alreadyPlaced = true;
                        break;
                    }
                    else
                    {
                        polygonWithLine = i;
                        foundPolygon = true;
                    }
                }
            }

            if(!alreadyPlaced)
            {
                if (foundPolygon)
                {
                    int polygonVertexCount = polygons[polygonWithLine].Count;
                    print("Splitting polygon " + polygonWithLine + ".");
                    polygons.Add(new List<int>());
                    print("Polygon has a count of " + polygonVertexCount + ".");
                    for(int i = 0; i < sides; i++)
                    {
                        if (polygons[polygonWithLine].Contains((i + fromVertex) % sides))
                        {
                            print("Polygon 1 of 2 contains vertex #" + ((i + fromVertex) % sides) + ".");
                            polygons[polygons.Count - 1].Add((i + fromVertex) % sides);
                            if ((i + fromVertex) % sides == toVertex)
                            {
                                break;
                            }
                            print("Polygon now has a count of " + polygons[polygons.Count - 1].Count);
                        }
                    }
                    polygons.Add(new List<int>());
                    for (int i = 0; i < sides; i++)
                    {
                        if (polygons[polygonWithLine].Contains((i + toVertex) % sides))
                        {
                            print("Polygon 2 of 2 contains vertex #" + ((i + toVertex) % sides) + ".");
                            polygons[polygons.Count - 1].Add((i + toVertex) % sides);
                            if ((i + toVertex) % sides == fromVertex)
                            {
                                break;
                            }
                            print("Polygon now has a count of " + polygons[polygons.Count - 1].Count);
                        }
                    }
                    polygons.RemoveAt(polygonWithLine);
                }
            }

            for (int i = 0; i < polygons.Count; i++)
            {
                if (polygons[i].Count != 3)
                {
                    print("Polygon " + i + " is not a triangle. Keep splitting.");
                    break;
                }
                else if(i == polygons.Count - 1)
                {
                    print("All polygons are triangles.");
                    allTriangles = true;
                }
            }
        }

        for(int i = 0; i < polygons.Count; i++)
        {
            print("Polygon " + i + "'s count: " + polygons[i].Count);
            for(int j = 0; j < polygons[i].Count; j++)
            {
                print("Polygon " + i + ": " + polygons[i][j]);
            }
        }
    }

    void CreateShape()
    {
		GameObject button;
		Renderer buttonRenderer;
        List<int> triangles = new List<int>();

        //Generate polygon with slices (splitIntoTriangles() needed)
        for (int i = 0; i < polygons.Count; i++)
        {
            Mesh mesh = new Mesh();

            button = Instantiate(blankButton,gameObject.transform.position,Quaternion.identity);
            button.transform.SetParent(gameObject.transform, false);
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.GetComponent<MeshFilter>().mesh = mesh;

            triangles.Clear();
            for(int j = 0; j < polygons[i].Count; j++)
            {
                triangles.Add(polygons[i][j]);
            }
            for(int j = 0; j < polygons[i].Count; j++)
            {
                triangles.Add(triangles[j] + sides);
            }
            int secondVertex = triangles[1];
            triangles[1] = triangles[2];
            triangles[2] = secondVertex;

            for (int j = 0; j < polygons[i].Count; j++)
            {
                triangles.Add(polygons[i][j]); //a vertex
                triangles.Add(polygons[i][(j + 1) % polygons[i].Count]); //the next vertex
                triangles.Add(polygons[i][j] + sides); //the top vertex

                triangles.Add(polygons[i][((j + 1) % polygons[i].Count)] + sides); //the next top vertex
                triangles.Add(polygons[i][j] + sides); //the top vertex
                triangles.Add(polygons[i][(j + 1) % polygons[i].Count]); //the next vertex
            }

            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();

            buttonRenderer = button.GetComponent<Renderer>();
            buttonRenderer.material = materials[UnityEngine.Random.Range(0, materials.Length)];
        }

        //Just generate the polygon with no slices
        /*for(int i = 0; i < sides; i++)
        {
            Mesh mesh = new Mesh();

            button = Instantiate(blankButton, gameObject.transform.position, Quaternion.identity);
            button.transform.SetParent(gameObject.transform, false);
            button.transform.localPosition = new Vector3(0, 0, 0);
            button.GetComponent<MeshFilter>().mesh = mesh;


        }*/
    }

    float xpos(float a)
    {
        return 1 / (Mathf.Sqrt(Mathf.Pow(Mathf.Tan(a/(180/Mathf.PI)),2)+1));
    }

    float ypos(float a)
    {
        return Mathf.Tan(a / (180 / Mathf.PI));
    }
}
