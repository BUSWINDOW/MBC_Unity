using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;

public class TerrainTest : MonoBehaviour
{
    public Terrain terrain;
    public Transform test;
    public GameObject prefab;
    void Start()
    {
        Debug.Log(terrain.terrainData.size.x);
        Debug.Log(terrain.terrainData.size.z);
        // 월드 좌표를 터레인 좌표로 변환
        Vector3 terrainPosition = this.test.position - terrain.transform.position;

        // 터레인 좌표를 높이 맵 좌표로 변환 (0~1 범위)
        float normalizedX = terrainPosition.x / terrain.terrainData.size.x;
        float normalizedZ = terrainPosition.z / terrain.terrainData.size.z;

        // 높이값 가져오기
        float height = terrain.terrainData.GetInterpolatedHeight(normalizedX, normalizedZ);

        // 높이값 출력 (디버깅용)
        Debug.Log("Terrain Height at " + this.test.position + ": " + height);
        //StartCoroutine(RandomSpawn());
    }
    IEnumerator RandomSpawn()
    {
        while (true)
        {
            // 높이값 가져오기
            CreateTrash();

            //터레인의 랜덤좌표의 높이값을 받아와서, 해당 위치에 오브젝트를 생성
            //터레인의 원래 좌표를 계산해서 정확한 위치에 생성될수 있도록
            yield return new WaitForSeconds(3f);
        }
    }

    public GameObject CreateTrash()
    {
        var x = Random.Range(0, 1f);
        var z = Random.Range(0, 1f);
        float height = terrain.terrainData.GetInterpolatedHeight(x, z);

        //trashinfo에 들어갈 내용
        var pos = new Vector3(x * terrain.terrainData.size.x + terrain.transform.position.x
            , height + terrain.transform.position.y,
            z * terrain.terrainData.size.z + terrain.transform.position.z); // 좌표값
        // 저 좌표값을 기반으로 한 cell값
        // eNum기반 쓰래기 종류값
        // eNum기반 쓰래기 상태값
        // 회전을 넣게 된다면 회전값

        // 이에 대해서 오브젝트를 생성하는게 아닌, json을 활용해서 데이터를 우선적으로 생성한다.



        var trash = Instantiate(prefab, new Vector3(x * terrain.terrainData.size.x + terrain.transform.position.x
            , height + terrain.transform.position.y,
            z * terrain.terrainData.size.z + terrain.transform.position.z), Quaternion.identity);
        return trash;
    }
}
