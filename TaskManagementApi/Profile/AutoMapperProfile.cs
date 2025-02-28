using AutoMapper;
using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Category;
using TaskManagementApi.Dtos.CategoryDtos;
using TaskManagementApi.Dtos.Label;
using TaskManagementApi.Dtos.Task;
using TaskManagementApi.Dtos.TaskComment;
using TaskManagementApi.Dtos.TaskLabel;
using TaskManagementApi.Dtos.User;
using TaskManagementApi.Models;
using Task = TaskManagementApi.Models.Task;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Category, CategoryDataDto>();
        CreateMap<CategoryCreateDto, Category>();


        CreateMap<Label, LabelDataDto>();
        CreateMap<LabelCreateDto, Label>();
        CreateMap<LabelUpdateDto, Label>();


        CreateMap<TaskCommentCreateDto, TaskComment>();
        CreateMap<TaskComment, TaskCommentDataDto>();


        CreateMap<Task, TaskDataDto>();
        CreateMap<TaskCreateDto, Task>();
        CreateMap<TaskUpdateDto, Task>();


        CreateMap<TaskLabelCreateDto, TaskLabel>();
        CreateMap<TaskLabel, TaskLabelDataDto>();


        CreateMap<User, UserDataDto>();
    }
}