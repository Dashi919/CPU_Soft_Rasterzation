# CPU_Soft_Rasterzation
 A CPU soft rasterization by using wpf and c#
 版本 0.1 已经实现光栅化功能
 WSAD可以水平移动摄像机，可以往场景添加Cube，但Cube越多，每帧更新的时间越长

 接下来要添加的特性和修复的BUG有：
 1.阴影的计算（Shadow Map）
 2.参数反射在UI上（Reflection）
 3.摄像机的旋转（Camera Rotation）
 4.离线光线追踪 (RayTracing)


2022/10/11
为了让camera空间投影回light空间，我保存了每个插值后的着色点的worldPos，但插值用的是viewport中的重心坐标系，会使worldPos出现误差，导致投影到light空间的坐标错误。
解决思路一：网友说的透视矫正 https://zhuanlan.zhihu.com/p/403259571
解决思路二：MVP逆矩阵乘回去