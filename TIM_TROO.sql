USE [master]
GO
/****** Object:  Database [TIM_TROO]    Script Date: 6/6/2023 6:48:14 PM ******/
CREATE DATABASE [TIM_TROO]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TIM_TROO', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\TIM_TROO.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TIM_TROO_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\TIM_TROO_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TIM_TROO] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TIM_TROO].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TIM_TROO] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TIM_TROO] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TIM_TROO] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TIM_TROO] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TIM_TROO] SET ARITHABORT OFF 
GO
ALTER DATABASE [TIM_TROO] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [TIM_TROO] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TIM_TROO] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TIM_TROO] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TIM_TROO] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TIM_TROO] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TIM_TROO] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TIM_TROO] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TIM_TROO] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TIM_TROO] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TIM_TROO] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TIM_TROO] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TIM_TROO] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TIM_TROO] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TIM_TROO] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TIM_TROO] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TIM_TROO] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TIM_TROO] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TIM_TROO] SET  MULTI_USER 
GO
ALTER DATABASE [TIM_TROO] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TIM_TROO] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TIM_TROO] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TIM_TROO] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TIM_TROO] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TIM_TROO] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [TIM_TROO] SET QUERY_STORE = OFF
GO
USE [TIM_TROO]
GO
/****** Object:  Table [dbo].[ADMIN]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ADMIN](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](50) NULL,
	[SDT] [varchar](10) NULL,
	[Avatar] [varchar](255) NULL,
	[TrangThai] [tinyint] NULL,
	[Id_TaiKhoan] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANH_CCCD]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANH_CCCD](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTaiKhoan] [int] NULL,
	[IdCardPhoto] [varchar](255) NULL,
	[IdCardPhoto2] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CSVATCHAT]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CSVATCHAT](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_PhongTro] [int] NULL,
	[Ten] [nvarchar](50) NULL,
	[MoTa] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CHUTRO]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CHUTRO](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Facebook] [varchar](100) NULL,
	[Avatar] [varchar](100) NULL,
	[TrangThai] [tinyint] NULL,
	[Id_TaiKhoan] [int] NULL,
 CONSTRAINT [PK__CHUTRO__3214EC07C413C23E] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DONHANG]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DONHANG](
	[IdDH] [int] IDENTITY(1,1) NOT NULL,
	[DaThanhToan] [bit] NULL,
	[NgayDat] [smalldatetime] NULL,
	[Id_NguoiDung] [int] NULL,
	[Id_Phong] [int] NULL,
 CONSTRAINT [PK_DONHANG] PRIMARY KEY CLUSTERED 
(
	[IdDH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HOTRO]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HOTRO](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](50) NULL,
	[Sdt] [varchar](10) NULL,
	[MotaVande] [nvarchar](max) NULL,
 CONSTRAINT [PK_HOTRO] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IMAGES]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMAGES](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_PhongTro] [int] NULL,
	[Url_Path] [nvarchar](255) NULL,
	[ten] [nvarchar](100) NULL,
	[Url_Path2] [nvarchar](255) NULL,
	[Url_Path3] [nvarchar](255) NULL,
	[Url_Path4] [nvarchar](255) NULL,
	[Url_Path5] [nvarchar](255) NULL,
	[Id_TaiKhoan] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KHUVUC]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KHUVUC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](50) NULL,
	[Slug] [nvarchar](255) NULL,
	[TieuDe] [nvarchar](255) NULL,
	[MoTa] [nvarchar](255) NULL,
	[Parent_Id] [bigint] NULL,
	[Type] [tinyint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LUUTIN]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LUUTIN](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_NguoiDung] [int] NULL,
	[Id_PhongTro] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MENU]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MENU](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](100) NULL,
	[Parent_id] [int] NULL,
	[type] [tinyint] NULL,
	[Link] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NGUOIDUNG]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NGUOIDUNG](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Facebook] [varchar](255) NULL,
	[Avatar] [varchar](255) NULL,
	[Id_TaiKhoan] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PHONGTRO]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PHONGTRO](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_ChuTro] [int] NULL,
	[TenPhong] [nvarchar](255) NULL,
	[Slug] [nvarchar](255) NULL,
	[AnhBia] [varchar](255) NULL,
	[MoTa] [nvarchar](300) NULL,
	[GiaCa] [money] NULL,
	[HoaHong] [money] NULL,
	[TrangThai] [tinyint] NULL,
	[DienTich] [int] NULL,
	[SoLuong] [int] NULL,
	[Contents] [text] NULL,
	[Doituong] [tinyint] NULL,
	[Map] [text] NULL,
	[Video_link] [text] NULL,
	[Ngay] [smalldatetime] NULL,
	[Diachi] [nvarchar](155) NULL,
	[Nuoc] [money] NULL,
	[Dien] [money] NULL,
	[Internet] [money] NULL,
	[GuiXe] [money] NULL,
	[KhuVuc] [nvarchar](150) NULL,
 CONSTRAINT [PK__PHONGTRO__3214EC0702CD2E73] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PHONGTRO_DUYET]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PHONGTRO_DUYET](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_PhongTro] [int] NULL,
	[Id_ChuTro] [int] NULL,
	[TenPhong] [nvarchar](1) NULL,
	[Slug] [nvarchar](1) NULL,
	[AnhBia] [varchar](1) NULL,
	[MoTa] [nvarchar](1) NULL,
	[GiaCa] [money] NULL,
	[HoaHong] [money] NULL,
	[TrangThai] [tinyint] NOT NULL,
	[DienTich] [int] NULL,
	[SoLuong] [int] NULL,
	[Contents] [text] NULL,
	[Doituong] [tinyint] NULL,
	[Map] [text] NULL,
	[Video_link] [text] NULL,
	[Ngay] [smalldatetime] NULL,
	[Diachi] [nvarchar](1) NULL,
	[Nuoc] [money] NULL,
	[Dien] [money] NULL,
	[Internet] [money] NULL,
	[GuiXe] [money] NULL,
	[KhuVuc] [nvarchar](1) NULL,
	[IsApproved] [bit] NOT NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [nvarchar](1) NULL,
	[RejectReason] [nvarchar](1) NULL,
 CONSTRAINT [PK_PHONGTRO_DUYET] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TAIKHOAN]    Script Date: 6/6/2023 6:48:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TAIKHOAN](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaiKhoan] [varchar](50) NULL,
	[MatKhau] [varchar](50) NULL,
	[HoTen] [nvarchar](50) NULL,
	[PhanQuyen] [int] NULL,
	[Email] [varchar](50) NULL,
	[SDT] [varchar](10) NULL,
	[DiaChi] [nvarchar](50) NULL,
	[NgaySinh] [smalldatetime] NULL,
	[CCCD] [varchar](25) NULL,
 CONSTRAINT [PK_TAIKHOAN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ADMIN] ON 

INSERT [dbo].[ADMIN] ([Id], [Email], [SDT], [Avatar], [TrangThai], [Id_TaiKhoan]) VALUES (1, N'admin123@gmail.com', N'01234567', NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[ADMIN] OFF
GO
SET IDENTITY_INSERT [dbo].[ANH_CCCD] ON 

INSERT [dbo].[ANH_CCCD] ([Id], [IdTaiKhoan], [IdCardPhoto], [IdCardPhoto2]) VALUES (6, 65, N'z4345192741484_9dc9264834bdc0fb15df9edfc2c6553b.jpg', N'z4345192750085_a3f31b6cf2b6392db0cd12874eea0f6c.jpg')
INSERT [dbo].[ANH_CCCD] ([Id], [IdTaiKhoan], [IdCardPhoto], [IdCardPhoto2]) VALUES (8, 67, N'z4345192741484_9dc9264834bdc0fb15df9edfc2c6553b.jpg', N'z4345192750085_a3f31b6cf2b6392db0cd12874eea0f6c.jpg')
SET IDENTITY_INSERT [dbo].[ANH_CCCD] OFF
GO
SET IDENTITY_INSERT [dbo].[CHUTRO] ON 

INSERT [dbo].[CHUTRO] ([Id], [Facebook], [Avatar], [TrangThai], [Id_TaiKhoan]) VALUES (1, NULL, NULL, 1, 2)
INSERT [dbo].[CHUTRO] ([Id], [Facebook], [Avatar], [TrangThai], [Id_TaiKhoan]) VALUES (2, NULL, NULL, 1, 3)
INSERT [dbo].[CHUTRO] ([Id], [Facebook], [Avatar], [TrangThai], [Id_TaiKhoan]) VALUES (142, NULL, NULL, 1, 50)
INSERT [dbo].[CHUTRO] ([Id], [Facebook], [Avatar], [TrangThai], [Id_TaiKhoan]) VALUES (153, NULL, NULL, 1, 24)
INSERT [dbo].[CHUTRO] ([Id], [Facebook], [Avatar], [TrangThai], [Id_TaiKhoan]) VALUES (154, NULL, NULL, 1, 55)
INSERT [dbo].[CHUTRO] ([Id], [Facebook], [Avatar], [TrangThai], [Id_TaiKhoan]) VALUES (163, NULL, NULL, 1, 65)
INSERT [dbo].[CHUTRO] ([Id], [Facebook], [Avatar], [TrangThai], [Id_TaiKhoan]) VALUES (165, NULL, NULL, 1, 67)
SET IDENTITY_INSERT [dbo].[CHUTRO] OFF
GO
SET IDENTITY_INSERT [dbo].[DONHANG] ON 

INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (2, NULL, CAST(N'2023-03-11T09:38:00' AS SmallDateTime), 3, 15)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (4, NULL, CAST(N'2023-04-18T14:09:00' AS SmallDateTime), 1, 19)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (5, NULL, CAST(N'2023-04-18T14:21:00' AS SmallDateTime), 1, 4)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1007, NULL, CAST(N'2023-04-22T22:11:00' AS SmallDateTime), 1, 5)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1008, NULL, CAST(N'2023-04-22T22:26:00' AS SmallDateTime), 1, 6)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1009, NULL, CAST(N'2023-04-22T22:29:00' AS SmallDateTime), 1, 7)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1010, NULL, CAST(N'2023-04-22T22:35:00' AS SmallDateTime), 1, 12)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1011, NULL, CAST(N'2023-04-22T22:38:00' AS SmallDateTime), 1, 15)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1012, NULL, CAST(N'2023-04-22T22:44:00' AS SmallDateTime), 1, 20)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1013, NULL, CAST(N'2023-04-22T22:53:00' AS SmallDateTime), 1, 19)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1014, NULL, CAST(N'2023-04-26T12:20:00' AS SmallDateTime), 6, 6)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1015, NULL, CAST(N'2023-05-15T20:00:00' AS SmallDateTime), 1, 35)
INSERT [dbo].[DONHANG] ([IdDH], [DaThanhToan], [NgayDat], [Id_NguoiDung], [Id_Phong]) VALUES (1016, NULL, CAST(N'2023-05-22T19:44:00' AS SmallDateTime), 1, 4)
SET IDENTITY_INSERT [dbo].[DONHANG] OFF
GO
SET IDENTITY_INSERT [dbo].[IMAGES] ON 

INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (1, 4, N'a3.jpg', NULL, N'a2.jpg', N'a1.jpg', N'a4.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (3, 5, N'a1.jpg ', NULL, N'a2.jpg', N'a4.jpg', N'a3.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (4, 6, N'a2.jpg', NULL, N'a4.jpg', N'a3.jpg', N'a1.jpg ', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (7, 7, N'a4.jpg', NULL, N'a2.jpg', N'a1.jpg', N'a3.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (9, 12, N'a4.jpg', NULL, N'a1.jpg', N'a2.jpg', N'a3.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (10, 15, N'a4.jpg', NULL, N'a2.jpg', N'a1.jpg', N'a3.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (11, 17, N'a2.jpg', NULL, N'a2.jpg', N'a2.jpg', N'a2.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (12, 18, N'a18_1.jpg', NULL, N'a18_2.jpg', N'a18_3.jpg', N'a18_4.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (13, 19, N'a19_1.jpg', NULL, N'a19_2.jpg', N'a19_3.jpg', N'a19_4.jpg', N'a19_5.jpg', NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (14, 20, N'a20_1.jpg', NULL, N'a20_2.jpg', N'a20_3.jpg', N'a20_4.jpg', N'a20_5.jpg', NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (15, 23, N'a20_1.jbg', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (21, 29, N'a29_1.jpg', NULL, N'a29_2.jpg', N'a29_3.jpg', N'a29_4.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (22, 30, N'a30_1.jpg', NULL, N'a30_2.jpg', N'a30.jpg', N'a30_4.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (23, 31, N'a31_1.jpg', NULL, N'a31_2.jpg', N'a31_3.jpg', NULL, NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (24, 32, N'a32_1.jpg', NULL, N'a32_2.jpg', N'a32_3.jpg', N'a32_4.jpg', NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (25, 33, N'a2.jpg', NULL, N'a3.jpg', N'a5.jpg', NULL, NULL, NULL)
INSERT [dbo].[IMAGES] ([Id], [Id_PhongTro], [Url_Path], [ten], [Url_Path2], [Url_Path3], [Url_Path4], [Url_Path5], [Id_TaiKhoan]) VALUES (26, 35, N'a1.jpg', NULL, N'a2 - Copy.jpg', N'a2.jpg', N'a3.jpg', NULL, NULL)
SET IDENTITY_INSERT [dbo].[IMAGES] OFF
GO
SET IDENTITY_INSERT [dbo].[KHUVUC] ON 

INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (1, N'Hiệp Thành ', N'Binh-Duong', N'Các nhà trong đ?a bàn B?nh Dương', N'', 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (2, N'Phú Lợi', N'Thu-Dau-Mot', N'Các nhà trọ trong địa bàn Thủ Dầu Một', N'', 1, 2)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (3, N'Phú Cường', N'Phu-Hoa', N'Các nhà trong đ?a bàn Phư?ng Phú H?a', N'', 1, 2)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (4, N'Phú Hòa', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (5, N'Phú Thọ', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (6, N'Chánh Nghĩa', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (7, N'Định Hòa', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (8, N'Hòa Phú', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (9, N'Phú Mỹ ', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (10, N'Phú Tân ', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (11, N'Tân An', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (12, N'Hiệp An', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (13, N'Tương Hiệp Bình', NULL, NULL, NULL, 0, 1)
INSERT [dbo].[KHUVUC] ([Id], [Ten], [Slug], [TieuDe], [MoTa], [Parent_Id], [Type]) VALUES (14, N'Chánh Mỹ', NULL, NULL, NULL, 0, 1)
SET IDENTITY_INSERT [dbo].[KHUVUC] OFF
GO
SET IDENTITY_INSERT [dbo].[NGUOIDUNG] ON 

INSERT [dbo].[NGUOIDUNG] ([Id], [Facebook], [Avatar], [Id_TaiKhoan]) VALUES (1, NULL, NULL, 4)
INSERT [dbo].[NGUOIDUNG] ([Id], [Facebook], [Avatar], [Id_TaiKhoan]) VALUES (2, NULL, NULL, 14)
INSERT [dbo].[NGUOIDUNG] ([Id], [Facebook], [Avatar], [Id_TaiKhoan]) VALUES (3, NULL, NULL, 17)
INSERT [dbo].[NGUOIDUNG] ([Id], [Facebook], [Avatar], [Id_TaiKhoan]) VALUES (4, NULL, NULL, 25)
INSERT [dbo].[NGUOIDUNG] ([Id], [Facebook], [Avatar], [Id_TaiKhoan]) VALUES (5, NULL, NULL, 33)
INSERT [dbo].[NGUOIDUNG] ([Id], [Facebook], [Avatar], [Id_TaiKhoan]) VALUES (6, NULL, NULL, 40)
INSERT [dbo].[NGUOIDUNG] ([Id], [Facebook], [Avatar], [Id_TaiKhoan]) VALUES (9, NULL, NULL, 45)
INSERT [dbo].[NGUOIDUNG] ([Id], [Facebook], [Avatar], [Id_TaiKhoan]) VALUES (11, NULL, NULL, 54)
SET IDENTITY_INSERT [dbo].[NGUOIDUNG] OFF
GO
SET IDENTITY_INSERT [dbo].[PHONGTRO] ON 

INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (4, 1, N'Phòng giá rẻ', NULL, N'tro04.jpg', N'-Phòng trọ đẹp ', 1000000.0000, NULL, 2, 16, 1, NULL, 1, NULL, NULL, CAST(N'2023-05-22T19:42:00' AS SmallDateTime), N'Hẻm 147 đường Nguyễn Thị Minh Khai, P. Phú Hoà', 20000.0000, 3000.0000, 40000.0000, 0.0000, N'Phú Hòa')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (5, 1, N'Phòng trọ mini', NULL, N'tro02.jpg', N'-Phòng trọ đẹp ', 800000.0000, NULL, 3, 16, 2, NULL, 1, NULL, NULL, CAST(N'2023-04-22T22:35:00' AS SmallDateTime), N'668 đường Phú Lợi, Phường Phú Hòa, Thủ Dầu Một, Bình Dương.', 25000.0000, 3500.0000, 45000.0000, 0.0000, N'Hiệp Thành')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (6, 1, N'Phòng mới bình dương', NULL, N'tro03.jpg', N'-Cho thuê căn hộ có nội thất, Máy lạnh, Giường.', 1500000.0000, NULL, 3, 12, 2, NULL, 1, NULL, NULL, CAST(N'2023-04-26T12:21:00' AS SmallDateTime), N'178/57/14 Huỳnh Văn Lũy ,khu 7 phường Phú Lợi', 25000.0000, 3000.0000, 50000.0000, 50000.0000, N'Phú Lợi')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (7, 1, N'Phòng trọ cho sinh viên', NULL, N'tro06.jpg', N'-Phòng trọ đẹp ', 1400000.0000, NULL, 3, 12, 3, NULL, 1, NULL, NULL, CAST(N'2023-04-22T22:30:00' AS SmallDateTime), N'Hẻm 178/92A đường Huỳnh Văn Lũy, Phường Phú Lợi, Thủ Dầu Một.', 30000.0000, 2500.0000, 50000.0000, 60000.0000, N'Phú Thọ')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (12, 2, N'Phòng đẹp', NULL, N'tro01.jpg', N'-Phòng trọ đẹp ', 1000000.0000, NULL, 2, 14, 3, NULL, 1, NULL, NULL, CAST(N'2023-04-20T22:58:00' AS SmallDateTime), N'39/102 Đường Trần Bình Trọng, Phường Phú Thọ, Thủ Dầu Một, Bình Dương.', 20000.0000, 2000.0000, 45000.0000, 0.0000, N'Chánh Nghĩa')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (15, 1, N'Phòng trọ bình dương', NULL, N'tro05.jpg', N'-Phòng trọ đẹp 

', 900000.0000, NULL, 3, 16, 1, NULL, 1, NULL, NULL, CAST(N'2023-04-22T22:44:00' AS SmallDateTime), N'Bình Dương.', 15000.0000, 3000.0000, 50000.0000, 0.0000, N'Chánh Nghĩa')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (16, 2, N'Phòng trọ cao cấp', NULL, N'tro07.jpg', N'-Phòng trọ nâng cấp, giá ưu đãi', 1200000.0000, NULL, 0, 20, 1, NULL, 1, NULL, NULL, CAST(N'2022-11-23T00:00:00' AS SmallDateTime), N'hẻm 147 đường Nguyễn Thị Minh Khai - P. Phú Hoà.', 30000.0000, 3500.0000, 50000.0000, 50000.0000, N'Tân An')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (17, 142, N'Phòng trọ minh tâm', NULL, N'tro10.jpg', N'-Phòng', 1000000.0000, NULL, 0, 25, 2, NULL, 1, NULL, NULL, CAST(N'2023-03-12T00:00:00' AS SmallDateTime), N'đường lê hòng phông, thủ dầu một, bình dương', 3000.0000, 3500.0000, 20000.0000, 2000.0000, N'Phú Lợi')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (18, 142, N'Trọ gần Đại học Thù Dầu Một', NULL, N'a18_1.jpg', N'Phòng rộng rãi thoáng mát', 2000000.0000, NULL, 1, 20, 3, NULL, 1, NULL, NULL, CAST(N'2023-04-12T00:00:00' AS SmallDateTime), N'113, Trần Văn Ơn, Phường Phú Hòa, Thành Phố Thủ Dầu Một, Bình Dương', 12000.0000, 3200.0000, 10000.0000, 2000.0000, N'Phú Hòa')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (19, 142, N'Nhà Trọ Bình An', NULL, N'a19_1.jpg', N'Phòng óc sạch sẽ thoang mát', 1700000.0000, NULL, 2, 30, 3, NULL, 1, NULL, NULL, CAST(N'2023-04-18T14:10:00' AS SmallDateTime), N'1238/13, Lê Hồng Phong, Phường Phú Thọ, Thủ Dầu Một, Bình Dương', 15000.0000, 3500.0000, 0.0000, 0.0000, N'Phú Thọ')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (20, 1, N'Cho Thuê Nhà Trọ đường Huỳnh Văn Lũy ', NULL, N'a20_1.jpg', N'Cho Thuê nhà trọ mặt tiền', 1800000.0000, NULL, 3, 16, 1, NULL, 1, NULL, NULL, CAST(N'2023-04-22T22:54:00' AS SmallDateTime), N'Huỳnh Văn Lũy, Phường Phú Lợi, Thủ Dầu Một', 10000.0000, 3500.0000, 0.0000, 0.0000, N'Phú Lợi')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (29, 1, N'Phòng trọ cho sinh viên TDM, cổng đường Phú Lợi', NULL, N'a29_1.jpg', N'Phòng trọ cho Sinh Viên Đại học TDM, Ngay cổng đường Phú Lợi, siêu thị bách hoá xanh. Có gác, yên tĩnh học tập', 1800000.0000, NULL, 1, 25, 3, NULL, 1, NULL, NULL, CAST(N'2023-04-15T00:00:00' AS SmallDateTime), N'Hẻm 172, Phường Phú Hòa, Thành phố Thủ Dầu Một', 5000.0000, 3000.0000, 0.0000, 0.0000, N'Phú Hòa')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (30, 1, N'Phòng trọ gần trường ĐH Thủ Dầu Một', NULL, N'a30_1.jpg', N'Phòng rộng rãi, thoáng mát. Cách trường ĐH Thủ Dầu Một 450m', 2000000.0000, NULL, 1, 18, 2, NULL, 1, NULL, NULL, CAST(N'2023-04-05T00:00:00' AS SmallDateTime), N'113/70/2, Đường Trần Văn Ơn, Phường Phú Hòa, Thành Phố Thủ Dầu Một', 12000.0000, 3500.0000, 0.0000, 0.0000, N'Phú Hòa')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (31, 2, N'Phòng trọ gần trường Đại Học TDM', NULL, N'a31_1.jpg', N'Cho thuê p trọ gần trường Đại học TDM, kv an ninh, tiện ích xung quanh. Có BHX ngay đầu đường. Miễn fi wifi.', 1800000.0000, NULL, 1, 12, 2, NULL, 1, NULL, NULL, CAST(N'2023-04-03T00:00:00' AS SmallDateTime), N'Phú Lợi, Phường Phú Hòa, Thành Phố Thủ Dầu Một', 6000.0000, 3500.0000, 0.0000, 0.0000, N'Phú Hòa')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (32, 2, N'Cho thuê trọ mới', NULL, N'a32_1.jpg', N'Nhà trọ mới sạch sẽ,an ninh,thoáng mát,miễn phí wifi,đường ô tô vào được', 1600000.0000, NULL, 1, 24, 2, NULL, 1, NULL, NULL, CAST(N'2023-04-02T00:00:00' AS SmallDateTime), N'hẻm 194, Đường Nguyễn Thị Minh Khai, Phường Phú Hòa , Thủ Dầu Một', 5000.0000, 3500.0000, 0.0000, 0.0000, N'Phú Hòa')
INSERT [dbo].[PHONGTRO] ([Id], [Id_ChuTro], [TenPhong], [Slug], [AnhBia], [MoTa], [GiaCa], [HoaHong], [TrangThai], [DienTich], [SoLuong], [Contents], [Doituong], [Map], [Video_link], [Ngay], [Diachi], [Nuoc], [Dien], [Internet], [GuiXe], [KhuVuc]) VALUES (35, 163, N'Phong A', NULL, N'a1 - Copy.jpg', N'AAA

', 1000000.0000, NULL, 1, 12, 1, NULL, 1, NULL, NULL, CAST(N'2023-05-15T20:01:00' AS SmallDateTime), N'18,dx 076 , khu phố 2', 15000.0000, 3000.0000, 50000.0000, 0.0000, N'Binh Duong')
SET IDENTITY_INSERT [dbo].[PHONGTRO] OFF
GO
SET IDENTITY_INSERT [dbo].[TAIKHOAN] ON 

INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (1, N'admin', N'admin', N'ADMIN', 1, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (2, N'chutro1', N'12345', N'Hoàng', 2, NULL, NULL, N'Bình Dương', CAST(N'1994-01-01T00:00:00' AS SmallDateTime), N'123457865')
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (3, N'chutro2', N'12345', N'Hiếu', 2, N'tung231@gmail.com', N'0456456465', N'TP HCM', CAST(N'1999-01-04T00:00:00' AS SmallDateTime), N'07989564123')
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (4, N'nguoidung1', N'12345', N'HoàngHuy', 3, N'hoang12345@gmail.com', N'033242457', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (5, N'nguoidung2', N'12345', N'Phúc Hiếu', 3, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (10, N'123hieu', N'12345', N'Phuc Hieu', 2, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (11, N'chutro3', N'123456', N'OK', 2, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (14, N'nguoidung3', N'12345', N'NGUOIDUNG', 3, N'tam710@gmail.com', N'0339911993', N'Bình Dương', CAST(N'2002-10-07T00:00:00' AS SmallDateTime), N'0363256363')
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (15, N'a123', N'12345', N'aaaa', 2, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (16, N'a123', N'12345', N'aaaaaa', 2, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (17, N'tammm', N'12345', N'tam', 3, N'tam@gmail.com', N'113', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (19, N'chutro2', N'2', N'chutro2', 2, N'chutro2@gmail.com', N'0333333333', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (20, N'chutro3', N'3', N'chutro2', 2, N'afkkenta710@gmail.com', N'12121212', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (21, N'chutro3', N'3', N'chutro3', 2, N'afkkenta710@gmail.com', N'12121212', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (22, N'chutro4', N'4', N'chutro4', 2, N'afkkenta710@gmail.com', N'12121212', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (23, N'chutro4', N'4', N'chutro4', 2, N'chutro4@gmail.com', N'1231313131', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (24, N'a', N'a', N'tam', 2, N'asd@gmail.com', N'12131231', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (25, N'a', N'a', N'tam', 3, N'asd@gmail.com', N'12131231', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (26, N'tam', N'tam', N'tam', 2, N'tam@gmail.com', N'12313', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (27, N'tam', N'tam', N'hhhhhh', 2, N'tam@gmail.com', N'12313', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (28, N'h', N'h', N'hhhhhh', 2, N'tam@gmail.com', N'12313', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (33, N'1', N'1', N'1', 3, N'1', N'1', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (37, N'chutro5', N'5', N'chutro1', 2, N'ádasdasd', N'123123', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (38, N'chutro5', N'5', N'chutro1', 2, N'ádasdasd', N'123123', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (39, N'ab123', N'12345', N'AAA', 2, N'abba11@gmail.com', N'0123123', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (40, N'NGUOIDUNG05', N'12345', N'nguoidung5', 3, N'abc123321@gmail.com', N'0987894561', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (45, N'222', N'222', N'222', 3, N'222', N'222', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (50, N'888', N'888', N'888', 2, N'888', N'888', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (53, N'000', N'000', N'000', 2, N'000', N'000', NULL, NULL, NULL)
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (54, N'777', N'777', N'777', 3, N'777', N'777', N'777', CAST(N'2023-03-30T00:00:00' AS SmallDateTime), N'777')
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (55, N'999', N'999', N'999', 2, N'999', N'999', N'999', CAST(N'2023-04-10T00:00:00' AS SmallDateTime), N'999')
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (65, N'hoang', N'1', N'Do Huy Hoang', 2, N'hoang1907@gmail.com', N'0332247226', N'Bình Dương', CAST(N'2023-05-15T00:00:00' AS SmallDateTime), N'074202000069')
INSERT [dbo].[TAIKHOAN] ([Id], [TaiKhoan], [MatKhau], [HoTen], [PhanQuyen], [Email], [SDT], [DiaChi], [NgaySinh], [CCCD]) VALUES (67, N'kim', N'1', N'jix', 2, N'hoang@gmail.com', N'0332247226', N'Binh Duong', CAST(N'2002-12-06T00:00:00' AS SmallDateTime), N'11111')
SET IDENTITY_INSERT [dbo].[TAIKHOAN] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__ADMIN__A9D10534C9813FDE]    Script Date: 6/6/2023 6:48:14 PM ******/
ALTER TABLE [dbo].[ADMIN] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KHUVUC] ADD  DEFAULT ((0)) FOR [Parent_Id]
GO
ALTER TABLE [dbo].[KHUVUC] ADD  DEFAULT ((1)) FOR [Type]
GO
ALTER TABLE [dbo].[PHONGTRO] ADD  CONSTRAINT [DF__PHONGTRO__GiaCa__4E88ABD4]  DEFAULT ((0)) FOR [GiaCa]
GO
ALTER TABLE [dbo].[PHONGTRO] ADD  CONSTRAINT [DF__PHONGTRO__TrangT__4F7CD00D]  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[PHONGTRO_DUYET] ADD  DEFAULT ((4)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[PHONGTRO_DUYET] ADD  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[ANH_CCCD]  WITH CHECK ADD FOREIGN KEY([IdTaiKhoan])
REFERENCES [dbo].[TAIKHOAN] ([Id])
GO
ALTER TABLE [dbo].[ANH_CCCD]  WITH CHECK ADD  CONSTRAINT [FK_ANH_CCCD_IdTaiKhoan] FOREIGN KEY([IdTaiKhoan])
REFERENCES [dbo].[TAIKHOAN] ([Id])
GO
ALTER TABLE [dbo].[ANH_CCCD] CHECK CONSTRAINT [FK_ANH_CCCD_IdTaiKhoan]
GO
ALTER TABLE [dbo].[CSVATCHAT]  WITH CHECK ADD  CONSTRAINT [FK_CSVC_PT] FOREIGN KEY([Id_PhongTro])
REFERENCES [dbo].[PHONGTRO] ([Id])
GO
ALTER TABLE [dbo].[CSVATCHAT] CHECK CONSTRAINT [FK_CSVC_PT]
GO
ALTER TABLE [dbo].[CHUTRO]  WITH CHECK ADD  CONSTRAINT [FK_CHUTRO_TAIKHOAN] FOREIGN KEY([Id_TaiKhoan])
REFERENCES [dbo].[TAIKHOAN] ([Id])
GO
ALTER TABLE [dbo].[CHUTRO] CHECK CONSTRAINT [FK_CHUTRO_TAIKHOAN]
GO
ALTER TABLE [dbo].[DONHANG]  WITH CHECK ADD  CONSTRAINT [FK_DONHANG_NGUOIDUNG] FOREIGN KEY([Id_NguoiDung])
REFERENCES [dbo].[NGUOIDUNG] ([Id])
GO
ALTER TABLE [dbo].[DONHANG] CHECK CONSTRAINT [FK_DONHANG_NGUOIDUNG]
GO
ALTER TABLE [dbo].[LUUTIN]  WITH CHECK ADD  CONSTRAINT [FK_LT_ND] FOREIGN KEY([Id_NguoiDung])
REFERENCES [dbo].[NGUOIDUNG] ([Id])
GO
ALTER TABLE [dbo].[LUUTIN] CHECK CONSTRAINT [FK_LT_ND]
GO
ALTER TABLE [dbo].[LUUTIN]  WITH CHECK ADD  CONSTRAINT [FK_LT_PT] FOREIGN KEY([Id_PhongTro])
REFERENCES [dbo].[PHONGTRO] ([Id])
GO
ALTER TABLE [dbo].[LUUTIN] CHECK CONSTRAINT [FK_LT_PT]
GO
ALTER TABLE [dbo].[NGUOIDUNG]  WITH CHECK ADD  CONSTRAINT [FK_NGUOIDUNG_TAIKHOAN] FOREIGN KEY([Id_TaiKhoan])
REFERENCES [dbo].[TAIKHOAN] ([Id])
GO
ALTER TABLE [dbo].[NGUOIDUNG] CHECK CONSTRAINT [FK_NGUOIDUNG_TAIKHOAN]
GO
ALTER TABLE [dbo].[PHONGTRO_DUYET]  WITH CHECK ADD  CONSTRAINT [FK_PHONGTRO_DUYET_CHUTRO] FOREIGN KEY([Id_ChuTro])
REFERENCES [dbo].[CHUTRO] ([Id])
GO
ALTER TABLE [dbo].[PHONGTRO_DUYET] CHECK CONSTRAINT [FK_PHONGTRO_DUYET_CHUTRO]
GO
ALTER TABLE [dbo].[PHONGTRO]  WITH CHECK ADD  CONSTRAINT [CK__PHONGTRO__DienTi__59063A47] CHECK  (([DienTich]>(0)))
GO
ALTER TABLE [dbo].[PHONGTRO] CHECK CONSTRAINT [CK__PHONGTRO__DienTi__59063A47]
GO
ALTER TABLE [dbo].[PHONGTRO]  WITH CHECK ADD  CONSTRAINT [CK__PHONGTRO__GiaCa__59FA5E80] CHECK  (([GiaCa]>=(0)))
GO
ALTER TABLE [dbo].[PHONGTRO] CHECK CONSTRAINT [CK__PHONGTRO__GiaCa__59FA5E80]
GO
ALTER TABLE [dbo].[PHONGTRO]  WITH CHECK ADD  CONSTRAINT [CK__PHONGTRO__SoLuon__5AEE82B9] CHECK  (([SoLuong]>(0)))
GO
ALTER TABLE [dbo].[PHONGTRO] CHECK CONSTRAINT [CK__PHONGTRO__SoLuon__5AEE82B9]
GO
USE [master]
GO
ALTER DATABASE [TIM_TROO] SET  READ_WRITE 
GO
