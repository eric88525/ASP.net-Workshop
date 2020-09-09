--Q1

SELECT  r.KEEPER_ID AS 'KeeperId',m.USER_CNAME AS 'CName'
,m.USER_ENAME AS 'EName',YEAR(r.LEND_DATE) AS 'Borrow Year'
,COUNT(*) AS 'BorrowCnt'
FROM dbo.MEMBER_M m
INNER  JOIN dbo.BOOK_LEND_RECORD r
	ON m.USER_ID = r.KEEPER_ID
GROUP BY r.KEEPER_ID,m.USER_CNAME,m.USER_ENAME,YEAR(r.LEND_DATE)
GO

--Q2

SELECT TOP(5) blr.BOOK_ID ,bd.BOOK_NAME,COUNT(*) AS C
FROM dbo.BOOK_LEND_RECORD blr
	INNER JOIN dbo.BOOK_DATA bd
		ON blr.BOOK_ID = bd.BOOK_ID
GROUP BY blr.BOOK_ID,blr.BOOK_ID,bd.BOOK_NAME
ORDER BY COUNT(*) DESC
GO

--Q3

;WITH etc  AS
(SELECT 
CASE MONTH(rec.LEND_DATE) 
	WHEN 1 THEN '2019/01~2019/03'
	WHEN 2 THEN '2019/01~2019/03'
	WHEN 3 THEN '2019/01~2019/03'
	WHEN 4 THEN '2019/04~2019/06'
	WHEN 5 THEN '2019/04~2019/06'
	WHEN 6 THEN '2019/04~2019/06'
	WHEN 7 THEN '2019/07~2019/09'
	WHEN 8 THEN '2019/07~2019/09'
	WHEN 9 THEN '2019/07~2019/09'
	WHEN 10 THEN '2019/10~2019/12'
	WHEN 11 THEN '2019/10~2019/12'
	WHEN 12 THEN '2019/10~2019/12'
	ELSE ''
END AS q
FROM dbo.BOOK_LEND_RECORD rec
WHERE YEAR(rec.LEND_DATE) = 2019)
SELECT q AS quarter,COUNT(*) AS 'QTY'
FROM etc
GROUP BY q
GO

--Q4

;WITH etc
AS
(SELECT
	ROW_NUMBER() OVER (PARTITION BY c.BOOK_CLASS_NAME ORDER BY COUNT(*) DESC) AS Seq
	  , c.BOOK_CLASS_NAME AS 'BookClass'
	   ,d.BOOK_ID AS 'BookId'
	   ,d.BOOK_NAME AS 'BookName'
	   ,COUNT(*) AS Cnt

	FROM BOOK_LEND_RECORD rec
	INNER JOIN BOOK_DATA d
		ON rec.BOOK_ID = d.BOOK_ID
	INNER JOIN BOOK_CLASS c
		ON d.BOOK_CLASS_ID = c.BOOK_CLASS_ID
	GROUP BY c.BOOK_CLASS_ID
			,d.BOOK_ID
			,c.BOOK_CLASS_NAME
			,d.BOOK_NAME)
SELECT *
FROM etc
WHERE Seq < 4
GO

--Q5

;WITH etc AS(
SELECT id,b.name,
CASE b.yy WHEN 2016 THEN COUNT(yy) ELSE 0 END AS '2016',
CASE b.yy WHEN 2017 THEN COUNT(yy)  ELSE 0 END AS '2017',
CASE b.yy WHEN 2018 THEN COUNT(yy)  ELSE 0 END AS '2018',
CASE b.yy WHEN 2019 THEN COUNT(yy)  ELSE 0 END AS '2019'
FROM 
(
SELECT c.BOOK_CLASS_ID AS id,c.BOOK_CLASS_NAME AS name,YEAR(rec.LEND_DATE)  AS yy
FROM BOOK_LEND_RECORD rec
INNER JOIN BOOK_DATA d
	ON rec.BOOK_ID = d.BOOK_ID
INNER JOIN BOOK_CLASS c
	ON D.BOOK_CLASS_ID = c.BOOK_CLASS_ID
)
AS b
GROUP BY b.id,b.yy,b.name
)

SELECT id AS 'ClassId',name AS 'ClassName',SUM([2016]) AS 'CNT2016',SUM([2017]) AS 'CNT2017',SUM([2018]) AS 'CNT2018',SUM([2019]) AS 'CNT2019'
FROM  etc 
GROUP  BY id,name
ORDER BY  id
GO

--Q6
IF OBJECT_ID('dbo.v1','V') IS NOT NULL 
	DROP VIEW dbo.v1
GO

CREATE VIEW dbo.v1 AS
SELECT c.BOOK_CLASS_ID AS id,c.BOOK_CLASS_NAME AS name,YEAR(rec.LEND_DATE)  AS yy
FROM BOOK_LEND_RECORD rec
INNER JOIN BOOK_DATA d
	ON rec.BOOK_ID = d.BOOK_ID
INNER JOIN BOOK_CLASS c
	ON D.BOOK_CLASS_ID = c.BOOK_CLASS_ID;
GO

SELECT id AS 'ClassId',name AS 'ClassName',[2016] AS 'CNT2016',[2017] AS 'CNT2017' ,[2018] AS 'CNT2018',[2019] AS 'CNT2019'
FROM dbo.v1 AS D 
PIVOT(COUNT(yy) FOR yy 
		IN( [2016],[2017],[2018],[2019])
		) AS pvt
ORDER BY id
GO

--Q7

IF OBJECT_ID('dbo.books','V') IS NOT NULL 
	DROP VIEW dbo.books
GO
CREATE VIEW dbo.books AS
SELECT c.BOOK_CLASS_ID AS id,c.BOOK_CLASS_NAME AS name,YEAR(rec.LEND_DATE)  AS yy
FROM BOOK_LEND_RECORD rec
INNER JOIN BOOK_DATA d
	ON rec.BOOK_ID = d.BOOK_ID
INNER JOIN BOOK_CLASS c
	ON D.BOOK_CLASS_ID = c.BOOK_CLASS_ID;
GO

/* store dim col*/
DECLARE @cols NVARCHAR(MAX)= N''
DECLARE @colsName NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(yy) ,N',' + QUOTENAME(yy))
,@colsName = @colsName + iif(@colsName = N'',QUOTENAME(yy) +' AS CNT'+ CONVERT(varchar, yy)  , N',' + QUOTENAME(yy)+' AS CNT'+ CONVERT(varchar, yy) )
FROM 
(
    SELECT DISTINCT(yy) 
    FROM dbo.books AS books
) T
ORDER BY yy

/*print @cols
print @colsName*/

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
SELECT id AS ClassId,name AS ClassName,' +@colsName + '
FROM dbo.books as V
PIVOT
(
  COUNT(yy) FOR yy 
  IN ('
  + @cols
  + ')
) AS pvt
ORDER BY id'
EXEC sp_executesql @sql
GO

--Q8

IF OBJECT_ID('dbo.books','V') IS NOT NULL 
	DROP VIEW dbo.books
GO
CREATE VIEW dbo.books AS
SELECT d.BOOK_ID,d.BOOK_BOUGHT_DATE,rec.LEND_DATE,c.BOOK_CLASS_ID,c.BOOK_CLASS_NAME,d.BOOK_AMOUNT,rec.KEEPER_ID,M.USER_CNAME,M.USER_ENAME,code.CODE_ID,code.CODE_NAME
FROM BOOK_LEND_RECORD rec
INNER JOIN BOOK_DATA d
	ON rec.BOOK_ID = d.BOOK_ID
INNER JOIN BOOK_CLASS c
	ON D.BOOK_CLASS_ID = c.BOOK_CLASS_ID
INNER JOIN BOOK_CODE code
	ON D.BOOK_STATUS = code.CODE_ID
INNER JOIN MEMBER_M M
	ON rec.KEEPER_ID = M.USER_ID
GO

SELECT BOOK_ID AS '書本ID', convert(varchar, book_bought_date, 111) AS '購書日期',
convert(varchar, lend_date, 111) AS '借閱日期',BOOK_CLASS_ID + '-' + BOOK_CLASS_NAME  AS '書籍類別',
KEEPER_ID +'-'+ USER_CNAME +'(' +USER_ENAME+')' AS '借閱人',
CODE_ID+'-'+CODE_NAME AS '狀態', BOOK_AMOUNT AS '購書金額'
FROM DBO.books b
WHERE b.KEEPER_ID = '0002'
ORDER BY BOOK_AMOUNT DESC
GO

--Q9

INSERT INTO BOOK_LEND_RECORD (BOOK_ID,KEEPER_ID,LEND_DATE,CRE_DATE,CRE_USR,MOD_DATE,MOD_USR)
VALUES (2004,'0002','20200128','20190102',0002,'20190102',0002)
GO

--Q10

DELETE FROM BOOK_LEND_RECORD
WHERE KEEPER_ID = '0002'
AND BOOK_ID = '2004'
AND LEND_DATE = '2020/01/02'