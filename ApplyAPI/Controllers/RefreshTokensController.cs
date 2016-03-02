using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ApplyAPI.Repositories;

namespace ApplyAPI.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController: ApiController
    {
        private readonly AuthRepository repo;

        public RefreshTokensController()
        {
            repo = new AuthRepository();
        }

        //[Authorize(Users = "Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(repo.GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await repo.RemoveRefreshToken(tokenId);

            if (result)
            {
                return Ok();
            }

            return BadRequest("Token Id does not exist");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
