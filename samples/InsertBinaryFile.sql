INSERT INTO apotheca.Documents (Name, Extension, Description, Document, CreatedOn, CreatedByUserId)
SELECT
	'Sample.doc' as Name
	, 'doc' AS Extension
	, null AS Description
	, * FROM OPENROWSET(BULK 'C:\Temp\Sample.doc', SINGLE_BLOB) AS Document
	, (select getutcdate() AS CreatedOn) as CreatedOn
	, (SELECT newid() as CreatedByUserId) as CreatedByUserId
GO

--select getutcdate()

