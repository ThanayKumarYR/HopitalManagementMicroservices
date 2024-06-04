create database Microservices_HospitalManagement_adminDB;

--drop database Microservices_HospitalManagement_adminDB;

use Microservices_HospitalManagement_adminDB;
go

--DEPARTMENT
CREATE PROCEDURE spCreateDepartment
    @DeptName NVARCHAR(100)
AS
BEGIN
    -- Check if the Department table exists, and create it if it does not
    IF NOT EXISTS (
        SELECT * FROM INFORMATION_SCHEMA.TABLES
        WHERE TABLE_NAME = 'Department'
    )
    BEGIN
        CREATE TABLE Department (
            DeptId INT PRIMARY KEY IDENTITY(1,1),
            DeptName NVARCHAR(100) NOT NULL
        );
    END;

    -- Insert the new department
    INSERT INTO Department (DeptName)
    VALUES (@DeptName);
    SELECT SCOPE_IDENTITY();
END;
GO

GO

CREATE PROCEDURE spGetDepartmentById
    @DeptId INT
AS
BEGIN
    SELECT * FROM Department
    WHERE DeptId = @DeptId;
END;
GO

CREATE PROCEDURE spUpdateDepartment
    @DeptId INT,
    @DeptName NVARCHAR(100)
AS
BEGIN
    UPDATE Department
    SET DeptName = @DeptName
    WHERE DeptId = @DeptId;
END;
GO

CREATE PROCEDURE spDeleteDepartment
    @DeptId INT
AS
BEGIN
    DELETE FROM Department
    WHERE DeptId = @DeptId;
END;
GO

CREATE PROCEDURE spGetDepartmentByName
    @DeptName NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Department
    WHERE DeptName = @DeptName;
END;
GO

--DOCTOR

CREATE PROCEDURE spCreateDoctor
    @DoctorId INT,
    @DeptId INT,
    @DoctorName NVARCHAR(100),
    @DoctorAge INT,
    @DoctorAvailable BIT,
    @Specialization NVARCHAR(100),
    @Qualifications NVARCHAR(255)
AS
BEGIN
    -- Check if the Doctor table exists, and create it if it does not
    IF NOT EXISTS (
        SELECT * FROM INFORMATION_SCHEMA.TABLES
        WHERE TABLE_NAME = 'Doctor'
    )
    BEGIN
        CREATE TABLE Doctor (
            DoctorId INT PRIMARY KEY,
            DeptId INT,
            DoctorName NVARCHAR(100) NOT NULL,
            DoctorAge INT,
            DoctorAvailable BIT,
            Specialization NVARCHAR(100),
            Qualifications NVARCHAR(255)
        );
    END;

    -- Insert the new doctor
    INSERT INTO Doctor (DoctorId, DeptId, DoctorName, DoctorAge, DoctorAvailable, Specialization, Qualifications)
    VALUES (@DoctorId, @DeptId, @DoctorName, @DoctorAge, @DoctorAvailable, @Specialization, @Qualifications);
END;
GO

CREATE PROCEDURE spGetDoctorById
    @DoctorId INT
AS
BEGIN
    SELECT * FROM Doctor
    WHERE DoctorId = @DoctorId;
END;
GO

CREATE PROCEDURE spGetAllDoctors
AS
BEGIN
    SELECT * FROM Doctor;
END;
GO

CREATE PROCEDURE spUpdateDoctor
    @DoctorId INT,
    @DoctorName NVARCHAR(100),
    @DoctorAge INT,
    @DoctorAvailable BIT,
    @Specialization NVARCHAR(100),
    @Qualifications NVARCHAR(255)
AS
BEGIN
    UPDATE Doctor 
    SET DoctorName = @DoctorName,
        DoctorAge = @DoctorAge,
        DoctorAvailable = @DoctorAvailable,
        Specialization = @Specialization,
        Qualifications = @Qualifications
    WHERE DoctorId = @DoctorId;
END;
GO

CREATE PROCEDURE spDeleteDoctor
    @DoctorId INT
AS
BEGIN
    DELETE FROM Doctor
    WHERE DoctorId = @DoctorId;
END;
GO

CREATE PROCEDURE spGetDoctorsBySpecialization
    @Specialization NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Doctor
    WHERE Specialization = @Specialization;
END;
GO


create database Microservices_HospitalManagement_patienthistorydb;

use Microservices_HospitalManagement_patienthistorydb;
go
CREATE PROCEDURE spCreateHistory
    @DoctorId INT,
    @PatientId INT,
    @Issue NVARCHAR(255),
    @VisitsToDoctor INT
AS
BEGIN
    IF NOT EXISTS (
        SELECT * FROM INFORMATION_SCHEMA.TABLES
        WHERE TABLE_NAME = 'History'
    )
    BEGIN
        CREATE TABLE History (
            HistoryId INT IDENTITY(1,1) PRIMARY KEY,
            DoctorId INT,
            PatientId INT,
            Issue NVARCHAR(255),
            VisitsToDoctor INT
        );
    END;

    INSERT INTO History (DoctorId, PatientId, Issue, VisitsToDoctor)
    VALUES (@DoctorId, @PatientId, @Issue, @VisitsToDoctor);
END;
GO

CREATE PROCEDURE spCreatePatientHistory
    @PatientId INT,
    @PatientName NVARCHAR(100),
    @Email NVARCHAR(100)
AS
BEGIN
    IF NOT EXISTS (
        SELECT * FROM INFORMATION_SCHEMA.TABLES
        WHERE TABLE_NAME = 'PatientHistory'
    )
    BEGIN
        CREATE TABLE PatientHistory (
            PatientId INT PRIMARY KEY,
            PatientName NVARCHAR(100),
            Email NVARCHAR(100)
        );
    END;

    INSERT INTO PatientHistory (PatientId, PatientName, Email)
    VALUES (@PatientId, @PatientName, @Email);
END;
GO

CREATE PROCEDURE spGetPatientDetails
    @PatientId INT
AS
BEGIN
    SELECT PatientId, PatientName, Email FROM PatientHistory
    WHERE PatientId = @PatientId;
END;
GO

CREATE PROCEDURE spGetPatientHistory
    @PatientId INT,
    @DoctorId INT
AS
BEGIN
    SELECT Issue, VisitsToDoctor FROM History
    WHERE PatientId = @PatientId AND DoctorId = @DoctorId;
END;
GO

create database Microservices_HospitalManagement_appointementdb;

use Microservices_HospitalManagement_appointementdb;
		
go
CREATE PROCEDURE spCreateAppointment
    @PatientName NVARCHAR(100),
    @PatientAge INT,
    @Issue NVARCHAR(255),
    @DoctorName NVARCHAR(100),
    @Specialization NVARCHAR(100),
    @AppointmentDate DATETIME,
    @Status BIT,
    @BookedWith INT,
    @BookedBy INT
AS
BEGIN
    INSERT INTO APPOINTMENT (PatientName, PatientAge, Issue, DoctorName, Specialization, AppointmentDate, Status, BookedWith, BookedBy)
    VALUES (@PatientName, @PatientAge, @Issue, @DoctorName, @Specialization, @AppointmentDate, @Status, @BookedWith, @BookedBy);
END;
GO

CREATE PROCEDURE spGetAllAppointmentsByPatient
    @PatientId INT
AS
BEGIN
    SELECT * FROM APPOINTMENT WHERE BookedBy = @PatientId;
END;
GO

CREATE PROCEDURE spGetAllAppointmentsByDoctor
    @DoctorId INT
AS
BEGIN
    SELECT * FROM APPOINTMENT WHERE BookedWith = @DoctorId;
END;
GO

CREATE PROCEDURE spGetAppointmentById
    @AppointmentId INT
AS
BEGIN
    SELECT * FROM APPOINTMENT WHERE AppointmentId = @AppointmentId;
END;
GO

CREATE PROCEDURE spUpdateAppointment
    @AppointmentId INT,
    @PatientName NVARCHAR(100),
    @PatientAge INT,
    @Issue NVARCHAR(255),
    @AppointmentDate DATETIME,
    @BookedBy INT
AS
BEGIN
    UPDATE APPOINTMENT 
    SET PatientName = @PatientName, 
        PatientAge = @PatientAge, 
        Issue = @Issue, 
        AppointmentDate = @AppointmentDate, 
        BookedBy = @BookedBy 
    WHERE AppointmentId = @AppointmentId;
END;
GO

CREATE PROCEDURE spUpdateStatus
    @AppointmentId INT,
    @Status BIT
AS
BEGIN
    UPDATE APPOINTMENT 
    SET Status = @Status 
    WHERE AppointmentId = @AppointmentId;
END;
GO

CREATE PROCEDURE spDeleteAppointment
    @AppointmentId INT,
    @BookedBy INT
AS
BEGIN
    DELETE FROM APPOINTMENT WHERE AppointmentId = @AppointmentId AND BookedBy = @BookedBy;
END;
GO

create database Microservices_HospitalManagement_userDB;
use Microservices_HospitalManagement_userDB;

go
CREATE PROCEDURE CreateUsersTableIfNotExists
AS
BEGIN
    IF NOT EXISTS (
        SELECT * FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_NAME = 'Users'
    )
    BEGIN
        CREATE TABLE Users (
            UserID INT IDENTITY(1,1) PRIMARY KEY,
            FirstName NVARCHAR(50) NOT NULL,
            LastName NVARCHAR(50) NOT NULL,
            Email NVARCHAR(100) UNIQUE NOT NULL,
            Password NVARCHAR(100) NOT NULL,
            Role NVARCHAR(50) CHECK (Role IN ('Admin', 'Doctor', 'Patient')) NOT NULL,
            IsApproved BIT DEFAULT 0 NOT NULL
        );
    END
END
go
CREATE PROCEDURE CheckEmailDuplication
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT COUNT(*)
    FROM Users
    WHERE Email = @Email;
END
go
CREATE PROCEDURE RegisterUser
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Password NVARCHAR(100),
    @Role NVARCHAR(50)
AS
BEGIN
    INSERT INTO Users (FirstName, LastName, Email, Password, Role)
    VALUES (@FirstName, @LastName, @Email, @Password, @Role);
END
go
CREATE PROCEDURE UserLogin
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT * 
    FROM Users 
    WHERE Email = @Email;
END





