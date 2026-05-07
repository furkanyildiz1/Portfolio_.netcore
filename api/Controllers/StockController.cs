using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    //çalışılan table gibi düşünelebilir controller atılır ve geriye kalanı alır api/stock
    [Route("api/[controller]")]
    //.net core bildiridir 
    [ApiController]

    //base sınıfından kalıtım alırız çünkü 200 404 gibi standar ynaıtları miras almak zorundayız
    public class StockController : ControllerBase
    {
        private readonly ApplicaitonDBContext _context;

        public StockController(ApplicaitonDBContext context)
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult GetAll()
        {
            var stocks = _context.Stocks.ToList()
                .Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        //json veriler olduğu içn frombody şarttır
        //istenmeyen şeyler apı den çekilip yanıltmasın diye dto kullna
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id}")]

        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var StockModel = _context.Stocks.FirstOrDefault(x=> x.Id == id);

            if(StockModel == null)
            {
                return NotFound();
            }

            StockModel.Symbol = updateDto.Symbol;
            StockModel.CompanyName = updateDto.CompanyName;
            StockModel.Purchase = updateDto.Purchase;
            StockModel.LastDiv = updateDto.LastDiv;
            StockModel.Industry = updateDto.Industry;
            StockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();
            return Ok(StockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var StockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
            if (StockModel == null)
            {
                return NotFound();
            }
            _context.Stocks.Remove(StockModel);
            _context.SaveChanges();
            return NoContent();
        }
    }
}