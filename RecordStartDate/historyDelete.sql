DELETE FROM catering..CustomerHistory
WHERE RecordStartDate >= 
	(
		SELECT max(FileDate)
		FROM catering..CustomerTmp
	);

UPDATE catering..CustomerHistory
SET RecordEndDate = '9999-12-31'
WHERE RecordEndDate = (
						SELECT dateadd(day,-1, max(FileDate))
						FROM catering..CustomerTmp
					);


