SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchKeyword]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SearchKeyword](
	[NumberID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SearchKeyword_NumberID]  DEFAULT (newid()),
	[SearchName] [nvarchar](256) NULL,
	[LoweredSearchName] [nvarchar](256) NULL,
	[TotalCount] [int] NULL CONSTRAINT [DF_SearchKeyword_TotalCount]  DEFAULT ((0)),
	[LastUpdatedDate] [datetime] NULL,
	[DataCount] [int] NULL CONSTRAINT [DF_SearchKeyword_DataCount]  DEFAULT ((0)),
 CONSTRAINT [PK_SearchKeyword] PRIMARY KEY CLUSTERED 
(
	[NumberID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [SearchKeyword_AspNet_SqlCacheNotification_Trigger] ON [dbo].[SearchKeyword]
                       FOR INSERT, UPDATE, DELETE AS BEGIN
                       SET NOCOUNT ON
                       EXEC dbo.AspNet_SqlCacheUpdateChangeIdStoredProcedure N'SearchKeyword'
                       END
                       
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Order](
	[OrderId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Order_OrderId]  DEFAULT (newid()),
	[OrderNum] [varchar](30) NULL,
	[UserId] [uniqueidentifier] NULL,
	[Receiver] [nvarchar](50) NULL,
	[ProviceCity] [nvarchar](30) NULL,
	[Address] [nvarchar](256) NULL,
	[Mobilephone] [varchar](20) NULL,
	[Telephone] [varchar](20) NULL,
	[Email] [nvarchar](256) NULL,
	[Products] [nvarchar](max) NULL,
	[TotalCount] [int] NULL CONSTRAINT [DF_Order_TotalCount]  DEFAULT ((0)),
	[TotalPrice] [decimal](18, 2) NULL CONSTRAINT [DF_Order_TotalPrice]  DEFAULT ((0)),
	[PayPrice] [decimal](18, 2) NULL CONSTRAINT [DF_Order_PayPrice]  DEFAULT ((0)),
	[PayOption] [nvarchar](20) NULL,
	[PayStatus] [tinyint] NULL CONSTRAINT [DF_Order_PayStatus]  DEFAULT ((0)),
	[Status] [tinyint] NULL CONSTRAINT [DF_Order_Status]  DEFAULT ((0)),
	[PayDate] [datetime] NULL CONSTRAINT [DF_Order_PayDate]  DEFAULT (((1754)-(1))-(1)),
	[LastUpdatedDate] [datetime] NULL CONSTRAINT [DF_Order_LastUpdatedDate]  DEFAULT (((1754)-(1))-(1)),
	[CreateDate] [datetime] NULL CONSTRAINT [DF_Order_CreateDate]  DEFAULT (((1754)-(1))-(1)),
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductItem](
	[ProductId] [varchar](40) NOT NULL,
	[PNum] [varchar](30) NULL,
	[StockNum] [int] NULL CONSTRAINT [DF_ProductItem_StockNum]  DEFAULT ((0)),
	[LImagesUrl] [nvarchar](max) NULL,
	[MImagesUrl] [nvarchar](max) NULL,
	[SImagesUrl] [nvarchar](max) NULL,
	[MarketPrice] [decimal](18, 2) NULL CONSTRAINT [DF_ProductItem_MarketPrice]  DEFAULT ((0)),
	[PayOptions] [nvarchar](300) NULL,
	[ViewCount] [int] NULL CONSTRAINT [DF_ProductItem_ViewCount]  DEFAULT ((0)),
	[CreateDate] [datetime] NULL CONSTRAINT [DF_ProductItem_CreateDate]  DEFAULT (((1754)-(1))-(1)),
 CONSTRAINT [PK_ProductItem] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductAttr]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductAttr](
	[ProductId] [varchar](40) NOT NULL,
	[CustomAttrs] [nvarchar](max) NULL,
	[Descr] [ntext] NULL,
	[CreateDate] [datetime] NULL CONSTRAINT [DF_ProductAttr_CreateDate]  DEFAULT (((1754)-(1))-(1)),
 CONSTRAINT [PK_ProductAttr] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Category](
	[NumberID] [varchar](40) NOT NULL,
	[CategoryName] [nvarchar](256) NULL,
	[ParentID] [varchar](40) NULL,
	[Sort] [int] NULL CONSTRAINT [DF_Category_Sort]  DEFAULT ((0)),
	[Remark] [nvarchar](300) NULL,
	[Title] [nvarchar](10) NULL,
	[CreateDate] [datetime] NULL CONSTRAINT [DF_Category_CreateDate]  DEFAULT (((1754)-(1))-(1)),
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[NumberID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [Category_AspNet_SqlCacheNotification_Trigger] ON [dbo].[Category]
                       FOR INSERT, UPDATE, DELETE AS BEGIN
                       SET NOCOUNT ON
                       EXEC dbo.AspNet_SqlCacheUpdateChangeIdStoredProcedure N'Category'
                       END
                       
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemProfile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SystemProfile](
	[NumberID] [varchar](40) NOT NULL,
	[Title] [nvarchar](50) NULL,
	[ContentText] [ntext] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_SystemProfile] PRIMARY KEY CLUSTERED 
(
	[NumberID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProvinceCity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProvinceCity](
	[NumberID] [varchar](50) NULL,
	[RegionName] [nvarchar](50) NULL,
	[RegionCode] [varchar](20) NULL,
	[ParentID] [varchar](50) NULL,
	[RegionLevel] [int] NULL CONSTRAINT [DF_ProvinceCity_RegionLevel]  DEFAULT ((0)),
	[RegionOrder] [int] NULL CONSTRAINT [DF_ProvinceCity_RegionOrder]  DEFAULT ((0)),
	[Ename] [nvarchar](50) NULL,
	[ShortEname] [nvarchar](20) NULL
) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [ProvinceCity_AspNet_SqlCacheNotification_Trigger] ON [dbo].[ProvinceCity]
                       FOR INSERT, UPDATE, DELETE AS BEGIN
                       SET NOCOUNT ON
                       EXEC dbo.AspNet_SqlCacheUpdateChangeIdStoredProcedure N'ProvinceCity'
                       END
                       
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Product](
	[ProductId] [varchar](40) NOT NULL,
	[CategoryId] [varchar](40) NULL,
	[ProductName] [nvarchar](256) NULL,
	[Subtitle] [nvarchar](256) NULL,
	[ProductPrice] [decimal](18, 2) NULL CONSTRAINT [DF_Product_ProductPrice]  DEFAULT ((0)),
	[ImagesUrl] [nvarchar](300) NULL,
	[CreateDate] [datetime] NULL CONSTRAINT [DF_Product_CreateDate]  DEFAULT (((1754)-(1))-(1)),
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [Product_AspNet_SqlCacheNotification_Trigger] ON [dbo].[Product]
                       FOR INSERT, UPDATE, DELETE AS BEGIN
                       SET NOCOUNT ON
                       EXEC dbo.AspNet_SqlCacheUpdateChangeIdStoredProcedure N'Product'
                       END
                       
