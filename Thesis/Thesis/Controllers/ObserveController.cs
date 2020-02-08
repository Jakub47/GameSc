using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thesis.DAL;
using Thesis.Infrastructure;
using Thesis.ViewModels;

namespace Thesis.Controllers
{
    public class ObserveController : Controller
    {
        private ObserveManager observeManager;
        private ISessionLayer sessionManager { get; set; }
        private ThesisContext db = new ThesisContext();

        public ObserveController()
        {
            sessionManager = new SessionLayer();
            observeManager = new ObserveManager(sessionManager, db);
        }

        // GET: Observe
        public ActionResult Index()
        {
            var elementsObserver = observeManager.GetObserver();
            ObserverViewModel vm = new ObserverViewModel()
            {
                ObservePosition = elementsObserver
            };
            return View(vm);
        }

        public ActionResult AddToObserve(int id)
        {
            observeManager.AddToObserver(id);

            return RedirectToAction("Index");
        }

        public int GetAmountOfElementsInObserver()
        {
            return observeManager.GetAmountFromObserver();
        }

        public ActionResult DeleteFromObserve(int GameId)
        {
            int ObserverAmount = observeManager.DeleteFromObserver(GameId);
            int ObserverPositionAmount = observeManager.GetAmountFromObserver();

            var valuee = new ObserveDeleteViewModel
            {
                IdOfPosition = GameId,
                ObserverAmount = ObserverPositionAmount,
                AmountOfDeletingItems = ObserverAmount
            };
            return Json(valuee);
        }

        public bool IsInObserverGame(int GameId)
        {
            if (observeManager.GetObserver() == null) return false;

            return observeManager.GetObserver().Any(a => a.Game.GameId == GameId);
        }
    }
}