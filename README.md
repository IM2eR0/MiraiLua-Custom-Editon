# MiraiLua Custom Editon (MCE)
- 提示：因最新版 MiraiLua 新增模块功能，Linux暂时停止后续维护以及更新，将由最新的 MiraiLua Custom Editon 继续维护
- 提示：因 MiraiLua Custom Editon(MCE) 不具备模块化功能，如有需要的功能请提交至Issue
- MiraiLua是基于 [Mirai.Net](https://github.com/SinoAHpx/Mirai.Net) / [mirai-api-http](https://github.com/project-mirai/mirai-api-http) 编写的以Lua为脚本引擎的QQ机器人框架。
- 原作者：[ABSD546316187](https://github.com/ABSD546316187)
- 原项目地址：[传送门](https://github.com/ABSD546316187/MiraiLua)

## 使用前须知
- 打包环境：Windows 11 x64 | Vistual Studio 2022 | .NET 7.0

- 系统要求相关：
  - Linux: Glibc 版本 > 2.29，指令集 x86_64，操作系统位数：64位
  - Windows: Windows 10 (Windows Server 2016)以上，指令集 x86_64，操作系统位数：32位

## 使用方法
- Linux: 切换到MCE所在文件夹后，在Linux控制台输入
```bash
./MiraiLua Custom Editon
```
- Windows: 双击MiraiLua Custom Editon.exe

- 配置主程序目录下的 `settings.xml`
  - `Address` 是 [mirai-api-http](https://github.com/project-mirai/mirai-api-http) 中配置的地址
  - `QQ` 是 [mirai](https://github.com/mamoe/mirai) 中配置的QQ号
  - `Key` 是 [mirai-api-http](https://github.com/project-mirai/mirai-api-http) 中配置的 `VerifyKey` (如果存在)

- 配置 `base-libs/basic/init.lua` 第2行 `enableQ` 为启用的群列表
  
## API
- API已搬往 [MiraiLua | 社区论坛](https://teasmc.cn/d/26-mirailua-api%E4%B8%80%E8%A7%88)
## 注意

- base-libs 文件夹用于存放MiraiLua的必要前置，这里的代码会被 C# 调用，切勿随意更改
- plugins 文件夹用于存放用户插件
