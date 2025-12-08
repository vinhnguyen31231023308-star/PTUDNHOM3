IF EXISTS (SELECT name FROM sys.databases WHERE name = 'HairNova')
BEGIN
    ALTER DATABASE [HairNova] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE [HairNova]
END
GO

CREATE DATABASE [HairNova]
GO

USE HairNova
GO

-- 1. BẢNG KHÁCH HÀNG
CREATE TABLE KhachHang (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    TenDangNhap NVARCHAR(50) UNIQUE,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    MatKhau NVARCHAR(255),
    HoTen NVARCHAR(100),
    SoDienThoai VARCHAR(15),
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    AnhDaiDien NVARCHAR(255),
    TrangThai BIT null,
    NgayTao DATETIME null,
    NgayCapNhat DATETIME null,
    CONSTRAINT [PK_KhachHang] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 2. BẢNG ĐỊA CHỈ KHÁCH HÀNG
CREATE TABLE DiaChiKhachHang (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    KhachHangId BIGINT NOT NULL,
    TenNguoiNhan NVARCHAR(100),
    SoDienThoaiNguoiNhan VARCHAR(15),
    TinhThanh NVARCHAR(100),
    Phuong NVARCHAR(100),
    DiaChiCuThe NVARCHAR(255),
    LoaiDiaChi VARCHAR(20),
    MacDinh BIT null,
    CONSTRAINT [PK_DiaChiKhachHang] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    FOREIGN KEY (KhachHangId) REFERENCES KhachHang(Id) 
) ON [PRIMARY];
GO

-- 3. BẢNG NHÀ CUNG CẤP
CREATE TABLE NhaCungCap (
    Id INT IDENTITY(1,1) NOT NULL,
    TenNhaCungCap NVARCHAR(255) NOT NULL,
    EmailLienHe VARCHAR(100),
    SoDienThoai VARCHAR(20),
    DiaChi NVARCHAR(255),
    MaSoThue VARCHAR(50),
    NgayTao DATETIME null,
    CONSTRAINT [PK_NhaCungCap] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 4. BẢNG DANH MỤC CHA
CREATE TABLE DanhMuc (
    Id INT IDENTITY(1,1) NOT NULL,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    Slug VARCHAR(150) UNIQUE,
    MoTa NVARCHAR(500),
    HinhAnh NVARCHAR(255),
    HienThi BIT null,
    NgayTao DATETIME null,
    CONSTRAINT [PK_DanhMuc] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 5. BẢNG DANH MỤC CON
CREATE TABLE DanhMucCon (
    Id INT IDENTITY(1,1) NOT NULL,
    DanhMucId INT NOT NULL,
    TenDanhMucCon NVARCHAR(100) NOT NULL,
    Slug VARCHAR(150) UNIQUE,
    MoTa NVARCHAR(500),
    CONSTRAINT [PK_DanhMucCon] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    FOREIGN KEY (DanhMucId) REFERENCES DanhMuc(Id)
) ON [PRIMARY];
GO

-- 6. BẢNG SẢN PHẨM
CREATE TABLE SanPham (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    DanhMucConId INT,
    NhaCungCapId INT,
    TenSanPham NVARCHAR(255) NOT NULL,
    Slug VARCHAR(255) UNIQUE,
    MaSku VARCHAR(50) UNIQUE,
    GiaGoc DECIMAL(18, 2),
    GiaBan DECIMAL(18, 2) NOT NULL,
    MoTaNgan NVARCHAR(500),
    MoTaChiTiet NVARCHAR(MAX),
    SoLuongTon INT ,
    DaBan INT ,
    LuotXem INT ,
    NoiBat BIT NULL,
    HienThi BIT NULL,
    TheTieuDe NVARCHAR(255),
    TheMoTa NVARCHAR(500),
    NgayTao DATETIME NULL,        
    NgayCapNhat DATETIME NULL,    
    CONSTRAINT [PK_SanPham] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 7. BẢNG HÌNH ẢNH SẢN PHẨM
CREATE TABLE HinhAnhSanPham (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    SanPhamId BIGINT NOT NULL,
    DuongDanAnh NVARCHAR(255) NOT NULL,
    LaAnhChinh BIT null,
    ThuTu INT null,
    CONSTRAINT [PK_HinhAnhSanPham] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY];
GO

-- 8. BẢNG THUỘC TÍNH SẢN PHẨM
CREATE TABLE ThuocTinhSanPham (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    SanPhamId BIGINT NOT NULL,
    TenThuocTinh NVARCHAR(50), 
    GiaTri NVARCHAR(50),
    GiaCongThem DECIMAL(18, 2),
    CONSTRAINT [PK_ThuocTinhSanPham] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY];
GO

-- 9. BẢNG MÃ GIẢM GIÁ
CREATE TABLE MaGiamGia (
    Id INT IDENTITY(1,1) NOT NULL,
    MaCode VARCHAR(50) UNIQUE NOT NULL,
    MoTa NVARCHAR(255),
    LoaiGiamGia VARCHAR(20),
    GiaTriGiam DECIMAL(18, 2),
    GiamToiDa DECIMAL(18, 2),
    DonToiThieu DECIMAL(18, 2),
    SoLuong INT,
    DaSuDung INT ,
    GioiHanMoiNguoi INT ,
    NgayBatDau DATETIME,
    NgayKetThuc DATETIME,
    TrangThai BIT null,
    CONSTRAINT [PK_MaGiamGia] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 10. BẢNG TRẠNG THÁI ĐƠN HÀNG
CREATE TABLE TrangThaiDonHang (
    Id INT IDENTITY(1,1) NOT NULL,
    TenTrangThai NVARCHAR(50),
    CONSTRAINT [PK_TrangThaiDonHang] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 11. BẢNG ĐƠN HÀNG
CREATE TABLE DonHang (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    MaDonHang VARCHAR(20) UNIQUE,
    KhachHangId BIGINT,
    TenNguoiNhan NVARCHAR(100),
    SdtNguoiNhan VARCHAR(15),
    DiaChiGiaoHang NVARCHAR(500),
    TinhThanh NVARCHAR(100),
    QuanHuyen NVARCHAR(100),
    PhuongXa NVARCHAR(100),
    TongTienHang DECIMAL(18, 2),
    PhiVanChuyen DECIMAL(18, 2),
    GiamGia DECIMAL(18, 2) ,
    TongThanhToan DECIMAL(18, 2),
    MaGiamGiaId INT,
    TrangThaiDonHangId INT,
    GhiChu NVARCHAR(500),
    DonViVanChuyen NVARCHAR(50),
    MaVanDon VARCHAR(50),
    NgayTao DATETIME ,
    NgayCapNhat DATETIME ,
    CONSTRAINT [PK_DonHang] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    FOREIGN KEY (KhachHangId) REFERENCES KhachHang(Id),
    FOREIGN KEY (MaGiamGiaId) REFERENCES MaGiamGia(Id),
    FOREIGN KEY (TrangThaiDonHangId) REFERENCES TrangThaiDonHang(Id)
) ON [PRIMARY];
GO

-- 12. BẢNG CHI TIẾT ĐƠN HÀNG
CREATE TABLE ChiTietDonHang (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    DonHangId BIGINT NOT NULL,
    SanPhamId BIGINT,
    TenSanPham NVARCHAR(255),
    AnhSanPham NVARCHAR(255),
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18, 2),
    TongTien DECIMAL(18, 2),
    ThuocTinhDaChon NVARCHAR(255),
    CONSTRAINT [PK_ChiTietDonHang] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 13. BẢNG THANH TOÁN
CREATE TABLE ThanhToan (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    DonHangId BIGINT NOT NULL,
    PhuongThuc NVARCHAR(50),
    SoTien DECIMAL(18, 2),
    MaGiaoDich VARCHAR(100),
    TrangThai NVARCHAR(50),
    NgayThanhToan DATETIME,
    CONSTRAINT [PK_ThanhToan] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY];
GO

-- 14. BẢNG GIỎ HÀNG
CREATE TABLE GioHang (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    KhachHangId BIGINT,
    NgayTao DATETIME null,
    NgayCapNhat DATETIME ,
    CONSTRAINT [PK_GioHang] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 15. BẢNG CHI TIẾT GIỎ HÀNG
CREATE TABLE ChiTietGioHang (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    GioHangId BIGINT NOT NULL,
    SanPhamId BIGINT NOT NULL,
    SoLuong INT ,
    ThuocTinh NVARCHAR(255),
    CONSTRAINT [PK_ChiTietGioHang] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) ON [PRIMARY];
GO

-- 16. BẢNG ĐÁNH GIÁ
CREATE TABLE DanhGia (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    KhachHangId BIGINT NOT NULL,
    SanPhamId BIGINT NOT NULL,
    DonHangId BIGINT,
    SoSao INT CHECK (SoSao >= 1 AND SoSao <= 5),
    TieuDe NVARCHAR(100),
    NoiDung NVARCHAR(MAX),
    HinhAnh NVARCHAR(MAX),
    DaDuyet BIT null,
    NgayTao DATETIME null,
    CONSTRAINT [PK_DanhGia] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
  
) ON [PRIMARY];
GO

-- 17. BẢNG SẢN PHẨM YÊU THÍCH
CREATE TABLE SanPhamYeuThich (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    KhachHangId BIGINT NOT NULL,
    SanPhamId BIGINT NOT NULL,
    NgayThem DATETIME ,
    CONSTRAINT [PK_SanPhamYeuThich] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
  
) ON [PRIMARY];
GO
-- 18. BẢNG LIÊN HỆ TƯ VẤN
CREATE TABLE LienHeTuVan (
    Id INT IDENTITY(1,1) NOT NULL,
    HoTen NVARCHAR(100),
    SoDienThoai VARCHAR(20),
    Email NVARCHAR(100),
    NoiDung NVARCHAR(MAX),
    TrangThai NVARCHAR(50),
    NgayGui DATETIME ,
    CONSTRAINT [PK_LienHeTuVan] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 19. BẢNG DANH MỤC TIN TỨC
CREATE TABLE DanhMucTinTuc (
    Id INT IDENTITY(1,1) NOT NULL,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    TrangThai BIT null,
    CONSTRAINT [PK_DanhMucTinTuc] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 20. BẢNG TIN TỨC
CREATE TABLE TinTuc (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    DanhMucTinTucId INT,
    TieuDe NVARCHAR(255) NOT NULL,
    Slug VARCHAR(255),
    HinhAnh NVARCHAR(255),
    TomTat NVARCHAR(500),
    NoiDung NVARCHAR(MAX),
    LuotXem INT ,
    TacGiaId BIGINT,
    NgayXuatBan DATETIME,
    CONSTRAINT [PK_TinTuc] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    FOREIGN KEY (DanhMucTinTucId) REFERENCES DanhMucTinTuc(Id),
    FOREIGN KEY (TacGiaId) REFERENCES KhachHang(Id)
) ON [PRIMARY];
GO

-- 21. BẢNG NHÂN VIÊN
CREATE TABLE NhanVien (
    Id INT IDENTITY(1,1) NOT NULL,
    HoTen NVARCHAR(50) NOT NULL,
    NgaySinh DATE NULL,
    GioiTinh NVARCHAR(10) NULL,
    Email VARCHAR(50) UNIQUE NULL,
    DiaChi NVARCHAR(100) NULL,
    DienThoaiNha VARCHAR(15) NULL,
    DiDong VARCHAR(15) NOT NULL,
    DuongDanAnh NVARCHAR(500) NULL,
    NgayVaoLam DATETIME ,
    CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- 22. BẢNG TÀI KHOẢN QUẢN TRỊ
CREATE TABLE TaiKhoanQuanTri (
    Id INT IDENTITY(1,1) NOT NULL,
    NhanVienId INT NOT NULL,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(255) NOT NULL,
    LoaiQuyen INT NULL,
    GhiChu NVARCHAR(255) NULL,
    TrangThai BIT null,
    CONSTRAINT [PK_TaiKhoanQuanTri] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

USE HairNova
GO
-- 1. DATA KHÁCH HÀNG (5 người)
INSERT INTO KhachHang (TenDangNhap, Email, MatKhau, HoTen, SoDienThoai, GioiTinh, NgaySinh, TrangThai) VALUES
(N'nguyenvana', 'nguyenvana@gmail.com', N'hashed_password_1', N'Nguyễn Văn A', '0901234567', N'Nam', '1995-01-15', 1),
(N'tranthib', 'tranthib@gmail.com', N'hashed_password_2', N'Trần Thị B', '0902345678', N'Nữ', '1998-05-20', 1),
(N'levanc', 'levanc@gmail.com', N'hashed_password_3', N'Lê Văn C', '0903456789', N'Nam', '2000-09-10', 1),
(N'phamthid', 'phamthid@gmail.com', N'hashed_password_4', N'Phạm Thị D', '0904567890', N'Nữ', '1992-12-25', 1),
(N'hoangvane', 'hoangvane@gmail.com', N'hashed_password_5', N'Hoàng Văn E', '0905678901', N'Nam', '1990-07-30', 1);
GO

-- 2. DATA ĐỊA CHỈ KHÁCH HÀNG
INSERT INTO DiaChiKhachHang (KhachHangId, TenNguoiNhan, SoDienThoaiNguoiNhan, TinhThanh, Phuong, DiaChiCuThe, LoaiDiaChi, MacDinh) VALUES
(1, N'Nguyễn Văn A', '0901234567', N'Hà Nội', N'Đống Đa', N'12 Chùa Bộc', 'NhaRieng', 1),
(2, N'Trần Thị B', '0902345678', N'Hồ Chí Minh', N'Quận 1', N'45 Lê Lợi', 'CongTy', 1),
(3, N'Lê Văn C', '0903456789', N'Đà Nẵng', N'Hải Châu', N'88 Bạch Đằng', 'NhaRieng', 1),
(4, N'Phạm Thị D', '0904567890', N'Cần Thơ', N'Ninh Kiều', N'102 đường 3/2', 'NhaRieng', 1),
(5, N'Hoàng Văn E', '0905678901', N'Hải Phòng', N'Lê Chân', N'15 Tô Hiệu', 'NhaRieng', 1);
GO

-- 3. DATA NHÀ CUNG CẤP (5 NCC)
INSERT INTO NhaCungCap (TenNhaCungCap, EmailLienHe, SoDienThoai, DiaChi, MaSoThue) VALUES
(N'Công Ty L''Oreal Việt Nam', 'contact@loreal.vn', '02839105678', N'Tòa nhà Vincom, Q1, TP.HCM', '0101234567'),
(N'Sao Thái Dương', 'lienhe@thaiduong.com.vn', '02436789012', N'Hoàng Mai, Hà Nội', '0102345678'),
(N'Unilever Việt Nam', 'support@unilever.vn', '02854123456', N'Quận 7, TP.HCM', '0103456789'),
(N'Mỹ Phẩm Tóc Cao Cấp Goldwell', 'info@goldwell.vn', '02438889999', N'Cầu Giấy, Hà Nội', '0104567890'),
(N'Organic Hair Care', 'sales@organic.vn', '0909999888', N'Thanh Xuân, Hà Nội', '0105678901');
GO

-- 4. DATA DANH MỤC CHA (4 Danh mục)
INSERT INTO DanhMuc (TenDanhMuc, Slug, MoTa) VALUES
(N'Chăm Sóc Tóc', 'cham-soc-toc', N'Các sản phẩm làm sạch và nuôi dưỡng tóc'),
(N'Tạo Kiểu Tóc', 'tao-kieu-toc', N'Sáp, Gôm, Gel vuốt tóc'),
(N'Hóa Chất Tóc', 'hoa-chat-toc', N'Thuốc nhuộm, uốn, duỗi'),
(N'Dụng Cụ Làm Tóc', 'dung-cu-lam-toc', N'Máy sấy, lược, kẹp tóc');
GO

-- 5. DATA DANH MỤC CON (5 Danh mục con)
INSERT INTO DanhMucCon (DanhMucId, TenDanhMucCon, Slug, MoTa) VALUES
(1, N'Dầu Gội', 'dau-goi', N'Làm sạch da đầu'),
(1, N'Dầu Xả & Kem Ủ', 'dau-xa-kem-u', N'Mềm mượt tóc'),
(1, N'Tinh Dầu & Serum', 'tinh-dau-serum', N'Phục hồi hư tổn'),
(3, N'Thuốc Nhuộm Tóc', 'thuoc-nhuom-toc', N'Thay đổi màu tóc'),
(2, N'Sáp Vuốt Tóc', 'sap-vuot-toc', N'Giữ nếp cho nam giới');
GO

-- 6. DATA SẢN PHẨM (10 Sản phẩm đa dạng)
-- Giả định ID DanhMucCon: 1=Dầu gội, 2=Xả/Ủ, 3=Serum, 4=Nhuộm, 5=Sáp
INSERT INTO SanPham (DanhMucConId, NhaCungCapId, TenSanPham, Slug, MaSku, GiaGoc, GiaBan, MoTaNgan, MoTaChiTiet, SoLuongTon, NoiBat) VALUES
-- Dầu gội
(1, 2, N'Dầu gội Dược liệu Thái Dương 7', 'dau-goi-thai-duong-7', 'DG-TD7-200', 120000, 105000, N'Sạch gàu, hết ngứa, nuôi dưỡng tóc.', N'Chiết xuất từ hương nhu, bồ kết...', 100, 1),
(1, 3, N'Dầu gội TRESemmé Keratin Smooth', 'tresemme-keratin', 'DG-TRE-450', 180000, 159000, N'Cho mái tóc vào nếp suôn mượt.', N'Công thức chứa Keratin và tinh dầu Argan...', 150, 1),
(1, 5, N'Dầu gội Bưởi Grapefruit Organic', 'dau-goi-buoi', 'DG-GRAPE', 250000, 199000, N'Kích thích mọc tóc, giảm gãy rụng.', N'Thành phần thiên nhiên 100%...', 80, 0),

-- Dầu xả / Kem ủ
(2, 3, N'Dầu xả Sunsilk Óng Mượt Rạng Ngời', 'dau-xa-sunsilk', 'DX-SUN-320', 85000, 79000, N'Hệ dưỡng chất Activ-Infusion.', N'Bổ sung Collagen và Protein...', 200, 0),
(2, 1, N'Kem ủ tóc L''Oreal Elseve Fall Resist', 'kem-u-loreal', 'KU-LOR-200', 160000, 135000, N'Ngăn gãy rụng chuyên sâu.', N'Tinh chất Arginine nuôi dưỡng tóc từ gốc...', 90, 1),

-- Thuốc nhuộm
(4, 4, N'Thuốc nhuộm Goldwell Topchic Nâu Hạt Dẻ', 'nhuom-goldwell-nau', 'TN-GOLD-5N', 300000, 280000, N'Màu nhuộm thời trang cao cấp.', N'Lên màu chuẩn, không hại tóc...', 50, 1),
(4, 1, N'Thuốc nhuộm L''Oreal Excellence Creme Đen', 'nhuom-loreal-den', 'TN-LOR-100', 190000, 165000, N'Phủ bạc 100%, bảo vệ tóc 3 tác động.', N'Dễ dàng sử dụng tại nhà...', 120, 0),
(4, 3, N'Bọt nhuộm tóc Blackpink Hello Bubble Hồng', 'nhuom-blackpink', 'TN-BP-ROSE', 220000, 189000, N'Màu Rose Gold thời thượng.', N'Dạng bọt tiện lợi, không chứa Amoniac...', 60, 1),

-- Dưỡng tóc
(3, 5, N'Tinh dầu dưỡng tóc Moroccanoil Treatment', 'moroccanoil', 'SR-MOR-25', 350000, 310000, N'Phục hồi tóc hư tổn hiệu quả.', N'Dầu Argan nguyên chất...', 40, 1),

-- Tạo kiểu
(5, 5, N'Sáp vuốt tóc Volcanic Clay', 'sap-volcanic', 'SAP-VOL', 280000, 240000, N'Giữ nếp cực tốt, độ phồng cao.', N'Thích hợp cho tóc dày, cứng...', 70, 0);
GO

-- 7. DATA HÌNH ẢNH SẢN PHẨM
INSERT INTO HinhAnhSanPham (SanPhamId, DuongDanAnh, LaAnhChinh) VALUES
(1, '/images/products/thai-duong-7-main.jpg', 1),
(1, '/images/products/thai-duong-7-back.jpg', 0),
(2, '/images/products/tresemme-main.jpg', 1),
(6, '/images/products/goldwell-5n.jpg', 1),
(9, '/images/products/moroccanoil.jpg', 1);
GO

-- 8. DATA THUỘC TÍNH SẢN PHẨM
INSERT INTO ThuocTinhSanPham (SanPhamId, TenThuocTinh, GiaTri, GiaCongThem) VALUES
(1, N'Dung tích', '200ml', 0),
(1, N'Dung tích', '500ml', 80000),
(6, N'Màu sắc', N'Nâu hạt dẻ', 0),
(6, N'Màu sắc', N'Nâu lạnh', 10000),
(9, N'Dung tích', '25ml', 0);
GO

-- 9. DATA MÃ GIẢM GIÁ
INSERT INTO MaGiamGia (MaCode, MoTa, LoaiGiamGia, GiaTriGiam, GiamToiDa, DonToiThieu, SoLuong, NgayBatDau, NgayKetThuc) VALUES
('HAIRNOVA10', N'Giảm 10% cho đơn đầu', 'PhanTram', 10, 50000, 100000, 1000, GETDATE(), DATEADD(day, 30, GETDATE())),
('FREESHIP', N'Miễn phí vận chuyển', 'CoDinh', 30000, 30000, 200000, 500, GETDATE(), DATEADD(day, 30, GETDATE())),
('SALE50K', N'Giảm 50k cho đơn 500k', 'CoDinh', 50000, 50000, 500000, 200, GETDATE(), DATEADD(day, 7, GETDATE())),
('BLACKFRIDAY', N'Siêu sale tháng 11', 'PhanTram', 20, 100000, 300000, 100, DATEADD(month, 1, GETDATE()), DATEADD(month, 2, GETDATE())),
('VIPMEMBER', N'Ưu đãi khách VIP', 'PhanTram', 15, 200000, 0, 50, GETDATE(), DATEADD(year, 1, GETDATE()));
GO

-- 10. DATA TRẠNG THÁI ĐƠN HÀNG
INSERT INTO TrangThaiDonHang (TenTrangThai) VALUES
(N'Chờ xác nhận'),
(N'Đang xử lý'),
(N'Đang giao hàng'),
(N'Đã giao hàng'),
(N'Đã hủy');
GO

-- 11. DATA ĐƠN HÀNG (3 Đơn)
INSERT INTO DonHang (MaDonHang, KhachHangId, TenNguoiNhan, SdtNguoiNhan, DiaChiGiaoHang, TongTienHang, PhiVanChuyen, TongThanhToan, TrangThaiDonHangId, DonViVanChuyen) VALUES
('DH0001', 1, N'Nguyễn Văn A', '0901234567', N'12 Chùa Bộc, Hà Nội', 210000, 30000, 240000, 1, 'GHN'),
('DH0002', 2, N'Trần Thị B', '0902345678', N'45 Lê Lợi, TP.HCM', 310000, 0, 310000, 3, 'ShopeeExpress'),
('DH0003', 1, N'Nguyễn Văn A', '0901234567', N'12 Chùa Bộc, Hà Nội', 500000, 30000, 530000, 4, 'ViettelPost');
GO

-- 12. DATA CHI TIẾT ĐƠN HÀNG
INSERT INTO ChiTietDonHang (DonHangId, SanPhamId, TenSanPham, SoLuong, DonGia, TongTien) VALUES
(1, 1, N'Dầu gội Dược liệu Thái Dương 7', 2, 105000, 210000), -- Mua 2 chai dầu gội
(2, 9, N'Tinh dầu dưỡng tóc Moroccanoil Treatment', 1, 310000, 310000), -- Mua 1 serum
(3, 6, N'Thuốc nhuộm Goldwell Topchic', 1, 280000, 280000),
(3, 8, N'Bọt nhuộm tóc Blackpink Hello Bubble', 1, 189000, 189000);
GO

-- 13. DATA THANH TOÁN
INSERT INTO ThanhToan (DonHangId, PhuongThuc, SoTien, MaGiaoDich, TrangThai, NgayThanhToan) VALUES
(1, N'COD', 240000, NULL, N'ChuaThanhToan', NULL),
(2, N'Momo', 310000, 'MOMO123456', N'DaThanhToan', GETDATE()),
(3, N'VNPAY', 530000, 'VNPAY98765', N'DaThanhToan', GETDATE());
GO

-- 14. DATA GIỎ HÀNG
INSERT INTO GioHang (KhachHangId) VALUES (3), (4), (5);
GO

-- 15. DATA CHI TIẾT GIỎ HÀNG
INSERT INTO ChiTietGioHang (GioHangId, SanPhamId, SoLuong) VALUES
(1, 2, 1), -- Giỏ hàng khách 3 có 1 dầu gội Tresemme
(1, 5, 1); -- Và 1 kem ủ L'Oreal
GO

-- 16. DATA ĐÁNH GIÁ
INSERT INTO DanhGia (KhachHangId, SanPhamId, DonHangId, SoSao, NoiDung, DaDuyet) VALUES
(1, 1, 1, 5, N'Dầu gội rất thơm, mượt tóc.', 1),
(2, 9, 2, 4, N'Sản phẩm tốt nhưng giá hơi cao.', 1),
(3, 6, NULL, 5, N'Màu lên chuẩn đẹp.', 1),
(1, 8, NULL, 3, N'Màu hơi tối so với ảnh.', 0),
(4, 5, NULL, 5, N'Ủ xong tóc mềm như bún.', 1);
GO

-- 17. DATA SẢN PHẨM YÊU THÍCH
INSERT INTO SanPhamYeuThich (KhachHangId, SanPhamId) VALUES
(1, 9), -- Khách 1 thích Moroccanoil
(2, 8), -- Khách 2 thích Blackpink dye
(3, 10),
(1, 2),
(5, 6);
GO

-- 18. DATA LIÊN HỆ TƯ VẤN
INSERT INTO LienHeTuVan (HoTen, SoDienThoai, Email, NoiDung) VALUES
(N'Phạm Văn X', '0912345678', 'x@email.com', N'Tôi bị rụng tóc nhiều thì dùng loại nào?'),
(N'Lê Thị Y', '0923456789', 'y@email.com', N'Tư vấn giúp tôi thuốc nhuộm phủ bạc.'),
(N'Trần Z', '0934567890', 'z@email.com', N'Shop có ship hỏa tốc không?'),
(N'Nguyễn K', '0945678901', 'k@email.com', N'Muốn làm đại lý phân phối.'),
(N'Võ H', '0956789012', 'h@email.com', N'Sản phẩm bị lỗi nắp chai.');
GO

-- 19. DATA DANH MỤC TIN TỨC
INSERT INTO DanhMucTinTuc (TenDanhMuc) VALUES
(N'Kiến thức chăm sóc tóc'),
(N'Xu hướng kiểu tóc'),
(N'Khuyến mãi & Sự kiện'),
(N'Review sản phẩm'),
(N'Tuyển dụng');
GO

-- 20. DATA TIN TỨC
INSERT INTO TinTuc (DanhMucTinTucId, TieuDe, Slug, TomTat, NoiDung, TacGiaId) VALUES
(1, N'5 Cách giảm rụng tóc tại nhà', 'cach-giam-rung-toc', N'Hướng dẫn chi tiết...', N'Nội dung bài viết về giảm rụng tóc...', 1),
(2, N'Màu tóc hot trend 2024', 'mau-toc-hot-2024', N'Top những màu nhuộm...', N'Nội dung về xu hướng màu tóc...', 1),
(3, N'Săn sale 12.12 cùng HairNova', 'san-sale-12-12', N'Giảm giá lên đến 50%...', N'Chi tiết chương trình khuyến mãi...', 1),
(1, N'Phân biệt dầu gội Organic và hóa chất', 'phan-biet-dau-goi', N'Cách nhận biết...', N'Nội dung chi tiết...', 1),
(2, N'Kiểu tóc Layer cho mặt tròn', 'layer-mat-tron', N'Tư vấn kiểu tóc...', N'Nội dung chi tiết...', 1);
GO

-- 21. DATA NHÂN VIÊN
INSERT INTO NhanVien (HoTen, NgaySinh, GioiTinh, Email, DiDong) VALUES
(N'Nguyễn Quản Trị', '1985-01-01', N'Nam', 'admin@hairnova.com', '0999888777'),
(N'Lê Nhân Viên', '1995-05-05', N'Nữ', 'staff@hairnova.com', '0999666555'),
(N'Trần Kho', '1990-03-03', N'Nam', 'kho@hairnova.com', '0999444333'),
(N'Phạm CSKH', '1998-08-08', N'Nữ', 'cskh@hairnova.com', '0999222111'),
(N'Võ Shipper', '1992-02-02', N'Nam', 'shipper@hairnova.com', '0999111000');
GO

-- 22. DATA TÀI KHOẢN QUẢN TRỊ
INSERT INTO TaiKhoanQuanTri (NhanVienId, TenDangNhap, MatKhau, LoaiQuyen, TrangThai) VALUES
(1, 'admin', 'admin_hash_pass', 1, 1), -- 1: Admin
(2, 'staff01', 'staff_hash_pass', 2, 1), -- 2: Nhân viên bán hàng
(3, 'kho01', 'kho_hash_pass', 3, 1), -- 3: Thủ kho
(4, 'cskh01', 'cskh_hash_pass', 2, 1),
(5, 'shipper01', 'ship_hash_pass', 4, 1); -- 4: Vận chuyển
GO

-- 1. Bảng KhachHang
GO
ALTER TABLE [dbo].[KhachHang] ADD CONSTRAINT [DF_KhachHang_TrangThai] DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[KhachHang] ADD CONSTRAINT [DF_KhachHang_NgayTao] DEFAULT (getdate()) FOR [NgayTao]
GO

-- 2. Bảng DiaChiKhachHang
ALTER TABLE [dbo].[DiaChiKhachHang] ADD CONSTRAINT [DF_DiaChi_MacDinh] DEFAULT ((0)) FOR [MacDinh]
GO

-- 3. Bảng DanhMuc & DanhMucCon
ALTER TABLE [dbo].[DanhMuc] ADD CONSTRAINT [DF_DanhMuc_HienThi] DEFAULT ((1)) FOR [HienThi]
GO
ALTER TABLE [dbo].[DanhMuc] ADD CONSTRAINT [DF_DanhMuc_NgayTao] DEFAULT (getdate()) FOR [NgayTao]
GO

-- 4. Bảng SanPham
ALTER TABLE [dbo].[SanPham] ADD CONSTRAINT [DF_SanPham_SoLuongTon] DEFAULT ((0)) FOR [SoLuongTon]
GO
ALTER TABLE [dbo].[SanPham] ADD CONSTRAINT [DF_SanPham_DaBan] DEFAULT ((0)) FOR [DaBan]
GO
ALTER TABLE [dbo].[SanPham] ADD CONSTRAINT [DF_SanPham_LuotXem] DEFAULT ((0)) FOR [LuotXem]
GO
ALTER TABLE [dbo].[SanPham] ADD CONSTRAINT [DF_SanPham_NoiBat] DEFAULT ((0)) FOR [NoiBat]
GO
ALTER TABLE [dbo].[SanPham] ADD CONSTRAINT [DF_SanPham_HienThi] DEFAULT ((1)) FOR [HienThi]
GO
ALTER TABLE [dbo].[SanPham] ADD CONSTRAINT [DF_SanPham_NgayTao] DEFAULT (getdate()) FOR [NgayTao]
GO

-- 5. Bảng HinhAnhSanPham
ALTER TABLE [dbo].[HinhAnhSanPham] ADD CONSTRAINT [DF_HinhAnh_LaAnhChinh] DEFAULT ((0)) FOR [LaAnhChinh]
GO
ALTER TABLE [dbo].[HinhAnhSanPham] ADD CONSTRAINT [DF_HinhAnh_ThuTu] DEFAULT ((0)) FOR [ThuTu]
GO

-- 6. Bảng MaGiamGia
ALTER TABLE [dbo].[MaGiamGia] ADD CONSTRAINT [DF_MaGiamGia_DaSuDung] DEFAULT ((0)) FOR [DaSuDung]
GO
ALTER TABLE [dbo].[MaGiamGia] ADD CONSTRAINT [DF_MaGiamGia_TrangThai] DEFAULT ((1)) FOR [TrangThai]
GO

-- 7. Bảng DonHang
ALTER TABLE [dbo].[DonHang] ADD CONSTRAINT [DF_DonHang_GiamGia] DEFAULT ((0)) FOR [GiamGia]
GO
ALTER TABLE [dbo].[DonHang] ADD CONSTRAINT [DF_DonHang_NgayTao] DEFAULT (getdate()) FOR [NgayTao]
GO

-- 8. Bảng ChiTietDonHang
ALTER TABLE [dbo].[ChiTietDonHang] ADD CONSTRAINT [DF_ChiTietDonHang_SoLuong] DEFAULT ((1)) FOR [SoLuong]
GO
ALTER TABLE [dbo].[ChiTietDonHang] ADD CONSTRAINT [DF_ChiTietDonHang_DonGia] DEFAULT ((0)) FOR [DonGia]
GO

-- 9. Bảng GioHang & ChiTietGioHang
ALTER TABLE [dbo].[GioHang] ADD CONSTRAINT [DF_GioHang_NgayTao] DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[ChiTietGioHang] ADD CONSTRAINT [DF_ChiTietGioHang_SoLuong] DEFAULT ((1)) FOR [SoLuong]
GO

-- 10. Bảng DanhGia
ALTER TABLE [dbo].[DanhGia] ADD CONSTRAINT [DF_DanhGia_DaDuyet] DEFAULT ((0)) FOR [DaDuyet]
GO
ALTER TABLE [dbo].[DanhGia] ADD CONSTRAINT [DF_DanhGia_NgayTao] DEFAULT (getdate()) FOR [NgayTao]
GO

-- 11. Bảng TaiKhoanQuanTri
ALTER TABLE [dbo].[TaiKhoanQuanTri] ADD CONSTRAINT [DF_Admin_TrangThai] DEFAULT ((1)) FOR [TrangThai]
GO

-- 1. DiaChi -> KhachHang
ALTER TABLE [dbo].[DiaChiKhachHang] WITH CHECK ADD CONSTRAINT [FK_DiaChi_KhachHang] FOREIGN KEY([KhachHangId])
REFERENCES [dbo].[KhachHang] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DiaChiKhachHang] CHECK CONSTRAINT [FK_DiaChi_KhachHang]
GO

-- 2. DanhMucCon -> DanhMuc
ALTER TABLE [dbo].[DanhMucCon] WITH CHECK ADD CONSTRAINT [FK_DanhMucCon_DanhMuc] FOREIGN KEY([DanhMucId])
REFERENCES [dbo].[DanhMuc] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DanhMucCon] CHECK CONSTRAINT [FK_DanhMucCon_DanhMuc]
GO

-- 3. SanPham -> DanhMucCon & NhaCungCap
ALTER TABLE [dbo].[SanPham] WITH CHECK ADD CONSTRAINT [FK_SanPham_DanhMucCon] FOREIGN KEY([DanhMucConId])
REFERENCES [dbo].[DanhMucCon] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[SanPham] CHECK CONSTRAINT [FK_SanPham_DanhMucCon]
GO

ALTER TABLE [dbo].[SanPham] WITH CHECK ADD CONSTRAINT [FK_SanPham_NhaCungCap] FOREIGN KEY([NhaCungCapId])
REFERENCES [dbo].[NhaCungCap] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[SanPham] CHECK CONSTRAINT [FK_SanPham_NhaCungCap]
GO

-- 4. HinhAnhSanPham -> SanPham
ALTER TABLE [dbo].[HinhAnhSanPham] WITH CHECK ADD CONSTRAINT [FK_HinhAnh_SanPham] FOREIGN KEY([SanPhamId])
REFERENCES [dbo].[SanPham] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HinhAnhSanPham] CHECK CONSTRAINT [FK_HinhAnh_SanPham]
GO

-- 5. DonHang -> KhachHang, MaGiamGia, TrangThai
ALTER TABLE [dbo].[DonHang] WITH CHECK ADD CONSTRAINT [FK_DonHang_KhachHang] FOREIGN KEY([KhachHangId])
REFERENCES [dbo].[KhachHang] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_KhachHang]
GO

ALTER TABLE [dbo].[DonHang] WITH CHECK ADD CONSTRAINT [FK_DonHang_MaGiamGia] FOREIGN KEY([MaGiamGiaId])
REFERENCES [dbo].[MaGiamGia] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_MaGiamGia]
GO

ALTER TABLE [dbo].[DonHang] WITH CHECK ADD CONSTRAINT [FK_DonHang_TrangThai] FOREIGN KEY([TrangThaiDonHangId])
REFERENCES [dbo].[TrangThaiDonHang] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_TrangThai]
GO

-- 6. ChiTietDonHang -> DonHang & SanPham
ALTER TABLE [dbo].[ChiTietDonHang] WITH CHECK ADD CONSTRAINT [FK_ChiTietDonHang_DonHang] FOREIGN KEY([DonHangId])
REFERENCES [dbo].[DonHang] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [FK_ChiTietDonHang_DonHang]
GO

ALTER TABLE [dbo].[ChiTietDonHang] WITH CHECK ADD CONSTRAINT [FK_ChiTietDonHang_SanPham] FOREIGN KEY([SanPhamId])
REFERENCES [dbo].[SanPham] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [FK_ChiTietDonHang_SanPham]
GO

-- 7. ThanhToan -> DonHang
ALTER TABLE [dbo].[ThanhToan] WITH CHECK ADD CONSTRAINT [FK_ThanhToan_DonHang] FOREIGN KEY([DonHangId])
REFERENCES [dbo].[DonHang] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThanhToan] CHECK CONSTRAINT [FK_ThanhToan_DonHang]
GO

-- 8. ChiTietGioHang -> GioHang & SanPham
ALTER TABLE [dbo].[ChiTietGioHang] WITH CHECK ADD CONSTRAINT [FK_ChiTietGioHang_GioHang] FOREIGN KEY([GioHangId])
REFERENCES [dbo].[GioHang] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietGioHang] CHECK CONSTRAINT [FK_ChiTietGioHang_GioHang]
GO

ALTER TABLE [dbo].[ChiTietGioHang] WITH CHECK ADD CONSTRAINT [FK_ChiTietGioHang_SanPham] FOREIGN KEY([SanPhamId])
REFERENCES [dbo].[SanPham] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietGioHang] CHECK CONSTRAINT [FK_ChiTietGioHang_SanPham]
GO

-- 9. DanhGia -> KhachHang & SanPham
ALTER TABLE [dbo].[DanhGia] WITH CHECK ADD CONSTRAINT [FK_DanhGia_KhachHang] FOREIGN KEY([KhachHangId])
REFERENCES [dbo].[KhachHang] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[DanhGia] CHECK CONSTRAINT [FK_DanhGia_KhachHang]
GO

ALTER TABLE [dbo].[DanhGia] WITH CHECK ADD CONSTRAINT [FK_DanhGia_SanPham] FOREIGN KEY([SanPhamId])
REFERENCES [dbo].[SanPham] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[DanhGia] CHECK CONSTRAINT [FK_DanhGia_SanPham]
GO

-- 10. TaiKhoanQuanTri -> NhanVien
ALTER TABLE [dbo].[TaiKhoanQuanTri] WITH CHECK ADD CONSTRAINT [FK_Admin_NhanVien] FOREIGN KEY([NhanVienId])
REFERENCES [dbo].[NhanVien] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TaiKhoanQuanTri] CHECK CONSTRAINT [FK_Admin_NhanVien]
GO

-- ThuocTinhSanPham -> SanPham
ALTER TABLE [dbo].[ThuocTinhSanPham] WITH CHECK ADD CONSTRAINT [FK_ThuocTinhSanPham_SanPham] 
FOREIGN KEY([SanPhamId])
REFERENCES [dbo].[SanPham] ([Id])
GO
ALTER TABLE [dbo].[ThuocTinhSanPham] CHECK CONSTRAINT [FK_ThuocTinhSanPham_SanPham]
GO

-- YeuThich -> KhachHang
ALTER TABLE [dbo].[SanPhamYeuThich] WITH CHECK ADD CONSTRAINT [FK_SanPhamYeuThich_KhachHang] FOREIGN KEY([KhachHangId])
REFERENCES [dbo].[KhachHang] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SanPhamYeuThich] CHECK CONSTRAINT [FK_SanPhamYeuThich_KhachHang]
GO

-- YeuThich -> SanPham
ALTER TABLE [dbo].[SanPhamYeuThich] WITH CHECK ADD CONSTRAINT [FK_SanPhamYeuThich_SanPham] FOREIGN KEY([SanPhamId])
REFERENCES [dbo].[SanPham] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SanPhamYeuThich] CHECK CONSTRAINT [FK_SanPhamYeuThich_SanPham]
GO

