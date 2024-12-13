using AutoMapper;
using Quantumart.QP8.DAL.Entities;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class DynamicImageProfile : Profile
    {
        public DynamicImageProfile()
        {
            CreateMap<DynamicImage, DynamicImageFieldDAL>(MemberList.None)
                .ForMember(data => data.Field, opt => opt.Ignore())
                .ForMember(data => data.Width, opt => opt.MapFrom(biz => biz.Width == 0 ? (short?)null : biz.Width))
                .ForMember(data => data.Height, opt => opt.MapFrom(biz => biz.Height == 0 ? (short?)null : biz.Height))
                .ForMember(data => data.Quality, opt => opt.MapFrom(biz => biz.Quality == 0 ? (short?)null : biz.Quality))
                ;

            CreateMap<DynamicImageFieldDAL, DynamicImage>(MemberList.None)
                .ForMember(biz => biz.Field, opt => opt.Ignore())
                .ForMember(biz => biz.Width, opt => opt.MapFrom(data => data.Width ?? 0))
                .ForMember(biz => biz.Height, opt => opt.MapFrom(data => data.Height ?? 0))
                .ForMember(biz => biz.Quality, opt => opt.MapFrom(data => data.Quality ?? 0))
                ;
        }
    }
}
