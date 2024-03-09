using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI;
using Unity.AI.Navigation;

public class BuildNavmesh : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshSurface nav;
    public static bool navmeshReady;
    private void Start()
    {
        nav = GetComponent<NavMeshSurface>();

        StartCoroutine(CheckLoaded());
    }
    public void Build()
    {
        nav.BuildNavMesh();
        navmeshReady = true;
    }
    private IEnumerator CheckStateLoadedRequirement()
    {

        while (!StateLoaded.isLoaded)
        {
            yield return null;
        }


    }
    private IEnumerator CheckLoaded()
    {
        yield return CheckStateLoadedRequirement();
        nav.BuildNavMesh();
        navmeshReady = true;
    }
}
