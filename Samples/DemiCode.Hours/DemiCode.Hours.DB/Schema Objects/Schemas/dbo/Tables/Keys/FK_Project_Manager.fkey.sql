ALTER TABLE [dbo].[Project]
    ADD CONSTRAINT [FK_Project_Manager] FOREIGN KEY ([Manager]) REFERENCES [dbo].[Employee] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

