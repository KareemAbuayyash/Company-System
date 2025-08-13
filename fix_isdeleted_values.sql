-- Fix IsDeleted values in the database
-- This script ensures all active records have IsDeleted = 0 (false)

-- Update Users table
UPDATE Users SET IsDeleted = 0 WHERE IsDeleted IS NULL OR IsDeleted != 0;

-- Update Roles table  
UPDATE Roles SET IsDeleted = 0 WHERE IsDeleted IS NULL OR IsDeleted != 0;

-- Update Departments table
UPDATE Departments SET IsDeleted = 0 WHERE IsDeleted IS NULL OR IsDeleted != 0;

-- Update MainPageContents table
UPDATE MainPageContents SET IsDeleted = 0 WHERE IsDeleted IS NULL OR IsDeleted != 0;

-- Update Notes table
UPDATE Notes SET IsDeleted = 0 WHERE IsDeleted IS NULL OR IsDeleted != 0;

-- Verify the changes
SELECT 'Users' as TableName, COUNT(*) as TotalRecords, SUM(CASE WHEN IsDeleted = 0 THEN 1 ELSE 0 END) as ActiveRecords, SUM(CASE WHEN IsDeleted = 1 THEN 1 ELSE 0 END) as DeletedRecords FROM Users
UNION ALL
SELECT 'Roles' as TableName, COUNT(*) as TotalRecords, SUM(CASE WHEN IsDeleted = 0 THEN 1 ELSE 0 END) as ActiveRecords, SUM(CASE WHEN IsDeleted = 1 THEN 1 ELSE 0 END) as DeletedRecords FROM Roles
UNION ALL
SELECT 'Departments' as TableName, COUNT(*) as TotalRecords, SUM(CASE WHEN IsDeleted = 0 THEN 1 ELSE 0 END) as ActiveRecords, SUM(CASE WHEN IsDeleted = 1 THEN 1 ELSE 0 END) as DeletedRecords FROM Departments
UNION ALL
SELECT 'MainPageContents' as TableName, COUNT(*) as TotalRecords, SUM(CASE WHEN IsDeleted = 0 THEN 1 ELSE 0 END) as ActiveRecords, SUM(CASE WHEN IsDeleted = 1 THEN 1 ELSE 0 END) as DeletedRecords FROM MainPageContents
UNION ALL
SELECT 'Notes' as TableName, COUNT(*) as TotalRecords, SUM(CASE WHEN IsDeleted = 0 THEN 1 ELSE 0 END) as ActiveRecords, SUM(CASE WHEN IsDeleted = 1 THEN 1 ELSE 0 END) as DeletedRecords FROM Notes;
