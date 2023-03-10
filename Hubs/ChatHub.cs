using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task UpdateVysledek(int druzstvoID, int startovniCislo,string druzstvoNazev, DateTime? vysledek1, DateTime? vysledek2)
        {
            await Clients.All.SendAsync("UpdateVysledek",druzstvoID, startovniCislo, vysledek1, vysledek2);
        }
    }
}
