-- Remove relations, easy since we named them
ALTER TABLE [Bid] DROP CONSTRAINT [Bid_Fk_Item];
ALTER TABLE [Bid] DROP CONSTRAINT [Bid_Fk_Buyer];
ALTER TABLE [Item] DROP CONSTRAINT [Item_Fk_Seller];

-- then delete tables.  If we hadn't removed relations first then we'd have to drop these
-- tables in a very specific order to avoid trying to violate constraints
DROP TABLE [Bid];
DROP TABLE [Item];
DROP TABLE [Seller];
DROP TABLE [Buyer];

-- After running this script we can re-use the database.  No need to create a new one.