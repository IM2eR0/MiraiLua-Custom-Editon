# MiraiLua For Linux
- MiraiLua是基于 [Mirai.Net](https://github.com/SinoAHpx/Mirai.Net) / [mirai-api-http](https://github.com/project-mirai/mirai-api-http) 编写的以Lua为脚本引擎的QQ机器人框架。
- 原作者：[ABSD546316187](https://github.com/ABSD546316187)
- 原项目地址：[传送门](https://github.com/ABSD546316187/MiraiLua)

## 使用前须知
- 打包环境：Windows 11 x64 | Vistual Studio 2022 | .NET Core 3.1
- 打包目标：Linux x64 | 不依赖运行库
- 最低要求：Glibc 版本 > 2.29
- 测试环境：Debian 11

## 使用方法

- 切换到MiraiLua所在文件夹后，在Linux控制台输入
```bash
./MiraiLua
```
- 配置主程序目录下的 `settings.xml`
  - `Address` 是 [mirai-api-http](https://github.com/project-mirai/mirai-api-http) 中配置的地址
  - `QQ` 是 [mirai](https://github.com/mamoe/mirai) 中配置的QQ号
  - `Key` 是 [mirai-api-http](https://github.com/project-mirai/mirai-api-http) 中配置的 `VerifyKey` (如果存在)

- 配置 `base-libs/basic/init.lua` 第2行 `enableQ` 为启用的群列表
  
## API
- API已搬往 [MiraiLua社区论坛](https://teasmc.cn)
## 注意

- base-libs 文件夹用于存放MiraiLua的必要前置，这里的代码会被 C# 调用，切勿随意更改
- user-libs 文件夹用于存放用户前置
- user-plugins 文件夹用于存放用户插件

- 加载顺序如下 base-libs > user-libs > user-plugins
- `每个文件夹下的插件按照文件夹名的先后顺序进行排序 例：特殊符号 > 数字 0~9 > 大写字母 A~Z > 小写字母 a~z`