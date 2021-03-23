using RaceVenturaAPI.Helpers;
using RaceVenturaAPI.ViewModels.AppApi;
using RaceVenturaAPI.ViewModels.Races;
using System;

namespace RaceVenturaAPI.Extensions
{
    public static class TeamViewModelExtensions
    {

        public static void AddQrCode(this TeamViewModel viewModel, Guid raceId)
        {
            var txtQRCode = $"QrCodeType: {QrCodeTypes.RegisterToRace}, RaceId:{raceId}, TeamId:{viewModel.TeamId}";
            var stream = RaceQrCodeHelper.CreateQrCodes(txtQRCode);
            viewModel.QrCodeArray = stream.ToArray();
        }
    }
}
