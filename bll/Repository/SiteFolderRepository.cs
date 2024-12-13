using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quantumart.QP8.BLL.Helpers;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL;
using Quantumart.QP8.Resources;
using SiteFolderDAL = Quantumart.QP8.DAL.Entities.SiteFolderDAL;

namespace Quantumart.QP8.BLL.Repository
{
    internal class SiteFolderRepository : FolderRepository
    {
        private DbSet<SiteFolderDAL> CurrentSet => QPContext.EFContext.SiteFolderSet;

        public override Folder GetById(int id)
        {
            return QPContext.Map<SiteFolder>(CurrentSet.Include("LastModifiedByUser").SingleOrDefault(s => s.Id == id));
        }

        public override Folder GetChildByName(int parentId, string name)
        {
            return QPContext.Map<SiteFolder>(CurrentSet.SingleOrDefault(s => s.ParentId == parentId && s.Name == name));
        }

        public override Folder GetRoot(int parentEntityId)
        {
            return QPContext.Map<SiteFolder>(CurrentSet.SingleOrDefault(s => s.SiteId == parentEntityId && s.ParentId == null));
        }

        public override Folder CreateInDb(Folder folder) => DefaultRepository.Save<SiteFolder, SiteFolderDAL>((SiteFolder)folder);

        public override Folder CreateInDbAsAdmin(Folder folder) => DefaultRepository.SaveAsAdmin<SiteFolder, SiteFolderDAL>((SiteFolder)folder);

        public override IEnumerable<Folder> GetAllChildrenFromDb(int parentId)
        {
            return QPContext.Map<SiteFolder[]>(CurrentSet.Where(c => c.ParentId == parentId).ToList());
        }

        public override IEnumerable<Folder> GetChildrenFromDb(int parentEntityId, int parentId)
        {
            using var scope = new QPConnectionScope();
            return QPContext.Map<SiteFolder[]>(
                Common.GetChildFoldersList(scope.DbConnection, QPContext.EFContext, QPContext.IsAdmin, QPContext.CurrentUserId,
                        parentEntityId, true, parentId, PermissionLevel.List, false, out _)
                    .ToList()
            );
        }

        public override IEnumerable<Folder> GetChildrenWithSync(int parentEntityId, int? parentId, PathHelper pathHelper)
        {
            var newId = Synchronize(parentEntityId, parentId, pathHelper);
            return GetChildrenFromDb(parentEntityId, newId);
        }

        public override List<PathSecurityInfo> GetPaths(int siteId)
        {
            return CurrentSet.Where(n => n.SiteId == siteId)
                .Select(n => new PathSecurityInfo
                {
                    Id = (int)n.Id,
                    Path = n.Path.Replace('\\', System.IO.Path.DirectorySeparatorChar)
                }).ToList();
        }

        public override string FolderNotFoundMessage(int id) => string.Format(LibraryStrings.SiteFolderNotExists);

        protected override Folder UpdateInDb(Folder folder)
        {
            using (var scope = new QPConnectionScope())
            {
                Common.UpdateSiteSubFoldersPath(scope.DbConnection, folder.Id, folder.Path, QPContext.CurrentUserId, DateTime.Now);
            }

            return DefaultRepository.Update<SiteFolder, SiteFolderDAL>((SiteFolder)folder);
        }

        protected override void DeleteFromDb(Folder folder)
        {
            void Traverse(Folder f)
            {
                f.LoadAllChildren = true;
                f.AutoLoadChildren = true;
                if (f.Children != null)
                {
                    foreach (var sb in f.Children)
                    {
                        Traverse((Folder)sb);
                    }
                }

                DefaultRepository.Delete<SiteFolderDAL>(f.Id);
            }

            Traverse(folder);
        }
    }
}
