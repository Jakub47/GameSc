using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Thesis.DAL;
using Thesis.Models;

namespace Thesis.Infrastructure
{
    public class ObserveManager
    {
        private ThesisContext context;
        private ISessionLayer session;

        public ObserveManager(ISessionLayer session, ThesisContext context)
        {
            this.session = session;
            this.context = context;
        }

        public List<ObservePosition> GetObserver()
        {
            List<ObservePosition> observe;

            if (session.Get<List<ObservePosition>>(ConfigurationManager.AppSettings["ObserverSessionKey"]) == null)
            {
                observe = new List<ObservePosition>();
            }
            else
            {
                observe = session.Get<List<ObservePosition>>(ConfigurationManager.AppSettings["ObserverSessionKey"]) as List<ObservePosition>;
            }

            return observe;
        }

        public void AddToObserver(int gameId)
        {
            var observer = GetObserver();
            var observerPosition = observer.Find(g => g.Game.GameId == gameId);

            if (observerPosition != null)
                observerPosition.Amount = observerPosition.Amount;
            else
            {
                var gameToAdd = context.Games.Where(b => b.GameId == gameId).SingleOrDefault();

                if (gameToAdd != null)
                {
                    var newObserverPosition = new ObservePosition()
                    {
                        Game = gameToAdd,
                        Amount = 1,
                    };
                    observer.Add(newObserverPosition);
                }
            }

            session.Set(ConfigurationManager.AppSettings["ObserverSessionKey"], observer);

        }
        public int DeleteFromObserver(int gameId)
        {
            var observer = GetObserver();
            var observerPosition = observer.Find(k => k.Game.GameId == gameId);


            if (observerPosition != null)
            {
                if (observerPosition.Amount > 1)
                {
                    observerPosition.Amount--;
                    return observerPosition.Amount;
                }
                else
                {
                    observer.Remove(observerPosition);
                }
            }
            return 0;
        }

        public int GetAmountFromObserver()
        {
            var observer = GetObserver();
            return observer.Sum(o => o.Amount);
        }

        

        //public void Empty()
        //{
        //    session.Set<List<PositionOfExchange>>(ConfigurationManager.AppSettings["ObserverSessionKey"], null);
        //}
    }
}
