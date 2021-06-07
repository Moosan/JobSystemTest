using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
public class JonSample : MonoBehaviour
{
    [BurstCompile]
    private struct SampleJob : IJob
    {
        public int Value1;
        public int Value2;
        public NativeArray<int> Result;

        public void Execute()
        {
            for(var idx = 0; idx < Result.Length; ++idx)
            {
                Result[idx] = Value1 + Value2;
            }
        }
    }

    private const int ProcessCount = 1000000;

    private void Update()
    {
        ExecuteSampleJob();
    }

    private void ExecuteSampleJob()
    {
        // バッファ生成。
        var result = new NativeArray<int>(ProcessCount, Allocator.TempJob);

        // ジョブ生成。
        var sampleJob = new SampleJob {Value1 = 10, Value2 = 20, Result = result};

        // ジョブ実行。
        var jobHandle = sampleJob.Schedule();

        // ジョブ完了待機。
        jobHandle.Complete();

        // バッファの破棄。
        result.Dispose();
    }
}
