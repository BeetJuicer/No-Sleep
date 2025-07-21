using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum AttackDirection
{
    Left = -1,
    Right = 1
}

public class UpwardRockBarrageAbility : MonoBehaviour
{
    [SerializeField] private GameObject rock;
    [SerializeField] private int rockAmount;
    [SerializeField] private float distanceIntervals;
    [SerializeField] private float timeInterval;
    [SerializeField] private Vector3 rockRotation;

    [Header("FX")]
    [SerializeField] private GameObject vfx;
    [SerializeField] private Vector3 vfxOffset;

    private Vector3 startPosition;

    [NaughtyAttributes.Button]
    public void UseLeft()
    {
        UseAbility(AttackDirection.Left);
    }

    [NaughtyAttributes.Button]
    public void UseRight()
    {
        UseAbility(AttackDirection.Right);
    }

    private void UseAbility(AttackDirection direction)
    {
        StartCoroutine(SpawnRocksWithDelay(direction));
    }

    private IEnumerator SpawnRocksWithDelay(AttackDirection direction)
    {
        startPosition = transform.position;
        for (int i = 0; i < rockAmount; i++)
        {
            Vector3 spawnPoint = new Vector3(startPosition.x + i * distanceIntervals * (int)direction, startPosition.y, startPosition.z);

            Instantiate(rock, spawnPoint, Quaternion.Euler(rockRotation * (int)direction));
            Instantiate(vfx, spawnPoint + vfxOffset, vfx.transform.rotation);

            if (i < rockAmount - 1) // Don't wait after the last rock
                yield return new WaitForSeconds(timeInterval);
        }
    }
}
