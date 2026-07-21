using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController (UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo )
        {
            _userManager = userManager;
            _stockRepo = stockRepo;     
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize] // yetkilendirme için

        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            //kullanıcyı bulmak
            var appUser = await _userManager.FindByNameAsync(username);
            //kullanıcı portfofyöünü bulucaz-veri tabanında eriş ve oturum açmış belli kullanıcya ait hepsini çek
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);

        }

        [HttpPost]
        [Authorize]
        
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            //kulanıcı oluştur
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            //stockıd getir
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if(stock == null) return BadRequest("stock not found");

            //portfolyü kurucaz
            
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("cannot some portfolio");

            //portföy nesenni oluşturup veri tabanına verme burada olucak
            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };
            //creatre işlemi doğrudan burda yapmak kötü olucak - repository

            await _portfolioRepo.CreateAsync(portfolioModel);
            if(portfolioModel == null)
            {
                return StatusCode(500, "couldnt create");
            }
            else
            {
                return Created();
            }

        }

        [HttpDelete]
        [Authorize]

        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            //filtreleme

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if(filteredStock.Count()==1)
            {
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("stock not in your protfolio");
            }
            return Ok();
        }


    }
}