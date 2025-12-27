SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documents](
	[Id] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](256) NOT NULL,
	[Content] [nvarchar](1024) NOT NULL,
	[MetaData] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[CreatedDateTime] [datetime2](3) NOT NULL,
	[ProcessedDateTime] [datetime2](3) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScanResults](
	[Id] [uniqueidentifier] NOT NULL,
	[DocumentId] [uniqueidentifier] NOT NULL,
	[ScanType] [int] NOT NULL,
	[Position] [int] NOT NULL,
	[MatchedText] [nvarchar](1024) NOT NULL,
	[CreatedDateTime] [datetime2](3) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ScanResults] ADD  CONSTRAINT [PK_ScanResult] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ScanResults]  WITH CHECK ADD  CONSTRAINT [FK_ScanResult_Documents] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Documents] ([Id])
GO
ALTER TABLE [dbo].[ScanResults] CHECK CONSTRAINT [FK_ScanResult_Documents]
GO
