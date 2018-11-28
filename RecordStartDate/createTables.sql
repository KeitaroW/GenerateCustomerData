if not exists
(
	select TABLE_NAME
	FROM sales.INFORMATION_SCHEMA.TABLES
	WHERE TABLE_NAME = 'CustomerHistory'
)


CREATE TABLE CustomerHistory(
	CustomerID INTEGER,
	FirstName VARCHAR(50),
	Gender VARCHAR(20),
	Family VARCHAR(50),
	FederalState VARCHAR(50),
	City VARCHAR(50),
	RecordStartDate Date,
	RecordEndDate Date DEFAULT '9999-12-31',
	CONSTRAINT PK_CUSTOMER_HISTORY PRIMARY KEY (CustomerID, RecordStartDate)
)

if not exists
(
	select TABLE_NAME
	FROM sales.INFORMATION_SCHEMA.TABLES
	WHERE TABLE_NAME = 'CustomerTmp'
)

CREATE TABLE CustomerTmp(
	CustomerID INTEGER,
	FirstName VARCHAR(50),
	Gender VARCHAR(20),
	Family VARCHAR(50),
	FederalState VARCHAR(50),
	City VARCHAR(50),
	FileDate Date,
	CONSTRAINT PK_CUSTOMER_TMP PRIMARY KEY (CustomerID)
)