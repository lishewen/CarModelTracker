# 🚗 CarModelTracker — 车模收藏管理

一个基于 .NET MAUI 的车模收藏管理应用，用于记录和管理你的汽车模型收藏。

## 功能

- **车模管理** — 添加、编辑、删除车模记录（品牌、车型、比例、颜色、价格等）
- **搜索筛选** — 按品牌/车型搜索，按比例筛选
- **统计概览** — 总数量、总价值、按品牌/比例分组统计
- **数据导出** — 导出为 Excel 文件
- **本地存储** — SQLite 本地数据库，数据存在手机上

## 技术栈

- .NET MAUI 10 (Android)
- MVVM 架构（CommunityToolkit.Mvvm）
- SQLite 本地数据库
- ClosedXML 导出 Excel
- 依赖注入

## 项目结构

```
CarModelTracker/
├── Models/           # 数据模型
├── ViewModels/       # ViewModel 层
├── Views/            # XAML 页面
├── Services/         # 数据库、导出、统计服务
├── Resources/        # 样式、字体、图标
└── Platforms/        # 平台特定代码
```

## 运行

1. 安装 [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
2. 安装 Visual Studio 2022（含 MAUI 工作负载）
3. 打开 `CarModelTracker.slnx`
4. 选择 Android 模拟器或连接真机
5. 运行

## License

MIT
