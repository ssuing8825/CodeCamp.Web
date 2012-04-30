using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCamp.Web.Models;

namespace CodeCamp.Web.Repository
{

    public class PlayerRepository : CodeCamp.Web.Repository.IPlayerRepository
    {
        private IList<Player> players;
        private int nextPlayerID;

        public PlayerRepository()
        {
            players = new List<Player>();
            players.Add(new Player { Id = 1, Name = "Steve Suing", Rank = 8540, SkillLevel = "D" });
            players.Add(new Player { Id = 2, Name = "Kane Croft", Rank = 34, SkillLevel = "A" });
            players.Add(new Player { Id = 3, Name = "Bob Harris", Rank = 2, SkillLevel = "A" });
            players.Add(new Player { Id = 4, Name = "John Smith", Rank = 5065, SkillLevel = "C" });
            nextPlayerID = players.Count + 1;

        }

        public void Update(Player updatedPlayer)
        {
            var player = this.Get(updatedPlayer.Id);
            player.Name = updatedPlayer.Name;
            player.Rank = updatedPlayer.Rank;
            player.SkillLevel = updatedPlayer.SkillLevel;

        }

        public List<Player> GetAll()
        {
            return players.ToList();
        }

        public Player Get(int id)
        {
            return players.SingleOrDefault(c => c.Id == id);
        }

        public void Post(Player player)
        {
            player.Id = nextPlayerID++;
            players.Add(player);
        }

        public void Delete(int id)
        {
            var player = this.Get(id);
            players.Remove(player);
        }

    }
}