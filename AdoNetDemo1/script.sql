CREATE DATABASE [AdoNetDemo]
USE [AdoNetDemo]
CREATE TABLE [Demo] (
                        [id] INT IDENTITY PRIMARY KEY ,
                        [name] NVARCHAR(25) NOT NULL
);
INSERT INTO [Demo] ([name]) VALUES ('Noname'), ('Bill'), ('John');
SELECT 2 * 2;
ALTER DATABASE [AdoNetDemo] SET ENABLE_BROKER;
CREATE QUEUE AklContentToteChangeMessages;
CREATE SERVICE AklContentToteChangeNotifications
    ON QUEUE AklContentToteChangeMessages
    ([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]);
INSERT INTO [Demo] ([name]) VALUES ('Testname4');