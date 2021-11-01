using FirstAPI.Models;
using FirstAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishesService)
        {
            _dishService = dishesService;
        }

        [HttpGet("{id}")]
        public ActionResult<DishDto> GetById([FromRoute] int id, [FromRoute] int restaurantId)
        {
            DishDto dto = _dishService.GetById(id, restaurantId);
            return Ok(dto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<DishDto>> GetAll([FromRoute] int restaurantId)
        {
            IEnumerable<DishDto> dtos = _dishService.GetAll(restaurantId);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateDishDto dto, [FromRoute] int restaurantId)
        {
            int id = _dishService.CreateDish(dto, restaurantId);
            return Created($"/api/restaurant/{restaurantId}/dish/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            _dishService.Update(dto, restaurantId, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id, [FromRoute] int restaurantId)
        {
            _dishService.Delete(id, restaurantId);
            return Ok();
        }
    }
}
