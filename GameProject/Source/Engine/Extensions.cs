using DenWild.Source.Engine;
using DenWild.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Source.Engine
{
    public static class Extensions
    {
        public static Vector2 Rotate(this Vector2 vector, float angle)
        {
            var (x, y) = (vector.X, vector.Y);
            float rotatedX = (float)(x * Math.Cos(angle) - y * Math.Sin(angle));
            float rotatedY = (float)(x * Math.Sin(angle) + y * Math.Cos(angle));
            return new Vector2(rotatedX, rotatedY);
        }

        public static float ChangeRotationed(Vector2 rotatVector)
        {
            if (rotatVector.Y > 0 && rotatVector.X > 0)
                return (float)(Math.Atan(rotatVector.Y / rotatVector.X));
            else if (rotatVector.Y < 0 && rotatVector.X < 0)
                return (float)(Math.Atan(rotatVector.Y / rotatVector.X) + Math.PI);
            else if (rotatVector.Y > 0 && rotatVector.X < 0)
                return (float)(Math.Atan(rotatVector.Y / rotatVector.X) + Math.PI);
            return (float)(Math.Atan(rotatVector.Y / rotatVector.X));
        }

        public static void FloydWarshall_00(int[] matrix, int sz)
        {
            for (var k = 0; k < sz; ++k)
            {
                for (var i = 0; i < sz; ++i)
                {
                    for (var j = 0; j < sz; ++j)
                    {
                        var distance = matrix[i * sz + k] + matrix[k * sz + j];
                        if (matrix[i * sz + j] > distance)
                        {
                            matrix[i * sz + j] = distance;
                        }
                    }
                }
            }
        }

        public static bool CheckColision(this Entity checkEntity)
        {
            foreach(var entity in Globals.AllEntity)
            {
                if ((entity.Position - checkEntity.Position).Length() <
                    (entity.Dims.X + checkEntity.Dims.X) / 2)
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}
