-- Initialize database with required tables and indexes
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ClaudeSearchDb')
BEGIN
    CREATE DATABASE ClaudeSearchDb;
END
GO

USE ClaudeSearchDb;
GO

-- Enable full-text search if not already enabled
IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = 'SearchCatalog')
BEGIN
    CREATE FULLTEXT CATALOG SearchCatalog AS DEFAULT;
END
