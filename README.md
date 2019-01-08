已经实现的模块：
  - 数据相关
    - 本地持久化数据类
    - 静态读表工具类
  - 其它：
    - 待添加
------
#当前版本存在的问题：
  - UIWordPos： 在Canvas为 ScreenSpace - Camera 的时候，坐标对应不正确、在主摄像机为透视的情况下坐标出现问题，现已解决，但在透视下的距离没有掌握好
  - UIWordPos：当透视时，每次取的Z值都在变换。
  - ListLocalInt : CreateDefaultData方法里不能返回值，  SetData 的创建默认数据时不走CreateDefaultData
  
------
#可以设计或者更新的：
  - Python 导出读表cs脚本，解放程序重复的生产力。
  - ModelBase里面的默认字段去掉，在正式的项目里多余。StaticJson里面加入一个获取随机ID，获取随机ID并且不重复的方法。
  - Transform 相关的东西：缩放等，可以直接挂上脚本使用
  - UI：全局Tip功能
  - 角色数据结构及UI选择的Demo
      - 游戏实物的生成
      - UI视图的类型：
          - 3D渲染2D
  - KMTools 更新
  - IOS 推送更新
  
------
#猜想阶段，等待技术认证：
   - 类似PoolManager的插件，直接声明一个类，包括自己的Prefab引用，可以直接取，可以限制数量。
   - LocalData 监听数据的父类，写完之后可以对想要的数据进行监听
   - LocalData GM 管理场景，可以直接去设置全局数据
   - LocalList 相关的脚本，考虑去掉，使用起来必要性不大

------
#已知BUG：
   - IOS推送的 AddNotificationMessage() isRe 参数没有用到
