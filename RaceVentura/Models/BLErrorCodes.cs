namespace RaceVentura.Models
{
    public enum BLErrorCodes
    {
        Duplicate = 1,
        UserUnauthorized = 2,
        NotFound = 3,
        MaxIdsReached = 4,
        AnswerIncorrect = 5,
        CoordinatesIncorrect = 6,
        NotActiveStage = 7,
        RaceNotStarted = 8,
        RaceEnded = 9,
        EmailNotConfirmed = 10,
        InvalidToken = 11,
        NotAsignedToOrganization = 12,
    }
}
