using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bigmode
{
    public static class Rand
    {
        public static RandState State(int seed) => new(seed);
        
        public static bool Chance(float chance) => Random.value <= chance;
        
        public static Vector3Int Offset(int max)
        {
            var x = Mathf.RoundToInt(max * Random.value);
            var y = Mathf.RoundToInt(max * Random.value);
            return new Vector3Int(x, y, 0);
        }
        
        public static RotationAction RotationAction => Constants.RotationActions.RandomElement();
    }
    
    public readonly struct RandState: IDisposable
    {
        private readonly Random.State prevState;
        
        public RandState(int seed)
        {
            prevState = Random.state;
            Random.InitState(seed);
        }
        
        public void Dispose()
        {
            Random.state = prevState;
        }
    }
}
