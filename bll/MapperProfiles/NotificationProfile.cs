using System;
using AutoMapper;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL.Entities;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationsDAL>(MemberList.None)
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Workflow, opt => opt.Ignore())
                .ForMember(data => data.FromUser, opt => opt.Ignore())
                .ForMember(data => data.ToUser, opt => opt.Ignore())
                .ForMember(data => data.ToUserGroup, opt => opt.Ignore())
                .ForMember(data => data.FromBackenduserId, opt => opt.MapFrom(src => src.FromBackenduser ? src.FromBackenduserId : SpecialIds.AdminUserId))
                .ForMember(data => data.Content, opt => opt.Ignore())
                .AfterMap(SetDalProperties);

            CreateMap<NotificationsDAL, Notification>(MemberList.None).AfterMap(SetBizProperties);

            CreateMap<NotificationObjectFormat, ObjectFormatDAL>(MemberList.None)
                .ForMember(data => data.Locked, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (DateTime?)src.Locked))
                .ForMember(data => data.LockedBy, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (int?)src.LockedBy))
                .AfterMap(SetDalObjectFormatProperties);

            CreateMap<ObjectFormatDAL, NotificationObjectFormat>(MemberList.None);
        }

        private static void SetBizProperties(NotificationsDAL dataObject, Notification bizObject)
        {
            bizObject.SelectedReceiverType = bizObject.ComputeReceiverType();
            if (string.IsNullOrEmpty(bizObject.ExternalUrl))
            {
                bizObject.ExternalUrl = bizObject.Content.Site.ExternalUrl;
            }
        }

        private static void SetDalProperties(Notification bizObject, NotificationsDAL dataObject)
        {
            if (string.Equals(bizObject.ExternalUrl, bizObject.Content.Site.ExternalUrl) || !bizObject.IsExternal)
            {
                dataObject.ExternalUrl = null;
            }
        }

        private static void SetDalObjectFormatProperties(NotificationObjectFormat bizObject, ObjectFormatDAL dataObject)
        {
            if (bizObject.Assembled == null)
            {
                dataObject.Assembled = bizObject.Created;
            }
        }
    }
}
