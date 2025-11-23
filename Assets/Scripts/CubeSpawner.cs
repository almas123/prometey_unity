using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public float moveSpeed = 5f;
    public float minBound = -5f;
    public float maxBound = 5f;
    public int maxCubes = 5;
    
    private GameObject[] Cubes;
    private int cubeCount;
    
    private void Start()
    {
        Cubes = new GameObject[maxCubes];
        
        if (cubePrefab == null)
        {
            cubePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubePrefab.SetActive(false);
        }
    }
    
   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCube();
        }
        
        MoveAllCubes();
    }
    
    private void SpawnCube()
    {
        if (cubeCount >= maxCubes)
        {
            return;
        }
        
        GameObject newCube = Instantiate(cubePrefab, new Vector3(0, 1, 0), Quaternion.identity);
        newCube.SetActive(true);
        
        Rigidbody rb = newCube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(rb);
        }
        
        Cubes[cubeCount] = newCube;
        cubeCount++;
    }
    
    private void MoveAllCubes()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
    
        Vector3 movement = new Vector3(horizontal, 0, vertical);
    
        for (int i = 0; i < cubeCount; i++)
        {
            if (Cubes[i] != null)
            {
                Cubes[i].transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
                CheckBounds(Cubes[i]);
            }
        }
    }
    
    private void CheckBounds(GameObject cube)
    {
        Vector3 pos = cube.transform.position;
        
        if (pos.x < minBound || pos.x > maxBound)
        {
            cube.transform.position = new Vector3(0, 1, 0);
            Debug.Log("Куб вышел за границу по X!");
            return;
        }
        
        if (pos.z < minBound || pos.y > maxBound)
        {
            cube.transform.position = new Vector3(0, 1, 0);
            Debug.Log("Куб вышел за границу по y!");
        }
    }
}