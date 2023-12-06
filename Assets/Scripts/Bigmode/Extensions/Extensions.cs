using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bigmode
{
    public static class Extensions
    {
        public static Vector3Int Cell(this Vector3 pos) => World.Cell(pos);
        
        public static IEnumerable<Vector3Int> EdgeCells(this BoundsInt bounds)
        {
            for (var x = bounds.xMin; x <= bounds.xMax; x++)
            {
                yield return new Vector3Int(x, bounds.yMin, 0);
                yield return new Vector3Int(x, bounds.yMax, 0);
            }
            
            for (var y = bounds.yMin + 1; y < bounds.yMax; y++)
            {
                yield return new Vector3Int(bounds.xMin, y, 0);
                yield return new Vector3Int(bounds.xMax, y, 0);
            }
        }

        public static BoundsInt ExpandedBy(this BoundsInt bounds, int amount)
        {
            var min = bounds.min;
            var max = bounds.max;
            
            min -= new Vector3Int(amount, amount, 0);
            min += new Vector3Int(amount, amount, 0);
            
            bounds.SetMinMax(min, max);
            
            return bounds;
        }
        
        public static T RandomElement<T>(this IList<T> list)
        {
            return list.Count == 0 ? default : list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T[,] Transpose<T>(this T[,] array)
        {
            var created = new T[array.GetLength(1), array.GetLength(0)];

            for (var i = 0; i < created.GetLength(0); i++)
            {
                for (var j = 0; j < created.GetLength(1); j++)
                {
                    created[i, j] = array[j, i];
                }
            }

            return created;
        }

        public static T[,] GetRotation<T>(this T[,] array, RotationAction rotation)
        {
            return rotation switch
            {
                RotationAction.Normal => array,
                RotationAction.Left => array.RotateLeft(),
                RotationAction.Right => array.RotateRight(),
                RotationAction.Back => array.RotateOpposite(),
                _ => null
            };
        }
        
        public static T[,] RotateRight<T>(this T[,] array)
        {
            var created = array.Transpose();
            
            ReverseRows(created);
            
            return created;
        }
        
        public static T[,] RotateLeft<T>(this T[,] array)
        {
            var created = array.Copy();
            
            ReverseRows(created);
            
            return created.Transpose();
        }
        
        public static T[,] RotateOpposite<T>(this T[,] array)
        {
            return array.RotateRight().RotateRight();
        }
        
        public static T[,] Copy<T>(this T[,] array)
        {
            var created = new T[array.GetLength(0), array.GetLength(1)];
            
            for (var i = 0; i < created.GetLength(0); i++)
            {
                for (var j = 0; j < created.GetLength(1); j++)
                {
                    created[i, j] = array[i, j];
                }
            }
            
            return created;
        }
        
        public static void ReverseRows<T>(this T[,] array)
        {
            for (var i = 0; i < array.GetLength(1); i++)
            {
                var row = new T[array.GetLength(0)];

                for (var j = 0; j < array.GetLength(0); j++)
                {
                    row[j] = array[j, i];
                }

                Array.Reverse(row);

                for (var j = 0; j < array.GetLength(0); j++)
                {
                    array[j, i] = row[j];
                }
            }
        }
    }
}