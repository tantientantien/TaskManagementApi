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


        CreateMap<Task, TaskDataDto>()
            .ForMember(dest => dest.AttachmentCount, opt => opt.MapFrom(src => src.Attachments.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.TaskComments.Count));
        //.ForMember(dest => dest.labels, opt => opt.MapFrom(src =>
        //    src.TaskLabels.Where(tl => tl.Label != null)
        //                  .Select(tl => new TaskLabelDataDto
        //                  {
        //                      Id = tl.Label.Id,
        //                      Name = tl.Label.Name,
        //                      Color = tl.Label.Color
        //                  }).ToList()
        //));




        CreateMap<TaskCreateDto, Task>();
        CreateMap<TaskUpdateDto, Task>();

        CreateMap<Task, TaskDetailDto>()
     .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.TaskComments
         .Select(tc => new TaskCommentDataDto
         {
             id = tc.Id,
             Content = tc.Content,
             CreatedAt = tc.CreatedAt,
             User = tc.User != null ? new UserDataDto
             {
                 Id = tc.User.Id,
                 Email = tc.User.Email,
             } : null
         }).ToList()
     ))
     .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.TaskLabels
         .Where(tl => tl.Label != null)
         .Select(tl => new LabelDataDto
         {
             Id = tl.Label.Id,
             Name = tl.Label.Name,
             Color = tl.Label.Color
         }).ToList()
     ))
     .ReverseMap();


        CreateMap<TaskLabel, TaskLabelDataDto>().ReverseMap();
        CreateMap<TaskLabelCreateDto, TaskLabel>();

        CreateMap<User, UserDataDto>().ReverseMap();
        //.ForMember(dest => dest.)

        CreateMap<TaskAttachment, TaskAttachmentDto>().ReverseMap();
    }
}