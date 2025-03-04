CREATE DATABASE FootballBooking;
USE FootballBooking;

CREATE TABLE Accounts (
    account_id INT IDENTITY(1,1) PRIMARY KEY, 
    username VARCHAR(50) NOT NULL,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(10) CHECK (role IN ('admin', 'user')) NOT NULL, 
    email VARCHAR(100),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);
INSERT INTO Accounts (username, password, role, email)
VALUES ('admin', '123456', 'admin', 'admin@gmail.com');
INSERT INTO Accounts (username, password, role, email)
VALUES ('kien', '123456', 'user', 'kien@gmail.com');

CREATE TABLE Fields (
    field_id INT IDENTITY(1,1) PRIMARY KEY, 
    field_name VARCHAR(50) NOT NULL,
);

CREATE TABLE Prices (
    price_id INT IDENTITY(1,1) PRIMARY KEY, 
    field_id INT,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    price DECIMAL(10, 2) NOT NULL, -- Giá sân theo giờ
    FOREIGN KEY (field_id) REFERENCES Fields(field_id)
);

CREATE TABLE Bookings (
    booking_id INT IDENTITY(1,1) PRIMARY KEY, 
    account_id INT,
    field_id INT,
    booking_date DATE NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    total_price DECIMAL(10, 2) NOT NULL,
    status VARCHAR(10) CHECK (status IN ('pending', 'confirmed', 'cancelled')) NOT NULL,
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id),
    FOREIGN KEY (field_id) REFERENCES Fields(field_id)
);

CREATE TABLE Payments (
    payment_id INT IDENTITY(1,1) PRIMARY KEY,
    booking_id INT,
    payment_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    amount DECIMAL(10, 2) NOT NULL, -- Số tiền thanh toán
    method VARCHAR(20) CHECK (method IN ('credit_card', 'cash', 'paypal')) NOT NULL, 
    status VARCHAR(10) CHECK (status IN ('pending', 'completed', 'failed')) NOT NULL, 
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);

CREATE TABLE FieldSchedule (
    schedule_id INT IDENTITY(1,1) PRIMARY KEY,
    field_id INT,
    booking_date DATE NOT NULL, 
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    status VARCHAR(10) CHECK (status IN ('available', 'booked')) NOT NULL,
    booking_id INT DEFAULT NULL, 
    FOREIGN KEY (field_id) REFERENCES Fields(field_id),
    FOREIGN KEY (booking_id) REFERENCES Bookings(booking_id)
);
INSERT INTO Fields (field_name)
VALUES 
('Sân 1'), 
('Sân 2'), 
('Sân 3'), 
('Sân 4');

INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '06:00', '07:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '07:30', '09:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '09:00', '10:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '10:30', '12:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '13:00', '14:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '14:30', '16:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '16:00', '17:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '17:30', '19:00', 800000); 
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '19:00', '20:30', 700000); 
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '20:30', '22:00', 600000); 
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (1, '22:00', '23:30', 500000);

-- Sân 2
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '06:00', '07:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '07:30', '09:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '09:00', '10:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '10:30', '12:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '13:00', '14:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '14:30', '16:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '16:00', '17:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '17:30', '19:00', 800000); 
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '19:00', '20:30', 700000); 
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '20:30', '22:00', 600000); 
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (2, '22:00', '23:30', 500000);

-- Sân 3
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '06:00', '07:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '07:30', '09:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '09:00', '10:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '10:30', '12:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '13:00', '14:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '14:30', '16:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '16:00', '17:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '17:30', '19:00', 800000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '19:00', '20:30', 700000); 
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '20:30', '22:00', 600000); 
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (3, '22:00', '23:30', 500000);

-- Sân 4
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '06:00', '07:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '07:30', '09:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '09:00', '10:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '10:30', '12:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '13:00', '14:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '14:30', '16:00', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '16:00', '17:30', 500000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '17:30', '19:00', 800000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '19:00', '20:30', 700000); 
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '20:30', '22:00', 600000);
INSERT INTO Prices (field_id, start_time, end_time, price) VALUES (4, '22:00', '23:30', 500000);

TRUNCATE TABLE Fields;
DELETE FROM Prices;
SELECT * FROM Fields;
DBCC CHECKIDENT ('Prices', RESEED, 0);

SELECT * FROM Prices

EXEC msdb.dbo.sp_help_jobhistory  
    @job_name = 'Generate Daily Field Schedule'

USE FootballBooking; 
GO

DECLARE @FieldID INT;
DECLARE @StartTime TIME;
DECLARE @EndTime TIME;
DECLARE @Date DATE;
DECLARE @DaysAhead INT = 7; 


DECLARE FieldCursor CURSOR FOR 
SELECT field_id FROM Fields;

OPEN FieldCursor;
FETCH NEXT FROM FieldCursor INTO @FieldID;

WHILE @@FETCH_STATUS = 0
BEGIN
    
    DECLARE @i INT = 0;
    WHILE @i < @DaysAhead
    BEGIN
        SET @Date = DATEADD(DAY, @i, CAST(GETDATE() AS DATE));

        -- Kiểm tra xem ngày này đã có trong FieldSchedule chưa
        IF NOT EXISTS (SELECT 1 FROM FieldSchedule WHERE booking_date = @Date AND field_id = @FieldID)
        BEGIN
            -- Tạo khung giờ buổi sáng (06:00 - 12:00)
            SET @StartTime = '06:00';
            WHILE @StartTime < '12:00'
            BEGIN
                SET @EndTime = DATEADD(MINUTE, 90, @StartTime);

                INSERT INTO FieldSchedule (field_id, booking_date, start_time, end_time, status)
                VALUES (@FieldID, @Date, @StartTime, @EndTime, 'available');

                SET @StartTime = @EndTime;
            END

            -- Tạo khung giờ buổi chiều & tối (13:00 - 23:30)
            SET @StartTime = '13:00';
            WHILE @StartTime < '23:30'
            BEGIN
                SET @EndTime = DATEADD(MINUTE, 90, @StartTime);

                INSERT INTO FieldSchedule (field_id, booking_date, start_time, end_time, status)
                VALUES (@FieldID, @Date, @StartTime, @EndTime, 'available');

                SET @StartTime = @EndTime;
            END
        END

        SET @i = @i + 1;
    END

    FETCH NEXT FROM FieldCursor INTO @FieldID;
END

CLOSE FieldCursor;
DEALLOCATE FieldCursor;

PRINT '✅ Lịch sân bóng đã được tạo cho 7 ngày tiếp theo.';

SELECT name 
FROM sys.check_constraints 
WHERE parent_object_id = OBJECT_ID('Bookings');

