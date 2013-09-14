select
*
from
farmacias2

if   exists ( select * from INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE
where TABLE_NAME='Farmacia'
and CONSTRAINT_NAME='UQ_FarmaciaNIT')
ALTER TABLE Farmacia drop CONSTRAINT UQ_FarmaciaNIT ;
GO

ALTER TABLE medicos2
ALTER COLUMN cedula  nvarchar(50)  NULL;

delete medicos2 where cedula is null

sp_columns cliente
select
Cedula,
upper(Apellidos+' '+Nombres),
'Zona',
'CiudadID',
'Convenios',
'Optometra',
'Ninguna',
'DireccionConsulta',
'Telefono1',
'Movil',
'Email',
'Universidad',
'NumeroPacientesDia',
'MayoriaDePacientesProvienen',
'HorarioVisita',
'Edad',
'Cumpleanos',
'Secretaria',
'CumpleanosSecretaria',
'UsuarioID',
'Estado',
'FechaCreacion',
'Observaciones',
'Perfil',
'SpeakerDe'
from medico2
