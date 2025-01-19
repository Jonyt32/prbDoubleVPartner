USE [prbDoubleParners];
GO

ALTER TABLE Personas
ADD Identificacion AS (TipoIdentificacion + ' - ' + NumeroIdentificacion) PERSISTED;

ALTER TABLE Personas
ADD NombresApellidos AS (Nombres + ' ' + Apellidos) PERSISTED;