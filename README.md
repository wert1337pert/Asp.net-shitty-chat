# Asp.net-shitty-chat
a crappy chat system made in asp.net
- Database is not included in this source code but you can just create a database named main and create these following tables

`
CREATE TABLE [dbo].[Chats] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Content]    VARCHAR (MAX) DEFAULT ('fail') NULL,
    [Userid]     INT           DEFAULT ((1)) NOT NULL,
    [PostedDate] VARCHAR (50)  DEFAULT ('09/11/2001') NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);`

and

`
CREATE TABLE [dbo].[Users] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Username]  VARCHAR (25)  DEFAULT ('Unknown') NOT NULL,
    [Password]  VARCHAR (MAX) NOT NULL,
    [AuthToken] VARCHAR (100) NOT NULL,
    [Biography] VARCHAR (250) DEFAULT ('I am new to this shitty ass chat site') NULL,
    [JoinDate]  VARCHAR (50)  DEFAULT ('09/11/2001') NOT NULL,
    [Admin]     VARCHAR (7)   DEFAULT ('false') NOT NULL,
    [Email]     VARCHAR (MAX) DEFAULT ('example@nice.com') NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
`
