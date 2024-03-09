using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;

public class AsyncTest : MonoBehaviour
{
    private NativeArray<int> result;
    private JobHandle resultHandle;
    private bool isAsync;
    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        result = new NativeArray<int>(1, Allocator.Persistent);
        isAsync = false;
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning && FloorPlacer.isPlace)
        {
            AsyncJob asyncJob = new AsyncJob { result = result };
            resultHandle = asyncJob.Schedule();
            isAsync = true;
            isRunning = true;
            FloorPlacer.isPlace = false;
        }

        if (isAsync && resultHandle.IsCompleted)

        {
            isAsync = false;
            isRunning = false;
            resultHandle.Complete();

            Debug.Log(result[0]);

        }


    }
}


[BurstCompile]
public struct AsyncJob : IJob
{
    public NativeArray<int> result;

    public void Execute()
    {
        for (int i = 0; i < 999999999; i++)
        {
            result[0] = i;
            result[0] = i;

            result[0] = i;
            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;


            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;


            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;


            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;
            result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i; result[0] = i;
            result[0] = i;

            result[0] = i;

            result[0] = i;
            result[0] = i;

            result[0] = i;



        }
    }
}