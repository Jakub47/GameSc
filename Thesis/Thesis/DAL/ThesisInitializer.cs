using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Thesis.Migrations;
using Thesis.Models;

namespace Thesis.DAL
{
    public class ThesisInitializer : MigrateDatabaseToLatestVersion<ThesisContext,Configuration>
    {
        public static void DataSeed(ThesisContext context)
        {
            //Create data only for 1)Games 2)Post 3)GamingEvent 4)CategoryEvent 5)CategoryGame
            var categoriesGames = new List<CategoryGame>
            {
                new CategoryGame() {Name = "FPS", Description="Kill enemies in first person mode", Picture = "fps.png",
                                    Icon="fps.svg"},
                new CategoryGame() {Name = "RPG", Description="Gain expiercence,kill monsters in fantasy word", Picture = "rpg.png",
                                    Icon="rpg.svg"},
                new CategoryGame() {Name = "Fighting", Description="Master skiils fightning with others", Picture = "fighting.png",
                                    Icon="fighting.svg"},
                new CategoryGame() {Name = "Strategy", Description="Plan your moves in strategy", Picture = "strategy.png",
                                    Icon="strategy.svg"},
            };
            categoriesGames.ForEach(a => context.CategoryGames.AddOrUpdate(a));
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            var categoryEvents = new List<CategoryEvent>
            {
                new CategoryEvent() {Name = "Gaming Together", Description="Gather players to play online", Picture = "gt.png",
                                    Icon="gt.svg"},
                new CategoryEvent() {Name = "Create mod", Description="Gather people to create mod or game together", Picture = "cmo.png",
                                    Icon="cmo.svg"},
                new CategoryEvent() {Name = "Create Map", Description="Gather players to create map for game", Picture = "cmp.png",
                                    Icon="cmp.svg"},
            };
            categoryEvents.ForEach(a => context.CategoryEvents.AddOrUpdate(a));
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            var categoriesPosts = new List<CategoryPost>
            {
                new CategoryPost() {Name = "Samopoczucie", Description="Napisz jak się czujesz", Picture = "sp.png",
                                    Icon="sp.svg"},
                new CategoryPost() {Name = "Osiagniecia", Description="Pochwal się niesamowitymi wydarzeniami", Picture = "os.png",
                                    Icon="os.svg"},
                new CategoryPost() {Name = "Wspomnienia", Description="Powspominaj dobre czasy z dobrymi grami", Picture = "ws.png",
                                    Icon="ws.svg"},
            };
            categoriesPosts.ForEach(b => context.CategoryPosts.AddOrUpdate(b));
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            // ************No need for seed for games************
            var games = new List<Game>
            {
                new Game() {CategoryGameId = 1, Title = "Doom", Publisher = "id Software",
                Description = "The Doom (stylized as DOOM) franchise is a series of first-person shooter video games developed by id Software, and related novels, comics, board games, and film adaptation. The series focuses on the exploits of an unnamed space marine operating under the auspices of the Union Aerospace Corporation (UAC), who fights hordes of demons and the undead in order to survive.",
                ShortDescription = "The Doom (stylized as DOOM) franchise is a series of first-person shooter video games", MainPicture = "Doom.png",
                GamesForExchange = "Witcher 3|Max Payne| Star Wars|Dragon Ball"},
                new Game() {CategoryGameId = 2, Title = "Witcher 3", Publisher = "CD Projekt",
                Description = "Wiedźmin (wydany pod nazwą międzynarodową The Witcher, w Czechach jako Zaklínač, w Rosji jako Wied´mak (Ведьмак)) – komputerowa gra fabularna w konwencji fantasy, wyprodukowana przez polskie studio CD Projekt RED i wydana przez Atari[1]. Jej premiera odbyła się 26 października 2007 roku na platformie Microsoft Windows, natomiast w kwietniu 2012 roku ukazała się jej wersja na komputery z systemem OS X.",
                ShortDescription = "Komputerowa gra fabularna w konwencji fantasy, wyprodukowana przez polskie studio", MainPicture = "Witcher3.png",
                GamesForExchange = "Doom|Quake|Warcraft 3|Dragon Quest"},
                new Game() {CategoryGameId = 3, Title = "Tekken 6", Publisher = "Namco",
                Description = "japońska konsolowa gra wyprodukowana przez Namco. Gra została wydana w 1994 i 1995 roku, w latach 1995, 1997, 2000 i 2011 wydano jej specjalne edycje.",
                ShortDescription = "japońska konsolowa gra wyprodukowana przez Namco", MainPicture = "Tekken6.png",
                GamesForExchange = "Devil may Cry |Robin Hood|"},
                new Game() {CategoryGameId = 4, Title = "Warcraft 3", Publisher = "Blizzard Entertainment",
                Description = "Warcraft III: Reign of Chaos – strategiczna gra czasu rzeczywistego wydana w 2002 roku przez firmę Blizzard, będąca trzecią częścią cyklu Warcraft. Akcja gry rozgrywa się w fantastycznym świecie Azeroth zamieszkiwanym przez takie istoty, jak elfy, orkowie oraz ludzie. Tłem fabuły jest walka tych ras przeciwko inwazji sił nieumarłych, znanych jako Plaga oraz Płonący Legion. W roku 2003 wydany został dodatek do gry, zatytułowany Warcraft III: The Frozen Throne, a w roku 2004 gra MMORPG osadzona w świecie Azeroth – World of Warcraft.",
                ShortDescription = "strategiczna gra czasu rzeczywistego wydana w 2002 roku przez firmę Blizzard,", MainPicture = "Warcraft3.png",
                GamesForExchange = "Metro 2033"},
            };
            games.ForEach(a => context.Games.AddOrUpdate(a));
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            // ************No need for seed for games************


            var posts = new List<Post>
            {
                new Post() {CategoryPostId = 1,Title = "LOL ale fart :)", Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas sed dui eu mauris mollis efficitur non a purus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla sed velit eget dui varius posuere. Proin auctor, elit sed lacinia consectetur, purus nibh gravida felis, sit amet eleifend neque ipsum ut sem. In euismod erat nisi, ac feugiat felis sagittis in. Curabitur ex lectus, imperdiet a dolor et, consequat scelerisque erat. Mauris vulputate porta pulvinar. Praesent eget laoreet nisl. Vivamus luctus dolor et est mollis, ac laoreet dolor viverra. Praesent sed lorem vel nunc ornare tristique. Nullam convallis felis turpis, sollicitudin eleifend dolor luctus nec.",
                            DateOfInsert = DateTime.Now, MainPicture = "Fart.png"},
                new Post() {CategoryPostId = 2,Title = "Gdzie moge znalezc taka bron", Content = "Integer pellentesque condimentum nunc ac tristique. Praesent scelerisque pretium lorem eget porttitor. Praesent mollis sollicitudin nisl, luctus vulputate nulla rutrum in. Morbi et tellus id orci tristique congue ut viverra felis. Mauris tempor metus vitae venenatis varius. Vivamus euismod viverra sapien eget maximus.",
                            DateOfInsert = DateTime.Now, MainPicture = "Bron.png"},
                new Post() {CategoryPostId = 3,Title = "SSS w dmc 5", Content = "Praesent tincidunt commodo nulla, vel tempus ipsum vulputate nec. Integer blandit ante ipsum, sit amet pellentesque neque egestas id. In pretium eros id mauris rutrum feugiat.",
                            DateOfInsert = DateTime.Now, MainPicture = "SSS.png"},
            };
            posts.ForEach(a => context.Posts.AddOrUpdate(a));
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            var gamingEvents = new List<GamingEvent>
            {
                new GamingEvent() {CategoryEventId = 1, Title = "Zbiorka osob do csa",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam sit amet sem lacus. Curabitur nulla lacus, gravida vel dolor sed, hendrerit viverra tellus. Nulla ultrices purus tempor lobortis feugiat. Maecenas in velit quis ligula maximus facilisis id rutrum ante. Proin euismod nulla quis turpis finibus, ut congue ante blandit. Vivamus vel diam molestie, sodales augue in, cursus mauris. Vivamus dictum ex luctus purus tincidunt, tempus aliquet nulla ornare.",
                DateOfEvent = DateTime.Now},
                new GamingEvent() {CategoryEventId = 2, Title = "Mod do gothica 2",
                Content = "In eu metus at felis cursus vehicula. Donec tristique, ex non elementum lacinia, ex nulla rutrum quam, non feugiat nibh ante sed mi. ",
                DateOfEvent = DateTime.Now},
                new GamingEvent() {CategoryEventId = 3, Title = "Nowa mapa do warcraft 3",
                Content = "Quisque nulla lorem, malesuada vel nibh id, porta vehicula nulla. Mauris auctor a elit molestie eleifend. Nunc egestas pellentesque tellus nec aliquet. Cras enim libero, rhoncus eget venenatis sit amet, maximus non ligula. Vestibulum tortor leo, eleifend vitae tristique ut, dictum commodo libero.",
                DateOfEvent = DateTime.Now},
            };
            gamingEvents.ForEach(a => context.GamingEvents.AddOrUpdate(a));
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public static void SeedUsers(ThesisContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            const string name = "admin@thesis.pl";
            const string password = "P@ssw0rd";
            const string roleName = "Admin";

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, UserInformation = new UserInformation(name),Nickname = "Admin123"};
                var result = userManager.Create(user, password);
            }

            user.MainPicture = "yos217.png";

            var games = context.Games.ToList();
            foreach(var game in games)
            {
                game.User = user;
                game.UserId = user.Id;
            }
            var posts = context.Posts.ToList();
            foreach (var post in posts)
            {
                post.User = user;
                post.UserId = user.Id;
            }
            var gamesEvents = context.GamingEvents.ToList();
            foreach (var gameEvent in gamesEvents)
            {
                gameEvent.Publisher = user;
                gameEvent.UserId = user.Id;
            }

            context.SaveChanges();



            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }
}