ALTER TABLE [dbo].[WorkItem]
    ADD CONSTRAINT [FK_WorkItem_Project] FOREIGN KEY ([Project]) REFERENCES [dbo].[Project] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

