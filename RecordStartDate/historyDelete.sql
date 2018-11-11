DELETE FROM sales..CustomerHistory
WHERE RecordStartDate >= 
	(
		SELECT max(FileDate)
		FROM sales..CustomerTmp
	);

UPDATE sales..CustomerHistory
SET RecordEndDate = '9999-12-31'
WHERE RecordEndDate = (
						SELECT dateadd(day,-1, max(FileDate))
						FROM sales..CustomerTmp
					);


