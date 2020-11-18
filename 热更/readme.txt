资源热更
1，参考资料   https://www.lfzxb.top/gameframework_reshotfix/  注意，一定在打包设置中要生成full目录文件  因为真正热更是用full目录下的资源
  且只有打包full目录才会在BuildReport/0_1_0_2/BuildLog.txt 中生成 hashcode 和长度

2，热更成功后  资源默认存在 persistentDataPath路径下

3，HFS文件软件 特别注意 需要使用虚拟文件路径

3，下载文件夹是通过一个文件表来实现下载的