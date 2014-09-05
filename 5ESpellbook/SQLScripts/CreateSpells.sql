USE [YOURDATABASE]
GO

/****** Object:  Table [dbo].[Spells]    Script Date: 9/5/2014 2:18:53 PM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Spells](
	[SpellID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Level] [int] NOT NULL,
	[School] [varchar](200) NULL,
	[OfClass] [varchar](200) NULL,
	[Action] [varchar](200) NULL,
	[Range] [varchar](200) NULL,
	[Components] [varchar](500) NULL,
	[Duration] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[IsRitual] [bit] NULL,
	[Keywords] [varchar](max) NULL,
 CONSTRAINT [PrimaryKey_7a83f663-febf-4feb-9a0a-d00ec9b75ef8] PRIMARY KEY CLUSTERED 
(
	[SpellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Spells] ADD  CONSTRAINT [ColumnDefault_33b6984f-18e1-4fb9-8c20-f369316f7edc]  DEFAULT ((0)) FOR [IsRitual]
GO


