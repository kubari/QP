using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL;
using Quantumart.QP8.DAL.Entities;

namespace Quantumart.QP8.BLL.Repository
{
    internal static class ContentConstraintRepository
    {
        internal static IEnumerable<ContentConstraint> GetConstraintsByContentId(int id)
        {
            return QPContext.Map<List<ContentConstraint>>(QPContext.EFContext.ContentConstraintSet.Include("Rules").Where(s => s.ContentId == id).ToList());
        }

        internal static ContentConstraint GetConstraintByFieldId(int id)
        {
            ContentConstraint result = null;
            var constraintId = QPContext.EFContext.ContentConstraintRuleSet.Where(s => s.FieldId == id).Select(s => s.ConstraintId).SingleOrDefault();
            if (constraintId > 0)
            {
                result = QPContext.Map<ContentConstraint>(QPContext.EFContext.ContentConstraintSet.Include("Rules").SingleOrDefault(s => s.Id == constraintId));
            }
            return result;
        }

        private static void ChangeContentIndexesTriggerState(DbConnection cnn, bool enable)
        {
            Common.ChangeTriggerState(cnn, "td_content_indexes", enable);
            Common.ChangeTriggerState(cnn, "tu_content_indexes", enable);
        }

        internal static ContentConstraint Save(ContentConstraint constraint)
        {
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }

            if (!constraint.IsNew)
            {
                throw new ArgumentException("Метод вызван для существующего в БД ContentConstraint");
            }


            // Сохраняем ограничение только если есть правила
            if (constraint.Rules != null && constraint.Rules.Any())
            {
                var ccDal = QPContext.Map<ContentConstraintDAL>(constraint);

                using (var scope = new QPConnectionScope())
                {

                    ccDal = DefaultRepository.SimpleSave(ccDal);
                    foreach (var rule in ccDal.Rules)
                    {
                        rule.Constraint = ccDal;
                        rule.ConstraintId = ccDal.Id;
                        DefaultRepository.SimpleSave(rule);
                    }

                    if (constraint.IsComplex)
                    {
                        Common.CreateComplexIndex(QPContext.EFContext, scope.DbConnection, ccDal);
                    }
                }

                var newConstraint = QPContext.Map<ContentConstraint>(ccDal);
                return newConstraint;
            }



            return constraint;
        }



        internal static ContentConstraint Update(ContentConstraint constraint)
        {
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }

            if (constraint.IsNew)
            {
                throw new ArgumentException("Метод вызван для несуществующего в БД ContentConstraint");
            }

            // если нет правил, то удалить ограничение
            if (constraint.Rules == null || !constraint.Rules.Any())
            {
                Delete(constraint);
                return null;
            }

            using var scope = new QPConnectionScope();
            try
            {
                if (QPContext.DatabaseType == DatabaseType.SqlServer)
                {
                    ChangeContentIndexesTriggerState(scope.DbConnection, false);
                }

                var context = QPContext.EFContext;
                var oldConstraintDal = context.ContentConstraintSet.Include(n => n.Rules)
                    .Single(d => d.Id == constraint.Id);
                Common.DropComplexIndex(scope.DbConnection, oldConstraintDal);

                // удалить все правила которые уже есть
                DefaultRepository.SimpleDeleteBulk(oldConstraintDal.Rules.ToArray(), context);

                // создать новые записи для правил
                foreach (var rule in constraint.Rules)
                {
                    rule.ConstraintId = constraint.Id;
                }

                var newDalList = QPContext.Map<ContentConstraintRuleDAL[]>(constraint.Rules.ToList());
                var list = DefaultRepository.SimpleSaveBulk(newDalList.AsEnumerable()).ToList();
                constraint.Rules =  QPContext.Map<ContentConstraintRule[]>(list).ToArray();

                if (constraint.IsComplex)
                {
                    var dal =  QPContext.Map<ContentConstraintDAL>(constraint);
                    Common.CreateComplexIndex(QPContext.EFContext, scope.DbConnection, dal);
                }
            }
            finally
            {
                if (QPContext.DatabaseType == DatabaseType.SqlServer)
                {
                    ChangeContentIndexesTriggerState(scope.DbConnection, true);
                }
            }

            return constraint;
        }

        public static void Delete(ContentConstraint constraint)
        {
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }

            if (constraint.IsNew)
            {
                throw new ArgumentException("Метод вызван для несуществующего в БД ContentConstraint");
            }

            using var scope = new QPConnectionScope();
            try
            {
                if (QPContext.DatabaseType == DatabaseType.SqlServer)
                {
                    ChangeContentIndexesTriggerState(scope.DbConnection, false);
                }
                var dal =  QPContext.Map<ContentConstraintDAL>(constraint);
                Common.DropComplexIndex(scope.DbConnection, dal);
                DefaultRepository.Delete<ContentConstraintDAL>(constraint.Id);

            }
            finally
            {
                if (QPContext.DatabaseType == DatabaseType.SqlServer)
                {
                    ChangeContentIndexesTriggerState(scope.DbConnection, true);
                }
            }
        }



        internal static void CopyContentConstrainRules(string relationsBetweenConstraintsXml, string relationsBetweenAttributesXml)
        {
            using (new QPConnectionScope())
            {
                Common.CopyContentConstrainRules(QPConnectionScope.Current.DbConnection, relationsBetweenConstraintsXml, relationsBetweenAttributesXml);
            }
        }
    }
}
