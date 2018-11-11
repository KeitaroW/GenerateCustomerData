SELECT * 
INTO #UPDATE_DATA
FROM(
		SELECT CustomerID,
			FirstName,
			Family,
			Gender,
			FederalState,
			City
		FROM CustomerTmp
		Except
		SELECT CustomerID,
			FirstName,
			Family,
			Gender,
			FederalState,
			City
		FROM CustomerHistory
		WHERE RecordEndDate = '9999-12-31'
) upd;


--Kunden, bei denen sich Werte geändert haben
UPDATE CustomerHistory
SET RecordEndDate = (
						SELECT dateadd(day,-1, max(FileDate))
						FROM sales..CustomerTmp
					)
WHERE RecordEndDate = '9999-12-31'
AND
CustomerID in
	(
		SELECT CustomerID
		FROM #UPDATE_DATA
	)
;

--Kunden, die gekündigt haben
UPDATE CustomerHistory
SET RecordEndDate = (
						SELECT dateadd(day,-1, max(FileDate))
						FROM sales..CustomerTmp
					)
WHERE RecordEndDate = '9999-12-31'
AND
CustomerID in
	(
		SELECT CustomerID
		FROM CustomerHistory
		WHERE RecordEndDate = '9999-12-31'
		EXCEPT
		SELECT CustomerID
		FROM CustomerTmp
	)
;


INSERT INTO CustomerHistory (CustomerID, RecordStartDate, RecordEndDate, FirstName, Family, Gender, FederalState, City)
SELECT  udt.CustomerID,
		dat.RSD,
		'9999-12-31',
		udt.FirstName,
		udt.Family,
		udt.Gender,
		udt.FederalState,
		udt.City
FROM #UPDATE_DATA udt,
		(
			SELECT max(FileDate) as RSD
			FROM CustomerTmp
		)dat;


--Test
SELECT *
FROM CustomerHistory
ORDER BY CustomerID, RecordStartDate;



