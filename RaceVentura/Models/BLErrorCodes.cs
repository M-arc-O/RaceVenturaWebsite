namespace RaceVentura.Models
{
    public enum BLErrorCodes
    {
        Duplicate = 1,
        UserUnauthorized = 2,
        EmailNotConfirmed = 3,
        NotFound = 4,
        MaxIdsReached = 5,
        AnswerIncorrect = 6,
        CoordinatesIncorrect = 7,
        NotActiveStage = 8,
        RaceNotStarted = 9,
        RaceEnded = 10
    }
}
