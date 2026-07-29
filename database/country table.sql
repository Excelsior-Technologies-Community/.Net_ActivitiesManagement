CREATE TABLE [dbo].[tbl_Country_mst](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](max) NULL,
	[StatusFlag] [nvarchar](max) NULL,
	[ShortCode] [nvarchar](max) NULL,
	[CountryFlagImage] [nvarchar](max) NULL,
	[IsIntrested] [bit] NULL,
	[IsIntrestedFlag] [nvarchar](max) NULL,
	[IsPastRejection] [bit] NULL,
	[IsLead] [nchar](1) NULL,
	[IsInquiry] [bit] NULL,
	[IsRegistration] [nchar](1) NULL,
	[IsCoaching] [nchar](1) NULL,
	[IsProcess] [nchar](1) NULL,
	[DisplayIndex] [int] NULL,
	[CreateUser] [bigint] NULL,
	[UpdateUser] [bigint] NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL)
 
CREATE TABLE [dbo].[tbl_State_mst](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NOT NULL,
	[StateName] [nvarchar](200) NOT NULL,
	[ShortCode] [nvarchar](50) NULL,
	[StatusFlag] [nvarchar](1) NOT NULL,
	[CreateUser] [int] NULL,
	[UpdateUser] [int] NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL)
 
CREATE TABLE [dbo].[tbl_City_mst](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[CityName] [nvarchar](200) NOT NULL,
	[ShortCode] [nvarchar](50) NULL,
	[StatusFlag] [nvarchar](1) NOT NULL,
	[CreateUser] [int] NULL,
	[UpdateUser] [int] NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL)
 
CREATE TABLE [dbo].[tbl_Area_mst](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[Area] [nvarchar](200) NOT NULL,
	[Pincode] [nvarchar](20) NULL,
	[StatusFlag] [nvarchar](1) NOT NULL,
	[CreateUser] [int] NULL,
	[UpdateUser] [int] NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL)