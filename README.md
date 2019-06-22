# Ray-Obb Intersection( Unity Version)

原实现：
[misc05_picking_custom.cpp](https://github.com/opengl-tutorials/ogl/blob/master/misc05_picking/misc05_picking_custom.cpp)

原实现基本思路：
其实就是将射线转换到obb的坐标空间中，再用Ray与AABB相交检测的方法，来检测Ray是否与Obb相交。

参考：
1. [Picking with custom Ray-OBB function](http://www.opengl-tutorial.org/miscellaneous/clicking-on-objects/picking-with-custom-ray-obb-function/)
2. [光线与包围盒（AABB）的相交检测算法](https://blog.csdn.net/linuxheik/article/details/53395394)
3. [A Minimal Ray-Tracer: Rendering Simple Shapes (Sphere, Cube, Disk, Plane, etc.)](https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-box-intersection)

