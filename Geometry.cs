using UnityEngine;
using System.Collections;
using System.Text;
using System;

namespace Utility.Geometry
{
    
    public class Geometry
    {
        /// <summary>
        /// 检查与某个轴是否相交
        /// </summary>
        /// <param name="delta">obb - origin，就是平移部分，就是射线原点在obb空间中的相对位置,也就是直线的B值</param>
        /// <param name="axis">obb 某个基向量</param>
        /// <param name="rayDir"> 射线的方向，最好就是单位向量 </param>
        /// <param name="leftPlane"> 垂直于axis的直线，“位于左侧”的那一个，参考 Slabs method 的图示</param>
        /// <param name="rightPlane"> 垂直于axis的直线，“位于右侧”的那一个，参考 Slabs method 的图示</param>
        /// <param name="tMin">射线与左侧相交时t的值</param>
        /// <param name="tMax">射线与右侧相交时t的值</param>
        /// <returns></returns>
        private bool IsRayIntersectsAxis(Vector3 delta, Vector3 axis, Vector3 rayDir, float leftPlane, float rightPlane, ref float tMin, ref float tMax)
        {
            /*
             *  射线的方程可以理解成
             *  f(t) = O + t * D 
             *  
             *  切面的方程
             *  f(x) = x ，这里的x就是leftPlane或者rightPlane 
             *  
             *  射线如果与切面相交
             *  
             *  O + t * D = x  
             *  
             *  ==>
             *  
             *  t = ( x - O ) / D , 交点就是
             *  
             */

            //算出原点在axis上的投影！！！
            float e = Vector3.Dot(axis, delta); 

            //计算一下rayDir在 axis方向上的投影，
            float f = Vector3.Dot(rayDir, axis); 

            //如果f == 0 ，就是说射线在该轴完全没有投影
            if (Math.Abs(f) > 0)  
            {
                float t1 = (e + leftPlane) / f;
                float t2 = (e + rightPlane) / f;

                if (t1 > t2)
                {
                    float w = t1;
                    t1 = t2;
                    t2 = w;
                }

                //离开平面时，最小的t值
                if (t2 < tMax)
                {
                    tMax = t2;
                }

                //进入平面时，最大的t值
                if (t1 > tMin)
                {
                    tMin = t1;
                }

                //如果离开平面的t值 比 进入平面的t值要小的话，那么与该轴，必然是没有相交的
                if (tMax < tMin)
                {
                    return false;
                }
            }
            else
            {
                //else是因为 射线与 axis 垂直，就是与leftPlane和rightPlane平行
                //下面这个判定就是说，如果e ，也就是射线的原点不在 (leftPlane,rightPlane)内的话，那么就是显示不在该轴对应的切面内。
                if (-e + leftPlane > 0
                    || -e + rightPlane < 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检测射线是否与Obb相交
        /// </summary>
        /// <param name="rayOrigin">射线的起点</param>
        /// <param name="rayDir">射线的方向</param>
        /// <param name="obbWorldPos">obb中心点在世界空间的坐标</param>
        /// <param name="obbExtend">obb的各维度size的一半</param>
        /// <param name="axisArray">obb的在模型空间中基向量</param>
        /// Reference：
        /// http://www.opengl-tutorial.org/miscellaneous/clicking-on-objects/picking-with-custom-ray-obb-function/
        /// https://github.com/opengl-tutorials/ogl/blob/master/misc05_picking/misc05_picking_custom.cpp
        /// https://blog.csdn.net/linuxheik/article/details/53395394
        /// https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-box-intersection
        /// <returns></returns>
        public bool IsRayIntersectsObb(Vector3 rayOrigin, Vector3 rayDir,
            Vector3 obbWorldPos, Vector3 obbExtents, Vector3[] axisArray)
        {

            float tMin = 0.0f;
            float tMax = float.MaxValue;

            Vector3 delta = obbWorldPos - rayOrigin;  //平移部分

            //意义类似于 AABB 的 Min和Max
            Vector3 obbBoundsMin = new Vector3(-obbExtents.x, -obbExtents.y, -obbExtents.z);
            Vector3 obbBoundsMax = new Vector3(obbExtents.x, obbExtents.y, obbExtents.z);

            Vector3 rayDirNormalize = rayDir.normalized; 

            //注意：与xAxis的相交检测，除了射线平行且射线原点不在两夹面内的情况会返回false之外
            //还有一种情况就是， obb 在 射线原点的后面，也就是算出来的t值是负的。那么这种情况也是不通过的。
            //其余情况都是无脑通过，为的是计算出tMin和tMax，给与当前axis组成一个面的下一个axis使用。
            //也就是符合 Slab-Method的操作
            //Test intersection with 2 planes perpendicular the OBB's X axis 
            if (!IsRayIntersectsAxis(delta, axisArray[0], rayDirNormalize, obbBoundsMin.x, obbBoundsMax.x, ref tMin, ref tMax))
            {
                return false;
            }

            //Test intersection with 2 planes perpendicular the OBB's Y axis 
            if (!IsRayIntersectsAxis(delta, axisArray[1], rayDirNormalize, obbBoundsMin.y, obbBoundsMax.y, ref tMin, ref tMax))
            {
                return false;
            }

            //Test intersection with 2 planes perpendicular the OBB's Z axis 
            if (!IsRayIntersectsAxis(delta, axisArray[2], rayDirNormalize, obbBoundsMin.z, obbBoundsMax.z, ref tMin, ref tMax))
            {
                return false;
            }

            return true;
        }

    }


}

