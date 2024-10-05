-- If you want to create the DB from code run the next line
--CREATE DATABASE [AuctionHouse];

-- And if you need to switch DB's run this
--USE [AuctionHouse];

-- *************** Create tables/entities ********************
CREATE TABLE [Buyer] 
(
  [ID]        INT          NOT NULL IDENTITY(1, 1) PRIMARY KEY,
  [FirstName] NVARCHAR(50) NOT NULL,
  [LastName]  NVARCHAR(50) NOT NULL,
  [Email]     NVARCHAR(50) NOT NULL
);

CREATE TABLE [Seller] 
(
  [ID]        INT          NOT NULL IDENTITY(1, 1) PRIMARY KEY,
  [FirstName] NVARCHAR(50) NOT NULL,
  [LastName]  NVARCHAR(50) NOT NULL,
  [Email]     NVARCHAR(50) NOT NULL,
  [Phone]     NVARCHAR(15),
  [TaxIDNumber] NVARCHAR(12) NOT NULL
);

CREATE TABLE [Item] 
(
  [ID]          INT           NOT NULL IDENTITY(1, 1) PRIMARY KEY,
  [Name]        NVARCHAR(50)  NOT NULL,
  [Description] NVARCHAR(256) NOT NULL,
  [SellerID]    INT           -- In this model an Item doesn't have to have a seller
);

-- and for some reason a Bid can be created without a price, time, buyer or item.
CREATE TABLE [Bid] 
(
  [ID]            INT             NOT NULL IDENTITY(1, 1) PRIMARY KEY,
  [Price]         DECIMAL(17,2),
  [TimeSubmitted] DATETIME,
  [BuyerID]       INT,
  [ItemID]        INT
);

-- *************** Add foreign key relations using named constraints ********************
ALTER TABLE [Item] ADD CONSTRAINT [Item_Fk_Seller] FOREIGN KEY ([SellerID]) REFERENCES [Seller] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [Bid]  ADD CONSTRAINT [Bid_Fk_Buyer]   FOREIGN KEY ([BuyerID])  REFERENCES [Buyer]  ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [Bid]  ADD CONSTRAINT [Bid_Fk_Item]    FOREIGN KEY ([ItemID])   REFERENCES [Item]   ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
