USE [master]
GO
/****** Object:  Database [Wind]    Script Date: 09/22/2023 14:15:12 ******/
CREATE DATABASE [Wind] ON  PRIMARY 
( NAME = N'Wind', FILENAME = N'D:\DataBase\Wind.mdf' , SIZE = 172032KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Wind_log', FILENAME = N'D:\DataBase\Wind_log.ldf' , SIZE = 2304KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Wind] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Wind].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Wind] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Wind] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Wind] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Wind] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Wind] SET ARITHABORT OFF
GO
ALTER DATABASE [Wind] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [Wind] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [Wind] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Wind] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Wind] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Wind] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Wind] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Wind] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Wind] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Wind] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Wind] SET  DISABLE_BROKER
GO
ALTER DATABASE [Wind] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Wind] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Wind] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Wind] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Wind] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Wind] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Wind] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [Wind] SET  READ_WRITE
GO
ALTER DATABASE [Wind] SET RECOVERY FULL
GO
ALTER DATABASE [Wind] SET  MULTI_USER
GO
ALTER DATABASE [Wind] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Wind] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'Wind', N'ON'
GO
USE [Wind]
GO
/****** Object:  Table [dbo].[Test_Main]    Script Date: 09/22/2023 14:15:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Test_Main](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MainID] [varchar](50) NULL,
	[MainName] [nvarchar](50) NULL,
	[TypeID] [int] NULL,
	[Quantity] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
	[IsShow] [bit] NULL,
	[Img] [varchar](200) NULL,
	[Files] [varchar](200) NULL,
	[Remark] [nvarchar](500) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Test_Main] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'MainID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'MainName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属类型（外键）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'TypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'Quantity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'Img'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'Files'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'测试模块主表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Test_Main'
GO
/****** Object:  Table [dbo].[Sys_Type]    Script Date: 09/22/2023 14:15:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Type](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sys_Type] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Type', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型表、分类表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Type'
GO
/****** Object:  Table [dbo].[Sys_Admin]    Script Date: 09/22/2023 14:15:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sys_Admin](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserPwd] [varchar](500) NULL,
 CONSTRAINT [PK_Sys_Admin] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Admin', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码（加密后的字符串）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Admin', @level2type=N'COLUMN',@level2name=N'UserPwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员（后台登录）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Admin'
GO
