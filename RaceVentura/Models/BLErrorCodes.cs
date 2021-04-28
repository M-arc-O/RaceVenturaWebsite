namespace RaceVentura.Models
{
    public enum BLErrorCodes
    {
        // User related
        UserUnauthorized = 000,
        EmailNotConfirmed = 001,
        InvalidToken = 002,

        // Organisation related
        NotAsignedToOrganization = 100,

        // Entity related
        Duplicate = 200,
        NotFound = 201,

        // Race related
        MaxIdsReached = 300,
        AnswerIncorrect = 301,
        CoordinatesIncorrect = 302,
        NotActiveStage = 303,
        RaceNotStarted = 304,
        RaceEnded = 305,
    }
}
