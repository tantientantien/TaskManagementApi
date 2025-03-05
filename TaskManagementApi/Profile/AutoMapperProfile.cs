using AutoMapper;
using TaskManagementApi.Dtos;
using TaskManagementApi.Dtos.Blob;
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
        CreateMap<Category, CategoryDataDto>().ReverseMap();
        CreateMap<CategoryCreateDto, Category>();

        CreateMap<Label, LabelDataDto>().ReverseMap();
        CreateMap<LabelCreateDto, Label>();
        CreateMap<LabelUpdateDto, Label>();

        CreateMap<TaskComment, TaskCommentDataDto>().ReverseMap();
        CreateMap<TaskCommentCreateDto, TaskComment>();

        CreateMap<Task, TaskDataDto>().ReverseMap();
        CreateMap<TaskCreateDto, Task>();
        CreateMap<TaskUpdateDto, Task>();

        CreateMap<TaskLabel, TaskLabelDataDto>().ReverseMap();
        CreateMap<TaskLabelCreateDto, TaskLabel>();

        CreateMap<User, UserDataDto>().ReverseMap();

        CreateMap<TaskAttachment, TaskAttachmentDto>().ReverseMap();
    }
}