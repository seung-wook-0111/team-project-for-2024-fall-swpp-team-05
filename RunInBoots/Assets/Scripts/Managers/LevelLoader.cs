using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public string stage;
    public string index;


    private string fileName;
    private string path;
    private TerrainData terrainData;

    private void Start()
    {

    }

    public void LoadLevel()
    {
        fileName = $"Stage_{stage}_{index}";
        path = Application.dataPath + "/Resources/TerrainData/" + fileName + ".json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            terrainData = JsonUtility.FromJson<TerrainData>(json);
            Debug.Log("Level data loaded from " + path);

            CreateObjectsFromData();
        }
        else
        {
            Debug.LogError("No terrain data fount at " + path);
        }
    }

    private void CreateObjectsFromData()
    {
        Transform parentTransform = this.transform;

        foreach (var objectPosition in terrainData.objectPositions.objPos)
        {
            // ������ �̸�
            string prefabName = objectPosition.name;
            GameObject prefab = Resources.Load<GameObject>("LevelObject/" + prefabName);

            if (prefab == null)
            {
                Debug.LogWarning($"Prefab '{prefabName}'�� ã�� �� �����ϴ�.");
                continue;
            }

            // �� ��ġ�� ������Ʈ�� ����
            foreach (Vector3 position in objectPosition.positions)
            {
                Instantiate(prefab, position, Quaternion.identity, parentTransform);
                Debug.Log($"Placing object '{prefabName}' at {position}");
            }
        }

        // ������ ���� ������ ó�� (�ɼ�)
        foreach (var pipeData in terrainData.pipeConnections.pipeList)
        {
            Debug.Log($"Pipe ID: {pipeData.pipeID}, Target Terrain Index: {pipeData.targetTerrainIndex}, Target Pipe ID: {pipeData.targetPipeID}");
            // �ʿ��� ��� �������� ���� ������ �߰��� �� �ֽ��ϴ�
        }
    }
}