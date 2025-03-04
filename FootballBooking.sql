-- Tạo database
CREATE DATABASE FootballBooking;
USE FootballBooking;

-- Tạo bảng Accounts
CREATE TABLE Accounts (
    account_id INT IDENTITY(1,1) PRIMARY KEY, -- Sử dụng IDENTITY thay cho AUTO_INCREMENT
    username VARCHAR(50) NOT NULL,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(10) CHECK (role IN ('admin', 'user')) NOT NULL, -- Dùng VARCHAR và CHECK để giới hạn giá trị
    email VARCHAR(100),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tạo bảng Fields
CREATE TABLE Fields (
    field_id INT IDENTITY(1,1) PRIMARY KEY, -- Sử dụng IDENTITY thay cho AUTO_INCREMENT
    field_name VARCHAR(50) NOT NULL,
);

-- Tạo bảng Prices
CREATE TABLE Prices (
    price_id INT IDENTITY(1,1) PRIMARY KEY, -- Sử dụng IDENTITY thay cho AUTO_INCREMENT
    field_id INT,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    price DECIMAL(10, 2) NOT NULL, -- Giá sân theo giờ
    FOREIGN KEY (field_id) REFERENCES Fields(field_id)
);

-- Tạo bảng Bookings
CREATE TABLE Bookings (
    booking_id INT IDENTITY(1,1) PRIMARY KEY, -- Sử dụng IDENTITY thay cho AUTO_INCREMENT
    account_id INT,
    field_id INT,
    booking_date DATE NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    total_price DECIMAL(10, 2) NOT NULL, -- Tổng giá trị của booking
    status VARCHAR(10) CHECK (status IN ('pending', 'confirmed', 'cancelled')) NOT NULL, -- Trạng thái booking
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id),
    FOREIGN KEY (field_id) REFERENCES Fields(field_id)
);

-- Tạo bảng Payments
CREATE TABLE Payments (
    payment_id INT IDENTITY(1,1) PRIMARY KEY, -- Sử dụng IDENTITY thay cho AUTO_INCREMENT
    booking_id INT,
    payment_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    amount DECIMAL(10, 2) NOT NULL, -- Số tiền thanh toán
    method VARCHAR(20) CHECK (method IN ('credit_card', 'cash', 'paypal')) NOT NULL, -- Phương thức thanh toán
    status VARCHAR(10) CHECK (status IN ('pending', 'completed', 'failed')) NOT NULL, -- Trạng thái thanh toán
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);

-- Tạo bảng FieldSchedule
CREATE TABLE FieldSchedule (
    schedule_id INT IDENTITY(1,1) PRIMARY KEY, -- Sử dụng IDENTITY thay cho AUTO_INCREMENT
    field_id INT,
    booking_date DATE NOT NULL, -- Ngày của lịch
    start_time TIME NOT NULL, -- Thời gian bắt đầu khung giờ
    end_time TIME NOT NULL, -- Thời gian kết thúc khung giờ
    status VARCHAR(10) CHECK (status IN ('available', 'booked')) NOT NULL, -- Trạng thái: 'available' (còn trống) hoặc 'booked' (đã đặt)
    booking_id INT DEFAULT NULL, -- ID booking nếu đã đặt
    FOREIGN KEY (field_id) REFERENCES Fields(field_id),
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);
