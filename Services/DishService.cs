using AutoMapper;
using FirstAPI.Entities;
using FirstAPI.Exceptions;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPI.Services
{
    public interface IDishService
    {
        DishDto GetById(int id, int restaurantId);
        IEnumerable<DishDto> GetAll(int restauratnId);
        int CreateDish(CreateDishDto dto, int restaurantId);
        void Update(CreateDishDto dto, int restaurantId, int dishId);
        void Delete(int dishId, int restaurantId);
    }

    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public int CreateDish(CreateDishDto dto ,int restaurantId)
        {
            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = restaurantId;

            Console.WriteLine(dish.Id);

            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        public void Delete(int dishId, int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            TryNotFoundExceptionIfNull(restaurant, "Restaurant not found");

            var dish = restaurant.Dishes.FirstOrDefault(p => p.Id == dishId);

            TryNotFoundExceptionIfNull(dish, "Dish not found");

            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();
        }

        public IEnumerable<DishDto> GetAll(int restaurantId)
        {
            Restaurant restaurant = GetRestaurantById(restaurantId);

            TryNotFoundExceptionIfNull(restaurant, "Restaurant not found");

            List<DishDto> dtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return dtos;
        }

        public DishDto GetById(int id, int restaurantId)
        {
            Restaurant restaurant = GetRestaurantById(restaurantId);

            TryNotFoundExceptionIfNull(restaurant, "Restaurant not found");

            Dish dish = restaurant.Dishes.FirstOrDefault(r => r.Id == id);
            DishDto dto = _mapper.Map<DishDto>(dish);

            return dto;
        }

        public void Update(CreateDishDto dto, int restaurantId, int dishId)
        {
            var dish = _dbContext.Dishes.FirstOrDefault(r => r.Id == dishId);

            TryNotFoundExceptionIfNull(dish, "Dish not found");

            dish.Name = dto.Name;
            dish.Description = dto.Description;
            dish.Price = dto.Price;

            _dbContext.SaveChanges();
        }

        private Restaurant GetRestaurantById(int id)
        {
            Restaurant restaurant = _dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            return restaurant;
        }

        private void TryNotFoundExceptionIfNull(object obj, string message)
        {
            if (obj is null)
                throw new NotFoundException(message);
        }
    }
}
