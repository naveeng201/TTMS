using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace TTMS.Models
{
    public partial class TTMSEntities
    {
        public override int SaveChanges()
        {
            try
            {
                var entities = ChangeTracker.Entries().Where(x => x.Entity is IBaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
                //for now considering User Id as 1 always
                var currentUserId = 1;
                foreach (var entity in entities)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((IBaseEntity)entity.Entity).CreatedDate = DateTime.Now;
                        ((IBaseEntity)entity.Entity).CreatedBy = currentUserId;
                    }

                    ((IBaseEntity)entity.Entity).ModifiedDate = DateTime.Now;
                    ((IBaseEntity)entity.Entity).ModifiedBy = currentUserId;
                }
                return base.SaveChanges();
            }
            catch(DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var exceptionMessage = string.Concat(/*ex.Message*/"Validation failed for one or more entities. ", " The validation errors are : ", string.Join("  ", errorMessages));
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}