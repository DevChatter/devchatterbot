﻿using System.Collections.Generic;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Events
{
    public class CurrencyGenerator
    {
        private readonly object _userCreationLock = new object();
        private readonly IRepository _repository;
        private readonly ChatUserCollection _chatUserCollection;

        public CurrencyGenerator(List<IChatClient> chatClients, IRepository repository)
        {
            _repository = repository;
            _chatUserCollection = new ChatUserCollection(repository);
            foreach (IChatClient chatClient in chatClients)
            {
                chatClient.OnUserNoticed += ChatClientOnOnUserNoticed;
                chatClient.OnUserLeft += ChatClientOnUserLeft;
            }
        }

        private void ChatClientOnOnUserNoticed(object sender, UserStatusEventArgs eventArgs)
        {
            if (_chatUserCollection.NeedToWatchUser(eventArgs.DisplayName))
            {
                ChatUser userFromDb = GetOrCreateChatUser(eventArgs);
                _chatUserCollection.WatchUser(userFromDb);
            }
        }

        private ChatUser GetOrCreateChatUser(UserStatusEventArgs eventArgs)
        {
            lock (_userCreationLock)
            {
                ChatUser userFromDb = _repository.Single(ChatUserPolicy.ByDisplayName(eventArgs.DisplayName));
                userFromDb = userFromDb ?? _repository.Create(eventArgs.ToChatUser());
                return userFromDb;
            }
        }

        private void ChatClientOnUserLeft(object sender, UserStatusEventArgs eventArgs)
        {
            GetOrCreateChatUser(eventArgs);

            _chatUserCollection.StopWatching(eventArgs.DisplayName);
        }

        public void UpdateCurrency()
        {
            _chatUserCollection.UpdateEachChatter(x => x.Tokens += 10);
        }

        public void AddCurrencyTo(List<string> listOfNames, int tokensToAdd)
        {
            _chatUserCollection.UpdateSpecficChatters(x => x.Tokens += tokensToAdd, 
                x => listOfNames.Contains(x.DisplayName));
        }
    }
}