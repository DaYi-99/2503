
-- TABLE CUSTOMER --
create table CUSTOMER (
ID_CUS			Int identity(1,1),
NAME			nVarchar(50)		Not Null,
UN				Varchar(50)			Unique,
PW				Varchar(20)			Not Null,
EMAIL			Varchar(50)			Unique,
ADDRESS			nVarchar(200),
PHONENUMBER		Varchar(10),
DATEOFBIRTH		Datetime
CONSTRAINT PK_CU PRIMARY KEY(ID_CUS)
)


-- TABLE CATEGORIES --
Create Table CATEGORIES
(
ID_CAT			int Identity(1,1)	primary key,
NAME			nvarchar(50)		NOT NULL
)




-- TABLE PRODUCT --
CREATE TABLE PRODUCT
(
ID_PRO			INT IDENTITY(1,1),
NAME			NVARCHAR(100)		NOT NULL,
PRICE			Decimal(18,0)		CHECK (PRICE>=0),
SIZE			VARCHAR(10),
DESCRIPTION		NVarchar(Max),
IMAGE			VARCHAR(500),
DATEUPDATE		DATETIME,
QUANTITIES		INT,
ID_CAT			INT,
Constraint PK_PR Primary Key(ID_PRO),
Constraint FK_PR_CA Foreign Key(ID_CAT) References CATEGORIES(ID_CAT),
)


-- TABLE ORDERS --
CREATE TABLE ORDERS
(
ID_ORD			INT IDENTITY(1,1)	primary key,
PAYMENT			bit,
STTSHIP			bit,
DATEORDER		Datetime,
DATESHIP		Datetime,
ID_CUS			INT,
CONSTRAINT FK_OR_CU FOREIGN KEY (ID_CUS) REFERENCES CUSTOMER(ID_CUS)
)
sp_rename 'ORDERS.ThanhToan','PAYMENT','COLUMN'; --doi ten cot

-- TABLE ORDER_DETAIL --
CREATE TABLE ORDER_DETAIL
(
ID_ORD			INT,
ID_PRO			INT,
QUANTITY		Int					Check(QUANTITY>0),
UNITPRICE		Decimal(18,0)		Check(UNITPRICE>=0),
CONSTRAINT PK_OD PRIMARY KEY(ID_ORD,ID_PRO),
CONSTRAINT FK_OD_OR FOREIGN KEY (ID_ORD) REFERENCES ORDERS(ID_ORD),
CONSTRAINT FK_OD_PR FOREIGN KEY (ID_PRO) REFERENCES PRODUCT(ID_PRO)
)


-- TABLE ADMIN --
create table ADMIN (
UN				varchar(30)			primary key,
PW				varchar(30)			not null,
NAME			nvarchar(100)
)
