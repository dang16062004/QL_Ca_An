USE [master]
GO
/****** Object:  Database [QLCaAn]    Script Date: 11/07/2025 9:16:41 SA ******/
CREATE DATABASE [QLCaAn]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QLCaAn', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQL2016\MSSQL\DATA\QLCaAn.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QLCaAn_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQL2016\MSSQL\DATA\QLCaAn_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [QLCaAn] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QLCaAn].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QLCaAn] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QLCaAn] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QLCaAn] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QLCaAn] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QLCaAn] SET ARITHABORT OFF 
GO
ALTER DATABASE [QLCaAn] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QLCaAn] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QLCaAn] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QLCaAn] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QLCaAn] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QLCaAn] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QLCaAn] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QLCaAn] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QLCaAn] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QLCaAn] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QLCaAn] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QLCaAn] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QLCaAn] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QLCaAn] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QLCaAn] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QLCaAn] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QLCaAn] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QLCaAn] SET RECOVERY FULL 
GO
ALTER DATABASE [QLCaAn] SET  MULTI_USER 
GO
ALTER DATABASE [QLCaAn] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QLCaAn] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QLCaAn] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QLCaAn] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QLCaAn] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QLCaAn] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'QLCaAn', N'ON'
GO
ALTER DATABASE [QLCaAn] SET QUERY_STORE = ON
GO
ALTER DATABASE [QLCaAn] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [QLCaAn]
GO
/****** Object:  Table [dbo].[ChiTietDonDK]    Script Date: 11/07/2025 9:16:42 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonDK](
	[ID_ChiTietDonDK] [int] IDENTITY(1,1) NOT NULL,
	[SoLuong] [int] NOT NULL,
	[ID_NhanVien] [int] NULL,
	[ID_DonDK] [int] NOT NULL,
 CONSTRAINT [PK_ChiTietDonDK] PRIMARY KEY CLUSTERED 
(
	[ID_ChiTietDonDK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonDK]    Script Date: 11/07/2025 9:16:42 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonDK](
	[ID_DonDK] [int] IDENTITY(1,1) NOT NULL,
	[NgayDK] [datetime] NOT NULL,
	[LoaiDK] [nchar](20) NOT NULL,
	[ID_NhanVien] [int] NOT NULL,
	[CaAn] [int] NOT NULL,
	[TrangThai] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DonDK] PRIMARY KEY CLUSTERED 
(
	[ID_DonDK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 11/07/2025 9:16:42 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[ID_NhanVien] [int] IDENTITY(1,1) NOT NULL,
	[HoVaTen] [nvarchar](150) NOT NULL,
	[Namsinh] [date] NOT NULL,
	[TenDangNhap] [varchar](100) NOT NULL,
	[ID_Phong] [nchar](20) NOT NULL,
	[MatKhau] [nchar](50) NOT NULL,
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
(
	[ID_NhanVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien_TaiKhoan]    Script Date: 11/07/2025 9:16:42 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien_TaiKhoan](
	[ID_NhanVien] [int] NOT NULL,
	[ID_TaiKhoan] [int] NOT NULL,
 CONSTRAINT [PK_NhanVien_TaiKhoan] PRIMARY KEY CLUSTERED 
(
	[ID_NhanVien] ASC,
	[ID_TaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhongBan]    Script Date: 11/07/2025 9:16:42 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhongBan](
	[ID_Phong] [nchar](20) NOT NULL,
	[TenPhong] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PhongBan] PRIMARY KEY CLUSTERED 
(
	[ID_Phong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 11/07/2025 9:16:42 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[ID_TaiKhoan] [int] IDENTITY(1,1) NOT NULL,
	[NgayTao] [datetime] NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TaiKhoan] PRIMARY KEY CLUSTERED 
(
	[ID_TaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChiTietDonDK] ON 

INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (1, 12, 9, 1)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (2, 3, 9, 2)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (3, 3, 28, 2)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (4, 1, 9, 3)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (6, 1, 9, 5)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (7, 1, 33, 6)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (8, 2, 37, 7)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (9, 2, 36, 8)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (16, 2, 9, 15)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (17, 1, 34, 16)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (18, 1, 33, 17)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (19, 2, 33, 18)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (20, 2, 35, 19)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (21, 1, 9, 20)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (22, 1, 36, 21)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (24, 2, 36, 23)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (25, 3, 9, 24)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (26, 2, 39, 24)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (27, 1, 8, 31)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (28, 1, 33, 31)
INSERT [dbo].[ChiTietDonDK] ([ID_ChiTietDonDK], [SoLuong], [ID_NhanVien], [ID_DonDK]) VALUES (29, 2, 41, 32)
SET IDENTITY_INSERT [dbo].[ChiTietDonDK] OFF
GO
SET IDENTITY_INSERT [dbo].[DonDK] ON 

INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (1, CAST(N'2025-06-30T14:32:42.467' AS DateTime), N'CaNhan              ', 9, 3, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (2, CAST(N'2025-06-30T14:51:44.850' AS DateTime), N'TapThe              ', 9, 3, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (3, CAST(N'2025-06-30T14:52:53.373' AS DateTime), N'CaNhan              ', 9, 2, N'ChoXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (5, CAST(N'2025-07-01T10:12:09.197' AS DateTime), N'CaNhan              ', 9, 2, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (6, CAST(N'2025-07-08T14:38:28.493' AS DateTime), N'CaNhan              ', 33, 2, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (7, CAST(N'2025-07-08T14:40:21.183' AS DateTime), N'CaNhan              ', 37, 3, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (8, CAST(N'2025-07-08T14:41:28.097' AS DateTime), N'CaNhan              ', 36, 3, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (15, CAST(N'2025-07-08T15:54:33.443' AS DateTime), N'CaNhan              ', 9, 3, N'ChoXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (16, CAST(N'2025-07-09T11:37:25.083' AS DateTime), N'CANHAN              ', 34, 3, N'ChoXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (17, CAST(N'2025-07-09T11:40:33.530' AS DateTime), N'CANHAN              ', 33, 2, N'ChoXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (18, CAST(N'2025-07-09T14:51:34.583' AS DateTime), N'CaNhan              ', 33, 3, N'ChoXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (19, CAST(N'2025-07-09T14:52:27.743' AS DateTime), N'CaNhan              ', 35, 3, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (20, CAST(N'2025-07-09T14:54:34.953' AS DateTime), N'CaNhan              ', 9, 3, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (21, CAST(N'2025-07-09T16:58:24.847' AS DateTime), N'CaNhan              ', 36, 3, N'ChoXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (23, CAST(N'2025-07-10T08:44:50.060' AS DateTime), N'CaNhan              ', 36, 3, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (24, CAST(N'2025-07-10T14:15:27.790' AS DateTime), N'TapThe              ', 9, 3, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (31, CAST(N'2025-07-10T15:29:22.633' AS DateTime), N'TapThe              ', 41, 3, N'DaXacNhan')
INSERT [dbo].[DonDK] ([ID_DonDK], [NgayDK], [LoaiDK], [ID_NhanVien], [CaAn], [TrangThai]) VALUES (32, CAST(N'2025-07-10T16:07:35.143' AS DateTime), N'CaNhan              ', 41, 3, N'DaXacNhan')
SET IDENTITY_INSERT [dbo].[DonDK] OFF
GO
SET IDENTITY_INSERT [dbo].[NhanVien] ON 

INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (8, N'Phạm Công Đăng', CAST(N'2000-10-10' AS Date), N'dang@gmail.com', N'P01                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (9, N'Nguyễn Văn A', CAST(N'2009-02-09' AS Date), N'a@gmail.com', N'P10                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (10, N'Ngyên Văn  B', CAST(N'1995-05-01' AS Date), N'b@gmail.com', N'P09                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (28, N'abc', CAST(N'2000-05-02' AS Date), N'c@gmail.com', N'P03                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (33, N'Phạm DS', CAST(N'2025-07-01' AS Date), N'abc@gmail.com', N'P01                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (34, N'Lưng Văn Tài', CAST(N'2025-07-07' AS Date), N'tai@gmail.com', N'P07                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (35, N'Trần Phương ', CAST(N'2025-07-01' AS Date), N'phuong@gmail.com', N'P06                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (36, N'Đặng Uyên', CAST(N'2025-06-30' AS Date), N'uyen@gmail.com', N'P09                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (37, N'Nguyễn Song Sắt', CAST(N'2025-07-07' AS Date), N'sat@gmail.com', N'P04                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (39, N'Nguyễn Diva', CAST(N'2025-07-09' AS Date), N'a2@gmail.com', N'P10                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (40, N'Nguyexn DC', CAST(N'2025-07-10' AS Date), N'dc@gmail.com', N'P10                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (41, N'Nguyễn Ánh', CAST(N'2025-07-06' AS Date), N'anh@gmail.com', N'P01                 ', N'123                                               ')
INSERT [dbo].[NhanVien] ([ID_NhanVien], [HoVaTen], [Namsinh], [TenDangNhap], [ID_Phong], [MatKhau]) VALUES (42, N'Nguyễn Hương ', CAST(N'2025-07-10' AS Date), N'huong@gmail.com', N'P01                 ', N'123                                               ')
SET IDENTITY_INSERT [dbo].[NhanVien] OFF
GO
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (8, 1)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (8, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (9, 1)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (9, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (9, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (9, 4)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (10, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (10, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (33, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (33, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (34, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (34, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (35, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (35, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (36, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (36, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (37, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (37, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (39, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (39, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (40, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (40, 3)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (41, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (41, 4)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (42, 2)
INSERT [dbo].[NhanVien_TaiKhoan] ([ID_NhanVien], [ID_TaiKhoan]) VALUES (42, 3)
GO
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P01                 ', N'Kinh doanh')
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P02                 ', N'Hệ Thống')
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P03                 ', N'Marketing')
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P04                 ', N'Nhân sự')
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P05                 ', N'Hành Chính')
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P06                 ', N'IT')
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P07                 ', N'Chăm sóc khách hàng')
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P08                 ', N'Pháp chế')
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P09                 ', N'R&D')
INSERT [dbo].[PhongBan] ([ID_Phong], [TenPhong]) VALUES (N'P10                 ', N'Sản xuất')
GO
SET IDENTITY_INSERT [dbo].[TaiKhoan] ON 

INSERT [dbo].[TaiKhoan] ([ID_TaiKhoan], [NgayTao], [Role]) VALUES (1, CAST(N'2025-06-23T11:39:58.500' AS DateTime), N'Admin')
INSERT [dbo].[TaiKhoan] ([ID_TaiKhoan], [NgayTao], [Role]) VALUES (2, CAST(N'2025-06-23T11:40:21.173' AS DateTime), N'User')
INSERT [dbo].[TaiKhoan] ([ID_TaiKhoan], [NgayTao], [Role]) VALUES (3, CAST(N'2025-06-24T14:23:31.127' AS DateTime), N'CaNhan')
INSERT [dbo].[TaiKhoan] ([ID_TaiKhoan], [NgayTao], [Role]) VALUES (4, CAST(N'2025-06-25T08:52:47.797' AS DateTime), N'TapThe')
SET IDENTITY_INSERT [dbo].[TaiKhoan] OFF
GO
ALTER TABLE [dbo].[DonDK] ADD  CONSTRAINT [DF_DonDK_NgayDK]  DEFAULT (getdate()) FOR [NgayDK]
GO
ALTER TABLE [dbo].[TaiKhoan] ADD  CONSTRAINT [DF_TaiKhoan_NgayTao]  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[ChiTietDonDK]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonDK_ChiTietDonDK] FOREIGN KEY([ID_NhanVien])
REFERENCES [dbo].[NhanVien] ([ID_NhanVien])
GO
ALTER TABLE [dbo].[ChiTietDonDK] CHECK CONSTRAINT [FK_ChiTietDonDK_ChiTietDonDK]
GO
ALTER TABLE [dbo].[ChiTietDonDK]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonDK_DonDK] FOREIGN KEY([ID_DonDK])
REFERENCES [dbo].[DonDK] ([ID_DonDK])
GO
ALTER TABLE [dbo].[ChiTietDonDK] CHECK CONSTRAINT [FK_ChiTietDonDK_DonDK]
GO
ALTER TABLE [dbo].[DonDK]  WITH CHECK ADD  CONSTRAINT [FK_DonDK_NhanVien] FOREIGN KEY([ID_NhanVien])
REFERENCES [dbo].[NhanVien] ([ID_NhanVien])
GO
ALTER TABLE [dbo].[DonDK] CHECK CONSTRAINT [FK_DonDK_NhanVien]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_PhongBan] FOREIGN KEY([ID_Phong])
REFERENCES [dbo].[PhongBan] ([ID_Phong])
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [FK_NhanVien_PhongBan]
GO
ALTER TABLE [dbo].[NhanVien_TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_TaiKhoan_NhanVien] FOREIGN KEY([ID_NhanVien])
REFERENCES [dbo].[NhanVien] ([ID_NhanVien])
GO
ALTER TABLE [dbo].[NhanVien_TaiKhoan] CHECK CONSTRAINT [FK_NhanVien_TaiKhoan_NhanVien]
GO
ALTER TABLE [dbo].[NhanVien_TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_TaiKhoan_TaiKhoan] FOREIGN KEY([ID_TaiKhoan])
REFERENCES [dbo].[TaiKhoan] ([ID_TaiKhoan])
GO
ALTER TABLE [dbo].[NhanVien_TaiKhoan] CHECK CONSTRAINT [FK_NhanVien_TaiKhoan_TaiKhoan]
GO
USE [master]
GO
ALTER DATABASE [QLCaAn] SET  READ_WRITE 
GO
