using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

public class BurstSample : MonoBehaviour
{
    public void Calc()
    {
        var vec = Math.CalcBurst();
        Debug.Log(vec.x);
    }
    

}
[BurstCompile] // 必要
public static class Math
{
    // CompileFunctionPointerに渡すデリゲート.
    delegate void CalcDelegate(ref float3 vec);
    static CalcDelegate _calcDelegate;

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        // コンパイルした計算処理の関数ポインタを受け取ってデリゲートに保持.
        var funcPtr = BurstCompiler.CompileFunctionPointer<CalcDelegate>(CalcBurstImpl);
        _calcDelegate = funcPtr.Invoke;
    }

    public static float3 CalcBurst()
    {
        var vec = new float3();
        _calcDelegate(ref vec);
        return vec;
    }

    // コンパイル対象のメソッド
    // ※現状は戻り値を返すことが出来ないので、「参照渡し」か「ポインタ渡し」にする必要がある.
    [BurstCompile]
    static void CalcBurstImpl(ref float3 vec) => vec = Calc();  // Calcは上述のと同じ

    public static float3 Calc()
    {
        var vec = new float3();
        for (var i = 0; i < 10000000; i++)
        {
            var mat = new float4x4(quaternion.Euler(i, i, i), new float3(i, i, i));
            vec = math.transform(mat, vec);

            i += 1;
            mat = new float4x4(quaternion.Euler(i, i, i), new float3(i, i, i));
            vec = math.transform(mat, vec);

            i += 1;
            mat = new float4x4(quaternion.Euler(i, i, i), new float3(i, i, i));
            vec = math.transform(mat, vec);

            i += 1;
            mat = new float4x4(quaternion.Euler(i, i, i), new float3(i, i, i));
            vec = math.transform(mat, vec);

            i += 1;
            mat = new float4x4(quaternion.Euler(i, i, i), new float3(i, i, i));
            vec = math.transform(mat, vec);
        }

        return vec;
    }
}