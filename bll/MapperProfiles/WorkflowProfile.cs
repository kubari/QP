using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    public class WorkflowProfile : Profile
    {
        public WorkflowProfile()
        {
            CreateMap<Workflow, WorkflowDAL>(MemberList.None)
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Site, opt => opt.Ignore())
                .ForMember(data => data.WorkflowRules, opt => opt.Ignore())
                .ForMember(data => data.WorkflowAccess, opt => opt.Ignore())
                ;

            CreateMap<WorkflowRule, WorkflowRulesDAL>(MemberList.None)
                .ForMember(data => data.StatusType, opt => opt.Ignore())
                .ForMember(data => data.Workflow, opt => opt.Ignore());

            CreateMap<DataRow, WorkflowListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")))
                ;

            CreateMap<WorkflowDAL, Workflow>(MemberList.None);
            CreateMap<WorkflowRulesDAL, WorkflowRule>(MemberList.None)
                .ForMember(biz => biz.Description, opt  => opt.MapFrom(src => src.Description));
        }
    }
}
