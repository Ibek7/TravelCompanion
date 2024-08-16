/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS (SELECT * FROM AppUser WHERE Email = 'max.szczurek@gmail.com')
BEGIN
	INSERT INTO AppUser (AppUserGuid, Email, FirstName, LastName, IsActive)
	VALUES (NEWID(), 'max.szczurek@gmail.com', 'Max', 'Szczurek', 1)
END

IF NOT EXISTS (SELECT * FROM AppUser WHERE Email = 'bekamdawit551@gmail.com')
BEGIN
	INSERT INTO AppUser (AppUserGuid, Email, FirstName, LastName, IsActive)
	VALUES (NEWID(), 'bekamdawit551@gmail.com', 'Bekam', 'Guta', 1)
END