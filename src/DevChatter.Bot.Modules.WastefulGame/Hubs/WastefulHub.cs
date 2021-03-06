using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos;
using DevChatter.Bot.Modules.WastefulGame.Model;
using DevChatter.Bot.Modules.WastefulGame.Model.Enums;
using DevChatter.Bot.Modules.WastefulGame.Model.Specifications;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Hubs
{
    public class WastefulHub : Hub<IWastefulDisplay>
    {
        private readonly IGameRepository _repo;
        private readonly SurvivorRepo _survivorRepo;
        private readonly List<IChatClient> _chatClients;

        public WastefulHub(IList<IChatClient> chatClients,
            IGameRepository repo,
            SurvivorRepo survivorRepo)
        {
            _repo = repo;
            _survivorRepo = survivorRepo;
            _chatClients = chatClients.ToList();
        }

        public void GameEnd(int points, string playerName,
            string userId, EndTypes endType, int levelNumber,
            List<HeldItemDto> items, int money, string escapeType)
        {
          string itemDisplayText = items.Any()
                ? string.Join(", ", items.Select(x => x.Name))
                : "nothing";
            string message = $"{playerName} has {endType} on level {levelNumber} with {points} points while holding {itemDisplayText} and {money} coins.";
            _chatClients.ForEach(c => c.SendMessage(message));

            Survivor survivor = _survivorRepo.GetOrCreate(playerName, userId);
            Location location = _repo.Single(LocationSpecification.ByEscapeType(escapeType));

            survivor.Money += money; // You get to keep money even if dead.

            var inventoryItems = items.Select(x => x.ToInventoryItem()).ToList();
            survivor.ApplyEndGame(levelNumber, points, endType,
                inventoryItems, location);

            _survivorRepo.Save(survivor);
        }
    }
}
