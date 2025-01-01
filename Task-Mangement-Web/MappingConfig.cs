using AutoMapper;
using Task_mangement_Web.Models;
using Task_mangement_Web.Models.Dto;
using Task_Mangement_Web.Models.Dto;
using Task = Task_mangement_Web.Models.Task;

namespace Task_mangement_Web
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Task, TaskDto>();
            CreateMap<TaskDto, Task>();
            CreateMap<TaskDto, TaskUpdateDto>().ReverseMap();
        }
    }
}
