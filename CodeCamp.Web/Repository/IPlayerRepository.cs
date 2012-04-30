using System;
namespace CodeCamp.Web.Repository
{
   public interface IPlayerRepository
    {
        void Delete(int id);
        CodeCamp.Web.Models.Player Get(int id);
        System.Collections.Generic.List<CodeCamp.Web.Models.Player> GetAll();
        void Post(CodeCamp.Web.Models.Player player);
        void Update(CodeCamp.Web.Models.Player updatedPlayer);
    }
}
