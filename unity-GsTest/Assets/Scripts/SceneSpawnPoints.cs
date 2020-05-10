using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneSpawnPoints : MonoBehaviour
{
    public List<Transform> spawnpoints;

    private Queue<Transform> tempSpawnpoints;
    private void ResetSpawnpoint()
    {
        var temp = new List<Transform>(spawnpoints);
        tempSpawnpoints = new Queue<Transform>();
        while (temp.Count > 0)
        {
            var randomIndex = Random.Range(0, temp.Count);
            var spawnPoint = temp[randomIndex];
            tempSpawnpoints.Enqueue(spawnPoint);
            temp.RemoveAt(randomIndex);
        }
        
    }
    public Vector3 GetRandomSpawnPosition()
    {
        if (tempSpawnpoints == null || tempSpawnpoints.Count == 0)
            ResetSpawnpoint();
        var spawnPoint = tempSpawnpoints.Dequeue();
        return spawnPoint.position;
    }
}
