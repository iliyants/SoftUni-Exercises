using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
	internal class ChangeTracker<TEntity>
        where TEntity : class, new()
    {
        private readonly List<TEntity> allEntities;

        private readonly List<TEntity> added;

        private readonly List<TEntity> removed;

        public ChangeTracker(IEnumerable<TEntity> entities)
        {
            this.allEntities = CloneEntities(entities.ToList()).ToList();

            this.added = new List<TEntity>();

            this.removed = new List<TEntity>();
        }

        private static IEnumerable<TEntity> CloneEntities(IEnumerable<TEntity> entities)
        {
            var clonedEntities = new List<TEntity>();

            var propertiesToClone = typeof(TEntity).GetProperties()
                .Where(pi => AllowedSqlTypes.SqlTypes.Contains(pi.PropertyType))
                .ToArray();


            foreach (var entity in entities)
            { 
                var clonedInstance = new TEntity();

                foreach (var propertyInfo in propertiesToClone)
                {
                    var originalValue = propertyInfo.GetValue(entity);

                    propertyInfo.SetValue(clonedInstance, originalValue);
                }

                clonedEntities.Add(clonedInstance);
            }

            return clonedEntities;
        }

        public IReadOnlyCollection<TEntity> AllEntities => this.allEntities.AsReadOnly();
        public IReadOnlyCollection<TEntity> Added => this.added.AsReadOnly();
        public IReadOnlyCollection<TEntity> Removed => this.removed.AsReadOnly();

        public void Add(TEntity item) => this.added.Add(item);
        public void Remove(TEntity item) => this.removed.Add(item);

        public static bool IsModified(TEntity entity, TEntity proxyEntity)
        {
            var monitoredProperties = typeof(TEntity).GetProperties()
                .Where(pi => AllowedSqlTypes.SqlTypes.Contains(pi.PropertyType))
                .ToArray();


            var modifiedProperties = monitoredProperties
                .Where(pi => pi.GetValue(proxyEntity) != pi.GetValue(entity))
                .ToArray();

            var isModified = modifiedProperties.Any();

            return isModified;

        }


        public IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, TEntity entity)
        {
            var primaryKeyProperties = primaryKeys
                .Select(pi => pi.GetValue(entity))
                .ToArray();

            return primaryKeyProperties.ToArray();
        }

        public IEnumerable<TEntity> GetModifiedEntities(DbSet<TEntity> dbSet)
        {
            var modifiedEntities = new List<TEntity>();

            var primaryKeys = typeof(TEntity)
                .GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            foreach (var originalEntitity in this.allEntities)
            {
                var primaryKeyValues = GetPrimaryKeyValues(primaryKeys, originalEntitity);

                var dbSetEntity = dbSet.Entities.Single(e =>
                primaryKeyValues.SequenceEqual(GetPrimaryKeyValues(primaryKeys, e)));


                var isModified = IsModified(originalEntitity, dbSetEntity);
                if (isModified)
                {
                    modifiedEntities.Add(dbSetEntity);
                }

            }

            return modifiedEntities;
        }






    }
}