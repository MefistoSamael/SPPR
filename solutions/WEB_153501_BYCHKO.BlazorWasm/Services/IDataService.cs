﻿using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;

namespace WEB_153501_BYCHKO.BlazorWasm.Services
{
    public interface IDataService
    {
        
        // Список категорий объектов
        List<EngineTypeCategory> Categories { get; set; }
        //Список объектов
        List<Airplane> ObjectsList { get; set; }

        // Признак успешного ответа на запрос к Api
        bool Success { get; set; }
        // Сообщение об ошибке
        string ErrorMessage { get; set; }
        // Количество страниц списка
        int TotalPages { get; set; }
        // Номер текущей страницы
        int CurrentPage { get; set; }

        event Action DataLoaded;

        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <returns></returns>
        public Task GetProductListAsync(
         string? categoryNormalizedName,
         int pageNo = 1);

        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
        public Task GetProductByIdAsync(int id);

        /// <summary>
        /// Получение списка категорий
        /// </summary>
        /// <returns></returns>
        public Task GetCategoryListAsync();
    }
}
