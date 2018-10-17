CREATE DATABASE Adventure4You;
GO

USE Adventure4You;
GO

CREATE TABLE Race (
    RaceId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RaceName NVARCHAR(255) NOT NULL,
    RaceGuid NVARCHAR(38) NOT NULL,
    RaceCoordinatesCheckEnabled BIT
);

CREATE TABLE Points (
    PointId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    PointRaceId INT NOT NULL,
    PointName NVARCHAR(255) NOT NULL,
    PointGuid NVARCHAR(38) NOT NULL,
    PointCoordinates NVARCHAR(255)
);

CREATE TABLE TeamLinks (
    TeamLinkId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TeamLinkRaceId INT NOT NULL
);

CREATE TABLE Teams (
    TeamId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TeamName NVARCHAR(255) NOT NULL
);