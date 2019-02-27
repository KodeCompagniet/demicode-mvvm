ALTER TABLE [dbo].[WorkItem]
    ADD CONSTRAINT [FK_WorkItem_Employee] FOREIGN KEY ([Employee]) REFERENCES [dbo].[Employee] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

