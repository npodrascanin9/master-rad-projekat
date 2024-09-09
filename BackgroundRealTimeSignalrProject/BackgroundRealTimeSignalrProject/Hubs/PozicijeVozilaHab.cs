using BackgroundRealTimeSignalrProject.Models;
using Microsoft.AspNetCore.SignalR;

namespace BackgroundRealTimeSignalrProject.Hubs
{
    public class PozicijeVozilaHab : Hub
    {
        public async Task PosaljiPoslednjuPozicijuVozila(
            PoslednjaPozicijaVozila poslednjaPozicija)
        {
            await Clients.All.SendAsync(
                method: "mojametoda",
                arg1: poslednjaPozicija);
        }
    }
}
