USE PCOperaciones;

INSERT INTO TipoRol VALUES ('Cliente', 'Cliente');

ALTER TABLE Usuario ADD CONSTRAINT FK_TipoRol FOREIGN KEY (Rol) REFERENCES TipoRol (Rol);

--select * from Usuario

--select * from Cliente


ALTER TABLE Usuario ALTER COLUMN [Cedula] VARCHAR(20) NOT NULL;
ALTER TABLE Usuario ALTER COLUMN [Nombre] VARCHAR(50) NOT NULL;

INSERT INTO [Usuario]
           ([Cedula]
           ,[Nombre]
           ,[Rol]
           ,[Telefono]
           ,[Celular]
           ,[Email]
           ,[NombreUsuario]
           ,[Clave]
           ,[Estado]
           ,[FechaCreacion])
SELECT
NIT,
NmCliente,
'Cliente',
'300',
'300',
'email',
NIT,
'pci1234',
'1',
getDate()
from Cliente
GO;

SELECT REPLACE([NombreUsuario], '.', '') FROM Usuario order by [NombreUsuario]

UPDATE Usuario SET [NombreUsuario] = REPLACE([NombreUsuario], '.', '');

select REPLACE([NombreUsuario], '.', ''), count([NombreUsuario])


from usuario 


group by [NombreUsuario]


having count([NombreUsuario])>1

select * from usuario


select * from DocumentoOperaciones

if not  exists ( select * from INFORMATION_SCHEMA.COLUMNS
where TABLE_NAME='Seguimiento'
and COLUMN_NAME='VisibleCliente')
ALTER TABLE Seguimiento ADD VisibleCliente bit;
GO

update Seguimiento set VisibleCliente = 0


if not  exists ( select * from INFORMATION_SCHEMA.COLUMNS
where TABLE_NAME='ArchivoDocumento'
and COLUMN_NAME='VisibleCliente')
ALTER TABLE ArchivoDocumento ADD VisibleCliente bit;
GO

update ArchivoDocumento set VisibleCliente = 0

select * from usuario

