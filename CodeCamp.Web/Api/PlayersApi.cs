using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCamp.Web.Models;
using System.ServiceModel.Web;
using CodeCamp.Web.Repository;
using System.Net.Http;
using System.Net;

namespace CodeCamp.Web.Api
{

    public class PlayersApi
    {

        private IPlayerRepository repository;

        public PlayersApi(IPlayerRepository repository)
        {
            this.repository = repository;
        }

        [WebGet]
        public List<Player> Get()
        {
            return this.repository.GetAll();
        }


        [WebInvoke]
        public HttpResponseMessage<Player> Post(Player player)
        {
            this.repository.Post(player);
            var response = new HttpResponseMessage<Player>(player);
           
            response.StatusCode = HttpStatusCode.Created;
            return response;
        }


        [WebInvoke(UriTemplate = "{id}")]
        public Player Put(int id, Player player)
        {
            this.repository.Get(id);
            this.repository.Update(player);
            return player;
        }


        [WebInvoke(UriTemplate = "{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var deleted = this.repository.Get(id);
            this.repository.Delete(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

    }
}