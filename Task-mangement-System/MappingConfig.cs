using AutoMapper;
using Task_mangement_System.Models;
using Task_mangement_System.Models.Dto;
using Task = Task_mangement_System.Models.Task;

namespace Task_mangement_System
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
            CreateMap<Task,TaskCreateDto>().ReverseMap();
            CreateMap<Task,TaskUpdateDto>().ReverseMap();
            CreateMap<Task, TaskDto>();
            CreateMap<TaskDto, Task>();
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
        }
    }
}
