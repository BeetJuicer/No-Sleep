using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class FogPropagator : MonoBehaviour
{
    [SerializeField] private int initialLayers;
    [SerializeField] private float distanceInterval;
    [SerializeField] private GameObject smokePrefab;
    ObjectPool<Smoke> smokePool;
    Dictionary<Vector3Int, bool> smokeGrid = new(); // true if active, false if inactive.
    Grid grid;
    [SerializeField] Transform player;
    [SerializeField] int smokeSpawnRange;
    [SerializeField] LayerMask whatIsNoSmoke;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int total = initialLayers * initialLayers;
        smokePool = new ObjectPool<Smoke>(CreateSmoke,OnGetFromPool,OnReleaseToPool,OnDestroyFromPool,true, total*2, total*5);
        grid = GetComponent<Grid>();
        grid.cellSize = Vector3.one * distanceInterval;

        PropagateSmokes();
        StartCoroutine(nameof(GenerateSmokesPeriodically));
    }

    void PropagateSmokes()
    {
        Vector3 basePos = transform.position;

        //work from bottom left to top right
        for (int i = -initialLayers; i < initialLayers; i++)
        {
            for (int j = -initialLayers; j < initialLayers; j++)
            {

                Vector3 spawnPos = new Vector3(basePos.x + i * distanceInterval, basePos.y + j * distanceInterval, basePos.z);

                if (Physics2D.CircleCast(spawnPos, distanceInterval, Vector2.down, 0.1f, whatIsNoSmoke))
                {
                    print("niosmokedetected;");
                    continue;
                }

                var smoke = smokePool.Get();
                smoke.transform.position = spawnPos;
                
                Vector3Int cell = grid.WorldToCell(spawnPos);
                smoke.cellCoords = cell;
                smokeGrid.Add(cell, true); 
            }
        }
    }

    //check the players position and add new smokes to areas that the player will approach soon.
    private IEnumerator GenerateSmokesPeriodically()
    {
        Vector3Int lastPos = Vector3Int.zero;

        while (true)
        {   //Check in a radius around the player if we should generate new smokes.
            yield return new WaitForSeconds(0.5f);

            Vector3Int playerCell = grid.WorldToCell(player.position);
            
            if (lastPos == playerCell) continue;

            float distance = Vector3Int.Distance(lastPos, playerCell);

            // Get the direction the player is going
            Vector3Int direction = playerCell - lastPos;
            Vector3Int normalizedDirection = new Vector3Int(
                Mathf.Clamp(direction.x, -1, 1),
                Mathf.Clamp(direction.y, -1, 1),
                Mathf.Clamp(direction.z, -1, 1)
            );

            lastPos = playerCell;

            int startX = (normalizedDirection.x > 0) ? playerCell.x : playerCell.x - smokeSpawnRange;
            int endX = normalizedDirection.x > 0 ? playerCell.x + smokeSpawnRange : playerCell.x;

            int startY = (normalizedDirection.y > 0) ? playerCell.y : playerCell.y - smokeSpawnRange;
            int endY = (normalizedDirection.y > 0) ? playerCell.y + smokeSpawnRange : playerCell.y;

            int offsetX = 10 * MathF.Sign(startX);
            int offsetY = 10 * MathF.Sign(startY);

            startX += offsetX;
            startY += offsetY;

            for (int i = startX; i < endX; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    Vector3Int cell = new Vector3Int(i, j, 0);
                    if (smokeGrid.ContainsKey(cell))
                    {
                        continue;
                    }

                    Smoke smoke = smokePool.Get();
                    smoke.transform.position = grid.CellToWorld(cell);
                    smoke.cellCoords = cell;
                    smokeGrid.Add(cell, true);
                }
            }
        }
    }

    private Smoke CreateSmoke()
    {
        Smoke smoke = Instantiate(smokePrefab, this.transform).GetComponent<Smoke>();
        smoke.Pool = smokePool;
        return smoke;
    }

    private void OnGetFromPool(Smoke smoke)
    {
        smoke.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Smoke smoke)
    {
        smoke.gameObject.SetActive(false);
        smokeGrid[smoke.cellCoords] = false;
    }

    private void OnDestroyFromPool(Smoke smoke)
    {
        Destroy(smoke.gameObject);
    }
}
