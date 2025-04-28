using AutoMapper;
using Quantumart.QP8.BLL.Services.DTO;

namespace Quantumart.QP8.BLL.MapperProfiles;

public class EntityTreeItemProfile : Profile
{
    public EntityTreeItemProfile()
    {
        CreateFolderMapping();
    }

    private void CreateFolderMapping()
    {
        CreateMap<Folder, EntityTreeItem>(MemberList.None)
            .ForMember(data => data.Alias, opt => opt.MapFrom(src => src.OutputName));
    }
}
