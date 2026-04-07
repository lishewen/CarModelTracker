# CarModelTracker 项目上下文

## 项目概述

**CarModelTracker** 是一个基于 .NET MAUI 的 Android 应用，用于管理车模收藏。用户可以添加、编辑、删除车模记录，支持搜索筛选、统计概览和数据导出功能。

### 主要功能
- **车模 CRUD**：添加、编辑、删除车模记录（品牌、车型、比例、颜色、价格等）
- **搜索与筛选**：按品牌/车型搜索，按比例筛选
- **统计概览**：总数量、总价值、按品牌/比例分组统计
- **数据导出**：导出为 Excel 文件
- **本地存储**：SQLite 本地数据库

### 技术栈
- **.NET MAUI 10** (Android)
- **MVVM 架构** (CommunityToolkit.Mvvm)
- **SQLite** 本地数据库 (sqlite-net-pcl)
- **ClosedXML** Excel 导出
- **依赖注入** (MauiApp builder)

## 项目结构

```
CarModelTracker/
├── Models/
│   └── CarModel.cs          # 车模数据模型（SQLite 实体）
├── ViewModels/
│   ├── MainViewModel.cs     # 主页面 ViewModel（列表、搜索、筛选）
│   ├── CarModelViewModel.cs # 编辑页面 ViewModel
│   └── StatisticsViewModel.cs # 统计页面 ViewModel
├── Views/
│   ├── MainPage.xaml(.cs)   # 主页面（列表、搜索、筛选）
│   ├── CarModelPage.xaml(.cs) # 编辑/添加车模页面
│   └── StatisticsPage.xaml(.cs) # 统计页面
├── Services/
│   ├── DatabaseService.cs   # SQLite 数据访问（IDatabaseService）
│   ├── ExportService.cs     # Excel 导出（IExportService）
│   └── StatisticsService.cs # 统计计算（IStatisticsService）
├── Resources/               # 样式、字体、图标
├── Platforms/               # 平台特定代码（Android）
├── App.xaml(.cs)            # 应用入口
├── MauiProgram.cs           # 应用配置、DI 注册
├── CarModelTracker.csproj   # 项目配置
└── CarModelTracker.slnx     # 解决方案文件
```

## 构建与运行

### 前置要求
- .NET 10 SDK
- Visual Studio 2022（含 MAUI 工作负载）
- Android SDK（API 24+）

### 运行命令
```bash
# 还原依赖
dotnet restore

# 构建项目
dotnet build

# 运行（Android）
dotnet run -t:Run -f net10.0-android
```

### 开发环境
- 打开 `CarModelTracker.slnx` 进行开发
- 选择 Android 模拟器或连接真机调试

## 开发规范

### 代码风格
- **可空引用类型**：已启用 (`<Nullable>enable</Nullable>`)
- **隐式 using**：已启用 (`<ImplicitUsings>enable</ImplicitUsings>`)
- **命名空间**：`CarModelTracker.{Models|ViewModels|Views|Services}`
- **接口命名**：服务接口使用 `I` 前缀（如 `IDatabaseService`）

### MVVM 模式
- 使用 `CommunityToolkit.Mvvm` 的源生成器
- `[ObservableProperty]` 特性生成属性变更通知
- `[RelayCommand]` 特性生成异步命令
- ViewModel 通过 DI 注册，页面通过构造函数注入 ViewModel

### 数据访问
- SQLite 异步 API (`SQLiteAsyncConnection`)
- 服务接口与实现分离
- 数据库文件路径：`Environment.SpecialFolder.LocalApplicationData/CarModels.db3`

### 依赖注入注册 (MauiProgram.cs)
```csharp
// 服务（单例）
builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
builder.Services.AddSingleton<IExportService, ExportService>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();

// ViewModel（瞬态）
builder.Services.AddTransient<ViewModels.MainViewModel>();
builder.Services.AddTransient<ViewModels.CarModelViewModel>();
builder.Services.AddTransient<ViewModels.StatisticsViewModel>();

// 页面（瞬态）
builder.Services.AddTransient<Views.MainPage>();
builder.Services.AddTransient<Views.CarModelPage>();
builder.Services.AddTransient<Views.StatisticsPage>();
```

## CarModel 数据模型

| 属性 | 类型 | 说明 |
|------|------|------|
| Id | int | 主键，自增 |
| Brand | string | 品牌（AutoArt/Minichamps 等） |
| ModelName | string | 车型名称 |
| Scale | string | 比例（1:18/1:43/1:64 等） |
| Color | string | 颜色 |
| PurchaseDate | DateTime | 购买日期 |
| PurchasePrice | decimal | 购买价格 |
| Quantity | int | 数量 |
| Notes | string | 备注 |
| CreatedAt | DateTime | 创建时间 |
| UpdatedAt | DateTime | 更新时间 |
| TotalValue | decimal | 总价值（计算属性） |

## 常用操作

```bash
# 清理构建
dotnet clean

# 发布 APK
dotnet publish -f net10.0-android -c Release

# 查看项目信息
dotnet --info
```
