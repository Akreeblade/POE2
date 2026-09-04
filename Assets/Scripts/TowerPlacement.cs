using UnityEngine;
using UnityEngine.InputSystem;

public class TowerPlacement : MonoBehaviour
{
    private GameManager gameManager;
    
    [Header("Tower")]
    public GameObject towerPrefab;

    [Header("Terrain")]
    public LayerMask terrainLayer;

    [Header("Placement")]
    public float placementRadius = 2f;
    public LayerMask obstacleLayer;

    [Header("Visual")]
    public GameObject placementIndicatorPrefab;

    private GameObject placementIndicator;
    private Renderer indicatorRenderer;

    private Vector3 currentPosition;
    private bool canPlace;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        // Create the placement indicator
        placementIndicator = Instantiate(placementIndicatorPrefab);

        indicatorRenderer = placementIndicator.GetComponent<Renderer>();

        // Make its size match the placement radius
        placementIndicator.transform.localScale =
            Vector3.one * placementRadius * 2f;
    }

    void Update()
    {
        UpdatePlacementPosition();

        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace)
        {
            PlaceTower();
        }
    }

    void UpdatePlacementPosition()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            1000f,
            terrainLayer))
        {
            currentPosition = hit.point;

            // Move the indicator to the mouse position
            placementIndicator.transform.position = currentPosition;

            Collider[] nearbyObjects = Physics.OverlapSphere(
                currentPosition,
                placementRadius,
                obstacleLayer
            );

            canPlace = nearbyObjects.Length == 0;

            // Change colour based on whether placement is valid
            indicatorRenderer.material.color =
                canPlace ? Color.green : Color.red;

            placementIndicator.SetActive(true);
        }
        else
        {
            canPlace = false;

            // Hide the indicator if we're not pointing at terrain
            placementIndicator.SetActive(false);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void PlaceTower()
    {
        GameObject towerObject = Instantiate(
            towerPrefab,
            currentPosition,
            Quaternion.identity
        );

        MainTower newTower = towerObject.GetComponent<MainTower>();

        if (newTower == null)
        {
            Debug.LogError("Tower prefab does not have a MainTower component!");
            return;
        }

        if (gameManager == null)
        {
            Debug.LogError("No GameManager found!");
            return;
        }

        // Give the newly created tower to the GameManager
        gameManager.tower = newTower;

        Debug.Log("Tower assigned to GameManager: " + gameManager.tower);

        // Now generate the paths
        gameManager.GeneratePaths();

        Destroy(placementIndicator);

        enabled = false;
    }
}